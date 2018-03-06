using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Maddalena.Modules.Geocoding
{
    /// <remarks>
    ///     http://code.google.com/apis/maps/documentation/geocoding/
    /// </remarks>
    public class Geocoder : IGeocoder
    {
        const string keyMessage = "Only one of BusinessKey or ApiKey should be set on the Geocoder.";
        string apiKey;
        BusinessKey businessKey;

        public Geocoder()
        {
        }

        public Geocoder(BusinessKey businessKey)
        {
            BusinessKey = businessKey;
        }

        public Geocoder(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey
        {
            get => apiKey;
            set
            {
                if (businessKey != null)
                    throw new InvalidOperationException(keyMessage);
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ApiKey can not be null or empty");

                apiKey = value;
            }
        }

        public BusinessKey BusinessKey
        {
            get => businessKey;
            set
            {
                if (!string.IsNullOrEmpty(apiKey))
                    throw new InvalidOperationException(keyMessage);
                businessKey = value ?? throw new ArgumentException("BusinessKey can not be null");
            }
        }

        public IWebProxy Proxy { get; set; }
        public string Language { get; set; }
        public string RegionBias { get; set; }
        public Bounds BoundsBias { get; set; }
        public IList<ComponentFilter> ComponentFilters { get; set; }

        public string ServiceUrl
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("https://maps.googleapis.com/maps/api/geocode/xml?{0}={1}&sensor=false");

                if (!string.IsNullOrEmpty(Language))
                {
                    builder.Append("&language=");
                    builder.Append(WebUtility.UrlEncode(Language));
                }

                if (!string.IsNullOrEmpty(RegionBias))
                {
                    builder.Append("&region=");
                    builder.Append(WebUtility.UrlEncode(RegionBias));
                }

                if (!string.IsNullOrEmpty(ApiKey))
                {
                    builder.Append("&key=");
                    builder.Append(WebUtility.UrlEncode(ApiKey));
                }

                if (BusinessKey != null)
                {
                    builder.Append("&client=");
                    builder.Append(WebUtility.UrlEncode(BusinessKey.ClientId));
                    if (BusinessKey.HasChannel)
                    {
                        builder.Append("&channel=");
                        builder.Append(WebUtility.UrlEncode(BusinessKey.Channel));
                    }
                }

                if (BoundsBias != null)
                {
                    builder.Append("&bounds=");
                    builder.Append(BoundsBias.SouthWest.Latitude.ToString(CultureInfo.InvariantCulture));
                    builder.Append(",");
                    builder.Append(BoundsBias.SouthWest.Longitude.ToString(CultureInfo.InvariantCulture));
                    builder.Append("|");
                    builder.Append(BoundsBias.NorthEast.Latitude.ToString(CultureInfo.InvariantCulture));
                    builder.Append(",");
                    builder.Append(BoundsBias.NorthEast.Longitude.ToString(CultureInfo.InvariantCulture));
                }

                if (ComponentFilters != null)
                {
                    builder.Append("&components=");
                    builder.Append(string.Join("|", ComponentFilters.Select(x => x.Filter)));
                }

                return builder.ToString();
            }
        }

        async Task<IEnumerable<Address>> IGeocoder.GeocodeAsync(string address)
        {
            return await GeocodeAsync(address).ConfigureAwait(false);
        }

        async Task<IEnumerable<Address>> IGeocoder.GeocodeAsync(string street, string city, string state,
            string postalCode, string country)
        {
            return await GeocodeAsync(BuildAddress(street, city, state, postalCode, country)).ConfigureAwait(false);
        }

        async Task<IEnumerable<Address>> IGeocoder.ReverseGeocodeAsync(Location location)
        {
            return await ReverseGeocodeAsync(location).ConfigureAwait(false);
        }

        async Task<IEnumerable<Address>> IGeocoder.ReverseGeocodeAsync(double latitude, double longitude)
        {
            return await ReverseGeocodeAsync(latitude, longitude).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Address>> GeocodeAsync(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            var request = BuildWebRequest("address", WebUtility.UrlEncode(address));
            return await ProcessRequest(request).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Address>> ReverseGeocodeAsync(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            return await ReverseGeocodeAsync(location.Latitude, location.Longitude).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Address>> ReverseGeocodeAsync(double latitude, double longitude)
        {
            var request = BuildWebRequest("latlng", BuildGeolocation(latitude, longitude));
            return await ProcessRequest(request).ConfigureAwait(false);
        }

        private string BuildAddress(string street, string city, string state, string postalCode, string country)
        {
            return $"{street} {city}, {state} {postalCode}, {country}";
        }

        private string BuildGeolocation(double latitude, double longitude)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.00000000},{1:0.00000000}", latitude, longitude);
        }

        private async Task<IEnumerable<Address>> ProcessRequest(HttpRequestMessage request)
        {
            try
            {
                using (var client = BuildClient())
                {
                    return await ProcessWebResponse(await client.SendAsync(request).ConfigureAwait(false))
                        .ConfigureAwait(false);
                }
            }
            catch (GoogleGeocodingException)
            {
                //let these pass through
                throw;
            }
            catch (Exception ex)
            {
                //wrap in google exception
                throw new GoogleGeocodingException(ex);
            }
        }

        HttpClient BuildClient()
        {
            if (Proxy == null)
                return new HttpClient();

            var handler = new HttpClientHandler();
            handler.Proxy = Proxy;
            return new HttpClient(handler);
        }

        private HttpRequestMessage BuildWebRequest(string type, string value)
        {
            string url = string.Format(ServiceUrl, type, value);

            if (BusinessKey != null)
                url = BusinessKey.GenerateSignature(url);

            return new HttpRequestMessage(HttpMethod.Get, url);
        }

        private async Task<IEnumerable<Address>> ProcessWebResponse(HttpResponseMessage response)
        {
            XPathDocument xmlDoc = await LoadXmlResponse(response).ConfigureAwait(false);
            XPathNavigator nav = xmlDoc.CreateNavigator();

            GoogleStatus status = EvaluateStatus((string) nav.Evaluate("string(/GeocodeResponse/status)"));

            if (status != GoogleStatus.Ok && status != GoogleStatus.ZeroResults)
                throw new GoogleGeocodingException(status);

            if (status == GoogleStatus.Ok)
                return ParseAddresses(nav.Select("/GeocodeResponse/result")).ToArray();

            return new Address[0];
        }

        private async Task<XPathDocument> LoadXmlResponse(HttpResponseMessage response)
        {
            using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                XPathDocument doc = new XPathDocument(stream);
                return doc;
            }
        }

        private IEnumerable<Address> ParseAddresses(XPathNodeIterator nodes)
        {
            while (nodes.MoveNext())
            {
                XPathNavigator nav = nodes.Current;

                AddressType type = EvaluateType((string) nav.Evaluate("string(type)"));
                string placeId = (string) nav.Evaluate("string(place_id)");
                string formattedAddress = (string) nav.Evaluate("string(formatted_address)");

                var components = ParseComponents(nav.Select("address_component")).ToArray();

                double latitude = (double) nav.Evaluate("number(geometry/location/lat)");
                double longitude = (double) nav.Evaluate("number(geometry/location/lng)");
                Location coordinates = new Location(latitude, longitude);

                double neLatitude = (double) nav.Evaluate("number(geometry/viewport/northeast/lat)");
                double neLongitude = (double) nav.Evaluate("number(geometry/viewport/northeast/lng)");
                Location neCoordinates = new Location(neLatitude, neLongitude);

                double swLatitude = (double) nav.Evaluate("number(geometry/viewport/southwest/lat)");
                double swLongitude = (double) nav.Evaluate("number(geometry/viewport/southwest/lng)");
                Location swCoordinates = new Location(swLatitude, swLongitude);

                var viewport = new GoogleViewport {Northeast = neCoordinates, Southwest = swCoordinates};

                GoogleLocationType locationType =
                    EvaluateLocationType((string) nav.Evaluate("string(geometry/location_type)"));

                Bounds bounds = null;
                if (nav.SelectSingleNode("geometry/bounds") != null)
                {
                    double neBoundsLatitude = (double) nav.Evaluate("number(geometry/bounds/northeast/lat)");
                    double neBoundsLongitude = (double) nav.Evaluate("number(geometry/bounds/northeast/lng)");
                    Location neBoundsCoordinates = new Location(neBoundsLatitude, neBoundsLongitude);

                    double swBoundsLatitude = (double) nav.Evaluate("number(geometry/bounds/southwest/lat)");
                    double swBoundsLongitude = (double) nav.Evaluate("number(geometry/bounds/southwest/lng)");
                    Location swBoundsCoordinates = new Location(swBoundsLatitude, swBoundsLongitude);

                    bounds = new Bounds(swBoundsCoordinates, neBoundsCoordinates);
                }

                bool isPartialMatch;
                bool.TryParse((string) nav.Evaluate("string(partial_match)"), out isPartialMatch);

                yield return new Address(type, formattedAddress, components, coordinates, viewport, bounds,
                    isPartialMatch, locationType, placeId);
            }
        }

        private IEnumerable<AddressComponent> ParseComponents(XPathNodeIterator nodes)
        {
            while (nodes.MoveNext())
            {
                XPathNavigator nav = nodes.Current;

                string longName = (string) nav.Evaluate("string(long_name)");
                string shortName = (string) nav.Evaluate("string(short_name)");
                var types = ParseComponentTypes(nav.Select("type")).ToArray();

                if (types.Any()) //don't return an address component with no type
                    yield return new AddressComponent(types, longName, shortName);
            }
        }

        private IEnumerable<AddressType> ParseComponentTypes(XPathNodeIterator nodes)
        {
            while (nodes.MoveNext())
                yield return EvaluateType(nodes.Current.InnerXml);
        }

        /// <remarks>
        ///     http://code.google.com/apis/maps/documentation/geocoding/#StatusCodes
        /// </remarks>
        private GoogleStatus EvaluateStatus(string status)
        {
            switch (status)
            {
                case "OK": return GoogleStatus.Ok;
                case "ZERO_RESULTS": return GoogleStatus.ZeroResults;
                case "OVER_QUERY_LIMIT": return GoogleStatus.OverQueryLimit;
                case "REQUEST_DENIED": return GoogleStatus.RequestDenied;
                case "INVALID_REQUEST": return GoogleStatus.InvalidRequest;
                default: return GoogleStatus.Error;
            }
        }

        /// <remarks>
        ///     http://code.google.com/apis/maps/documentation/geocoding/#Types
        /// </remarks>
        private AddressType EvaluateType(string type)
        {
            switch (type)
            {
                case "street_address": return AddressType.StreetAddress;
                case "route": return AddressType.Route;
                case "intersection": return AddressType.Intersection;
                case "political": return AddressType.Political;
                case "country": return AddressType.Country;
                case "administrative_area_level_1": return AddressType.AdministrativeAreaLevel1;
                case "administrative_area_level_2": return AddressType.AdministrativeAreaLevel2;
                case "administrative_area_level_3": return AddressType.AdministrativeAreaLevel3;
                case "colloquial_area": return AddressType.ColloquialArea;
                case "locality": return AddressType.Locality;
                case "sublocality": return AddressType.SubLocality;
                case "neighborhood": return AddressType.Neighborhood;
                case "premise": return AddressType.Premise;
                case "subpremise": return AddressType.Subpremise;
                case "postal_code": return AddressType.PostalCode;
                case "natural_feature": return AddressType.NaturalFeature;
                case "airport": return AddressType.Airport;
                case "park": return AddressType.Park;
                case "point_of_interest": return AddressType.PointOfInterest;
                case "post_box": return AddressType.PostBox;
                case "street_number": return AddressType.StreetNumber;
                case "floor": return AddressType.Floor;
                case "room": return AddressType.Room;
                case "postal_town": return AddressType.PostalTown;
                case "establishment": return AddressType.Establishment;
                case "sublocality_level_1": return AddressType.SubLocalityLevel1;
                case "sublocality_level_2": return AddressType.SubLocalityLevel2;
                case "sublocality_level_3": return AddressType.SubLocalityLevel3;
                case "sublocality_level_4": return AddressType.SubLocalityLevel4;
                case "sublocality_level_5": return AddressType.SubLocalityLevel5;
                case "postal_code_suffix": return AddressType.PostalCodeSuffix;

                default: return AddressType.Unknown;
            }
        }

        /// <remarks>
        ///     https://developers.google.com/maps/documentation/geocoding/?csw=1#Results
        /// </remarks>
        private GoogleLocationType EvaluateLocationType(string type)
        {
            switch (type)
            {
                case "ROOFTOP": return GoogleLocationType.Rooftop;
                case "RANGE_INTERPOLATED": return GoogleLocationType.RangeInterpolated;
                case "GEOMETRIC_CENTER": return GoogleLocationType.GeometricCenter;
                case "APPROXIMATE": return GoogleLocationType.Approximate;

                default: return GoogleLocationType.Unknown;
            }
        }
    }
}