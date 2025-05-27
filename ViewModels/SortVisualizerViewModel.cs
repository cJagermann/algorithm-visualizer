using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AlgorithmVisualizer.Core;
using AlgorithmVisualizer.Core.Sort;
using AlgorithmVisualizer.Models;
using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;

namespace AlgorithmVisualizer.ViewModels;

public enum Algorithm {
    Bubble, Insertion, Shell, Merge, Quick, Selection
}

public class SortVisualizerViewModel : ViewModelBase, ISortVisualizer{
    public ICommand StartCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand StopCommand { get; }
    
    [Reactive] public ISeries[] Series { get; private set; }
    [Reactive] public int Complexity { get; private set; }
    [Reactive] public Algorithm SelectedAlgorithm { get; set; }
    [Reactive] public bool IsPaused { get; set; }
    
    public static string[] SortingAlgorithms { get; } = Enum.GetNames<Algorithm>();
    
    
    private SortingAlgorithm _sortingAlgorithm;
    private PauseTokenSource SortThreadPauseToken { get; set; }
    private CancellationTokenSource SortThreadCancelToken { get; set; }
    private Task? _task;
    
    private readonly Dictionary<Algorithm, Func<SortingAlgorithm>> _sortingHat = new() {
        {Algorithm.Bubble, () => new BubbleSort()},
        {Algorithm.Insertion, () => new InsertionSort()},
        // {Algorithm.Merge, () => new MergeSort()},
        {Algorithm.Quick, () => new QuickSort()},
        // {Algorithm.Selection, () => new SelectionSort()},
        {Algorithm.Shell, () => new ShellSort()},
    };

    public SortVisualizerViewModel() {
        StartCommand = ReactiveCommand.Create<SortVisualizerViewModel>(Start);
        PauseCommand = ReactiveCommand.Create<SortVisualizerViewModel>(Pause);
        StopCommand = ReactiveCommand.Create<SortVisualizerViewModel>(Stop);

        SortThreadPauseToken = new PauseTokenSource();
        SortThreadCancelToken = new CancellationTokenSource();
        
        SelectedAlgorithm = Algorithm.Bubble;
        _sortingAlgorithm = new BubbleSort();

        var columnSeries = new ColumnSeries<int>() {
            Values = _sortingAlgorithm.Values
        }.OnPointMeasured(point => {
            if (point.Visual is null) return;
            
            point.Visual.Fill = new SolidColorPaint(SKColors.CadetBlue);
        });
        
        Series = [
            columnSeries
        ];
    }

    private async Task SetValues_Async() {
        try {
            SortThreadCancelToken = new CancellationTokenSource();
            
            _sortingAlgorithm = _sortingHat[SelectedAlgorithm].Invoke();
        
            var paints = new SolidColorPaint[] {
                new(SKColors.CadetBlue),
                new(SKColors.SeaGreen),
                new (SKColors.YellowGreen),
                new (SKColors.OrangeRed),
                new (SKColors.IndianRed),
                new (SKColors.Violet),
                new (SKColors.MediumPurple),
                new (SKColors.RoyalBlue),
                new (SKColors.MediumSeaGreen),
                new (SKColors.GreenYellow),
                new (SKColors.Orange),
                new (SKColors.Red),
                new (SKColors.MediumVioletRed),
                new (SKColors.Purple),
                new (SKColors.SteelBlue),
                new (SKColors.PaleGreen),
                new (SKColors.Yellow),
                new (SKColors.DarkOrange),
                new (SKColors.DarkRed),
                new (SKColors.DarkViolet),
                new (SKColors.BlueViolet),
            };
            
            var columnSeries = new ColumnSeries<int>() {
                Values = _sortingAlgorithm.Values
            }.OnPointMeasured(point => {
                if (point.Visual is null) return;

                var paint = paints[_sortingAlgorithm.Groups[point.Index] % paints.Length];
                point.Visual.Fill = paint;
            });;
        
            Series = [
                columnSeries
            ];
            
            await Task.Run(() =>
                _sortingAlgorithm.Sort_Visualize(10, SortThreadPauseToken.Token, SortThreadCancelToken.Token));
        }
        catch (OperationCanceledException e) {
            Debug.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
        }
        finally {
            _task?.Dispose();
        }
    }

    private static void Start(SortVisualizerViewModel viewModel) {
        // the task has yet to be started
        if (viewModel._task is null || viewModel._task.Status != TaskStatus.Running)
            viewModel._task = viewModel.SetValues_Async();
        
        // step
        if (!viewModel.IsPaused) return;
        
        _ = viewModel.SortThreadPauseToken.ResumeAsync();
        _ = viewModel.SortThreadPauseToken.PauseAsync();
    }
    
    private static void Pause(SortVisualizerViewModel viewModel) {
        // triggers after the value is set, so if !IsPaused it was toggled already
        if (!viewModel.IsPaused) {
            _ = viewModel.SortThreadPauseToken.ResumeAsync();
            return;
        }
        _ = viewModel.SortThreadPauseToken.PauseAsync();
    }
    
    private static void Stop(SortVisualizerViewModel viewModel) {
        viewModel.SortThreadCancelToken.Cancel();
    }
}