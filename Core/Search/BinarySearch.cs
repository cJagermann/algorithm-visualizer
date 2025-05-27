using AlgorithmVisualizer.Models;

namespace AlgorithmVisualizer.Core.Search;

public class BinarySearch : SearchAlgorithm{
    
    public override int Search_Benchmark(int value) {
        var start = 0;
        var end = SortedValues.Length - 1;

        while (start <= end) {
            var mid = (start + end) / 2;

            // value is the midpoint
            if (SortedValues[mid] == value) {
                return mid;
            }
            // value is greater than the midpoint, move start point up
            if (SortedValues[mid] < value) {
                start = mid + 1;
            }
            // value is less than midpoint, move end point down
            else {
                end = mid - 1;
            }
        }
        
        return base.Search_Benchmark(value);
    }
}