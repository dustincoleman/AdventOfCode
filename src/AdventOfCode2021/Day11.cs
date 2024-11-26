using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace AdventOfCode2021
{
    public class Day11
    {
        [Fact]
        public void Part1()
        {
            Grid2<Octopus> grid = LoadGrid();
            long result = 0;

            for (int i = 0; i < 100; i++)
            {
                result += ProcessStep(grid);
            }

            Assert.Equal(1647, result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<Octopus> grid = LoadGrid();
            long result = 0;

            for (int i = 1; true; i++)
            {
                if (ProcessStep(grid) == 100)
                {
                    result = i;
                    break;
                }
            }

            Assert.Equal(348, result);
        }

        private Grid2<Octopus> LoadGrid()
        {
            string[] input = File.ReadAllLines("Day11Input.txt");
            Grid2<Octopus> grid = new Grid2<Octopus>(input[0].Length, input.Length);

            foreach (Point2 point in grid.AllPoints)
            {
                grid[point] = new Octopus(input[point.Y][point.X] - '0');
            }

            return grid;
        }

        private long ProcessStep(Grid2<Octopus> grid)
        {
            foreach (Point2 point in grid.AllPoints)
            {
                grid[point].Energy++;
            }

            foreach (Point2 point in grid.AllPoints)
            {
                TryFlash(grid, point);
            }

            long flashes = grid.AllPoints.Count(p => grid[p].HasFlashed);

            foreach (Point2 point in grid.AllPoints)
            {
                if (grid[point].HasFlashed)
                {
                    grid[point].HasFlashed = false;
                    grid[point].Energy = 0;
                }
            }

            return flashes;
        }

        private void TryFlash(Grid2<Octopus> grid, Point2 point)
        {
            Octopus o = grid[point];

            if (o.Energy > 9 && !o.HasFlashed)
            {
                o.HasFlashed = true;

                foreach (Point2 neighbor in point.Surrounding(grid.Bounds))
                {
                    grid[neighbor].Energy++;
                    TryFlash(grid, neighbor);
                }
            }
        }

        private class Octopus
        {
            internal int Energy;
            internal bool HasFlashed;

            internal Octopus(int energy)
            {
                Energy = energy;
            }
        }
    }
}
