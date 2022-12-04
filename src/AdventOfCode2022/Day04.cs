namespace AdventOfCode2022
{
    public class Day04
    {
        [Fact]
        public void Part1()
        {
            int result = 0;

            foreach (AssignmentPair pair in GetAssignments())
            {
                if (pair.Left.Contains(pair.Right) || pair.Right.Contains(pair.Left))
                {
                    result++;
                }
            }

            Assert.Equal(453, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 0;

            foreach (AssignmentPair pair in GetAssignments())
            {
                if (pair.Left.Overlaps(pair.Right))
                {
                    result++;
                }
            }

            Assert.Equal(919, result);
        }

        IEnumerable<AssignmentPair> GetAssignments()
        {
            foreach (string line in File.ReadLines("Day04.txt"))
            {
                string[] ranges = line.Split(',');
                string[] left = ranges[0].Split('-');
                string[] right = ranges[1].Split('-');

                yield return new AssignmentPair()
                {
                    Left = new Range(int.Parse(left[0]), int.Parse(left[1])),
                    Right = new Range(int.Parse(right[0]), int.Parse(right[1]))
                };
            }
        }
    }

    internal struct AssignmentPair
    {
        public Range Left;
        public Range Right;
    }

    internal class Range
    {
        public readonly int Begin;
        public readonly int End;

        public Range(int begin, int end)
        {
            if (begin > end) throw new ArgumentOutOfRangeException();
            Begin = begin;
            End = end;
        }

        public bool Contains(Range other)
        {
            return (other.Begin >= Begin && other.End <= End);
        }

        internal bool Overlaps(Range other)
        {
            return (Math.Min(End, other.End) - Math.Max(Begin, other.Begin) >= 0);
        }
    }
}