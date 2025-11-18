using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts; 
using LiveCharts.Wpf;

namespace MyOptimizer.Model.Algorithms
{
    public class GeneticAlgorithm : Optimizer
    {
        private const int N_POPULATION = 100;
        private const int W_BITS = 16; // w - кількість бітів
        private const double MUTATION_RATE = 0.01;

        // ВИПРАВЛЕННЯ: Видалено 'readonly', щоб дозволити заміну покоління (масиву)
        private Chromosome[] Population = new Chromosome[N_POPULATION];

        private readonly Random Rnd = new Random();

        public GeneticAlgorithm(ITestFunction function, int maxIterations) : base(function, maxIterations)
        {
            // Ініціалізація популяції
            for (int i = 0; i < N_POPULATION; i++)
            {
                Population[i] = new Chromosome();
                // Випадковий двійковий код (від 0 до 2^w - 1)
                Population[i].BinaryCode = Rnd.Next(1 << W_BITS);
                UpdateValueAndFitness(Population[i]);
            }
        }

        // Функція переведення двійкового коду в дійсне число x та обчислення F(x)
        private void UpdateValueAndFitness(Chromosome c)
        {
            // 2^w - 1
            double maxDec = (1 << W_BITS) - 1;
            double a = Function.MinX;
            double b = Function.MaxX;

            // Формула: x = Dec(code) / (2^w - 1) * (b - a) + a
            c.RealValue = (double)c.BinaryCode / maxDec * (b - a) + a;
            c.Fitness = Function.Calculate(c.RealValue);
        }

        // Оператор Схрещування (одноточкове)
        private Chromosome Crossover(Chromosome parent1, Chromosome parent2)
        {
            int crossoverPoint = Rnd.Next(1, W_BITS);
            int mask = (1 << crossoverPoint) - 1;

            int childCode = (parent1.BinaryCode & mask) | (parent2.BinaryCode & (~mask));

            return new Chromosome { BinaryCode = childCode };
        }

        // Оператор Мутації (бітовий фліп)
        private void Mutate(Chromosome c)
        {
            for (int i = 0; i < W_BITS; i++)
            {
                if (Rnd.NextDouble() < MUTATION_RATE)
                {
                    c.BinaryCode ^= (1 << i); // XOR для фліпу біта
                }
            }
        }

        // Оператор Відбору (Відбір турніром)
        private Chromosome TournamentSelection(int tournamentSize = 5)
        {
            Chromosome best = null;
            for (int i = 0; i < tournamentSize; i++)
            {
                Chromosome current = Population[Rnd.Next(N_POPULATION)];
                // Шукаємо мінімум (менша пристосованість - краще)
                if (best == null || current.Fitness < best.Fitness)
                {
                    best = current;
                }
            }
            return best;
        }

        public override OptimizationResultData RunOptimization()
        {
            var result = new OptimizationResultData();

            for (int t = 0; t < MaxIterations; t++)
            {
                Chromosome[] newPopulation = new Chromosome[N_POPULATION];

                // Елітарна стратегія: найкраща особина переходить без змін
                var bestChromosome = Population.OrderBy(c => c.Fitness).First();
                newPopulation[0] = bestChromosome;

                for (int i = 1; i < N_POPULATION; i++)
                {
                    // Відбір, Схрещування, Мутація
                    Chromosome parent1 = TournamentSelection();
                    Chromosome parent2 = TournamentSelection();
                    Chromosome child = Crossover(parent1, parent2);
                    Mutate(child);

                    // Оцінка пристосованості та значення
                    UpdateValueAndFitness(child);

                    newPopulation[i] = child;
                }

                // ЗАМІНА ПОКОЛІННЯ (тепер дозволено)
                Population = newPopulation;

                // Оновлення найкращої особини після генерації нового покоління
                bestChromosome = Population.OrderBy(c => c.Fitness).First();

                // Збереження історії для графіка
                result.History.Add(new IterationData { Iteration = t + 1, Fitness = bestChromosome.Fitness });
            }

            var finalBest = Population.OrderBy(c => c.Fitness).First();

            // Збереження фінального результату
            result.FinalX = finalBest.RealValue;
            result.FinalFx = finalBest.Fitness;

            return result;
        }
    }
}
