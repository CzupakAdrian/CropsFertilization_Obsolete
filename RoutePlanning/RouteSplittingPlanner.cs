using Common.DataStructures.Coordinates;
using System.Collections.Generic;

namespace RoutePlanning
{
    public interface IMultipleRoutesPlanner
    {
        List<Route> PlanRoutes(PointsSetBase points, DockStationLocation baseStation);
    }
}
