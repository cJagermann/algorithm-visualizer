using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AlgorithmVisualizer.ViewModels;

namespace AlgorithmVisualizer;

public class ViewLocator : IDataTemplate {
    public Control? Build(object? data) {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModels", "Views", StringComparison.Ordinal);
        name = name.Replace("ViewModel", "", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type == null) return new TextBlock {Text = "Not Found: " + name};
        
        var control = (Control) Activator.CreateInstance(type)!;
        control.DataContext = data;
        
        return control;

    }

    public bool Match(object? data) {
        return data is ViewModelBase;
    }
}