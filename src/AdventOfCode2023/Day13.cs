

namespace AdventOfCode2023;

public class Day13
{
    [Fact]
    public void Part1()
    {
        int answer = 0;

        foreach (Grid2<char> grid in PuzzleFile.ReadLineGroupsAsGrids("Day13.txt"))
        {
            answer += Summarize(grid);
        }

        Assert.Equal(36448, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = 0;

        foreach (Grid2<char> grid in PuzzleFile.ReadLineGroupsAsGrids("Day13.txt"))
        {
            int withSmudge = Summarize(grid);

            foreach (Point2 point in grid.AllPoints)
            {
                char temp = grid[point];
                grid[point] = (temp == '.') ? '#' : '.';

                int withoutSmudge = Summarize(grid, ignore: withSmudge);
                if (withoutSmudge != 0)
                {
                    answer += withoutSmudge;
                    break;
                }

                grid[point] = temp;
            }
        }

        Assert.Equal(35799, answer);
    }

    private int Summarize(Grid2<char> grid, int? ignore = null)
    {
        for (int i = 0; i < grid.Columns.Count - 1; i++)
        {
            if (ReflectsAtColumn(grid, i) && (!ignore.HasValue || ignore.Value != (i + 1)))
            {
                return i + 1;
            }
        }

        for (int i = 0; i < grid.Rows.Count - 1; i++)
        {
            if (ReflectsAtRow(grid, i) && (!ignore.HasValue || ignore.Value != ((i + 1) * 100)))
            {
                return (i + 1) * 100;
            }
        }

        return 0;
    }

    private bool ReflectsAtColumn(Grid2<char> grid, int i)
    {
        int left = i;
        int right = i + 1;

        while (left >= 0 && right < grid.Columns.Count)
        {
            if (!grid.Columns[left--].Equals(grid.Columns[right++]))
            {
                return false;
            }
        }

        return true;
    }

    private bool ReflectsAtRow(Grid2<char> grid, int i)
    {
        int top = i;
        int bottom = i + 1;

        while (top >= 0 && bottom < grid.Rows.Count)
        {
            if (!grid.Rows[top--].Equals(grid.Rows[bottom++]))
            {
                return false;
            }
        }

        return true;
    }
}
