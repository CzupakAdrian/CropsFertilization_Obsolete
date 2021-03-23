using System;
using System.Device.Location;

namespace Common.DataStructures.Coordinates
{
    public class Location : GeoCoordinate
    {
        private const int NOT_DOCK_STATION = -1;

        public Location()
            : this(NOT_DOCK_STATION)
        { }
        public Location(double latitude, double longitude)
            : this(latitude, longitude, NOT_DOCK_STATION)
        { }
        public Location(double latitude, double longitude, double altitude)
            : this(latitude, longitude, altitude, NOT_DOCK_STATION)
        { }
        public Location(double latitude, double longitude, double altitude, double horizontalAccuracy, double verticalAccuracy, double speed, double course)
            : this(latitude, longitude, altitude, horizontalAccuracy, verticalAccuracy, speed, course, NOT_DOCK_STATION)
        { }

        protected Location(int id)
            : base()
        { DockStationId = id; }
        protected Location(double latitude, double longitude, int id)
            : base(latitude, longitude)
        { DockStationId = id; }
        protected Location(double latitude, double longitude, double altitude, int id)
            : base(latitude, longitude, altitude)
        { DockStationId = id; }
        protected Location(double latitude, double longitude, double altitude, double horizontalAccuracy, double verticalAccuracy, double speed, double course, int id)
            : base(latitude, longitude, altitude, horizontalAccuracy, verticalAccuracy, speed, course)
        { DockStationId = id; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj.GetType() == GetType())
            {
                return Equals((Location)obj);
            }
            return base.Equals(obj);
        }
        public bool Equals(Location other)
        {
            if (other is null) return false;

            return other.IsDockStation && IsDockStation
                ? other.DockStationId == DockStationId
                : HasTheSame2DCoordinatesAs(other);
        }
        public bool HasTheSame2DCoordinatesAs(Location other)
        {
            return Longitude == other.Longitude
                && Latitude == other.Latitude;
        }
        public double GetDistanceTo(Location other)
        {
            return base.GetDistanceTo(other);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(Location left, Location right)
        {
            if (left is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Location left, Location right)
        {
            if (left is null) return true;
            return !left.Equals(right);
        }

        public bool IsDockStation { get { return DockStationId != NOT_DOCK_STATION; } }
        public int DockStationId { get; protected set; }
    }

    public class DockStationLocation : Location
    {
        public DockStationLocation(double latitude, double longitude, int id)
            : base(latitude, longitude, id)
        { }

        public DockStationLocation(double latitude, double longitude, double altitude, int id)
            : base(latitude, longitude, altitude, id)
        { }

        public DockStationLocation(double latitude, double longitude, double altitude, double horizontalAccuracy, double verticalAccuracy, double speed, double course, int id)
            : base(latitude, longitude, altitude, horizontalAccuracy, verticalAccuracy, speed, course, id)
        { }
    }
}
