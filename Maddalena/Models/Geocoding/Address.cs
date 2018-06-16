using System;
using System.Linq;

namespace Maddalena.Models.Geocoding
{
    /// <summary>
    ///     Most basic and generic form of address.
    ///     Just the full address string and a lat/long
    /// </summary>
    public class Address
    {
        string _formattedAddress = string.Empty;
        Location coordinates;
        string provider = string.Empty;

        public Address(AddressType type, string formattedAddress, AddressComponent[] components,
            Location coordinates, GoogleViewport viewport, Bounds bounds, bool isPartialMatch,
            GoogleLocationType locationType, string placeId)
            : this(formattedAddress, coordinates, "Google")
        {
            Type = type;
            Components = components ?? throw new ArgumentNullException(nameof(components));
            IsPartialMatch = isPartialMatch;
            Viewport = viewport;
            Bounds = bounds;
            LocationType = locationType;
            PlaceId = placeId;
        }

        public Address(string formattedAddress, Location coordinates, string provider)
        {
            FormattedAddress = formattedAddress;
            Coordinates = coordinates;
            Provider = provider;
        }

        public virtual string FormattedAddress
        {
            get => _formattedAddress;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("FormattedAddress is null or blank");

                _formattedAddress = value.Trim();
            }
        }

        public virtual Location Coordinates
        {
            get => coordinates;
            set => coordinates = value ?? throw new ArgumentNullException("Coordinates");
        }

        public virtual string Provider
        {
            get => provider;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Provider can not be null or blank");

                provider = value;
            }
        }

        public AddressType Type { get; set; }
        public GoogleLocationType LocationType { get; set; }
        public AddressComponent[] Components { get; set; }
        public bool IsPartialMatch { get; set; }
        public GoogleViewport Viewport { get; set; }
        public Bounds Bounds { get; set; }
        public string PlaceId { get; set; }

        public AddressComponent this[AddressType type]
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