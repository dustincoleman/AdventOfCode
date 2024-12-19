namespace AdventOfCode2024
{
    public class Day19
    {
        [Fact]
        public void Part1()
        {
            (List<string> available, List<string> designs) = LoadPuzzle();
            int result = designs.Count(design => PermutationsWorker(design, available, new Dictionary<string, long>(), fullCount: false) > 0);
            Assert.Equal(330, result);
        }

        [Fact]
        public void Part2()
        {
            (List<string> available, List<string> designs) = LoadPuzzle();
            Dictionary<string, long> cache = new Dictionary<string, long>();
            long result = designs.Sum(design => PermutationsWorker(design, available, cache, fullCount: true));
            Assert.Equal(950763269786650, result);
        }

        private long PermutationsWorker(string design, List<string> available, Dictionary<string, long> cache, bool fullCount)
        {
            if (design == string.Empty)
            {
                return 1;
            }

            long result = 0;

            if (cache.TryGetValue(design, out long cachedResult))
            {
                return cachedResult;
            }

            Dictionary<int, string> subStrings = new Dictionary<int, string>();

            foreach (string next in available)
            {
                if (next.Length <= design.Length && design.StartsWith(next))
                {
                    if (!subStrings.TryGetValue(next.Length, out string subString))
                    {
                        subString = design.Substring(next.Length);
                        subStrings.Add(next.Length, subString);
                    }

                    long temp = PermutationsWorker(subString, available, cache, fullCount);

                    if (!fullCount && temp > 0)
                    {
                        result = 1; // true
                        break;
                    }
                    
                    result += temp;
                }
            }

            cache.Add(design, result);

            return result;
        }

        private (List<string> available, List<string> designs) LoadPuzzle()
        {
            string[][] groups = PuzzleFile.ReadAllLineGroups("Day19.txt");
            return (groups[0][0].Split(", ").ToList(), groups[1].ToList());
        }
    }
}
