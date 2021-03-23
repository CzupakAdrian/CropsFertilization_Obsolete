using System;
using System.Collections.Generic;

namespace Common.DataStructures.Coordinates
{
    public class Polygon : PointsSetBase
    {
        public Polygon() : base() { }
        public Polygon(Polygon route) : base(route.Points)
        { }

        public Polygon(PointsSetBase route) : base(route.Points)
        { }

        public Polygon(List<Location> route) : base(route)
        { }

        public bool IsOrientedClockwise()
        {
            return SignedPolygonArea() >= 0;
        }

        /*****************
         * Not a real area in meters
         * just for computation
         */
        public double PolygonArea()
        {
            return Math.Abs(SignedPolygonArea());
        }

        private double SignedPolygonArea()
        {
            int num_points = Points.Count;
            Location[] pts = new Location[num_points + 1];
            Points.CopyTo(pts, 0);
            pts[num_points] = Points[0];

            // Get the areas.
            double area = 0;
            for (int i = 0; i < num_points; i++)
            {
                area +=
                    (pts[i + 1].Latitude - pts[i].Latitude) *
                    (pts[i + 1].Longitude + pts[i].Longitude) / 2;
            }

            return area;
        }
    }
}