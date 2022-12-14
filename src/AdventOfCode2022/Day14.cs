namespace AdventOfCode2022
{
    public class Day14
    {
        [Fact]
        public void Part1()
        {
            Puzzle puzzle = LoadPuzzle();
            Grid2<bool> cave = puzzle.Cave;

            Point2[] moves = new Point2[]
            {
                Point2.UnitY,
                Point2.UnitY - Point2.UnitX,
                Point2.UnitY + Point2.UnitX
            };

            Point2 TryFindNextRestingPlace()
            {
                Point2 sand = new Point2(500, 0);
                bool falling = true;

                while (falling && sand.Y <= puzzle.LowestPoint)
                {
                    falling = false;

                    foreach (Point2 move in moves)
                    {
                        Point2 temp = sand + move;
                        if (!cave[temp])
                        {
                            sand = temp;
                            falling = true;
                            break;
                        }
                    }
                }

                return sand;
            }

            int count = 0;

            while (true)
            {
                Point2 sand = TryFindNextRestingPlace();

                if (sand.Y > puzzle.LowestPoint)
                {
                    break;
                }

                cave[sand] = true;
                count++;
            }


            Assert.Equal(832, count);
        }

        [Fact]
        public void Part2()
        {
            Puzzle puzzle = LoadPuzzle();
            Grid2<bool> cave = puzzle.Cave;

            foreach (Point2 p in Point2.Line(new Point2(0, puzzle.LowestPoint + 2), new Point2(cave.Bounds.X - 1, puzzle.LowestPoint + 2)))
            {
                cave[p] = true;
            }

            Point2[] moves = new Point2[]
            {
                Point2.UnitY,
                Point2.UnitY - Point2.UnitX,
                Point2.UnitY + Point2.UnitX
            };

            Point2 TryFindNextRestingPlace()
            {
                Point2 sand = new Point2(500, 0);
                bool falling = true;

                while (falling)
                {
                    falling = false;

                    foreach (Point2 move in moves)
                    {
                        Point2 temp = sand + move;
                        if (!cave[temp])
                        {
                            sand = temp;
                            falling = true;
                            break;
                        }
                    }
                }

                return sand;
            }

            int count = 0;
            Point2 origin = new Point2(500, 0);

            while (true)
            {
                Point2 sand = TryFindNextRestingPlace();

                if (sand == origin)
                {
                    count++;
                    break;
                }

                cave[sand] = true;
                count++;
            }


            Assert.Equal(27601, count);
        }

        private Puzzle LoadPuzzle()
        {
            Grid2<bool> cave = new Grid2<bool>(1000, 200);
            int lowest = 0;

            foreach (string line in File.ReadAllLines("Day14.txt"))
            {
                Point2[] points = line.Split(" -> ").Select(Point2.Parse).ToArray();

                for (int i = 0; i < points.Length - 1; i++)
                {
                    foreach (Point2 p in Point2.Line(points[i], points[i + 1]))
                    {
                        cave[p] = true;
                        lowest = Math.Max(p.Y, lowest);
                    }
                }
            }

            return new Puzzle()
            {
                Cave = cave,
                LowestPoint = lowest
            };
        }

        private class Puzzle
        {
            internal Grid2<bool> Cave;
            internal int LowestPoint;
        }
    }
}