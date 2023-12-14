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
            answer += line.CountPossibleArrangements();
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
            answer += Unfold(line).CountPossibleArrangements();
        }

        Assert.Equal(8475948826693, answer);
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
        private Dictionary<Tuple<int, int>, long> cache = new Dictionary<Tuple<int, int>, long>();

        public string Chars;
        public int[] Ints;

        public long CountPossibleArrangements()
        {
            long count = 0;
            int totalDamaged = Ints.Sum();
            int totalOperational = Chars.Length - totalDamaged;

            cache.Clear();
            CountPossibleArrangementsHelper(totalSpringsAdded: 0, operationalGroupsAdded: 0, remainingOperationalSprings: totalOperational, ref count);

            return count;
        }

        private void CountPossibleArrangementsHelper(int totalSpringsAdded, int operationalGroupsAdded, int remainingOperationalSprings, ref long count)
        {
            int totalOperationalGroups = Ints.Length + 1; // First and last can be zero length
            int remainingOperationalGroups = totalOperationalGroups - operationalGroupsAdded;

            if (remainingOperationalGroups == 1)
            {
                if (IsMatching(operationalToAdd: remainingOperationalSprings, damagedToAdd: 0, totalSpringsAdded))
                {
                    count++;
                }
                return;
            }

            long countAtStart = count;
            bool allowZero = (operationalGroupsAdded == 0);
            int maxForThisGroup = remainingOperationalSprings - remainingOperationalGroups + 2;

            if (maxForThisGroup < 0 || (!allowZero && maxForThisGroup < 1))
            {
                throw new Exception("Unexpected");
            }

            Tuple<int, int> key = new Tuple<int, int>(totalSpringsAdded, remainingOperationalGroups);

            if (cache.TryGetValue(key, out long value))
            {
                count += value;
                return;
            }

            int damagedToAdd = Ints[operationalGroupsAdded];

            for (int i = allowZero ? 0 : 1; i <= maxForThisGroup; i++)
            {
                if (IsMatching(operationalToAdd: i, damagedToAdd, totalSpringsAdded))
                {
                    CountPossibleArrangementsHelper(totalSpringsAdded + i + damagedToAdd, operationalGroupsAdded + 1, remainingOperationalSprings - i, ref count);
                }
            }

            long possibilities = count - countAtStart;
            cache.Add(key, possibilities);
        }

        private bool IsMatching(int operationalToAdd, int damagedToAdd, int pos)
        {
            while (operationalToAdd-- > 0)
            {
                char ch = Chars[pos++];
                if (ch != '?' && ch != '.')
                {
                    return false;
                }
            }

            while (damagedToAdd-- > 0)
            {
                char ch = Chars[pos++];
                if (ch != '?' && ch != '#')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
