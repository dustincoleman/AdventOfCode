using Xunit;

namespace AdventOfCode2022
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            long answer = GetCaloriesPerElf().Max();
            Assert.Equal(68292, answer);
        }

        [Fact]
        public void Test2()
        {
            long answer = GetCaloriesPerElf().OrderByDescending(i => i).Take(3).Sum();
            Assert.Equal(203203, answer);
        }

        private IEnumerable<int> GetCaloriesPerElf()
        {
            int current = 0;

            foreach (string line in File.ReadAllLines("Day01.txt"))
            {
                if (line == string.Empty)
                {
                    yield return current;
                    current = 0;
                }
                else
                {
                    current += int.Parse(line);
                }
            }
        }
    }
}