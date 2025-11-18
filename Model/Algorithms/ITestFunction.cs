using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOptimizer.Model.Algorithms
{
    public interface ITestFunction
    {
        // Діапазон визначення змінної x (a)
        double MinX { get; }
        // Діапазон визначення змінної x (b)
        double MaxX { get; }

        // Обчислення значення функції для даного x (оцінка пристосованості)
        double Calculate(double x);
    }
}
