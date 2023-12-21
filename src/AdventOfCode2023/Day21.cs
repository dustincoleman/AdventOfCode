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
        int answer = 1;
        Assert.Equal(0, answer);
    }
}
