using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlgorithmVisualizer.Models;
using Avalonia.Collections;
using DynamicData;
using DynamicData.Binding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AlgorithmVisualizer.Core;

public abstract class SortingAlgorithm : ReactiveObject, ISortingAlgorithm {
    public virtual Func<int, int> BestCaseTimeComplexity { get; } = null!;
    public virtual Func<int, int> AvgCaseTimeComplexity { get; } = null!;
    public virtual Func<int, int> WorstCaseTimeComplexity { get; } = null!;

    int[] ISortingAlgorithm.Values {
        get => this.Values.ToArray();
        set => Values = new ObservableCollection<int>(value);
    }

    public new  ObservableCollection<int> Values { get; set; }
    public new  ObservableCollection<int> Groups { get; set; }

    public SortingAlgorithm() : this(64){
        // Values!.CollectionChanged += (sender, args) => Debug.WriteLine("Collection Changed!");
    }

    protected SortingAlgorithm(int valueCount = 64) {
        Reset(valueCount, DateTime.Now.GetHashCode());
    }

    public virtual int Sort_Benchmark() {
        throw new NotImplementedException();
    }

    public virtual async Task<int> Sort_Visualize(int delay, PauseToken pauseToken, CancellationToken cancelToken = new CancellationToken()) {
        throw new NotImplementedException();
    }
    
    protected async Task Wait(PauseToken pauseToken, int time) {
        await pauseToken.PauseIfRequestedAsync();
        await Task.Delay(time);
    }

    protected void CheckForCancellation(CancellationToken cancelToken) {
        if (cancelToken.IsCancellationRequested) {
            cancelToken.ThrowIfCancellationRequested();
        }
    }
    
    protected void ResetArray(int length = 64, int seed = 80085) {
        if (length <= 1) return;
        var rand = new Random(seed);
        var values = new int[length];

        for (var i = 0; i < values.Length; i++) {
            values[i] = i + 1;
        }

        // .NET 8.0
        rand.Shuffle(values);

        this.Values = new ObservableCollection<int>(values);
    }

    protected void ResetGroups() {
        if (Values.Count <= 1) return;
        var groups = new int[Values.Count];

        for (var i = 0; i < Values.Count; i++) {
            groups[i] = 0;
        }
        
        this.Groups = new ObservableCollection<int>(groups);
    }

    protected void Reset(int length = 64, int seed = 80085) {
        if (length <= 1) return;
        var rand = new Random(seed);
        var values = new int[length];
        var groups = new int[length];

        for (var i = 0; i < values.Length; i++) {
            values[i] = i + 1;
            groups[i] = 0;
        }

        // .NET 8.0
        rand.Shuffle(values);

        this.Values = new ObservableCollection<int>(values);
        this.Groups = new ObservableCollection<int>(groups);
    }
}