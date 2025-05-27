using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Core.Sort;

public class QuickSort : SortingAlgorithm{
    /// <summary>
    /// Best case for a BubbleSort is if all elements are already in order, O(n).
    /// </summary>
    public override Func<int, int> BestCaseTimeComplexity => i => (int) (i * Math.Log(i));
    
    /// <summary>
    /// Average case requires two for loops, therefore we end at O(n*n).
    /// </summary>
    public override Func<int, int> AvgCaseTimeComplexity => i => (int) (i * Math.Log(i));
    
    /// <summary>
    /// Worst case for a BubbleSort is if all elements are in revers order, O(n*n).
    /// </summary>
    public override Func<int, int> WorstCaseTimeComplexity => i => i*i;

    public override int Sort_Benchmark() {
        return base.Sort_Benchmark();
    }

    public override async Task<int> Sort_Visualize(int delay, PauseToken pauseToken, CancellationToken cancelToken = new ()) {
        if (Values.Count <= 1) return -1;
        
        var complexity = 0;

        await Sort_Async(0, Values.Count - 1, delay, pauseToken, cancelToken).ContinueWith(task => complexity += task.Result, cancelToken);

        return complexity;
    }

    private async Task<int> Sort_Async(int leftIdx, int rightIdx, int delay, PauseToken pauseToken, CancellationToken cancelToken = new()) {
        var left = leftIdx;
        var right = rightIdx;
        var pivot = Values[leftIdx];
        var complexity = 0;

        while (left <= right) {
            ResetGroups();
            
            // pivot - red
            Groups[leftIdx] = 3;
            
            while (Values[left] < pivot) {
                // left - green
                Groups[left] = 0;
                Groups[left + 1] = 1;
                left++;
                await Wait(pauseToken, delay / 4);
            }
            
            while (Values[right] > pivot) {
                Groups[right] = 0;
                Groups[right - 1] = 2;
                right--;
                await Wait(pauseToken, delay / 4);
            }
            
            if (left > right) continue;

            (Values[left], Values[right]) = (Values[right], Values[left]);

            left++;
            right--;

            await Wait(pauseToken, delay);
        }

        await Wait(pauseToken, delay);
        
        if (leftIdx < right)
            await Sort_Async(leftIdx, right, delay, pauseToken, cancelToken)
                .ContinueWith(task => complexity += task.Result, cancelToken);
        if (left < rightIdx)
            await Sort_Async(left, rightIdx, delay, pauseToken, cancelToken)
                .ContinueWith(task => complexity += task.Result, cancelToken);

        return complexity;
    }
}