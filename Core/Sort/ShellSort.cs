using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Core.Sort;

public class ShellSort : SortingAlgorithm {
    /// <summary>
    /// Best case for a BubbleSort is if all elements are already in order, O(n).
    /// </summary>
    public override Func<int, int> BestCaseTimeComplexity => i => (int) (i * Math.Log2(i));
    
    /// <summary>
    /// Average case requires two for loops, therefore we end at O(n*n).
    /// </summary>
    public override Func<int, int> AvgCaseTimeComplexity => i => (int) (i * Math.Log2(i));
    
    /// <summary>
    /// Worst case for a BubbleSort is if all elements are in revers order, O(n*n).
    /// </summary>
    public override Func<int, int> WorstCaseTimeComplexity => i => i*i;

    public ShellSort() : base() { }

    public ShellSort(int valueCount = 64) : base(valueCount) { }

    public override int Sort_Benchmark() {
        if (Values.Count <= 1) return -1;
        
        for (var interval = Values.Count / 2; interval > 0; interval /= 2) {
            for (var i = interval; i < Values.Count; i++) {
                var value = Values[i];
                var k = i;
                while (k >= interval && Values[k - interval] > value) {
                    Values[k] = Values[k - interval];
                    k -= interval;
                }

                Values[k] = value;
            }
        }

        return 0;
    }

    public override async Task<int> Sort_Visualize(int delay, PauseToken pauseToken, CancellationToken cancelToken = new()) {
        if (Values.Count <= 1) return -1;
        
        ResetGroups();
        
        for (var interval = Values.Count / 2; interval > 0; interval /= 2) {
            // build group list for visuals
            for (var i = 0; i < Values.Count; i++) {
                Groups[i] = i % interval;
            }
            
            // for each subgroup
            for (var i = interval; i < Values.Count; i++) {
                CheckForCancellation(cancelToken);
                
                var value = Values[i];
                var k = i;
                
                while (k >= interval && Values[k - interval] > value) {
                    Values[k] = Values[k - interval];
                    k -= interval;

                    await Wait(pauseToken, delay);
                }

                Values[k] = value;
                await Wait(pauseToken, delay);
            }
        }

        return 0;
    }
}