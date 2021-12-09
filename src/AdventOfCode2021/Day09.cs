﻿using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace AdventOfCode2021
{
    public class Day09
    {
        [Fact]
        public void Part1()
        {
            string[] input = File.ReadAllLines("Day09Input.txt");

            Point2 bounds = new Point2(input[0].Length, input.Length);
            Grid2 grid = new Grid2(bounds);

            foreach (Point2 point in Point2.Quadrant(bounds))
            {
                grid[point] = input[point.X][point.Y] - '0';
            }

            long result = Point2.Quadrant(bounds)
                                .Where(point => grid.Adjacent(point).All(value => value > grid[point]))
                                .Select(point => grid[point] + 1)
                                .Sum();

            Assert.Equal(462, result);
        }

        [Fact]
        public void Part2()
        {
            string[] input = File.ReadAllLines("Day09Input.txt");

            Point2 bounds = new Point2(input[0].Length, input.Length);
            Grid2 grid = new Grid2(bounds);

            foreach (Point2 point in Point2.Quadrant(bounds))
            {
                grid[point] = input[point.X][point.Y] - '0';
            }

            long result = Point2.Quadrant(bounds)
                                .Where(point => grid.Adjacent(point).All(value => value > grid[point]))
                                .Select(point => SizeOfBasin(point, grid, new bool[bounds.X, bounds.Y]))
                                .OrderByDescending(i => i)
                                .Take(3)
                                .Aggregate((x, y) => x * y);

            Assert.Equal(1397760, result);
        }

        private int SizeOfBasin(Point2 point, Grid2 grid, bool[,] visited)
        {
            int size = 0;

            if (grid[point] != 9 && !visited[point.X, point.Y])
            {
                visited[point.X, point.Y] = true;
                size = point.Adjacent(grid.Bounds).Sum(adj => SizeOfBasin(adj, grid, visited)) + 1;
            }

            return size;
        }
    }
}
