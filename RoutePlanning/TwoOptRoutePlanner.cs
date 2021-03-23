using Common.DataStructures.Coordinates;

namespace RoutePlanning
{
    public class TwoOptRoutePlanner : ISingleRoutePlanner
    {
        private Route Route;
        private Route NewRoute;
        private int PointsCount;
        private int Improvements;

        public TwoOptRoutePlanner(int improvements)
        {
            Improvements = improvements;
        }

        public Route PlanRoute(PointsSetBase pointsSet, DockStationLocation baseStation)
        {
            Route = new Route(pointsSet);

            if (pointsSet[0] != baseStation) Route.Insert(0, baseStation);
            if (pointsSet[pointsSet.PointsCount - 1] != baseStation) Route.Add(baseStation);

            NewRoute = new Route(Route);

            PointsCount = Route.PointsCount;

            var improve = 0;
            var iteration = 0;
            double bestDistance;

            while (improve < Improvements)
            {
                bestDistance = Route.Distance;

                for (var i = 1; i < PointsCount - 2; i++)
                {
                    for (var k = i + 1; k < PointsCount - 1; k++, iteration++)
                    {
                        Swap(i, k);
                        var newDistance = NewRoute.Distance;
                        if (!(newDistance < bestDistance))
                            continue;

                        improve = 0;
                        for (var j = 0; j < PointsCount; j++)
                            Route[j] = NewRoute[j];

                        bestDistance = newDistance;
                    }
                }
                improve++;
            }
            return Route;
        }

        private void Swap(int i, int k)
        {
            for (var m = 0; m < i; m++)
                NewRoute[m] = Route[m];

            for (int m = i, dec = 0; m <= k; m++, dec++)
                NewRoute[m] = Route[k - dec];

            for (var m = k + 1; m < PointsCount; m++)
                NewRoute[m] = Route[m];
        }
    }
}
