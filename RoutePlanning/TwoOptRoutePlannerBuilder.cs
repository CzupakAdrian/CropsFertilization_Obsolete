using Common;

namespace RoutePlanning
{
    public class TwoOptRoutePlannerBuilder : IBuilder<ISingleRoutePlanner>
    {
        private int Improvements;

        public TwoOptRoutePlannerBuilder(int improvements)
        {
            Improvements = improvements;
        }

        public ISingleRoutePlanner Build()
        {
            return new TwoOptRoutePlanner(Improvements);
        }
    }
}
