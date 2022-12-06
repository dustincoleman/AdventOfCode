using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    public class Day06
    {
        [Fact]
        public void Part1()
        {
            int result = 0;

            string stream = File.ReadAllLines("Day06.txt").First();
            HashSet<char> set = new HashSet<char>();

            for (int i = 0; i < stream.Length - 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    set.Add(stream[i + j]);
                }

                if (set.Count == 4)
                {
                    result = i + 4;
                    break;
                }

                set.Clear();
            }

            Assert.Equal(1876, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 0;

            string stream = File.ReadAllLines("Day06.txt").First();
            HashSet<char> set = new HashSet<char>();

            for (int i = 0; i < stream.Length - 13; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    set.Add(stream[i + j]);
                }

                if (set.Count == 14)
                {
                    result = i + 14;
                    break;
                }

                set.Clear();
            }

            Assert.Equal(2202, result);
        }

    }
}