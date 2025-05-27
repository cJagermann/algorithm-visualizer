using AlgorithmVisualizer.Models;
using DynamicData.Tests;

namespace AlgorithmVisualizer.Core;

public abstract class SearchAlgorithm : ISearchAlgorithm{
    public int[] UnsortedValues { get; set; }
    public int[] SortedValues { get; set; }

    public SearchAlgorithm() { }

    public virtual int Search_Benchmark(int value) => -1;

    public virtual int Search_Visualize() => -1;
}