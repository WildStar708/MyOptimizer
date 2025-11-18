using LiveCharts;
using LiveCharts.Wpf;
using MyOptimizer.Model.Algorithms;
using MyOptimizer.Model.Results;
using MyOptimizer.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyOptimizer.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        // Приклад тестової функції
        private readonly ITestFunction _function = new SimpleFunction();

        // --- Властивості, прив'язані до UI ---

        private int _maxIterations = 50;
        public int MaxIterations
        {
            get => _maxIterations;
            set => SetProperty(ref _maxIterations, value);
        }

        private OptimizationResult? _result;
        public OptimizationResult Result
        {
            get => _result!;
            // Присвоєння відбувається лише в конструкторі, щоб уникнути NRE
            set => SetProperty(ref _result, value);
        }

        private SeriesCollection? _historySeries;
        public SeriesCollection HistorySeries
        {
            get => _historySeries!;
            set => SetProperty(ref _historySeries, value);
        }

        private string[]? _labels;
        public string[] Labels
        {
            get => _labels!;
            set => SetProperty(ref _labels, value);
        }

        // --- Команди ---

        public ICommand RunGACommand { get; }
        public ICommand RunGWOCommand { get; }

        public MainViewModel()
        {
            // Ініціалізація моделей та команд
            Result = new OptimizationResult();
            HistorySeries = new SeriesCollection();

            RunGACommand = new RelayCommand(async _ => await RunOptimizationAsync(new GeneticAlgorithm(_function, MaxIterations), "Genetic Algorithm"));
            RunGWOCommand = new RelayCommand(async _ => await RunOptimizationAsync(new GreyWolfOptimizer(_function, MaxIterations), "Grey Wolf Optimizer"));
        }

        // --- Метод для запуску оптимізації ---

        private async Task RunOptimizationAsync(Optimizer algorithm, string name)
        {
            Result.Status = $"Running {name}...";

            OptimizationResultData data = null;

            // Виконання алгоритму в окремому потоці, щоб не блокувати UI
            await Task.Run(() =>
            {
                data = algorithm.RunOptimization();
            });

            // Оновлення результатів та графіків
            if (data != null)
            {
                Result.UpdateFromAlgorithmData(data, name);
                UpdateChart(data.History, name);
            }
        }

        // --- Логіка оновлення графіків LiveCharts ---

        private void UpdateChart(List<IterationData> history, string algorithmName)
        {
            // Дані для осі Y (значення F(x))
            var fitnessValues = history.Select(h => h.Fitness).ToArray();

            // Дані для осі X (номери ітерацій)
            Labels = history.Select(h => h.Iteration.ToString()).ToArray();

            // Створення серії для графіка
            var series = new LineSeries
            {
                Title = algorithmName,
                Values = new ChartValues<double>(fitnessValues),
                PointGeometry = DefaultGeometries.None, // Чиста лінія
                StrokeThickness = 2
            };

            // Оновлення колекції серій для відображення
            HistorySeries.Clear();
            HistorySeries.Add(series);
        }
    }
}
