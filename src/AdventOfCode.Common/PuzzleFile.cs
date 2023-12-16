﻿using System;
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

        public static Grid2<char> ReadAsGrid(string filename) => ReadLinesAsGrid(File.ReadAllLines(filename));
        public static Grid2<T> ReadAsGrid<T>(string filename, Func<char, T> parser) => ReadLinesAsGrid(File.ReadAllLines(filename), parser);
        public static List<Grid2<char>> ReadLineGroupsAsGrids(string filename) => ReadAllLineGroups(filename).Select(ReadLinesAsGrid).ToList();
        public static List<Grid2<T>> ReadLineGroupsAsGrids<T>(string filename, Func<char, T> parser) => ReadAllLineGroups(filename).Select(lines => ReadLinesAsGrid(lines, parser)).ToList();
        public static Grid2<char> ReadLinesAsGrid(string[] lines) => ReadLinesAsGrid(lines, ch => ch);

        public static Grid2<T> ReadLinesAsGrid<T>(string[] lines, Func<char, T> parser)
        {
            Point2 bounds = new Point2(lines[0].Length, lines.Length);
            Grid2<T> grid = new Grid2<T>(bounds);

            foreach (Point2 p in Point2.Quadrant(bounds))
            {
                grid[p] = parser(lines[p.Y][p.X]);
            }

            return grid;
        }
    }
}
