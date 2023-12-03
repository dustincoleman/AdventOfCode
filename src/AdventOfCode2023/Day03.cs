namespace AdventOfCode2023;

public class Day03
{
    [Fact]
    public void Part1()
    {
        int answer = GetNumbersAdjacentToSymbols(PuzzleFile.AsGrid("Day03.txt")).Sum();
        Assert.Equal(528799, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = GetNumbersAdjacentToGears(PuzzleFile.AsGrid("Day03.txt")).Where(list => list.Count == 2).Sum(list => list[0] * list[1]);
        Assert.Equal(84907174, answer);
    }

    private List<int> GetNumbersAdjacentToSymbols(Grid2<char> grid)
    {
        List<int> list = new List<int>();

        foreach (Grid2Row<char> row in grid.Rows)
        {
            for (int pos = 0; pos < row.Count; pos++)
            {
                int value = 0;
                bool foundSymbol = false;

                while (pos < row.Count && char.IsAsciiDigit(row[pos]))
                {
                    value *= 10;
                    value += row[pos] - '0';
                    foundSymbol |= row.Surrounding(pos).Any(ch => ch != '.' && !char.IsAsciiDigit(ch));
                    pos++;
                }

                if (foundSymbol)
                {
                    list.Add(value);
                }
            }
        }

        return list;
    }

    private ICollection<List<int>> GetNumbersAdjacentToGears(Grid2<char> grid)
    {
        AutoDictionary<Point2, List<int>> numbersByGear = new AutoDictionary<Point2, List<int>>();

        foreach (Grid2Row<char> row in grid.Rows)
        {
            for (int pos = 0; pos < row.Count; pos++)
            {
                int value = 0;
                HashSet<Point2> gears = new HashSet<Point2>();

                while (pos < row.Count && char.IsAsciiDigit(row[pos]))
                {
                    value *= 10;
                    value += row[pos] - '0';

                    foreach (Point2 point in row.SurroundingPoints(pos))
                    {
                        if (grid[point] == '*')
                        {
                            gears.Add(point);
                        }
                    }

                    pos++;
                }

                if (value > 0)
                {
                    foreach (Point2 point in gears)
                    {
                        numbersByGear[point].Add(value);
                    }
                }
            }
        }

        return numbersByGear.Values;
    }
}
