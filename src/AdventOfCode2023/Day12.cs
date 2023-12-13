namespace AdventOfCode2023;

public class Day12
{
    [Fact]
    public void Part1()
    {
        List<PuzzleLine> puzzle = LoadPuzzle();
        long answer = 0;

        foreach (PuzzleLine line in puzzle)
        {
            answer += CountPossibleArrangements(line);
        }

        Assert.Equal(6852, answer);
    }

    [Fact]
    public void Part2()
    {
        List<PuzzleLine> puzzle = LoadPuzzle();
        long answer = 0;

        foreach (PuzzleLine line in puzzle)
        {
            answer += CountPossibleArrangements(Unfold(line));
        }

        Assert.Equal(555, answer);
    }

    private PuzzleLine Unfold(PuzzleLine line)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(line.Chars);
        sb.Append('?');
        sb.Append(line.Chars);
        sb.Append('?');
        sb.Append(line.Chars);
        sb.Append('?');
        sb.Append(line.Chars);
        sb.Append('?');
        sb.Append(line.Chars);

        List<int> list = new List<int>();
        list.AddRange(line.Ints);
        list.AddRange(line.Ints);
        list.AddRange(line.Ints);
        list.AddRange(line.Ints);
        list.AddRange(line.Ints);

        return new PuzzleLine()
        {
            Chars = sb.ToString(),
            Ints = list.ToArray()
        };
    }

    private long CountPossibleArrangements(PuzzleLine line)
    {
        int damagedGroups = line.Ints.Length;
        int possibleOperationalGroups = damagedGroups + 1;
        int totalDamaged = line.Ints.Sum();
        int totalOperational = line.Chars.Length - totalDamaged;

        return CountCombinations(line, possibleOperationalGroups, totalOperational);
    }

    private static Stack<int> stack = new Stack<int>();

    private long CountCombinations(PuzzleLine line, int possibleOperationalGroups, int totalOperational)
    {
        long count = 0;
        stack.Clear();
        CountCombinationsHelper(line, possibleOperationalGroups, totalOperational, stack, ref count, 0, 0);
        return count;
    }

    private void CountCombinationsHelper(PuzzleLine line, int possibleOperationalGroups, int totalOperational, Stack<int> stack, ref long count, int pos, int idx)
    {
        if (possibleOperationalGroups == 1)
        {
            int tempPos = pos, tempIdx = idx;
            stack.Push(totalOperational);
            if (IsMatching(line, totalOperational, ref tempPos, ref tempIdx))
            {
                count++;
            }
            stack.Pop();
            return;
        }

        bool allowZero = !stack.Any() || possibleOperationalGroups == 1;
        int maxForThisGroup = (totalOperational > 0) ? totalOperational - possibleOperationalGroups + 2 : 0;

        if (maxForThisGroup < 0 || (!allowZero && maxForThisGroup < 1))
        {
            throw new Exception("Unexpected");
        }

        for (int i = allowZero ? 0 : 1; i <= maxForThisGroup; i++)
        {
            int tempPos = pos, tempIdx = idx;
            stack.Push(i);
            if (IsMatching(line, i, ref tempPos, ref tempIdx))
            {
                CountCombinationsHelper(line, possibleOperationalGroups - 1, totalOperational - i, stack, ref count, tempPos, tempIdx);
            }
            stack.Pop();
        }
    }

    private bool IsMatching(PuzzleLine line, int i, ref int pos, ref int idx)
    {
        string pattern = line.Chars;

        for (int j = 0; j < i; j++)
        {
            char ch = pattern[pos++];
            if (ch != '?' && ch != '.')
            {
                return false;
            }
        }

        if (idx < line.Ints.Length)
        {
            for (int k = 0; k < line.Ints[idx]; k++)
            {
                char ch = pattern[pos++];
                if (ch != '?' && ch != '#')
                {
                    return false;
                }
            }

            idx++;
        }

        return true;
    }

    private List<PuzzleLine> LoadPuzzle()
    {
        List<PuzzleLine> list = new List<PuzzleLine>();

        foreach (string line in File.ReadLines("Day12.txt"))
        {
            string[] split = line.Split(" ");
            list.Add(
                new PuzzleLine()
                {
                    Chars = split[0],
                    Ints = split[1].Split(',').Select(int.Parse).ToArray()
                });
        }

        return list;
    }

    private class PuzzleLine
    {
        public string Chars;
        public int[] Ints;
    }
}
