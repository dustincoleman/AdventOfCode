namespace AdventOfCode2024
{
    public class Day05
    {
        [Fact]
        public void Part1()
        {
            string[][] puzzle = PuzzleFile.ReadAllLineGroups("Day05.txt");
            Dictionary<int, HashSet<int>> rules = LoadRules(puzzle[0]);
            List<int[]> updates = LoadUpdates(puzzle[1]);

            int answer = 0;

            foreach (int[] update in updates)
            {
                bool correct = true;

                for (int i = 0; i < update.Length - 1; i++)
                {
                    if (rules.TryGetValue(update[i + 1], out HashSet<int> hashSet) && hashSet.Contains(update[i]))
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
            Dictionary<int, HashSet<int>> rules = LoadRules(puzzle[0]);
            List<int[]> updates = LoadUpdates(puzzle[1]);

            int answer = 0;

            foreach (int[] update in updates)
            {
                bool correct, corrected = false;

                do
                {
                    correct = true;

                    for (int i = 0; i < update.Length - 1; i++)
                    {
                        if (rules.TryGetValue(update[i + 1], out HashSet<int> hashSet) && hashSet.Contains(update[i]))
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

        private Dictionary<int, HashSet<int>> LoadRules(string[] rulesText)
        {
            Dictionary<int, HashSet<int>> rules = new Dictionary<int, HashSet<int>>();

            foreach (string line in rulesText)
            {
                int[] rule = line.Split('|').Select(int.Parse).ToArray();

                if (!rules.TryGetValue(rule[0], out HashSet<int> hashSet))
                {
                    hashSet = new HashSet<int>();
                    rules.Add(rule[0], hashSet);
                }

                hashSet.Add(rule[1]);
            }

            return rules;
        }

        private List<int[]> LoadUpdates(string[] updatesText)
        {
            List<int[]> updates = new List<int[]>();

            foreach (string line in updatesText)
            {
                updates.Add(line.Split(',').Select(int.Parse).ToArray());
            }

            return updates;
        }
    }
}
