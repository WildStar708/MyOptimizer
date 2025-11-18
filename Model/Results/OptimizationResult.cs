using MyOptimizer.Model.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOptimizer.Model.Results
{
    public class OptimizationResult : ViewModel.Base.ObservableObject // Успадковуємо для сповіщення UI
    {
        private string _algorithmName = "N/A";
        private double _optimalX = 0.0;
        private double _optimalFx = 0.0;
        private List<IterationData> _history = new List<IterationData>();
        private string _status = "Ready";

        // Ім'я алгоритму, що виконувався
        public string AlgorithmName
        {
            get => _algorithmName;
            set { _algorithmName = value; OnPropertyChanged(); }
        }

        // Оптимальне значення змінної x, знайдене алгоритмом
        public double OptimalX
        {
            get => _optimalX;
            set { _optimalX = value; OnPropertyChanged(); }
        }

        // Оптимальне значення цільової функції F(x)
        public double OptimalFx
        {
            get => _optimalFx;
            set { _optimalFx = value; OnPropertyChanged(); }
        }

        // Історія прогресу для побудови графіків
        public List<IterationData> History
        {
            get => _history;
            set { _history = value; OnPropertyChanged(); }
        }

        // Поточний статус програми (Running, Finished, Ready)
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        // Метод для оновлення всіх полів з об'єкта результату алгоритму
        public void UpdateFromAlgorithmData(OptimizationResultData data, string algorithmName)
        {
            AlgorithmName = algorithmName;
            OptimalX = data.FinalX;
            OptimalFx = data.FinalFx;
            History = data.History;
            Status = $"{algorithmName} finished successfully.";
        }
    }

}
