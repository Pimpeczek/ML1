using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML1
{
    public class Population
    {
        public bool[][] Individuals { get; protected set; }
        int itemCount;
        public int CrossoverRate { get; set; }
        protected Func<int, Task, int> evaluatingMethod;
        double mutationRate;
        int mutationCount;
        public int BaseRouletteChance = 1;
        public int TournamentSize = 1;
        public double MutationRate
        {
            get
            {
                return mutationRate;
            }
            set
            {
                mutationRate = value;
                mutationCount = (int)(itemCount * mutationRate);
            }
        }


        public Population(int itemCount, int populationSize)
        {
            this.itemCount = itemCount;
            Individuals = new bool[populationSize][];
            for (int i = populationSize - 1; i >= 0; i--)
            {
                Individuals[i] = GetRandomIndividual(itemCount);
            }
            evaluatingMethod = Evaluate1;
        }

        bool[] GetRandomIndividual(int itemCount)
        {
            bool[] arr = new bool[itemCount];

            for (int i = itemCount - 1; i >= 0; i--)
            {
                arr[i] = Misc.rng.Next(2) == 0;
            }

            return arr;
        }

        protected (int, int) GetSizeAndWeight(int individualID, Task task)
        {
            if (task == null)
                throw new ArgumentNullException("Task");

            bool[] individual = Individuals[individualID];

            if (individual.Length != task.ItemCount)
                throw new ArgumentException("task.ItemCount and individual.Length");

            int totalSize = 0;
            int totalWeight = 0;

            for (int i = itemCount - 1; i >= 0; i--)
            {
                if (individual[i])
                {
                    totalSize += task.Items[i, 0];
                    totalWeight += task.Items[i, 1];
                }
            }
            return (totalSize, totalWeight);
        }

        /// <summary>
        /// From the task.
        /// </summary>
        public int Evaluate1(int individualID, Task task)
        {
            (int, int) data = GetSizeAndWeight(individualID, task);

            if (data.Item1 > task.MaxSize || data.Item2 > task.MaxWeight)
                return 0;

            return data.Item1 + data.Item2;
        }

        /// <summary>
        /// My proposition.
        /// </summary>
        public int Evaluate2(int individualID, Task task)
        {
            (int, int) data = GetSizeAndWeight(individualID, task);

            return Math.Abs(data.Item1 - task.MaxSize) + Math.Abs(data.Item2 - task.MaxWeight);
        }

        public bool[] Crossover(int parent1, int parent2)
        {
            if (parent1 >= Individuals.Length)
                throw new ArgumentOutOfRangeException("parent1");

            if (parent1 >= Individuals.Length)
                throw new ArgumentOutOfRangeException("parent2");

            bool[] child = new bool[itemCount];
            int crossoverPoint;
            if (Misc.rng.NextDouble() > CrossoverRate)
            {
                if (Misc.rng.Next(2) == 0)
                {
                    crossoverPoint = 0;
                }
                else
                {
                    crossoverPoint = itemCount;
                }
            }
            else
            {
                crossoverPoint = Misc.rng.Next(1, itemCount);
            }

            bool[] parent = Individuals[parent1];
            for (int i = 0; i < crossoverPoint && i < itemCount; i++)
            {
                child[i] = parent[i];
            }

            parent = Individuals[parent2];
            for (int i = crossoverPoint; i < itemCount; i++)
            {
                child[i] = parent[i];
            }

            return child;
        }

        public bool[] Mutate(bool[] individual)
        {
            if (individual == null)
                throw new ArgumentNullException("individual");

            int id;
            for (int i = 0; i < mutationCount; i++)
            {
                id = Misc.rng.Next(itemCount);
                individual[id] = !individual[id];
            }

            return individual;
        }

        public bool[] MutateTrue(bool[] individual)
        {
            if (individual == null)
                throw new ArgumentNullException("individual");

            int[] mutations = Misc.GetRandomUniqueIntegers(mutationCount, itemCount);
            for (int i = 0; i < mutationCount; i++)
            {

                individual[mutations[i]] = !individual[mutations[i]];
            }

            return individual;
        }

        protected int SingleTournament(int size, Task task)
        {
            int[] contestants = Misc.GetRandomUniqueIntegers(size, Individuals.Length);
            int bestFitness = 0;
            int bestId = 0;
            int tempFitness;
            for(int i = size - 1; i >= 0; i--)
            {
                if((tempFitness = evaluatingMethod.Invoke(contestants[i], task)) > bestFitness)
                {
                    bestFitness = tempFitness;
                    bestId = contestants[i];
                }
            }
            return bestId;
        }

        public void Tournament(Task task)
        {

        }

        public void Roulette(Task task)
        {
            int[] fitnesses = new int[Individuals.Length];
            fitnesses[Individuals.Length - 1] = evaluatingMethod.Invoke(Individuals.Length - 1, task);
            
            for (int i = Individuals.Length - 2; i >= 0; i--)
            {
                fitnesses[i] = fitnesses[i+ 1] + evaluatingMethod.Invoke(i, task) + BaseRouletteChance;
            }

            int parent1;
            int parent2;
            int parent1Point;
            int parent2Point;

            bool[][] newPopulation = new bool[Individuals.Length][];

            for (int i = Individuals.Length - 1; i >= 0; i--)
            {
                parent1 = 0;
                parent2 = 0;
                parent1Point = Misc.rng.Next(Individuals.Length);

                while((parent2Point = Misc.rng.Next(Individuals.Length)) == parent1Point) { }

                for(int j = Individuals.Length - 1; j >= 0 && (parent1 < 0 || parent2 < 0); j--)
                {
                    if (parent1Point < fitnesses[j])
                        parent1 = j;
                    if (parent2Point < fitnesses[j])
                        parent2 = j;
                }
                newPopulation[i] = Mutate(Crossover(parent1, parent2));
            }

        }

    }
}
