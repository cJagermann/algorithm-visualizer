using System;
using System.Threading;
using System.Threading.Tasks;
using AlgorithmVisualizer.Core;

namespace AlgorithmVisualizer.Models;

public interface ISortingAlgorithm {
    public Func<int, int> BestCaseTimeComplexity { get; }
    public Func<int, int> AvgCaseTimeComplexity { get; }
    public Func<int, int> WorstCaseTimeComplexity { get; }
    
    public int[] Values { get; protected set; }

    public int Sort_Benchmark();
    public Task<int> Sort_Visualize(int delay, PauseToken pauseToken, CancellationToken cancelToken);
}