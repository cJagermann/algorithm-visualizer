using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AlgorithmVisualizer.Views;

public partial class SortVisualizer : UserControl {
    public SortVisualizer() {
        InitializeComponent();
    }

    protected override void OnInitialized() {
        base.OnInitialized();
    }

    private void Algorithms_OnLoaded(object? sender, RoutedEventArgs e) {
        //((sender as ComboBox)!).SelectedIndex = 0;
    }
}