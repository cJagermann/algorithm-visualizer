using ReactiveUI.Fody.Helpers;

namespace AlgorithmVisualizer.ViewModels;

public class MainWindowViewModel : ViewModelBase {
    [Reactive] public SortVisualizerViewModel SortVisualizer { get; private set; } = new();
}