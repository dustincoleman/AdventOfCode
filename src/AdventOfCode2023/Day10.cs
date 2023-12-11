using System.Text;

namespace AdventOfCode2023;

public class Day10
{
    [Fact]
    public void Part1()
    {
        int count = 0;
        Grid2<Pipe> map = LoadPuzzle();

        TraversePuzzle(map, (pos, direction) => count++);

        int answer = count / 2;
        Assert.Equal(6979, answer);
    }

    [Fact]
    public void Part2()
    {
        Grid2<Pipe> map = LoadPuzzle();
        Grid2<SearchState> searchGrid = new Grid2<SearchState>((map.Bounds * 2) - 1);

        TraversePuzzle(map, (pos, direction) =>
        {
            Point2 searchPos = pos * 2;
            Point2 searchPosIntermediate = searchPos + direction switch
            {
                Direction.N => Point2.UnitY,
                Direction.E => -Point2.UnitX,
                Direction.S => -Point2.UnitY,
                Direction.W => Point2.UnitX,
                _ => throw new Exception("Unexpected")
            };

            searchGrid[searchPos] = SearchState.Path;
            searchGrid[searchPosIntermediate] = SearchState.Path;
        });

        foreach (Point2 edgePoint in searchGrid.EdgePoints)
        {
            MarkOutsidePoints(edgePoint, searchGrid);
        }

        int answer = searchGrid.Points.Count(p => p % 2 == Point2.Zero && searchGrid[p] is SearchState.None);

        Assert.Equal(443, answer);
    }

    private void MarkOutsidePoints(Point2 point, Grid2<SearchState> searchGrid)
    {
        if (searchGrid[point] is SearchState.None)
        {
            searchGrid[point] = SearchState.Outside;
            foreach (Point2 adjPoint in searchGrid.AdjacentPoints(point))
            {
                MarkOutsidePoints(adjPoint, searchGrid);
            }
        }
    }

    private void TraversePuzzle(Grid2<Pipe> map, Action<Point2, Direction> action)
    {
        Point2 pos = map.Points.First(p => map[p] is Pipe.AnimalStartPos);
        Direction direction = Direction.None;

        if (map.InBounds(pos - Point2.UnitY) && map[pos - Point2.UnitY] is Pipe.Vertical or Pipe.BendSW or Pipe.BendSE)
        {
            pos -= Point2.UnitY;
            direction = Direction.N;
        }
        else if (map.InBounds(pos + Point2.UnitX) && map[pos + Point2.UnitX] is Pipe.Horizontal or Pipe.BendNW or Pipe.BendSW)
        {
            pos += Point2.UnitX;
            direction = Direction.E;
        }
        else if (map.InBounds(pos + Point2.UnitY) && map[pos + Point2.UnitY] is Pipe.Vertical or Pipe.BendNW or Pipe.BendNE)
        {
            pos += Point2.UnitY;
            direction = Direction.S;
        }
        else if (map.InBounds(pos - Point2.UnitX) && map[pos - Point2.UnitX] is Pipe.Horizontal or Pipe.BendNE or Pipe.BendSE)
        {
            pos -= Point2.UnitX;
            direction = Direction.W;
        }
        else
        {
            throw new Exception("Unexpected");
        }

        action(pos, direction);

        while (map[pos] != Pipe.AnimalStartPos)
        {
            switch (map[pos])
            {
                case Pipe.Vertical:
                    pos += direction switch
                    {
                        Direction.N => -Point2.UnitY,
                        Direction.S => Point2.UnitY,
                        _ => throw new Exception("Unexpected")
                    };
                    break;
                case Pipe.Horizontal:
                    pos += direction switch
                    {
                        Direction.W => -Point2.UnitX,
                        Direction.E => Point2.UnitX,
                        _ => throw new Exception("Unexpected")
                    };
                    break;
                case Pipe.BendNW:
                    switch (direction)
                    {
                        case Direction.S:
                            pos -= Point2.UnitX;
                            direction = Direction.W;
                            break;
                        case Direction.E:
                            pos -= Point2.UnitY;
                            direction = Direction.N;
                            break;
                        default:
                            throw new Exception("Unexpected");
                    }
                    break;
                case Pipe.BendNE:
                    switch (direction)
                    {
                        case Direction.S:
                            pos += Point2.UnitX;
                            direction = Direction.E;
                            break;
                        case Direction.W:
                            pos -= Point2.UnitY;
                            direction = Direction.N;
                            break;
                        default:
                            throw new Exception("Unexpected");
                    }
                    break;
                case Pipe.BendSW:
                    switch (direction)
                    {
                        case Direction.N:
                            pos -= Point2.UnitX;
                            direction = Direction.W;
                            break;
                        case Direction.E:
                            pos += Point2.UnitY;
                            direction = Direction.S;
                            break;
                        default:
                            throw new Exception("Unexpected");
                    }
                    break;
                case Pipe.BendSE:
                    switch (direction)
                    {
                        case Direction.N:
                            pos += Point2.UnitX;
                            direction = Direction.E;
                            break;
                        case Direction.W:
                            pos += Point2.UnitY;
                            direction = Direction.S;
                            break;
                        default:
                            throw new Exception("Unexpected");
                    }
                    break;
                default:
                    throw new Exception("Unexpected");
            }

            action(pos, direction);
        }
    }

    private Grid2<Pipe> LoadPuzzle()
    {
        string[] input = File.ReadAllLines("Day10.txt");

        Point2 bounds = new Point2(input[0].Length, input.Length);
        Grid2<Pipe> grid = new Grid2<Pipe>(bounds);
        
        foreach (Point2 p in Point2.Quadrant(bounds))
        {
            grid[p] = input[p.Y][p.X] switch
            {
                '.' => Pipe.None,
                '|' => Pipe.Vertical,
                '-' => Pipe.Horizontal,
                'L' => Pipe.BendNE,
                'J' => Pipe.BendNW,
                '7' => Pipe.BendSW,
                'F' => Pipe.BendSE,
                'S' => Pipe.AnimalStartPos,
                _ => throw new Exception("Unexpected input")
            }; ;
        }

        return grid;
    }

    private enum Pipe
    {
        None,
        Vertical,
        Horizontal,
        BendNW,
        BendNE,
        BendSW,
        BendSE,
        AnimalStartPos
    }

    private enum Direction
    {
        None,
        N,
        S,
        W,
        E
    }

    private enum SearchState
    {
        None,
        Path,
        Outside
    }
}
