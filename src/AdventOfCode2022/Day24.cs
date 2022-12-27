using System.Linq;

namespace AdventOfCode2022
{
    public class Day24
    {
        [Fact]
        public void Part1()
        {
            Puzzle puzzle = LoadPuzzle();
            int result = RunPuzzle(puzzle, Point2.Zero, puzzle.Bounds - 1);
            Assert.Equal(277, result);
        }

        [Fact]
        public void Part2()
        {
            Puzzle puzzle = LoadPuzzle();
            int result = RunPuzzle(puzzle, Point2.Zero, puzzle.Bounds - 1);
            result = RunPuzzle(puzzle, puzzle.Bounds - 1, Point2.Zero, result);
            result = RunPuzzle(puzzle, Point2.Zero, puzzle.Bounds - 1, result);
            Assert.Equal(277, result);
        }

        private int RunPuzzle(Puzzle puzzle, Point2 start, Point2 end, int minute = 1)
        {
            List<Point2> list = new List<Point2>();
            HashSet<Point2> hashSet = new HashSet<Point2>();
            
            for (;; minute++)
            {
                Grid2<bool> valley = puzzle.GetState(minute);
                Point2[] array = list.ToArray();
                list.Clear();
                hashSet.Clear();

                foreach (Point2 p in array)
                {
                    if (p == end)
                    {
                        return minute;
                    }

                    if (!valley[p] && hashSet.Add(p))
                    {
                        list.Add(p); // Wait
                    }

                    foreach (Point2 adj in p.Adjacent(valley.Bounds))
                    {
                        if (!valley[adj] && hashSet.Add(adj))
                        {
                            list.Add(adj); // Move
                        }
                    }
                }

                if (!valley[start] && hashSet.Add(start))
                {
                    list.Add(start); // Start
                }
            }
        }

        private Puzzle LoadPuzzle()
        {
            string[] lines = File.ReadAllLines("Day24.txt");
            Puzzle puzzle = new Puzzle();

            puzzle.Bounds = new Point2(lines[0].Length - 2, lines.Length - 2);

            foreach (Point2 p in Point2.Quadrant(puzzle.Bounds))
            {
                char ch = lines[p.Y + 1][p.X + 1];
                Blizzard b = null;

                if (ch is '^')
                {
                    b = new Blizzard() { Location = p, Direction = Direction.Up };
                }
                else if (ch is 'v')
                {
                    b = new Blizzard() { Location = p, Direction = Direction.Down };
                }
                else if (ch is '<')
                {
                    b = new Blizzard() { Location = p, Direction = Direction.Left };
                }
                else if (ch is '>')
                {
                    b = new Blizzard() { Location = p, Direction = Direction.Right };
                }

                if (b is not null)
                {
                    puzzle.Blizzards.Add(b);
                }
            }

            return puzzle;
        }

        private class Puzzle
        {
            internal Point2 Bounds;
            internal List<Blizzard> Blizzards = new List<Blizzard>();
            internal List<Grid2<bool>> States = new List<Grid2<bool>>();

            internal Grid2<bool> GetState(int minute)
            {
                while (minute >= States.Count)
                {
                    ComputeNextState();
                }

                return States[minute];
            }

            private void ComputeNextState()
            {
                Grid2<bool> grid = new Grid2<bool>(Bounds);

                foreach (Blizzard b in Blizzards)
                {
                    grid[b.Location] = true;

                    if (b.Direction is Direction.Up)
                    {
                        b.Location -= Point2.UnitY;
                        if (!grid.InBounds(b.Location))
                        {
                            b.Location = new Point2(b.Location.X, grid.Bounds.Y - 1);
                        }
                    }
                    else if (b.Direction is Direction.Down)
                    {
                        b.Location += Point2.UnitY;
                        if (!grid.InBounds(b.Location))
                        {
                            b.Location = new Point2(b.Location.X, 0);
                        }
                    }
                    else if (b.Direction is Direction.Left)
                    {
                        b.Location -= Point2.UnitX;
                        if (!grid.InBounds(b.Location))
                        {
                            b.Location = new Point2(grid.Bounds.X - 1, b.Location.Y);
                        }
                    }
                    else if (b.Direction is Direction.Right)
                    {
                        b.Location += Point2.UnitX;
                        if (!grid.InBounds(b.Location))
                        {
                            b.Location = new Point2(0, b.Location.Y);
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                States.Add(grid);
            }
        }

        private class Blizzard
        {
            internal Point2 Location;
            internal Direction Direction;
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}