using System;
using Common.DataStructures.Coordinates;
using System.Collections.Generic;

namespace RoutePlanning
{
    public class SimpleSplitRoutePlanner : IMultipleRoutesPlanner
    {
        double MaxDistance;
        public SimpleSplitRoutePlanner(double maxDistanceInMeters)
        {
            MaxDistance = maxDistanceInMeters;
        }

        public List<Route> PlanRoutes(PointsSetBase inputPoints, DockStationLocation baseStation)
        {
            List<Route> subRoutes = new List<Route>();

            {
                Route route = new Route(inputPoints);

                if (baseStation != inputPoints[0])
                    route.Insert(0, baseStation);

                if (baseStation != inputPoints[inputPoints.PointsCount - 1])
                    route.Add(baseStation);

                if (route.Distance < MaxDistance)
                {
                    subRoutes.Add(route);
                    return subRoutes;
                }
            }
            
            int lastComebackIndex = 1;
            InitRoute(baseStation, subRoutes);

            for (int i = 1; i < inputPoints.PointsCount - 1; i++)
            {
                var currentRoute = subRoutes[subRoutes.Count - 1];
                currentRoute.Insert(currentRoute.PointsCount - 1, inputPoints[i]);
                if (currentRoute.Distance >= MaxDistance)
                {
                    if (lastComebackIndex == i - 1)
                    {
                        throw new Exception("At least one point out of range");
                    }
                    currentRoute.RemoveAt(currentRoute.PointsCount - 2);
                    i--;
                    InitRoute(baseStation, subRoutes);
                    
                    lastComebackIndex = i;
                }
            }

            return subRoutes;
        }

        private static void InitRoute(DockStationLocation baseStation, List<Route> subRoutes)
        {
            var newRoute = new Route();
            newRoute.Add(baseStation);
            newRoute.Add(baseStation);
            subRoutes.Add(newRoute);
        }
    }
}
