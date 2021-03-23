using Common;

namespace RoutePlanning
{
    public class SimpleRoutePlannerBuilder : IBuilder<IMultipleRoutesPlanner>
    {
        private const int DEFAULT_IMPROVEMENTS_FACTOR = 10;

        double maxDistanceInMeters;
        int primaryImprovementsNum;
        int secondaryImprovementsNum;

        public SimpleRoutePlannerBuilder(double maxDistanceInMeters,
                                         int primaryImprovementsNum,
                                         int secondaryImprovementsNum)
        {
            this.maxDistanceInMeters = maxDistanceInMeters;
            this.primaryImprovementsNum = primaryImprovementsNum;
            this.secondaryImprovementsNum = secondaryImprovementsNum;
        }

        public SimpleRoutePlannerBuilder(double maxDistanceInMeters,
                                         int primaryImprovementsNum)
            : this(maxDistanceInMeters, primaryImprovementsNum, primaryImprovementsNum / DEFAULT_IMPROVEMENTS_FACTOR)
        { }

        public IMultipleRoutesPlanner Build()
        {
            return new SimpleRoutePlanner(
                new TwoOptRoutePlannerBuilder(primaryImprovementsNum),
                new SimpleSplitRoutePlannerBuilder(maxDistanceInMeters),
                new TwoOptRoutePlannerBuilder(secondaryImprovementsNum));
        }
    }
}
