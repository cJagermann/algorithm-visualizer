using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AlgorithmVisualizer.Models;

namespace AlgorithmVisualizer.Core.Sort;

public class BubbleSort : SortingAlgorithm {
    /// <summary>
    /// Best case for a BubbleSort is if all elements are already in order, O(n).
    /// </summary>
    public override Func<int, int> BestCaseTimeComplexity => i => i;
    
    /// <summary>
    /// Average case requires two for loops, therefore we end at O(n*n).
    /// </summary>
    public override Func<int, int> AvgCaseTimeComplexity => i => i*i;
    
    /// <summary>
    /// Worst case for a BubbleSort is if all elements are in revers order, O(n*n).
    /// </summary>
    public override Func<int, int> WorstCaseTimeComplexity => i => i*i;

    public BubbleSort() : base() { }

    public BubbleSort(int valueCount = 64) : base(valueCount) { }

    public override int Sort_Benchmark() {
        if (Values.Count <= 1) return -1;

        for (var i = 0; i < Values.Count - 1; i++) {
            for (var j = 0; j < Values.Count - 1; j++) {
                if (Values[j] <= Values[j + 1]) continue;

                (Values[j], Values[j + 1]) = (Values[j + 1], Values[j]);
            }
        }

        return 0;
    }

    public override async Task<int> Sort_Visualize(int delay, PauseToken pauseToken, CancellationToken cancelToken = new CancellationToken()) {
        if (Values.Count <= 1) return -1;
        
        ResetGroups();

        for (var i = 0; i < Values.Count - 1; i++) {
            for (var j = 0; j < Values.Count - 1; j++) {
                CheckForCancellation(cancelToken);
                
                // Currently being sampled
                Groups[j] = Groups[j + 1] = 3;
                
                await Wait(pauseToken, delay);
                
                // Check failed, continue to next index
                if (Values[j] <= Values[j + 1]) {
                    Groups[j] = Groups[j + 1] = 0;
                    continue;
                }
                
                (Values[j], Values[j + 1]) = (Values[j + 1], Values[j]);
                
                Groups[j] = Groups[j + 1] = 0;
            }
        }
        
        ResetGroups();

        return 0;
    }
}