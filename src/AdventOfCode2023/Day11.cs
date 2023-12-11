namespace AdventOfCode2023;

public class Day11
{
    [Fact]
    public void Part1()
    {
        List<Point2Big> puzzle = LoadPuzzle(2);
        long answer = 0;

        for (int i = 0; i < puzzle.Count; i++)
        {
            for (int j = i + 1; j < puzzle.Count; j++)
            {
                answer += (puzzle[j] - puzzle[i]).Manhattan();
            }
        }

        Assert.Equal(10494813, answer);
    }

    [Fact]
    public void Part2()
    {
        List<Point2Big> puzzle = LoadPuzzle(1000000);
        long answer = 0;

        for (int i = 0; i < puzzle.Count; i++)
        {
            for (int j = i + 1; j < puzzle.Count; j++)
            {
                answer += (puzzle[j] - puzzle[i]).Manhattan();
            }
        }

        Assert.Equal(840988812853, answer);
    }

    private List<Point2Big> LoadPuzzle(long expansion)
    {
        List<Point2Big> list = new List<Point2Big>();
        Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day11.txt");

        List<int> emptyRows = new List<int>();
        List<int> emptyColumns = new List<int>();

        for (int i = 0; i < puzzle.Rows.Count; i++)
        {
            if (puzzle.Rows[i].All(ch => ch is '.'))
            {
                emptyRows.Add(i);
            }
        }

        for (int i = 0; i < puzzle.Columns.Count; i++)
        {
            if (puzzle.Columns[i].All(ch => ch is '.'))
            {
                emptyColumns.Add(i);
            }
        }

        foreach (Point2 point in puzzle.Points)
        {
            if (puzzle[point] == '#')
            {
                Point2Big offset = new Point2Big(emptyColumns.Count(x => x < point.X), emptyRows.Count(y => y < point.Y)) * (expansion - 1);
                list.Add(new Point2Big(point.X, point.Y) + offset);
            }
        }

        return list;
    }
}
