namespace AdventOfCode2022
{
    public class Day04
    {
        [Fact]
        public void Part1()
        {
            int result = GetAssignments().Count(a => a.Left.Contains(a.Right) || a.Right.Contains(a.Left));
            Assert.Equal(453, result);
        }

        [Fact]
        public void Part2()
        {
            int result = GetAssignments().Count(a => a.Left.Overlaps(a.Right));
            Assert.Equal(919, result);
        }

        private IEnumerable<AssignmentPair> GetAssignments()
        {
            foreach (string line in File.ReadLines("Day04.txt"))
            {
                string[] ranges = line.Split('-', ',');

                yield return new AssignmentPair()
                {
                    Left = new Range(int.Parse(ranges[0]), int.Parse(ranges[1])),
                    Right = new Range(int.Parse(ranges[2]), int.Parse(ranges[3]))
                };
            }
        }
    }

    internal struct AssignmentPair
    {
        public Range Left;
        public Range Right;
    }
}