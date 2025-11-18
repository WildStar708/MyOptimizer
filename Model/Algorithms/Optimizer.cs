using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOptimizer.Model.Algorithms
{
    public class IterationData
    {
        public int Iteration { get; set; }
        public double Fitness { get; set; }
    }

    // Клас для повернення повного результату алгоритму
    public class OptimizationResultData
    {
        public double FinalX { get; set; }
        public double FinalFx { get; set; }
        public List<IterationData> History { get; set; } = new List<IterationData>();
    }

    // Абстрактний базовий клас для всіх алгоритмів
    public abstract class Optimizer
    {
        protected ITestFunction Function;
        protected int MaxIterations;

        public Optimizer(ITestFunction function, int maxIterations)
        {
            Function = function;
            MaxIterations = maxIterations;
        }

        // Абстрактний метод для запуску алгоритму, повертає повний результат
        public abstract OptimizationResultData RunOptimization();
    }
}
