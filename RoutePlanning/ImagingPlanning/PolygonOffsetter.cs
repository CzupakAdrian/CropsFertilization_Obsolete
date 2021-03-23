using Common.DataStructures.Coordinates;
using System;

namespace RoutePlanning.ImagingPlanning
{
    public partial class PolygonOffsetter
    {
        double Offset;
        bool IsOrientedClockwise;
        public PolygonOffsetter(double offsetInMeters)
        {
            Offset = offsetInMeters;
        }

        public Polygon GetEdited(Polygon polygon)
        {
            return GetEdited(polygon, Offset);
        }

        private Polygon GetEdited(Polygon polygon, double offset)
        {
            Polygon newPolygon = new Polygon();
            IsOrientedClockwise = polygon.IsOrientedClockwise();
            int pointsCount = polygon.PointsCount;
            for (int j = 0; j < pointsCount; j++)
            {
                int i = (j - 1);
                int k = (j + 1) % pointsCount;
                if (i < 0)
                    i += pointsCount;
               
                var stretch1 = MoveLinePerperdicularyByOffset(new Stretch(polygon[i], polygon[j]), offset);
                var stretch2 = MoveLinePerperdicularyByOffset(new Stretch(polygon[j], polygon[k]), offset);

                newPolygon.Add(stretch1.DirectionIntersectionWith(stretch2));
            }

            return newPolygon;
        }

        private Stretch MoveLinePerperdicularyByOffset(Stretch stretch, double offset)
        {
            Vector vector = new Vector(stretch.Start.Longitude - stretch.End.Longitude,
                                       stretch.End.Latitude - stretch.Start.Latitude);

            vector.Normalize();
            if (IsOrientedClockwise) vector.Enlarge(-1);

            return new Stretch(MovePointByVectorInMeters(stretch.Start, vector, offset),
                               MovePointByVectorInMeters(stretch.End, vector, offset));
        }

        public static Location MovePointByVectorInMeters(Location point, Vector vector, double distance)
        { // tu można wykorzystać info o prawie liniowej charakterystyce
            const double NORMAL_PRECISION = 0.0000018;
            Location a = new Location(point.Latitude, point.Longitude);
            var direction = new Vector(vector);
            direction.Normalize();
            direction.Enlarge(2.0 * distance * 360.0 / 40075000.0);
            var b = new Location(a.Latitude + direction.X,
                                      a.Longitude + direction.Y);

            var fa = point.GetDistanceTo(a) - distance;

            var x0 = new Location();
            while (Math.Abs(a.GetDistanceTo(b)) > NORMAL_PRECISION / 10e8)
            {
                x0 = new Location((a.Latitude + b.Latitude) / 2,
                                       (a.Longitude + b.Longitude) / 2);

                var f0 = point.GetDistanceTo(x0) - distance;

                if (Math.Abs(f0) <= 0.2)
                    break;
                if (fa * f0 < 0)
                    b = x0;
                else
                {
                    a = x0;
                    fa = f0;
                }
            }
            return x0;
        }
    }
}
