namespace AdventOfCode2023;

public class Day21
{
    [Fact]
    public void Part1()
    {
        Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day21.txt");
        HashSet<Point2> positions = new HashSet<Point2>() { puzzle.Points.Where(p => puzzle[p] == 'S').Single() };
        int steps = 64;

        while (steps-- > 0)
        {
            Point2[] currentPositions = positions.ToArray();
            positions.Clear();

            foreach (Point2 pos in currentPositions)
            {
                foreach (Direction direction in puzzle.DirectionsInBounds(pos))
                {
                    Point2 step = pos + direction;
                    if (puzzle[step] != '#')
                    {
                        positions.Add(pos + direction);
                    }
                }
            }
        }

        int answer = positions.Count;
        Assert.Equal(3731, answer);
    }

    const long totalSteps = 26501365;
    const int oddSteps = (int)(totalSteps % 2);

    [Fact]
    public void Part2()
    {
        Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day21.txt", (ch, pt) => new Cell(ch == '#', pt));

        //*********************************************//
        //             Brute Force Count               //
        //*********************************************//
        //var expansion = Expand(Expand(Expand(puzzle)));
        //Visit(expansion, expansion.CenterPoint, (int)totalSteps);
        //string str = ToString(expansion);
        //int cnt = expansion.Count(c => c.MinDistance < int.MaxValue && (c.MinDistance % 2) == oddSteps);

        int size = puzzle.Bounds.X;
        int center = puzzle.CenterPoint.X;

        // Puzzle input is a square map
        if (puzzle.Bounds.X != puzzle.Bounds.Y)
        {
            throw new Exception("Unexpected puzzle input");
        }

        // Puzzle input has a straight line to each edge from the start
        for (int i = 0; i < size; i++)
        {
            if (puzzle[i, center].IsBlock || puzzle[center, i].IsBlock)
            {
                throw new Exception("Unexpected puzzle input");
            }
        }

        // Puzzle has no obstructions on the edge
        foreach (Point2 pt in puzzle.EdgePoints)
        {
            if (puzzle[pt].IsBlock)
            {
                throw new Exception("Unexpected puzzle input");
            }
        }

        // Puzzle has total step count that perfectly reaches the edge
        if ((totalSteps - (size / 2)) % size != 0)
        {
            throw new Exception("Unexpected puzzle input");
        }

        long answer = 0;
        long reach = (totalSteps - (size / 2)) / size; // How many repeats of the garden do we enter moving from center to edge?
        long n = reach - 1;

        // Full Gardens
        int count = Count(puzzle, puzzle.CenterPoint, int.MaxValue);
        int altCount = Count(puzzle, puzzle.CenterPoint, int.MaxValue, alternates: true);

        // "Rings" moving outward alternate visited plots
        for (int i = 0; i <= n; i++)
        {
            long multiple = (i == 0) ? 1 : (long)4 * i;
            long spaces = (i % 2 == 0) ? count : altCount;
            answer += spaces * multiple;
        }

        // Poles (N, E, S, W)
        answer += Count(puzzle, puzzle.SouthCenter, size - 1, alternates: reach % 2 == 0); // North
        answer += Count(puzzle, puzzle.EastCenter, size - 1, alternates: reach % 2 == 0); // West
        answer += Count(puzzle, puzzle.NorthCenter, size - 1, alternates: reach % 2 == 0); // South
        answer += Count(puzzle, puzzle.WestCenter, size - 1, alternates: reach % 2 == 0); // East

        // Diagonal plots
        int small = (size / 2) - 1;
        int large = small + size;
        answer += Count(puzzle, puzzle.SECorner, small, alternates: reach % 2 == 0) * (n + 1);
        answer += Count(puzzle, puzzle.SECorner, large, alternates: reach % 2 == 1) * n;
        answer += Count(puzzle, puzzle.SWCorner, small, alternates: reach % 2 == 0) * (n + 1);
        answer += Count(puzzle, puzzle.SWCorner, large, alternates: reach % 2 == 1) * n;
        answer += Count(puzzle, puzzle.NECorner, small, alternates: reach % 2 == 0) * (n + 1);
        answer += Count(puzzle, puzzle.NECorner, large, alternates: reach % 2 == 1) * n;
        answer += Count(puzzle, puzzle.NWCorner, small, alternates: reach % 2 == 0) * (n + 1);
        answer += Count(puzzle, puzzle.NWCorner, large, alternates: reach % 2 == 1) * n;

        Assert.Equal(617565692567199, answer);
    }

