using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021
{
    public class Day20
    {
        [Fact]
        public void Part1()
        {
            int result = RunProblem(2);

            Assert.Equal(5326, result);
        }

        [Fact]
        public void Part2()
        {
            int result = RunProblem(50);

            Assert.Equal(17096, result);
        }

        private int RunProblem(int times)
        {
            string[] lines = File.ReadAllLines("Day20Input.txt");
            char[] lookupTable = lines[0].ToArray();

            Point2 origin = new Point2(times, times);
            Grid2<char> map = new Grid2<char>(lines[2].Length + origin.X * 2, lines.Length - 2 + origin.Y * 2);

            foreach (Point2 p in map.Points)
            {
                map[p] = '.';
            }

            foreach (Point2 p in Point2.Quadrant(new Point2(lines[2].Length, lines.Length - 2)))
            {
                map[p + origin] = lines[p.Y + 2][p.X];
            }

            for (int i = 0; i < times; i++)
            {
                Grid2<char> newMap = new Grid2<char>(map.Bounds);

                foreach (Point2 p in map.Points)
                {
                    newMap[p] = GetPixel(map, p, lookupTable);
                }

                map = newMap;
            }

            return map.Count(ch => ch == '#');
        }

        private char GetPixel(Grid2<char> map, Point2 center, char[] lookupTable)
        {
            int lookup = 0;

            foreach (Point2 p in GetLookupPoints(center, map.Bounds))
            {
                lookup <<= 1;
                lookup |= (map[p] == '#') ? 1 : 0;
            }

            return lookupTable[lookup];
        }

        private IEnumerable<Point2> GetLookupPoints(Point2 p, Point2 bounds)
        {
            Point2[] points = new Point2[9];

            points[0] = (p.X > 0 && p.Y > 0) ? p - Point2.UnitX - Point2.UnitY : Point2.Zero;
            points[1] = (p.Y > 0) ? p - Point2.UnitY : Point2.Zero;
            points[2] = (p.X < bounds.X - 1 && p.Y > 0) ? p + Point2.UnitX - Point2.UnitY : Point2.Zero;

            points[3] = (p.X > 0) ? p - Point2.UnitX : Point2.Zero;
            points[4] = p;
            points[5] = (p.X < bounds.X - 1) ? p + Point2.UnitX : Point2.Zero;

            points[6] = (p.X > 0 && p.Y < bounds.Y - 1) ? p - Point2.UnitX + Point2.UnitY : Point2.Zero;
            points[7] = (p.Y < bounds.Y - 1) ? p + Point2.UnitY : Point2.Zero;
            points[8] = (p.X < bounds.X - 1 && p.Y < bounds.Y - 1) ? p + Point2.UnitX + Point2.UnitY : Point2.Zero;

            return points;
        }
    }
}
