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
        return new PuzzleLine()
        {
            Pattern = string.Join("?", Enumerable.Repeat(line.Pattern, 5)),
            DamagedGroupSizes = Enumerable.Repeat(line.DamagedGroupSizes, 5).Aggregate((x,y) => x.Concat(y).ToArray())
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
                    Pattern = split[0],
                    DamagedGroupSizes = split[1].Split(',').Select(int.Parse).ToArray()
                });
        }

        return list;
    }

    private class PuzzleLine
    {
        private Dictionary<Tuple<int, int>, long> cache = new Dictionary<Tuple<int, int>, long>();

        public string Pattern;
        public int[] DamagedGroupSizes;

        public long CountPossibleArrangements()
        {
            long count = 0;
            int totalDamaged = DamagedGroupSizes.Sum();
            int totalOperational = Pattern.Length - totalDamaged;

            cache.Clear();
            CountPossibleArrangementsHelper(totalSpringsAdded: 0, operationalGroupsAdded: 0, remainingOperationalSprings: totalOperational, ref count);

            return count;
        }

        private void CountPossibleArrangementsHelper(int totalSpringsAdded, int operationalGroupsAdded, int remainingOperationalSprings, ref long count)
        {
            int totalOperationalGroups = DamagedGroupSizes.Length + 1; // First and last can be zero length
            int remainingOperationalGroups = totalOperationalGroups - operationalGroupsAdded - 1;

            if (remainingOperationalGroups == 0)
            {
                if (IsMatching(operationalToAdd: remainingOperationalSprings, damagedToAdd: 0, totalSpringsAdded))
                {
                    count++;
                }
                return;
            }

            long countAtStart = count;
            Tuple<int, int> key = new Tuple<int, int>(totalSpringsAdded, remainingOperationalGroups);

            if (cache.TryGetValue(key, out long value))
            {
                count += value;
                return;
            }

            int minOperationalSpringsToAdd = (operationalGroupsAdded == 0) ? 0 : 1; // First group can be zero length
            int maxOperationalSpringsToAdd = remainingOperationalSprings - remainingOperationalGroups + 1; // Last group can be zero length
            int damagedSpringsToAdd = DamagedGroupSizes[operationalGroupsAdded];

            for (int i = minOperationalSpringsToAdd; i <= maxOperationalSpringsToAdd; i++)
            {
                if (IsMatching(operationalToAdd: i, damagedSpringsToAdd, totalSpringsAdded))
                {
                    CountPossibleArrangementsHelper(totalSpringsAdded + i + damagedSpringsToAdd, operationalGroupsAdded + 1, remainingOperationalSprings - i, ref count);
                }
            }

            long possibilities = count - countAtStart;
            cache.Add(key, possibilities);
        }

        private bool IsMatching(int operationalToAdd, int damagedToAdd, int pos)
        {
            while (operationalToAdd-- > 0)
            {
                char ch = Pattern[pos++];
                if (ch != '?' && ch != '.')
                {
                    return false;
                }
            }

            while (damagedToAdd-- > 0)
            {
                char ch = Pattern[pos++];
                if (ch != '?' && ch != '#')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
