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

        public static Grid2<char> ReadAsGrid(string filename) => ReadLinesAsGrid(File.ReadAllLines(filename));
        public static Grid2<T> ReadAsGrid<T>(string filename, Func<char, T> parser) => ReadLinesAsGrid(File.ReadAllLines(filename), parser);
        public static Grid2<T> ReadAsGrid<T>(string filename, Func<char, Point2, T> parser) => ReadLinesAsGrid(File.ReadAllLines(filename), parser);
        public static TGrid ReadAsGrid<T, TGrid>(string filename, Func<char, Point2, T> parser, Func<Point2, TGrid> factory) where TGrid : Grid2<T> 
            => ReadLinesAsGrid(File.ReadAllLines(filename), parser, factory);

        public static List<Grid2<char>> ReadLineGroupsAsGrids(string filename) => ReadAllLineGroups(filename).Select(ReadLinesAsGrid).ToList();
        public static List<Grid2<T>> ReadLineGroupsAsGrids<T>(string filename, Func<char, T> parser) => ReadAllLineGroups(filename).Select(lines => ReadLinesAsGrid(lines, parser)).ToList();
        public static List<Grid2<T>> ReadLineGroupsAsGrids<T>(string filename, Func<char, Point2, T> parser) => ReadAllLineGroups(filename).Select(lines => ReadLinesAsGrid(lines, parser)).ToList();

        public static Grid2<char> ReadLinesAsGrid(string[] lines) => ReadLinesAsGrid(lines, ch => ch);
        public static Grid2<T> ReadLinesAsGrid<T>(string[] lines, Func<char, T> parser) => ReadLinesAsGrid(lines, (ch, pt) => parser(ch));
        public static Grid2<T> ReadLinesAsGrid<T>(string[] lines, Func<char, Point2, T> parser) => ReadLinesAsGrid(lines, parser, pt => new Grid2<T>(pt));

        public static TGrid ReadLinesAsGrid<T, TGrid>(string[] lines, Func<char, Point2, T> parser, Func<Point2, TGrid> factory) where TGrid : Grid2<T>
        {
            Point2 bounds = new Point2(lines[0].Length, lines.Length);
            TGrid grid = factory(bounds);

            foreach (Point2 p in Points.All(bounds))
            {
                grid[p] = parser(lines[p.Y][p.X], p);
            }

            return grid;
        }
    }
}
