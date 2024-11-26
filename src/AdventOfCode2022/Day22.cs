namespace AdventOfCode2022
{
    public class Day22
    {
        [Fact]
        public void Part1()
        {
            Puzzle puzzle = LoadPuzzle();
            Grid2<Tile> map = puzzle.Map;

            Point2 position = map.Rows.Skip(1).First().AllPoints.First(p => map[p] == Tile.Open);
            Direction direction = Direction.Right;

            foreach (string step in puzzle.Path)
            {
                if (step is "L")
                {
                    direction = (Direction)(((int)direction + 3) % 4);
                }
                else if (step is "R")
                {
                    direction = (Direction)(((int)direction + 1) % 4);
                }
                else
                {
                    position = Move(map, position, direction, int.Parse(step));
                }
            }

            long result = (1000 * position.Y) + (4 * position.X) + (int)direction;
            Assert.Equal(56372, result);
        }

        [Fact]
        public void Part2()
        {
            Puzzle2 puzzle = LoadPuzzle2();
            Cube<TilePoint> map = puzzle.Map;

            Point2 position = Point2.Zero;
            Direction direction = Direction.Right;

            foreach (string step in puzzle.Path)
            {
                if (step is "L")
                {
                    direction = (Direction)(((int)direction + 3) % 4);
                }
                else if (step is "R")
                {
                    direction = (Direction)(((int)direction + 1) % 4);
                }
                else
                {
                    (position, map) = Move2(map, position, direction, int.Parse(step));
                }
            }

            Point2 diff = map[1, 1].Point - map[0, 0].Point;
            int rotate = (diff.X, diff.Y) switch
            {
                (1, 1) => 0,
                (-1, 1) => 1,
                (-1, -1) => 2,
                (1, -1) => 3,
                _ => throw new Exception()
            };

            long result = (1000 * (map[position].Point.Y + 1)) + (4 * (map[position].Point.X + 1)) + ((int)direction + rotate) % 4;
            Assert.Equal(197047, result);
        }

        private Point2 Move(Grid2<Tile> map, Point2 position, Direction direction, int distance)
        {
            while (distance-- > 0)
            {
                Point2 next = direction switch
                {
                    Direction.Right => position + Point2.UnitX,
                    Direction.Down => position + Point2.UnitY,
                    Direction.Left => position - Point2.UnitX,
                    Direction.Up => position - Point2.UnitY,
                    _ => throw new Exception()
                };

                if (map[next] is Tile.None)
                {
                    next = direction switch
                    {
                        Direction.Right => map.Rows[position.Y].AllPoints.First(p => map[p] is Tile.Open or Tile.Wall),
                        Direction.Down => map.Columns[position.X].AllPoints.First(p => map[p] is Tile.Open or Tile.Wall),
                        Direction.Left => map.Rows[position.Y].AllPoints.Reverse().First(p => map[p] is Tile.Open or Tile.Wall),
                        Direction.Up => map.Columns[position.X].AllPoints.Reverse().First(p => map[p] is Tile.Open or Tile.Wall),
                        _ => throw new Exception()
                    };
                }

                if (map[next] is Tile.Wall)
                {
                    break;
                }

                position = next;
            }

            return position;
        }

        private (Point2, Cube<TilePoint>) Move2(Cube<TilePoint> map, Point2 position, Direction direction, int distance)
        {
            while (distance-- > 0)
            {
                Cube<TilePoint> nextMap = map;

                Point2 next = direction switch
                {
                    Direction.Right => position + Point2.UnitX,
                    Direction.Down => position + Point2.UnitY,
                    Direction.Left => position - Point2.UnitX,
                    Direction.Up => position - Point2.UnitY,
                    _ => throw new Exception()
                };

                if (!map.Face.InBounds(next))
                {
                    nextMap = direction switch
                    {
                        Direction.Right => map.RotateLeft(),
                        Direction.Down => map.RotateUp(),
                        Direction.Left => map.RotateRight(),
                        Direction.Up => map.RotateDown(),
                        _ => throw new Exception()
                    };

                    next = direction switch
                    {
                        Direction.Right => new Point2(0, position.Y),
                        Direction.Down => new Point2(position.X, 0),
                        Direction.Left => new Point2(map.Face.Bounds.X - 1, position.Y),
                        Direction.Up => new Point2(position.X, map.Face.Bounds.Y - 1),
                        _ => throw new Exception()
                    };
                }

                if (nextMap[next].Tile is Tile.Wall)
                {
                    break;
                }

                position = next;
                map = nextMap;
            }

            return (position, map);
        }

        private Puzzle LoadPuzzle()
        {
            string[][] input = PuzzleFile.ReadAllLineGroups("Day22.txt");
            int width = input[0].Select(l => l.Length).Max();

            Grid2<Tile> map = new Grid2<Tile>(width, input[0].Length);

            foreach (Point2 p in map.AllPoints)
            {
                if (p.X < input[0][p.Y].Length)
                {
                    map[p] = input[0][p.Y][p.X] switch
                    {
                        ' ' => Tile.None,
                        '.' => Tile.Open,
                        '#' => Tile.Wall,
                        _ => throw new Exception()
                    };
                }
                else
                {
                    map[p] = Tile.None;
                }
            }

            int pos = 0;
            int len = 0;
            string pathStr = input[1][0];
            List<string> path = new List<string>();

            for (; pos < pathStr.Length; pos++)
            {
                if (pathStr[pos] is 'L' or 'R')
                {
                    if (len > 0)
                    {
                        path.Add(pathStr.Substring(pos - len, len));
                        len = 0;
                    }

                    path.Add(pathStr[pos].ToString());
                }
                else
                {
                    len++;
                }
            }

            if (len > 0)
            {
                path.Add(pathStr.Substring(pos - len, len));
                len = 0;
            }

            return new Puzzle()
            {
                Map = map.SurroundWith(Tile.None),
                Path = path
            };
        }

        private Puzzle2 LoadPuzzle2()
        {
            string[][] input = PuzzleFile.ReadAllLineGroups("Day22.txt");

            int totalOrder = Math.Max(input[0].Select(l => l.Length).Max(), input[0].Length);
            int cubeOrder = totalOrder / 4;

            Grid2<TilePoint> grid = new Grid2<TilePoint>(totalOrder, totalOrder);

            foreach (Point2 p in grid.AllPoints)
            {
                Tile tile;

                if (p.Y < input[0].Length && p.X < input[0][p.Y].Length)
                {
                    tile = input[0][p.Y][p.X] switch
                    {
                        ' ' => Tile.None,
                        '.' => Tile.Open,
                        '#' => Tile.Wall,
                        _ => throw new Exception()
                    };
                }
                else
                {
                    tile = Tile.None;
                }

                grid[p] = new TilePoint()
                {
                    Tile = tile,
                    Point = p
                };
            }

            Grid2<Grid2<TilePoint>> regions = grid.Split(new Point2(cubeOrder, cubeOrder));
            Cube<TilePoint> cube = new Cube<TilePoint>(cubeOrder);

            Cube<TilePoint> orientedCube = cube; // Cube we will rotate up as we copy the map
            int column0 = 0;

            // Find the column of the first region on the map
            foreach (Point2 p in regions.Rows[0].AllPoints)
            {
                if (regions[p][0, 0].Tile != Tile.None)
                {
                    break;
                }

                column0++;
            }

            // Copy all the regions onto the cube
            for (int y = 0; y < 4; y++)
            {
                for (int xCnt = 0; xCnt < 4; xCnt++)
                {
                    int x = (xCnt + column0) % 4;
                    Grid2<TilePoint> region = regions[x, y];

                    // Copy this region if it contains data
                    if (region[0, 0].Tile != Tile.None)
                    {
                        foreach (Point2 p in orientedCube.Face.AllPoints)
                        {
                            if (orientedCube[p].Point != Point2.Zero || orientedCube[p].Tile != Tile.None)
                            {
                                throw new Exception();
                            }
                        }

                        foreach (Point2 p in region.AllPoints)
                        {
                            orientedCube[p] = region[p];
                        }
                    }

                    // Orient the temp cube to the next region column
                    orientedCube = orientedCube.RotateLeft();
                }

                // Cube is now back where we started on this row, find where to rotate it up
                if (y < 3)
                {
                    for (int xCnt = 0; xCnt < 4; xCnt++)
                    {
                        int x = (xCnt + column0) % 4;
                        Grid2<TilePoint> region = regions[x, y + 1];

                        // We found it
                        if (region[0, 0].Tile != Tile.None)
                        {
                            column0 = x;
                            orientedCube = orientedCube.RotateUp();
                            break;
                        }

                        // Orient the temp cube to the next region column
                        orientedCube = orientedCube.RotateLeft();
                    }
                }
            }

            int pos = 0;
            int len = 0;
            string pathStr = input[1][0];
            List<string> path = new List<string>();

            for (; pos < pathStr.Length; pos++)
            {
                if (pathStr[pos] is 'L' or 'R')
                {
                    if (len > 0)
                    {
                        path.Add(pathStr.Substring(pos - len, len));
                        len = 0;
                    }

                    path.Add(pathStr[pos].ToString());
                }
                else
                {
                    len++;
                }
            }

            if (len > 0)
            {
                path.Add(pathStr.Substring(pos - len, len));
                len = 0;
            }

            return new Puzzle2()
            {
                Map = cube,
                Path = path
            };
        }

        private class Puzzle
        {
            internal Grid2<Tile> Map;
            internal List<string> Path;
        }

        private class Puzzle2
        {
            internal Cube<TilePoint> Map;
            internal List<string> Path;
        }

        private enum Tile
        {
            None,
            Open,
            Wall
        }

        private struct TilePoint
        {
            internal Tile Tile;
            internal Point2 Point;
        }

        private enum Direction : int
        {
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3
        }
    }
}