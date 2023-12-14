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

            int damagedToAdd = DamagedGroupSizes[operationalGroupsAdded];

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