    private string ToString(Grid2<Cell> puzzle)
    {
        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < puzzle.Bounds.Y; y++)
        {
            for (int x = 0; x < puzzle.Bounds.X; x++)
            {
                sb.Append((puzzle[(x, y)].MinDistance < int.MaxValue) ? puzzle[(x, y)].MinDistance.ToString("00") : "  ");
                sb.Append(' ');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private Grid2<Cell> Expand(Grid2<Cell> puzzle)
    {
        Grid2<Cell> expansion = new Grid2<Cell>(puzzle.Bounds * 3);
        foreach (Point2 point in puzzle.Points)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Cell cell = puzzle[point];
                    Point2 dest = (point.X + (x * puzzle.Bounds.X), point.Y + (y * puzzle.Bounds.Y));
                    expansion[dest] = new Cell(cell.IsBlock, dest);
                }
            }
        }
        return expansion;
    }

    private void Visit(Grid2<Cell> puzzle, Point2 startPoint, int limit)
    {
        Cell start = puzzle[startPoint];
        start.MinDistance = 0;

        List<Cell> queue = new List<Cell>() { start };

        while (queue.Count != 0)
        {
            queue.Sort((left, right) => right.MinDistance.CompareTo(left.MinDistance));

            Cell cell = queue[queue.Count - 1];
            int distance = cell.MinDistance + 1;
            queue.RemoveAt(queue.Count - 1);

            if (distance <= limit)
            {
                foreach (Cell next in puzzle.Adjacent(cell.Location).Where(cell => !cell.IsBlock))
                {
                    if (next.MinDistance > distance)
                    {
                        next.MinDistance = distance;
                        next.Previous = cell;
                        queue.Add(next);
                    }
                }
            }
        }
    }

    private int Count(Grid2<Cell> puzzle, Point2 startPoint, int limit, bool alternates = false)
    {
        foreach (Cell cell in puzzle)
        {
            cell.MinDistance = int.MaxValue;
        }

        Cell start = puzzle[startPoint];
        start.MinDistance = 0;

        List<Cell> queue = new List<Cell>() { start };

        while (queue.Count != 0)
        {
            queue.Sort((left, right) => right.MinDistance.CompareTo(left.MinDistance));

            Cell cell = queue[queue.Count - 1];
            int distance = cell.MinDistance + 1;
            queue.RemoveAt(queue.Count - 1);

            if (distance <= limit)
            {
                foreach (Cell next in puzzle.Adjacent(cell.Location).Where(cell => !cell.IsBlock))
                {
                    if (next.MinDistance > distance)
                    {
                        next.MinDistance = distance;
                        next.Previous = cell;
                        queue.Add(next);
                    }
                }
            }
        }

        int check = oddSteps;

        if (alternates)
        {
            check = (check + 1) % 2;
        }

        return puzzle.Where(cell => cell.MinDistance != int.MaxValue && cell.MinDistance % 2 == check).Count();
    }

    private class Cell
    {
        internal int MinDistance = int.MaxValue;
        internal Cell Previous = null;

        internal Cell(bool isBlock, Point2 pt)
        {
            this.IsBlock = isBlock;
            this.Location = pt;
        }

        internal bool IsBlock { get; }
        internal Point2 Location { get; }
    }
}
