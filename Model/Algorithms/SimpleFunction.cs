using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOptimizer.Model.Algorithms
{
    public class SimpleFunction : ITestFunction
    {
        // Діапазон x
        public double MinX => -10.0;
        public double MaxX => 10.0;

        // F(x) = x^2 (шукаємо мінімум)
        public double Calculate(double x)
        {
            return x * x;
        }
    }
}
