using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOptimizer.Model.Algorithms
{
    public class Chromosome
    {
        public int BinaryCode { get; set; } // Двійковий код (хромосома)
        public double RealValue { get; set; } // Дійсне число x
        public double Fitness { get; set; }  // Пристосованість F(x)
    }
}
