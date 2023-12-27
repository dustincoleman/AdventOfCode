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

    [Fact]
    public void Part2()
    {
        Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day21.txt", (ch, pt) => new Cell() { Char = ch, Point = pt });
        Cell startingPoint = puzzle.Where(c => c.Char == 'S').Single();
        HashSet<Cell> set = new HashSet<Cell>() { startingPoint };
        startingPoint.Distance = 0;

        while(set.Count > 0)
        {
            Cell cell = set.OrderBy(c => c.Distance).First();
            set.Remove(cell);

            foreach (Cell neighbor in puzzle.Adjacent(cell.Point).Where(c => c.Char != '#'))
            {
                int distance = cell.Distance + 1;
                if (neighbor.Distance > distance)
                {
                    neighbor.Distance = distance;
                    set.Add(neighbor);
                }
            }
        }

        Cell[] all = puzzle.OrderBy(c => c.Distance).ToArray();
        Cell[] perimeter = puzzle.EdgePoints.Select(p => puzzle[p]).OrderBy(c => c.Distance).ToArray();

        int answer = 1;
        Assert.Equal(0, answer);
    }

    private class Cell
    {
        public char Char;
        public Point2 Point;
        public int Distance = int.MaxValue;
    }
}
