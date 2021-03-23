using Common;
using Common.DataStructures.Coordinates;
using System.Collections.Generic;

namespace RoutePlanning
{
    public class SimpleRoutePlanner : IMultipleRoutesPlanner
    {
        IBuilder<ISingleRoutePlanner> PrimaryOptimizerBuilder;
        IBuilder<IMultipleRoutesPlanner> SplitterBuilder;
        IBuilder<ISingleRoutePlanner> SecondaryOptimizerBuilder;
        public SimpleRoutePlanner(IBuilder<ISingleRoutePlanner> primaryOptimizerBuilder,
                                  IBuilder<IMultipleRoutesPlanner> splitterBuilder,
                                  IBuilder<ISingleRoutePlanner> secondaryOptimizerBuilder)
        {
            PrimaryOptimizerBuilder = primaryOptimizerBuilder;
            SplitterBuilder = splitterBuilder;
            SecondaryOptimizerBuilder = secondaryOptimizerBuilder;
        }

        public List<Route> PlanRoutes(PointsSetBase points, DockStationLocation baseStation)
        {
            List<Route> subroutes = GeneratePrimaryRoutesList(points, baseStation);

            var secondaryOptimizer = SecondaryOptimizerBuilder.Build();
            for (var i = 0; i < subroutes.Count; i++)
                subroutes[i] = secondaryOptimizer.PlanRoute(subroutes[i], baseStation);

            return subroutes;
        }

        private List<Route> GeneratePrimaryRoutesList(PointsSetBase points, DockStationLocation baseStation)
        {
            var splitter = SplitterBuilder.Build();
            Route primaryRoute = GeneratePrimaryRoute(points, baseStation);
            return splitter.PlanRoutes(primaryRoute, baseStation);
        }

        private Route GeneratePrimaryRoute(PointsSetBase points, DockStationLocation baseStation)
        {
            var primaryOptimizer = PrimaryOptimizerBuilder.Build();
            return primaryOptimizer.PlanRoute(points, baseStation);
        }
    }
}
