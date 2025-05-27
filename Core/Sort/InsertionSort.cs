using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Core.Sort;

public class InsertionSort : SortingAlgorithm{
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
    
    public InsertionSort() : base() { }

    public InsertionSort(int valueCount = 64) : base(valueCount) { }

    public override int Sort_Benchmark() {
        for (var i = 1; i < Values.Count; i++) {
            var value = Values[i];

            for (var j = i - 1; j >= 0; j--) {
                if (value < Values[j]) {
                    Values[j + 1] = Values[j];
                    Values[j] = value;

                    continue;
                }

                break;
            }
        }

        return 0;
    }

    public override async Task<int> Sort_Visualize(int delay, PauseToken pauseToken, CancellationToken cancelToken = new CancellationToken()) {
        if (Values.Count <= 1) return -1;
        
        ResetGroups();
        
        for (var i = 1; i < Values.Count; i++) {
            var value = Values[i];

            for (var j = i - 1; j >= 0; j--) {
                CheckForCancellation(cancelToken);
                
                // set sampled
                Groups[j] = Groups[j + 1] = 3;
                
                await Wait(pauseToken, delay);
                
                if (value < Values[j]) {
                    Values[j + 1] = Values[j];
                    Values[j] = value;
                    
                    // reset sampled
                    Groups[j] = Groups[j + 1] = 0;

                    continue;
                }
                // reset sampled
                Groups[j] = Groups[j + 1] = 0;

                break;
            }
        }
        
        ResetGroups();

        return 0;
    }
}