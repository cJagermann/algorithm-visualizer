namespace AlgorithmVisualizer.Models;

public interface ISearchAlgorithm {
    public int[] UnsortedValues { get; set; }
    public int[] SortedValues { get; set; }
    
    public int Search_Benchmark(int value);
    public int Search_Visualize();
}