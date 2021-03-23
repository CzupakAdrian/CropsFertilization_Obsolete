using Common.DataStructures.Coordinates;

namespace RoutePlanning
{
    public interface ISingleRoutePlanner
    {
        Route PlanRoute(PointsSetBase points, DockStationLocation baseStation);
    }
}