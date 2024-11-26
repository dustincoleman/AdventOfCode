using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCodeOther
{
    public class Year2017Day21
    {
        [Fact]
        public void Part1()
        {
            int result = CountPixels(5);
            Assert.Equal(194, result);
        }

        [Fact]
        public void Part2()
        {
            int result = CountPixels(18);
            Assert.Equal(2536879, result);
        }

        private int CountPixels(int iterations)
        {
            Dictionary<Grid2<bool>, Grid2<bool>> rules = ParseRules();
            Grid2<bool> fractal = ParseGrid(".#./..#/###");

            for (int i = 0; i < iterations; i++)
            {
                fractal = Enhance(fractal, rules);
            }

            return fractal.Count(value => value);
        }

        private Dictionary<Grid2<bool>, Grid2<bool>> ParseRules()
        {
            return File.ReadAllLines("Year2017Day21Input.txt")
                       .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                       .ToDictionary(pieces => ParseGrid(pieces[0]), pieces => ParseGrid(pieces[2]));
        }

        private Grid2<bool> ParseGrid(string input)
        {
            string[] rows = input.Split('/');

            Grid2<bool> grid = new Grid2<bool>(rows[0].Length, rows.Length);

            foreach (Point2 point in grid.AllPoints)
            {
                grid[point] = (rows[point.Y][point.X] == '#');
            }

            return grid;
        }

        private Grid2<bool> Enhance(Grid2<bool> fractal, Dictionary<Grid2<bool>, Grid2<bool>> rules)
        {
            Point2 sliceSize = (fractal.Bounds.X % 2 == 0) ? new Point2(2, 2) : new Point2(3, 3);
            Grid2<Grid2<bool>> pieces = fractal.Split(sliceSize);
            
            foreach (Point2 point in pieces.AllPoints)
            {
                pieces[point] = EnhancePiece(pieces[point], rules);
            }

            return Grid2<bool>.Combine(pieces);
        }

        private Grid2<bool> EnhancePiece(Grid2<bool> piece, Dictionary<Grid2<bool>, Grid2<bool>> rules)
        {
            foreach (Grid2<bool> permutation in piece.Permutations())
            {
                if (rules.TryGetValue(permutation, out Grid2<bool> result))
                {
                    return result;
                }
            }

            throw new Exception("No Match");
        }
    }
}
