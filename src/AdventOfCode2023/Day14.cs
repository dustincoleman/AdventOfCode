namespace AdventOfCode2023;

public class Day14
{
    [Fact]
    public void Part1()
    {
        Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day14.txt");

        Tilt(puzzle);

        int answer = CountLoad(puzzle);

        Assert.Equal(110565, answer);
    }

    [Fact]
    public void Part2()
    {
        Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day14.txt");

        int remainingTurns = 1000000000;
        int firstSeenRemainingTurns = -1;

        Dictionary<Grid2<char>, int> remainingTurnsByGrid = new Dictionary<Grid2<char>, int>()
        {
            { Grid2<char>.Copy(puzzle), remainingTurns }
        };

        while (remainingTurns-- > 0)
        {
            TiltAllWays(puzzle);

            if (remainingTurnsByGrid.TryGetValue(puzzle, out firstSeenRemainingTurns))
            {
                break;
            }

            remainingTurnsByGrid.Add(Grid2<char>.Copy(puzzle), remainingTurns);
        }

        remainingTurns = remainingTurns % (firstSeenRemainingTurns - remainingTurns);

        while (remainingTurns-- > 0)
        {
            TiltAllWays(puzzle);
        }

        int answer = CountLoad(puzzle);
        Assert.Equal(89845, answer);
    }

    private void Tilt(Grid2<char> puzzle)
    {
        foreach (Grid2Column<char> column in puzzle.Columns)
        {
            int nextRockRow = 0;

            for (int row = 0; row < column.Count; row++)
            {
                if (column[row] == 'O')
                {
                    column[row] = '.';
                    column[nextRockRow++] = 'O';
                }
                else if (column[row] == '#')
                {
                    nextRockRow = row + 1;
                }
            }
        }
    }

    private void TiltAllWays(Grid2<char> puzzle)
    {
        Tilt(puzzle);
        Tilt(puzzle.Rotate());
        Tilt(puzzle.Rotate180());
        Tilt(puzzle.RotateCCW());
    }

    private int CountLoad(Grid2<char> puzzle)
    {
        int load = 0;

        foreach (Grid2Column<char> column in puzzle.Columns)
        {
            for (int row = 0; row < column.Count; row++)
            {
                if (column[row] == 'O')
                {
                    load += column.Count - row;
                }
            }
        }

        return load;
    }
}
