namespace AdventOfCode2023;

public class Day11
{
    [Fact]
    public void Part1()
    {
        long answer = RunPuzzle(2);
        Assert.Equal(10494813, answer);
    }

    [Fact]
    public void Part2()
    {
        long answer = RunPuzzle(1000000);
        Assert.Equal(840988812853, answer);
    }

    private long RunPuzzle(long expansion)
    {
        List<Point2<long>> puzzle = LoadPuzzle(expansion);
        long answer = 0;

        for (int i = 0; i < puzzle.Count; i++)
        {
            for (int j = i + 1; j < puzzle.Count; j++)
            {
                answer += (puzzle[j] - puzzle[i]).Manhattan();
            }
        }

        return answer;
    }

    private List<Point2<long>> LoadPuzzle(long expansion)
    {
        List<Point2<long>> list = new List<Point2<long>>();
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

        foreach (Point2 point in puzzle.AllPoints)
        {
            if (puzzle[point] == '#')
            {
                Point2<long> offset = new Point2<long>(emptyColumns.Count(x => x < point.X), emptyRows.Count(y => y < point.Y)) * (expansion - 1);
                list.Add(point.As<long>() + offset);
            }
        }

        return list;
    }
}
