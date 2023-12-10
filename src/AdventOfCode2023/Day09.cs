
namespace AdventOfCode2023;

public class Day09
{
    [Fact]
    public void Part1()
    {
        int answer = LoadPuzzle().Sum(PredictNextValue);
        Assert.Equal(1887980197, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = LoadPuzzle().Sum(PredictPreviousValue);
        Assert.Equal(990, answer);
    }

    private int PredictNextValue(int[] history)
    {
        if (history.All(diff => diff is 0))
        {
            return 0;
        }

        List<int> diffs = new List<int>();

        for (int i = 1; i < history.Length; i++)
        {
            diffs.Add(history[i] - history[i - 1]);
        }

        return history.Last() + PredictNextValue(diffs.ToArray());
    }

    private int PredictPreviousValue(int[] history)
    {
        if (history.All(diff => diff is 0))
        {
            return 0;
        }

        List<int> diffs = new List<int>();

        for (int i = 1; i < history.Length; i++)
        {
            diffs.Add(history[i] - history[i - 1]);
        }

        return history.First() - PredictPreviousValue(diffs.ToArray());
    }

    private List<int[]> LoadPuzzle()
    {
        return File.ReadLines("Day09.txt")
            .Select(line => line.Split(' ').Select(int.Parse).ToArray())
            .ToList();
    }
}
