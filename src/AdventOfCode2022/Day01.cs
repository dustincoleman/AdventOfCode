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
            return PuzzleFile.ReadAllLineGroups("Day01.txt").Select(lines => lines.Sum(l => int.Parse(l)));
        }
    }
}