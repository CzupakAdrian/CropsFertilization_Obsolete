using Common;

namespace RoutePlanning
{
    public class SimpleSplitRoutePlannerBuilder : IBuilder<IMultipleRoutesPlanner>
    {
        double maxDistanceInMeters;

        public SimpleSplitRoutePlannerBuilder(double maxDistanceInMeters)
        {
            this.maxDistanceInMeters = maxDistanceInMeters;
        }

        public IMultipleRoutesPlanner Build()
        {
            return new SimpleSplitRoutePlanner(maxDistanceInMeters);
        }
    }
}
