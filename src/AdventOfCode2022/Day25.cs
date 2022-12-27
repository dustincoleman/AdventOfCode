using System.Linq;

namespace AdventOfCode2022
{
    public class Day25
    {
        [Fact]
        public void Part1()
        {
            long sum = File.ReadAllLines("Day25.txt").Select(SToL).Sum();
            Assert.Equal("2-=2==00-0==2=022=10", LToS(sum));
        }

        private long SToL(string snafu)
        {
            long l = 0;
            long multiplier = 1;

            foreach (char ch in snafu.Reverse())
            {
                l += multiplier * ch switch
                {
                    '=' => -2,
                    '-' => -1,
                    '0' => 0,
                    '1' => 1,
                    '2' => 2,
                    '3' => 3,
                    _ => throw new Exception()
                };
                multiplier *= 5;
            }

            return l;
        }

        private string LToS(long l)
        {
            Stack<char> stack = new Stack<char>();

            do
            {
                int temp = (int)(l % 5);

                if (temp < 3)
                {
                    stack.Push(temp.ToString()[0]);
                }
                else
                {
                    l += 5;
                    stack.Push((temp == 3) ? '=' : '-');
                }

                l /= 5;
            }
            while (l != 0);

            return new string(stack.ToArray());
        }
    }
}