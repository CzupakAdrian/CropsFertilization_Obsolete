using System;
using Common.DataStructures.Coordinates;
using Common.Debugging;

namespace Common.Optimization
{
    public abstract class SimulatedAnnealing
    {
        private const double MINIMAL_TEMPERATURE = 0.0001;
        private const int INITIAL_ITERATION = 0;
        private const double PERCENT_100 = 1.0;

        private int maxNumOfIterations;
        private double initialTemperature;
        private double alpha;
        protected Random random;

        public SimulatedAnnealing(int maxNumOfIterations, double initialTemperature, double alpha)
        {
            random = new Random(0);
            this.maxNumOfIterations = maxNumOfIterations;
            this.initialTemperature = initialTemperature;
            this.alpha = alpha;
        }

        public Route Optimize(PointsSetBase problem)
        {
            try
            {
                Route state = GenerateInitialState(problem);
                double energy = CalculateEnergy(state, problem);
                Route bestState = state;
                double bestEnergy = energy;
                int iteration = INITIAL_ITERATION;
                double currTemp = initialTemperature;
                while (!TerminationConditionFulfilled(iteration, currTemp))
                {
                    Route adjState = GenerateAdjacentState(state, problem);
                    double adjEnergy = CalculateEnergy(adjState, problem);
                    if (adjEnergy < bestEnergy)
                    {
                        bestState = adjState;
                        bestEnergy = adjEnergy;
                    }
                    if (AcceptanceProb(energy, adjEnergy, currTemp) > random.NextDouble())
                    {
                        state = adjState;
                        energy = adjEnergy;
                    }
                    currTemp *= alpha;
                    iteration++;
                }
                return bestState;
            }
            catch (Exception ex)
            {
                Logger.PrintSysLog(ex.Message);
                return default;
            }
        }

        protected abstract Route GenerateInitialState(PointsSetBase problem);
        protected abstract Route GenerateAdjacentState(Route currState, PointsSetBase problem);
        protected abstract double CalculateEnergy(Route state, PointsSetBase problem);

        private bool TerminationConditionFulfilled(int iteration, double currTemp)
        {
            return iteration >= maxNumOfIterations || currTemp <= MINIMAL_TEMPERATURE;
        }
        private double AcceptanceProb(double energy, double adjEnergy, double currTemp)
        {
            if (adjEnergy < energy)
                return PERCENT_100;
            else
                return Math.Exp((energy - adjEnergy) / currTemp);
        }
    }
}
