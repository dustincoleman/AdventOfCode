namespace AdventOfCode2024
{
    public class Day05
    {
        [Fact]
        public void Part1()
        {
            string[][] puzzle = PuzzleFile.ReadAllLineGroups("Day05.txt");
            HashSet<Point2> rules = new HashSet<Point2>(puzzle[0].Select(Point2.Parse));
            List<int[]> updates = puzzle[1].Select(line => line.Split(',').Select(int.Parse).ToArray()).ToList();

            int answer = 0;

            foreach (int[] update in updates)
            {
                bool correct = true;

                for (int i = 0; i < update.Length - 1; i++)
                {
                    if (rules.Contains((update[i + 1], update[i])))
                    {
                        correct = false;
                    }
                }

                if (correct)
                {
                    answer += update[update.Length / 2];
                }
            }

            Assert.Equal(5588, answer);
        }

        [Fact]
        public void Part2()
        {
            string[][] puzzle = PuzzleFile.ReadAllLineGroups("Day05.txt");
            HashSet<Point2> rules = new HashSet<Point2>(puzzle[0].Select(Point2.Parse));
            List<int[]> updates = puzzle[1].Select(line => line.Split(',').Select(int.Parse).ToArray()).ToList();

            int answer = 0;

            foreach (int[] update in updates)
            {
                bool correct, corrected = false;

                do
                {
                    correct = true;

                    for (int i = 0; i < update.Length - 1; i++)
                    {
                        if (rules.Contains((update[i + 1], update[i])))
                        {
                            correct = false;
                            corrected = true;
                            (update[i], update[i + 1]) = (update[i + 1], update[i]);
                        }
                    }
                } while (!correct);

                if (corrected)
                {
                    answer += update[update.Length / 2];
                }
            }

            Assert.Equal(5331, answer);
        }
    }
}
