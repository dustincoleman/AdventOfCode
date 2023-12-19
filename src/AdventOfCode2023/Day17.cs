using System.Diagnostics;
using System.Drawing;
using System.Linq.Expressions;

namespace AdventOfCode2023;

public class Day17
{
    [Fact]
    public void Part1()
    {
        int answer = SolvePuzzle(min: 1, max: 3);
        Assert.Equal(1023, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = SolvePuzzle(min: 4, max: 10);
        Assert.Equal(1165, answer);
    }

    private int SolvePuzzle(int min, int max)
    {
        Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day17.txt", ch => new Cell(heatLoss: ch - '0', min, max));
        List<Step> list = new List<Step>()
        {
            new Step() { Pos = Point2.Zero + Direction.East, Direction = Direction.East },
            new Step() { Pos = Point2.Zero + Direction.South, Direction = Direction.South },
        };

        while (list.Any())
        {
            Step step = list[0];
            list.RemoveAt(0);

            if (step.CountInDirection < max)
            {
                Cell cell = puzzle[step.Pos];
                int heatLoss = step.AcquiredHeatLoss + cell.HeatLoss;

                if (step.CountInDirection + 1 < min)
                {
                    Point2 newPos = step.Pos + step.Direction;

                    if (puzzle.InBounds(newPos))
                    {
                        step.Pos = newPos;
                        step.AcquiredHeatLoss = heatLoss;
                        step.CountInDirection++;

                        int index = list.BinarySearch(step);
                        list.Insert((index >= 0) ? index : ~index, step);
                    }

                    continue;
                }

                if (step.Pos == puzzle.Bounds - 1)
                {
                    return heatLoss;
                }

                if (cell.TotalHeatLoss[step.Direction][step.CountInDirection - min + 1] > heatLoss)
                {
                    cell.TotalHeatLoss[step.Direction][step.CountInDirection - min + 1] = heatLoss;

                    foreach (Direction direction in puzzle.DirectionsInBounds(step.Pos))
                    {
                        if (direction != step.Direction.Reverse())
                        {
                            Step nextStep = new Step()
                            {
                                Pos = step.Pos + direction,
                                AcquiredHeatLoss = heatLoss,
                                Direction = direction,
                                CountInDirection = (direction == step.Direction) ? step.CountInDirection + 1 : 0
                            };

                            int index = list.BinarySearch(nextStep);
                            list.Insert((index >= 0) ? index : ~index, nextStep);
                        }
                    }
                }
            }
        }

        throw new Exception("Answer not found");
    }

    private struct Step : IComparable<Step>
    {
        public Point2 Pos;
        public int AcquiredHeatLoss;
        public Direction Direction;
        public int CountInDirection;

        public int CompareTo(Step other)
        {
            return AcquiredHeatLoss.CompareTo(other.AcquiredHeatLoss);
        }
    }

    private class Cell
    {
        public int HeatLoss;
        public Dictionary<Direction, int[]> TotalHeatLoss = new Dictionary<Direction, int[]>();

        public Cell(int heatLoss, int min, int max)
        {
            HeatLoss = heatLoss;

            foreach (Direction direction in Direction.All())
            {
                TotalHeatLoss[direction] = Enumerable.Repeat(int.MaxValue, max - min + 1).ToArray();
            }
        }
    }
}
