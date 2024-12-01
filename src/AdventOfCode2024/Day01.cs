namespace AdventOfCode2024
{
    public class Day01
    {
        [Fact]
        public void Part1()
        {
            List<Point2> puzzle = File.ReadAllLines("Day01.txt").Select(Point2.Parse).ToList();
            List<int> list1 = puzzle.Select(pt => pt.X).ToList();
            List<int> list2 = puzzle.Select(pt => pt.Y).ToList();

            int answer = 0;
            list1.Sort();
            list2.Sort();

            for (int i = 0; i < list1.Count; i++)
            {
                answer += Math.Abs(list1[i] - list2[i]);
            }

            Assert.Equal(1222801, answer);
        }

        [Fact]
        public void Part2()
        {
            List<Point2> puzzle = File.ReadAllLines("Day01.txt").Select(Point2.Parse).ToList();
            List<int> list1 = puzzle.Select(pt => pt.X).ToList();
            List<int> list2 = puzzle.Select(pt => pt.Y).ToList();

            int answer = 0;

            foreach (int left in list1)
            {
                answer += left * list2.Count(i => i == left);
            }

            Assert.Equal(expected: 22545250, answer);
        }
    }
}