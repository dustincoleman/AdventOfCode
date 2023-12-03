using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.Common
{
    public static class PuzzleFile
    {
        public static string[][] ReadAllLineGroups(string filename)
        {
            List<string> lines = new List<string>();
            List<string[]> groups = new List<string[]>();

            foreach (string line in File.ReadAllLines(filename))
            {
                if (line == string.Empty)
                {
                    if (lines.Count > 0)
                    {
                        groups.Add(lines.ToArray());
                        lines.Clear();
                    }
                }
                else
                {
                    lines.Add(line);
                }
            }

            if (lines.Count > 0)
            {
                groups.Add(lines.ToArray());
            }

            return groups.ToArray();
        }

        public static Grid2<char> AsGrid(string filename)
        {
            string[] input = File.ReadAllLines(filename);

            Point2 bounds = new Point2(input[0].Length, input.Length);
            Grid2<char> grid = new Grid2<char>(bounds);

            foreach (Point2 p in Point2.Quadrant(bounds))
            {
                grid[p] = input[p.Y][p.X];
            }

            return grid;
        }
    }
}
