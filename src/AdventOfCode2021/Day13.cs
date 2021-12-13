using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2021
{
    public class Day13
    {
        [Fact]
        public void Part1()
        {
            ParseResult input = ParseInput();

            Grid2<bool> foldedGrid = Fold(input.Grid, input.Folds[0]);

            long result = foldedGrid.Count(x => x);

            Assert.Equal(592, result);
        }

        [Fact]
        public void Part2()
        {
            ParseResult input = ParseInput();

            foreach (Point2 fold in input.Folds)
            {
                input.Grid = Fold(input.Grid, fold);
            }

            string result = PrintGrid(input.Grid);

            string expected = "  ##  ##   ##    ## #### #### #  # #  # \r\n" +
                              "   # #  # #  #    # #    #    # #  #  # \r\n" +
                              "   # #    #  #    # ###  ###  ##   #  # \r\n" +
                              "   # # ## ####    # #    #    # #  #  # \r\n" +
                              "#  # #  # #  # #  # #    #    # #  #  # \r\n" +
                              " ##   ### #  #  ##  #### #    #  #  ##  \r\n";

            Assert.Equal(expected, result);
        }

        struct ParseResult
        {
            public Grid2<bool> Grid;
            public Point2[] Folds;
        }

        ParseResult ParseInput()
        {
            bool readingPoints = true;
            List<Point2> points = new List<Point2>();
            List<Point2> folds = new List<Point2>();

            foreach (string line in File.ReadLines("Day13Input.txt"))
            {
                if (readingPoints)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] parts = line.Split(',');
                        points.Add(new Point2(int.Parse(parts[0]), int.Parse(parts[1])));
                    }
                    else
                    {
                        readingPoints = false;
                    }
                }
                else
                {
                    string[] parts = line.Substring("fold along ".Length).Split('=');
                    
                    if (parts[0] == "x")
                    {
                        folds.Add(Point2.UnitX * int.Parse(parts[1]));
                    }
                    else
                    {
                        folds.Add(Point2.UnitY * int.Parse(parts[1]));
                    }
                }
            }

            Grid2<bool> grid = new Grid2<bool>(points.Max(p => p.X) + 1, points.Max(p => p.Y) + 1);

            foreach (Point2 point in points)
            {
                grid[point] = true;
            }

            return new ParseResult()
            {
                Grid = grid,
                Folds = folds.ToArray()
            };
        }

        private Grid2<bool> Fold(Grid2<bool> grid, Point2 fold)
        {
            Point2 foldedBounds;
            Func<Point2, Point2> getFirst;
            Func<Point2, Point2> getSecond;

            if (fold.X > 0)
            {
                foldedBounds = new Point2(Math.Max(fold.X, grid.Bounds.X - fold.X - 1), grid.Bounds.Y);
                getFirst = destination => new Point2(fold.X - foldedBounds.X + destination.X, destination.Y);
                getSecond = destination => new Point2(fold.X + foldedBounds.X - destination.X, destination.Y);
            }
            else
            {
                foldedBounds = new Point2(grid.Bounds.X, Math.Max(fold.Y, grid.Bounds.Y - fold.Y - 1));
                getFirst = destination => new Point2(destination.X, fold.Y - foldedBounds.Y + destination.Y);
                getSecond = destination => new Point2(destination.X, fold.Y + foldedBounds.Y - destination.Y);
            }

            Grid2<bool> foldedGrid = new Grid2<bool>(foldedBounds);

            foreach (Point2 destination in foldedGrid.Points)
            {
                Point2 first = getFirst(destination);
                Point2 second = getSecond(destination);

                if (first >= Point2.Zero)
                {
                    foldedGrid[destination] |= grid[first];
                }

                if (second < grid.Bounds)
                {
                    foldedGrid[destination] |= grid[second];
                }
            }

            return foldedGrid;
        }

        private string PrintGrid(Grid2<bool> grid)
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < grid.Bounds.Y; y++)
            {
                for (int x = 0; x < grid.Bounds.X; x++)
                {
                    sb.Append(grid[x, y] ? '#' : ' ');
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
