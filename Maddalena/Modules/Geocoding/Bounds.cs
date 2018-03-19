using System;

namespace Maddalena.Modules.Geocoding
{
    public class Bounds
    {
        public Bounds(double southWestLatitude, double southWestLongitude, double northEastLatitude,
            double northEastLongitude)
            : this(new Location(southWestLatitude, southWestLongitude),
                new Location(northEastLatitude, northEastLongitude))
        {
        }

        public Bounds(Location southWest, Location northEast)
        {
            if (southWest == null)
                throw new ArgumentNullException(nameof(southWest));

            if (northEast == null)
                throw new ArgumentNullException(nameof(northEast));

            if (southWest.Latitude > northEast.Latitude)
                throw new ArgumentException("southWest latitude cannot be greater than northEast latitude");

            SouthWest = southWest;
            NorthEast = northEast;
        }

        public Location SouthWest { get; set; }

        public Location NorthEast { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Bounds);
        }

        public bool Equals(Bounds bounds)
        {
            if (bounds == null)
                return false;

            return (SouthWest.Equals(bounds.SouthWest) && NorthEast.Equals(bounds.NorthEast));
        }

        public override int GetHashCode()
        {
            return SouthWest.GetHashCode() ^ NorthEast.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} | {1}", SouthWest, NorthEast);
        }
    }
}