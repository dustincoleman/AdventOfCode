using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    public class Day08
    {
        [Fact]
        public void Part1()
        {
            string[] lines = File.ReadAllLines("Day08.txt");
            Grid2<Tree> grid = new Grid2<Tree>(lines[0].Length, lines.Length);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    grid[x, y] = new Tree(int.Parse(lines[y][x].ToString()));
                }
            }

            for (int y = 0; y < grid.Bounds.Y; y++)
            {
                int tallest = -1;

                for (int x = 0; x < grid.Bounds.X; x++)
                {
                    Tree t = grid[x, y];
                    t.TallestLeft = tallest;
                    tallest = Math.Max(t.Height, tallest);
                }

                tallest = -1;

                for (int x = grid.Bounds.X - 1; x >= 0; x--)
                {
                    Tree t = grid[x, y];
                    t.TallestRight = tallest;
                    tallest = Math.Max(t.Height, tallest);
                }
            }

            for (int x = 0; x < grid.Bounds.X; x++)
            {
                int tallest = -1;

                for (int y = 0; y < grid.Bounds.Y; y++)
                {
                    Tree t = grid[x, y];
                    t.TallestAbove = tallest;
                    tallest = Math.Max(t.Height, tallest);
                }

                tallest = -1;

                for (int y = grid.Bounds.Y - 1; y >= 0; y--)
                {
                    Tree t = grid[x, y];
                    t.TallestBelow = tallest;
                    tallest = Math.Max(t.Height, tallest);
                }
            }

            int result = grid.Count(t => t.IsVisible);
            Assert.Equal(1681, result);
        }

        [Fact]
        public void Part2()
        {
            string[] lines = File.ReadAllLines("Day08.txt");
            Grid2<Tree> grid = new Grid2<Tree>(lines[0].Length, lines.Length);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    grid[x, y] = new Tree(int.Parse(lines[y][x].ToString()));
                }
            }

            int result = grid.Points.Max(p => GetViewingDistance(p, grid));
            Assert.Equal(201684, result);
        }

        private int GetViewingDistance(Point2 point, Grid2<Tree> grid)
        {
            Tree t = grid[point];

            int x = point.X;
            int y = point.Y;
            int left = 0;

            while (--x >= 0)
            {
                left++;

                if (grid[x, y].Height >= t.Height)
                {
                    break;
                }
            }

            x = point.X;
            y = point.Y;
            int right = 0;

            while (++x < grid.Bounds.X)
            {
                right++;

                if (grid[x, y].Height >= t.Height)
                {
                    break;
                }
            }

            x = point.X;
            y = point.Y;
            int above = 0;

            while (--y >= 0)
            {
                above++;

                if (grid[x, y].Height >= t.Height)
                {
                    break;
                }
            }

            x = point.X;
            y = point.Y;
            int below = 0;

            while (++y < grid.Bounds.Y)
            {
                below++;

                if (grid[x, y].Height >= t.Height)
                {
                    break;
                }
            }

            return left * right * above * below;
        }

        private class Tree
        {
            public readonly int Height; 
            public int TallestLeft;
            public int TallestRight;
            public int TallestAbove;
            public int TallestBelow;

            public Tree(int height)
            {
                Height = height;
            }

            public bool IsVisible => (TallestLeft < Height || TallestRight < Height || TallestAbove < Height || TallestBelow < Height);
        }
    }
}