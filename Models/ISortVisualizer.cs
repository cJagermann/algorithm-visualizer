using System.Windows.Input;

namespace AlgorithmVisualizer.Models;

public interface ISortVisualizer {
    public ICommand StartCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand StopCommand { get; }
}