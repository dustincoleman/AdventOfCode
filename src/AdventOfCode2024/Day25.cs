namespace AdventOfCode2024
{
    public class Day25
    {
        [Fact]
        public void Part1()
        {
            (List<int[]> locks, List<int[]> keys) = LoadPuzzle();
            int result = 0;

            foreach (int[] @lock in locks)
            {
                foreach (int[] key in keys)
                {
                    bool fits = true;

                    for (int col = 0; col < 5; col++)
                    {
                        if (@lock[col] + key[col] > 5)
                        {
                            fits = false;
                            break;
                        }
                    }

                    if (fits)
                    {
                        result++;
                    }
                }
            }

            Assert.Equal(3395, result);
        }

        private (List<int[]> locks, List<int[]> keys) LoadPuzzle()
        {
            string[][] groups = PuzzleFile.ReadAllLineGroups("Day25.txt");
            List<int[]> locks = new List<int[]>();
            List<int[]> keys = new List<int[]>();

            foreach (string[] group in groups)
            {
                int[] heights = new int[5];

                for (int lineNum = 1; lineNum < 6; lineNum++)
                {
                    string line = group[lineNum];

                    for (int col = 0; col < 5; col++)
                    {
                        if (line[col] == '#')
                        {
                            heights[col]++;
                        }
                    }
                }

                if (group[0] == "#####")
                {
                    locks.Add(heights);
                }
                else if (group[6] == "#####")
                {
                    keys.Add(heights);
                }
                else
                {
                    Assert.Fail();
                }
            }

            return (locks, keys);
        }
    }
}
