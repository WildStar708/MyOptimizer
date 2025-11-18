using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOptimizer.Model.Algorithms
{
    public class GreyWolfOptimizer : Optimizer
    {
        private const int N_WOLVES = 50;
        private readonly Wolf[] Wolves = new Wolf[N_WOLVES];

        // Кращі вовки (Alpha, Beta, Delta)
        private Wolf Alpha;
        private Wolf Beta;
        private Wolf Delta;

        private readonly Random Rnd = new Random();

        public GreyWolfOptimizer(ITestFunction function, int maxIterations) : base(function, maxIterations)
        {
            // Ініціалізація вовків
            for (int i = 0; i < N_WOLVES; i++)
            {
                Wolves[i] = new Wolf
                {
                    // Випадкова позиція в межах [a, b]
                    Position = Rnd.NextDouble() * (Function.MaxX - Function.MinX) + Function.MinX
                };
                Wolves[i].Fitness = Function.Calculate(Wolves[i].Position);
            }
        }

        private void UpdateBestWolves()
        {
            // Сортуємо вовків за пристосованістю (значенням функції) для знаходження мінімуму
            var sorted = Wolves.OrderBy(w => w.Fitness).ToArray();
            Alpha = sorted[0];
            Beta = sorted[1];
            Delta = sorted[2];
        }

        public override OptimizationResultData RunOptimization()
        {
            var result = new OptimizationResultData();
            UpdateBestWolves();

            for (int t = 0; t < MaxIterations; t++)
            {
                // Обчислення коефіцієнта 'a' (лінійно зменшується від 2 до 0)
                double a = 2.0 - 2.0 * t / MaxIterations;

                for (int i = 0; i < N_WOLVES; i++)
                {
                    Wolf currentWolf = Wolves[i];
                    double newPosition = 0;

                    Wolf[] leaders = new Wolf[] { Alpha, Beta, Delta };

                    for (int j = 0; j < 3; j++)
                    {
                        Wolf leader = leaders[j];
                        double A = 2.0 * a * Rnd.NextDouble() - a;
                        double C = 2.0 * Rnd.NextDouble();

                        // Обчислення вектора D та X
                        double D = Math.Abs(C * leader.Position - currentWolf.Position);
                        double X = leader.Position - A * D;

                        newPosition += X / 3.0; // Усереднення впливу трьох лідерів
                    }

                    // Обмеження позиції діапазоном [a, b]
                    currentWolf.Position = Math.Max(Function.MinX, Math.Min(Function.MaxX, newPosition));
                    currentWolf.Fitness = Function.Calculate(currentWolf.Position);
                }

                UpdateBestWolves(); // Оновлюємо Alpha, Beta, Delta

                // Збереження історії для графіка
                result.History.Add(new IterationData { Iteration = t + 1, Fitness = Alpha.Fitness });
            }

            // Збереження фінального результату
            result.FinalX = Alpha.Position;
            result.FinalFx = Alpha.Fitness;

            return result;
        }
    }
}
