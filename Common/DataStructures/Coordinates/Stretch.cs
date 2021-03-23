using System;

namespace Common.DataStructures.Coordinates
{
    public class Stretch
    {
        public Location Start, End;
        public Stretch(Location start, Location end)
        {
            Start = start;
            End = end;
        }
        public static class RelativePosition
        {
            public const int LEFT = 1;
            public const int ON_LINE = 0;
            public const int RIGHT = -1;
        }

        public double Length { get { return Start.GetDistanceTo(End); } }

        public bool Contains(Location point)
        {
            if (OnTheSameLine(point))
                return (float)point.Latitude  >= (float)Math.Min(Start.Latitude, End.Latitude)
                    && (float)point.Latitude  <= (float)Math.Max(Start.Latitude, End.Latitude)
                    && (float)point.Longitude >= (float)Math.Min(Start.Longitude, End.Longitude)
                    && (float)point.Longitude <= (float)Math.Max(Start.Longitude, End.Longitude);
            
            return false;
        }

        public bool OnTheSameLine(Location point)
        {
            return Det(Start, End, point) == 0;
        }

        public bool OnTheSameLine(Stretch stretch)
        {
            return OnTheSameLine(stretch.Start)
                && OnTheSameLine(stretch.End);
        }

        private static double Det(Location x, Location y, Location z)
        {
            var det = (float)(x.Latitude * y.Longitude +
                   y.Latitude * z.Longitude +
                   z.Latitude * x.Longitude -
                   z.Latitude * y.Longitude -
                   x.Latitude * z.Longitude -
                   y.Latitude * x.Longitude);
            det = (float)(((float)(int)(det * 10e5)) / 10e5);
            return det;
        }

        public bool ContainsBothEndsOf(Stretch stretch)
        {
            return Contains(stretch.Start)
                && Contains(stretch.End);
        }

        public bool ContainsAtLeastOneEndOf(Stretch stretch)
        {
            return Contains(stretch.Start)
                || Contains(stretch.End);
        }

        public int GetRelativePosition(Location point)
        {
            return Math.Sign(Det(Start, End, point));
        }

        public bool HasOnTheSameSide(Location p1, Location p2)
        {
            return GetRelativePosition(p1)
                == GetRelativePosition(p2);
        }

        public void MoveTo(Location newStartPoint)
        {
            var height = End.Longitude - Start.Longitude;
            var width = End.Latitude - Start.Latitude;

            Start.Latitude = newStartPoint.Latitude;
            Start.Longitude = newStartPoint.Longitude;
            End.Latitude = Start.Latitude + width;
            End.Longitude = Start.Longitude + height;
        }

        public bool Crossing(Stretch stretch)
        {
            return !HasOnTheSameSide(stretch.Start, stretch.End) && !stretch.HasOnTheSameSide(Start, End);
        }

        public Location DirectionIntersectionWith(Stretch stretch)
        {
            double dx12 = End.Longitude - Start.Longitude;
            double dy12 = End.Latitude - Start.Latitude;
            double dx34 = stretch.End.Longitude - stretch.Start.Longitude;
            double dy34 = stretch.End.Latitude - stretch.Start.Latitude;

            double t1 = ((Start.Longitude - stretch.Start.Longitude) * dy34 + (stretch.Start.Latitude - Start.Latitude) * dx34)
                      / (dy12 * dx34 - dx12 * dy34);

            if (double.IsInfinity(t1))
                return null;
            return new Location(Start.Latitude + dy12 * t1, Start.Longitude + dx12 * t1);
        }

        public Stretch (Location start, Vector vector)
        {
            Start = start;
            End = new Location(start.Latitude + vector.X, start.Longitude + vector.Y);
        }

        public static Stretch operator -(Stretch stretch)
        {
            return new Stretch(stretch.End, stretch.Start);
        }
    }
}
