namespace AdventOfCode2024
{
    public class Day14
    {
        [Fact]
        public void Part1()
        {
            List<Robot> puzzle = LoadPuzzle();
            Point2 bounds = new Point2(101, 103); // 11, 7   101, 103

            for (int i = 0; i < 100; i++)
            {
                foreach (Robot robot in puzzle)
                {
                    int x = (robot.Location.X + robot.Velocity.X + bounds.X) % bounds.X;
                    int y = (robot.Location.Y + robot.Velocity.Y + bounds.Y) % bounds.Y;
                    robot.Location = (x, y);
                }
            }

            Point2 midline = bounds / 2;
            long result =
                puzzle.Count(r => r.Location.X < midline.X && r.Location.Y < midline.Y) *
                puzzle.Count(r => r.Location.X < midline.X && r.Location.Y > midline.Y) *
                puzzle.Count(r => r.Location.X > midline.X && r.Location.Y < midline.Y) *
                puzzle.Count(r => r.Location.X > midline.X && r.Location.Y > midline.Y);

            Assert.Equal(215476074, result);
        }

        [Fact]
        public void Part2()
        {
            List<Robot> puzzle = LoadPuzzle();
            Point2 bounds = new Point2(101, 103); // 11, 7   101, 103
            Point2 midline = bounds / 2;
            long result = 0;

            for (int i = 0; i < 10000; i++)
            {
                HashSet<int>[] hashsets = [new HashSet<int>(), new HashSet<int>()];

                foreach (Robot robot in puzzle)
                {
                    int x = (robot.Location.X + robot.Velocity.X + bounds.X) % bounds.X;
                    int y = (robot.Location.Y + robot.Velocity.Y + bounds.Y) % bounds.Y;
                    robot.Location = (x, y);

                    if (robot.Location.X == midline.X - 1)
                    {
                        hashsets[0].Add(robot.Location.Y);
                    }
                    if (robot.Location.X == midline.X)
                    {
                        hashsets[1].Add(robot.Location.Y);
                    }
                }

                if (hashsets[0].Count > 25 && hashsets[1].Count > 25)
                {
                    string print = Print(puzzle, bounds);
                    result = i + 1;
                }
            }

            Assert.Equal(6285, result);
        }

        private string Print(List<Robot> puzzle, Point2<int> bounds)
        {
            Grid2<char> grid = new Grid2<char>(bounds);

            foreach (Point2 pt in grid.AllPoints)
            {
                grid[pt] = ' ';
            }
            foreach (Robot robot in puzzle)
            {
                grid[robot.Location] = 'X';
            }

            return grid.ToString(ch => ch.ToString());
        }

        private List<Robot> LoadPuzzle()
        {
            List<Robot> robots = new List<Robot>();

            foreach (string line in File.ReadAllLines("Day14.txt"))
            {
                string[] split = line.Replace("p=", string.Empty).Replace("v=", string.Empty).Split(' ');
                robots.Add(new Robot(Point2.Parse(split[0]), Point2.Parse(split[1])));
            }

            return robots;
        }

        private class Robot
        {
            internal Point2 Location;
            internal Point2 Velocity;

            internal Robot(Point2 location, Point2 velocity)
            {
                Location = location;
                Velocity = velocity;
            }
        }
    }
}
