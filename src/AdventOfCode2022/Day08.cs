using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    public class Day08
    {
        [Fact]
        public void Part1()
        {
            Grid2<int> grid = LoadPuzzle();

            int result = grid.Points.Count(p =>
                grid.TraverseLeft(p).All(x => grid[p] > x) ||
                grid.TraverseRight(p).All(x => grid[p] > x) ||
                grid.TraverseUp(p).All(x => grid[p] > x) ||
                grid.TraverseDown(p).All(x => grid[p] > x)
            );

            Assert.Equal(1681, result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<int> grid = LoadPuzzle();

            // The edge tree is always the last you will see
            foreach (Point2 point in grid.EdgePoints)
            {
                grid[point] = int.MaxValue;
            }

            int result = grid.InteriorPoints.Max(p =>
                (grid.TraverseLeft(p).TakeWhile(x => grid[p] > x).Count() + 1) *
                (grid.TraverseRight(p).TakeWhile(x => grid[p] > x).Count() + 1) *
                (grid.TraverseUp(p).TakeWhile(x => grid[p] > x).Count() + 1) *
                (grid.TraverseDown(p).TakeWhile(x => grid[p] > x).Count() + 1)
            );

            Assert.Equal(201684, result);
        }

        private Grid2<int> LoadPuzzle()
        {
            string[] lines = File.ReadAllLines("Day08.txt");
            Grid2<int> grid = new Grid2<int>(lines[0].Length, lines.Length);

            foreach (Point2 point in grid.Points)
            {
                grid[point] = int.Parse(lines[point.Y][point.X].ToString());
            }

            return grid;
        }
    }
}