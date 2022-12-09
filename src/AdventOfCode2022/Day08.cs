using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    public class Day08
    {
        [Fact]
        public void Part1()
        {
            Grid2<Tree> grid = LoadPuzzle();

            void VisitTree(Tree tree, ref int max)
            {
                tree.IsVisible |= (tree.Height > max);
                max = Math.Max(tree.Height, max);
            }

            void VisitSeries(List<Tree> series)
            {
                int max = -1;
                series.ForEach(t => VisitTree(t, ref max));

                series.Reverse();

                max = -1;
                series.ForEach(t => VisitTree(t, ref max));
            }

            foreach (Grid2Row<Tree> row in grid.Rows)
            {
                VisitSeries(row.ToList());
            }

            foreach (Grid2Column<Tree> column in grid.Columns)
            {
                VisitSeries(column.ToList());
            }

            int result = grid.Count(t => t.IsVisible);
            Assert.Equal(1681, result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<Tree> grid = LoadPuzzle();

            // The edge tree is always the last you will see
            foreach (Point2 point in grid.EdgePoints)
            {
                grid[point].Height = 9;
            }

            void VisitTree(Tree tree, int[] view)
            {
                int score = 0;
                int pivot = 10;

                if (view[0] > 0) // Not the first tree in the series
                {
                    score = view[tree.Height];
                    pivot = tree.Height + 1;
                }

                tree.ScenicScore *= score;

                for (int i = 0; i < pivot; i++) 
                { 
                    view[i] = 1;
                }

                for (int i = pivot; i < 10; i++)
                {
                    view[i]++;
                }
            }

            void VisitSeries(List<Tree> series)
            {
                int[] view = new int[10];
                series.ForEach(t => VisitTree(t, view));

                series.Reverse();

                view = new int[10];
                series.ForEach(t => VisitTree(t, view));
            }

            foreach (Grid2Row<Tree> row in grid.Rows)
            {
                VisitSeries(row.ToList());
            }

            foreach (Grid2Column<Tree> column in grid.Columns)
            {
                VisitSeries(column.ToList());
            }

            int result = grid.InteriorPoints.Max(p => grid[p].ScenicScore);
            Assert.Equal(201684, result);
        }

        private Grid2<Tree> LoadPuzzle()
        {
            string[] lines = File.ReadAllLines("Day08.txt");
            Grid2<Tree> grid = new Grid2<Tree>(lines[0].Length, lines.Length);

            foreach (Point2 point in grid.Points)
            {
                grid[point] = new Tree() { Height = int.Parse(lines[point.Y][point.X].ToString()) };
            }

            return grid;
        }

        private class Tree
        {
            public int Height = 0;
            public bool IsVisible = false;
            public int ScenicScore = 1;
        }
    }
}