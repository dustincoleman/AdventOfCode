namespace AdventOfCode2024
{
    public class Day11
    {
        [Fact]
        public void Part1()
        {
            List<int> puzzle = File.ReadAllText("Day11.txt").Split(' ').Select(int.Parse).ToList();
            long answer = SolvePuzzle(puzzle, 25);
            Assert.Equal(228668, answer);
        }

        [Fact]
        public void Part2()
        {
            List<int> puzzle = File.ReadAllText("Day11.txt").Split(' ').Select(int.Parse).ToList();
            long answer = SolvePuzzle(puzzle, 75);
            Assert.Equal(270673834779359, answer);
        }

        private long SolvePuzzle(List<int> puzzle, int times)
        {
            AutoDictionary<long, long> counts = new AutoDictionary<long, long>();

            foreach (int num in puzzle)
            {
                counts[num]++;
            }

            for (int i = 0; i < times; i++)
            {
                AutoDictionary<long, long> newCounts = new AutoDictionary<long, long>();

                foreach (KeyValuePair<long, long> kvp in counts)
                {
                    if (kvp.Key == 0)
                    {
                        newCounts[1] += kvp.Value;
                    }
                    else
                    {
                        string stone = kvp.Key.ToString();

                        if (stone.Length % 2 == 0)
                        {
                            newCounts[long.Parse(stone.Substring(0, stone.Length / 2))] += kvp.Value;
                            newCounts[long.Parse(stone.Substring(stone.Length / 2))] += kvp.Value;
                        }
                        else
                        {
                            newCounts[kvp.Key * 2024] += kvp.Value;
                        }
                    }
                }

                counts = newCounts;
            }

            return counts.Values.Sum();
        }
    }
}
