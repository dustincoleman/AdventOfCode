namespace AdventOfCode2022
{
    public class Day18
    {
        [Fact]
        public void Part1()
        {
            int result = 0;
            HashSet<Point3> points = new HashSet<Point3>(LoadPuzzle());

            foreach (Point3 point in points)
            {
                foreach (Point3 adj in point.Adjacent())
                {
                    if (!points.Contains(adj))
                    {
                        result++;
                    }
                }
            }

            Assert.Equal(3526, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 0;
            HashSet<Point3> points = new HashSet<Point3>(LoadPuzzle());

            Point3 min = points.Aggregate(Point3.Min);
            Point3 max = points.Aggregate(Point3.Max);

            bool IsExternal(Point3 current, HashSet<Point3> visited = null)
            {
                if (visited == null)
                {
                    visited = new HashSet<Point3>();
                }

                if (!visited.Add(current) || points.Contains(current))
                {
                    return false;
                }

                if (!(current.AllGreaterThanOrEqual(min) && current.AllLessThanOrEqual(max)))
                {
                    return true;
                }

                return
                    IsExternal(current + Point3.UnitX, visited) ||
                    IsExternal(current + Point3.UnitY, visited) ||
                    IsExternal(current + Point3.UnitZ, visited) ||
                    IsExternal(current - Point3.UnitX, visited) ||
                    IsExternal(current - Point3.UnitY, visited) ||
                    IsExternal(current - Point3.UnitZ, visited);
            }

            foreach (Point3 point in points)
            {
                foreach (Point3 adj in point.Adjacent())
                {
                    if (IsExternal(adj))
                    {
                        result++;
                    }
                }
            }

            Assert.Equal(2090, result);
        }

        private List<Point3> LoadPuzzle()
        {
            List<Point3> list = new List<Point3>();

            foreach (string line in File.ReadAllLines("Day18.txt"))
            {
                string[] split = line.Split(',');
                list.Add(new Point3(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
            }

            return list;
        }
    }
}