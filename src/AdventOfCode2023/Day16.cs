
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
            cell.EntryDirections.Clear();
        }

        Reflect(puzzle, entryPoint, entryDirection);

        int answer = puzzle.Count(cell => cell.EntryDirections.Any());
        return answer;
    }

    private void Reflect(Grid2<Cell> puzzle, Point2 startingPoint, Direction startingDirection)
    {
        Queue<(Point2, Direction)> queue = new Queue<(Point2<int>, Direction)>();
        queue.Enqueue((startingPoint, startingDirection));

        while (queue.Any())
        {
            (Point2 point, Direction direction) = queue.Dequeue();

            if (!puzzle.InBounds(point))
            {
                continue;
            }

            Cell cell = puzzle[point];

            if (!cell.EntryDirections.Add(direction))
            {
                continue;
            }

            switch (cell.Type)
            {
                case '.':
                    queue.Enqueue(Next(point, direction));
                    break;
                case '/':
                    if (direction.IsNorthOrSouth())
                    {
                        queue.Enqueue(Next(point, direction.TurnRight()));
                    }
                    else
                    {
                        queue.Enqueue(Next(point, direction.TurnLeft()));
                    }
                    break;
                case '\\':
                    if (direction.IsNorthOrSouth())
                    {
                        queue.Enqueue(Next(point, direction.TurnLeft()));
                    }
                    else
                    {
                        queue.Enqueue(Next(point, direction.TurnRight()));
                    }
                    break;
                case '|':
                    if (direction.IsNorthOrSouth())
                    {
                        queue.Enqueue(Next(point, direction));
                    }
                    else
                    {
                        queue.Enqueue(Next(point, direction.TurnLeft()));
                        queue.Enqueue(Next(point, direction.TurnRight()));
                    }
                    break;
                case '-':
                    if (direction.IsNorthOrSouth())
                    {
                        queue.Enqueue(Next(point, direction.TurnLeft()));
                        queue.Enqueue(Next(point, direction.TurnRight()));
                    }
                    else
                    {
                        queue.Enqueue(Next(point, direction));
                    }
                    break;
                default:
                    throw new Exception("Unknown Cell Type");
            }
        }
    }

    private (Point2, Direction) Next(Point2 point, Direction direction) => (point + direction, direction);

    private class Cell
    {
        public char Type;
        public HashSet<Direction> EntryDirections = new HashSet<Direction>();
    }
}
