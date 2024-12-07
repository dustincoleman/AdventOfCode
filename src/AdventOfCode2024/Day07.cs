namespace AdventOfCode2024
{
    public class Day07
    {
        [Fact]
        public void Part1()
        {
            List<long[]> puzzle = File.ReadAllLines("Day07.txt").Select(line => line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToList();
            long answer = 0;

            foreach (long[] equation in puzzle)
            {
                if (TryFindSolution(equation, equation[1], 2, allowConcat: false))
                {
                    answer += equation[0];
                }
            }

            Assert.Equal(538191549061, answer);
        }

        [Fact]
        public void Part2()
        {
            List<long[]> puzzle = File.ReadAllLines("Day07.txt").Select(line => line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToList();
            long answer = 0;

            foreach (long[] equation in puzzle)
            {
                if (TryFindSolution(equation, equation[1], 2, allowConcat: true))
                {
                    answer += equation[0];
                }
            }

            Assert.Equal(34612812972206, answer);
        }

        private bool TryFindSolution(long[] equation, long value, int pos, bool allowConcat)
        {
            if (value > equation[0])
            {
                return false;
            }

            if (pos >= equation.Length)
            {
                return (value == equation[0]);
            }

            return 
                TryFindSolution(equation, value + equation[pos], pos + 1, allowConcat) || 
                TryFindSolution(equation, value * equation[pos], pos + 1, allowConcat) ||
                (allowConcat ? TryFindSolution(equation, Concat(value, equation[pos]), pos + 1, allowConcat) : false);
        }

        private long Concat(long left, long right)
        {
            long temp = right;

            while (temp > 0)
            {
                left *= 10;
                temp /= 10;
            }

            return left + right;
        }
    }
}
