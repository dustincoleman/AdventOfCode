namespace AdventOfCode2024
{
    public class Day02
    {
        [Fact]
        public void Part1()
        {
            List<int[]> puzzle = File.ReadAllLines("Day02.txt")
                .Select(line => line.Split(' ').Select(int.Parse).ToArray())
                .ToList();

            int answer = 0;

            foreach (int[] report in puzzle)
            {
                if (IsSafe(report))
                {
                    answer++;
                }
            }

            Assert.Equal(expected: 598, answer);
        }

        [Fact]
        public void Part2()
        {
            List<int[]> puzzle = File.ReadAllLines("Day02.txt")
                .Select(line => line.Split(' ').Select(int.Parse).ToArray())
                .ToList();

            int answer = 0;

            foreach (int[] report in puzzle)
            {
                if (IsSafe(report))
                {
                    answer++;
                }
                else
                {
                    int[] pruned = new int[report.Length - 1];

                    for (int i = 0; i < report.Length; i++)
                    {
                        int dest = 0;

                        for (int j = 0; j < report.Length; j++)
                        {
                            if (j != i)
                            {
                                pruned[dest++] = report[j];
                            }
                        }

                        if (IsSafe(pruned))
                        {
                            answer++;
                            break;
                        }
                    }
                }
            }

            Assert.Equal(expected: 634, answer);
        }

        public bool IsSafe(int[] report)
        {
            bool isSafe = true;
            int sign = Math.Sign(report[1] - report[0]);

            for (int i = 0; i < report.Length - 1; i++)
            {
                int diff = report[i + 1] - report[i];
                if (Math.Sign(diff) != sign || diff == 0 || Math.Abs(diff) > 3)
                {
                    isSafe = false;
                    break;
                }
            }

            return isSafe;
        }
    }
}
