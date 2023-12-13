using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static Grid2<char> ReadAsGrid(string filename)
        {
            return ReadLinesAsGrid(File.ReadAllLines(filename));
        }

        public static List<Grid2<char>> ReadLineGroupsAsGrids(string filename)
        {
            return ReadAllLineGroups(filename).Select(ReadLinesAsGrid).ToList();
        }

        public static Grid2<char> ReadLinesAsGrid(string[] lines)
        {
            Point2 bounds = new Point2(lines[0].Length, lines.Length);
            Grid2<char> grid = new Grid2<char>(bounds);

            foreach (Point2 p in Point2.Quadrant(bounds))
            {
                grid[p] = lines[p.Y][p.X];
            }

            return grid;
        }
    }
}
