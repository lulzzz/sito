using System;
using System.Linq;
using Maddalena.ML.Geocoding;
using Mongolino;

namespace Maddalena.ML.Model
{
    /// <summary>
    ///     Most basic and generic form of address.
    ///     Just the full address string and a lat/long
    /// </summary>
    public class Address : DBObject<Address>
    {
        public Address() { }

        public Address(string formattedAddress, AddressComponent[] components,
            Location coordinates, GoogleViewport viewport, Bounds bounds, bool isPartialMatch,
            GoogleLocationType locationType, string placeId)
        {
            Components = components ?? throw new ArgumentNullException(nameof(components));
            IsPartialMatch = isPartialMatch;
            Viewport = viewport;
            Bounds = bounds;
            LocationType = locationType;
            PlaceId = placeId;
            FormattedAddress = formattedAddress;
            Coordinates = coordinates;
        }

        public AddressType Type { get; set; }

        public ObjectRef<Company> Company { get; set; }

        public ObjectRef<Person> Person { get; set; }

        public string FormattedAddress { get; set; }

        public Location Coordinates { get; set; }

        public GeoCodeType Precision => Components.SelectMany(x => x.Types).Min();

        public GoogleLocationType LocationType { get; set; }

        public AddressComponent[] Components { get; set; }

        public bool IsPartialMatch { get; set; }

        public GoogleViewport Viewport { get; set; }

        public Bounds Bounds { get; set; }

        public string PlaceId { get; set; }

        public AddressComponent this[Geocoding.GeoCodeType type]
        {
            get { return Components.FirstOrDefault(c => c.Types.Contains(type)); }
        }

        public virtual Distance DistanceBetween(Address address)
        {
            return Coordinates.DistanceBetween(address.Coordinates);
        }

        public virtual Distance DistanceBetween(Address address, DistanceUnits units)
        {
            return Coordinates.DistanceBetween(address.Coordinates, units);
        }

        public override string ToString() => FormattedAddress;
    }
}