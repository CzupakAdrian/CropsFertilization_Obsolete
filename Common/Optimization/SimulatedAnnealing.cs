using System;

namespace Common.Optimization
{
    public abstract class SimulatedAnnealing<State, ProblemData>
    {
        private const double MINIMAL_TEMPERATURE = 0.0001;
        private const int INITIAL_ITERATION = 0;
        private const double PERCENT_100 = 1.0;

        private int maxNumOfIterations;
        private double initialTemperature;
        private double alpha;
        private Random random;

        private State bestState;
        private double bestEnergy;

        public SimulatedAnnealing(int maxNumOfIterations, double initialTemperature, double alpha)
        {
            random = new Random(0);
            this.maxNumOfIterations = maxNumOfIterations;
            this.initialTemperature = initialTemperature;
            this.alpha = alpha;
        }
        public void Optimize(ProblemData problemData)
        {
            try
            {
                State state = RandomState(problemData);
                double energy = Energy(state, problemData);
                bestState = state;
                bestEnergy = energy;
                State adjState;
                double adjEnergy;
                int iteration = INITIAL_ITERATION;
                double currTemp = initialTemperature;
                while (iteration < maxNumOfIterations && currTemp > MINIMAL_TEMPERATURE)
                {
                    adjState = AdjacentState(state, problemData);
                    adjEnergy = Energy(adjState, problemData);
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
                    ++iteration;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
        public State GetOptimalState()
        {
            return bestState;
        }

        protected abstract State RandomState(ProblemData problemData);
        protected abstract State AdjacentState(State currState, ProblemData problemData);
        protected abstract double Energy(State state, ProblemData problemData);
        private double AcceptanceProb(double energy, double adjEnergy, double currTemp)
        {
            if (adjEnergy < energy)
                return PERCENT_100;
            else
                return Math.Exp((energy - adjEnergy) / currTemp);
        }
    }
}
