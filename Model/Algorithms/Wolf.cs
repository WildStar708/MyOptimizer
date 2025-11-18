using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOptimizer.Model.Algorithms
{
    public class Wolf
    {
        public double Position { get; set; } // Позиція вовка (x)
        public double Fitness { get; set; }  // Пристосованість (значення функції F(x))
    }
}
