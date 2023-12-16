
namespace AdventOfCode2023;

public class Day16
{
    [Fact]
    public void Part1()
    {
        Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day16.txt", ch => new Cell() { Type = ch });

        int answer = RunPuzzle(puzzle, Point2.Zero, Direction.East);

        Assert.Equal(8112, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = 0;
        Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day16.txt", ch => new Cell() { Type = ch });

        foreach (Point2 point in puzzle.EdgePoints)
        {
            if (point.X == 0)
            {
                answer = Math.Max(answer, RunPuzzle(puzzle, point, Direction.East));
            }
            if (point.X == puzzle.Bounds.X - 1)
            {
                answer = Math.Max(answer, RunPuzzle(puzzle, point, Direction.West));
            }
            if (point.Y == 0)
            {
                answer = Math.Max(answer, RunPuzzle(puzzle, point, Direction.South));
            }
            if (point.Y == puzzle.Bounds.Y - 1)
            {
                answer = Math.Max(answer, RunPuzzle(puzzle, point, Direction.North));
            }
        }

        Assert.Equal(8314, answer);
    }

    private int RunPuzzle(Grid2<Cell> puzzle, Point2 entryPoint, Direction entryDirection)
    {
        foreach (Cell cell in puzzle)
        {
            cell.EntryDirections = Direction.None;
        }

        Reflect(puzzle, entryPoint, entryDirection);

        int answer = puzzle.Count(cell => cell.EntryDirections != Direction.None);
        return answer;
    }

    private void Reflect(Grid2<Cell> puzzle, Point2 entryPoint, Direction entryDirection)
    {
        if (!puzzle.InBounds(entryPoint))
        {
            return;
        }

        Cell cell = puzzle[entryPoint];

        if (cell.EntryDirections.HasFlag(entryDirection))
        {
            return;
        }

        cell.EntryDirections |= entryDirection;

        switch (cell.Type)
        {
            case '.':
                ReflectStraight(puzzle, entryPoint, entryDirection);
                break;
            case '/':
                if (entryDirection is Direction.North or Direction.South)
                {
                    ReflectRight(puzzle, entryPoint, entryDirection);
                }
                else
                {
                    ReflectLeft(puzzle, entryPoint, entryDirection);
                }
                break;
            case '\\':
                if (entryDirection is Direction.North or Direction.South)
                {
                    ReflectLeft(puzzle, entryPoint, entryDirection);
                }
                else
                {
                    ReflectRight(puzzle, entryPoint, entryDirection);
                }
                break;
            case '|':
                if (entryDirection is Direction.North or Direction.South)
                {
                    ReflectStraight(puzzle, entryPoint, entryDirection);
                }
                else
                {
                    ReflectLeft(puzzle, entryPoint, entryDirection);
                    ReflectRight(puzzle, entryPoint, entryDirection);
                }
                break;
            case '-':
                if (entryDirection is Direction.North or Direction.South)
                {
                    ReflectLeft(puzzle, entryPoint, entryDirection);
                    ReflectRight(puzzle, entryPoint, entryDirection);
                }
                else
                {
                    ReflectStraight(puzzle, entryPoint, entryDirection);
                }
                break;
            default:
                throw new Exception("Unknown Cell Type");
        }
    }

    private void ReflectStraight(Grid2<Cell> puzzle, Point2 entryPoint, Direction entryDirection)
    {
        Reflect(puzzle, Next(entryPoint, entryDirection), entryDirection);
    }

    private void ReflectLeft(Grid2<Cell> puzzle, Point2 entryPoint, Direction entryDirection)
    {
        Direction direction = TurnLeft(entryDirection);
        Reflect(puzzle, Next(entryPoint, direction), direction);
    }

    private void ReflectRight(Grid2<Cell> puzzle, Point2 entryPoint, Direction entryDirection)
    {
        Direction direction = TurnRight(entryDirection);
        Reflect(puzzle, Next(entryPoint, direction), direction);
    }

    private Point2 Next(Point2 point, Direction direction)
    {
        return point + direction switch
        {
            Direction.North => -Point2.UnitY,
            Direction.South => Point2.UnitY,
            Direction.West => -Point2.UnitX,
            Direction.East => Point2.UnitX,
            _ => throw new Exception("No Direction")
        };
    }

    private Direction TurnLeft(Direction direction) => direction switch
    {
        Direction.North => Direction.West,
        Direction.South => Direction.East,
        Direction.West => Direction.South,
        Direction.East => Direction.North,
        _ => throw new Exception("No Direction")
    };

    private Direction TurnRight(Direction direction) => direction switch
    {
        Direction.North => Direction.East,
        Direction.South => Direction.West,
        Direction.West => Direction.North,
        Direction.East => Direction.South,
        _ => throw new Exception("No Direction")
    };

    private class Cell
    {
        public char Type;
        public Direction EntryDirections;
    }

    [Flags]
    private enum Direction
    {
        None = 0,
        North = 1,
        South = 2,
        East = 4,
        West = 8
    }
}
