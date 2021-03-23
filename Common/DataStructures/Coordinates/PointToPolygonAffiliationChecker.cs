namespace Common.DataStructures.Coordinates
{
    public class PointToPolygonAffiliationChecker
    {
        private Stretch ReferenceStretch;
        private readonly Polygon Polygon;
        private readonly int NumOfPoints;

        public static bool IsPointInPolygon(Location point, Polygon polygon)
        {
            var instance = new PointToPolygonAffiliationChecker(point, polygon);
            return instance.IsPointInPolygonImpl();
        }
        private bool IsPointInPolygonImpl()
        {
            int numberOfCrossings = 0;
            for (int i = 0; i < NumOfPoints; i++)
            {
                var stretch = new Stretch(Polygon.Points[i], Polygon.Points[(i + 1) % NumOfPoints]);
                if (stretch.Contains(ReferenceStretch.Start))
                    return true;
                if (ReferenceStretch.Crossing(stretch))
                    numberOfCrossings++;
            }
            return IsOdd(numberOfCrossings);
        }

        private PointToPolygonAffiliationChecker(Location basePoint, Polygon polygon)
        {
            Polygon = polygon;
            NumOfPoints = polygon.PointsCount;
            ReferenceStretch = new Stretch(basePoint, new Location(GetMaxX(Polygon) + 1, basePoint.Latitude));
        }

        private static double GetMaxX(Polygon polygon)
        {
            double currentMax = polygon.Points[0].Longitude;
            foreach (var c in polygon.Points)
                if (c.Longitude > currentMax)
                    currentMax = c.Longitude;
            return currentMax;
        }

        private static bool IsOdd(int x) { return x % 2 == 1; }

    }
}
