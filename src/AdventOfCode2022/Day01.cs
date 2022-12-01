using Xunit;

namespace AdventOfCode2022
{
    public class Day01
    {
        [Fact]
        public void Test1()
        {
            string[] input = File.ReadAllLines("Day01.txt");

            long current = 0;
            long max = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == string.Empty)
                {
                    max = Math.Max(max, current);
                    current = 0;
                }
                else
                {
                    current += int.Parse(input[i]);
                }
            }

            Assert.Equal(68292, max);
        }

        [Fact]
        public void Test2()
        {
            string[] input = File.ReadAllLines("Day01.txt");

            long current = 0;
            List<long> elves = new List<long>();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == string.Empty)
                {
                    elves.Add(current);
                    current = 0;
                }
                else
                {
                    current += int.Parse(input[i]);
                }
            }

            long answer = elves.OrderByDescending(i => i).Take(3).Sum();
            Assert.Equal(203203, answer);
        }
    }
}