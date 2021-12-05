using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace AdventOfCode2021
{
    public class Day05
    {
        [Fact]
        public void Part1()
        {
            Line[] input = File.ReadAllLines("Day05Input.txt").Select(Line.Parse).ToArray();

            Map map = new Map();

            foreach(Line line in input)
            {
                if (line.IsVertical || line.IsHorizontal)
                {
                    map.Plot(line);
                }
            }

            int result = map.CountPointsGreaterThan(1);

            Assert.Equal(7436, result);
        }

        [Fact]
        public void Part2()
        {
            Line[] input = File.ReadAllLines("Day05Input.txt").Select(Line.Parse).ToArray();

            Map map = new Map();

            foreach (Line line in input)
            {
                map.Plot(line);
            }

            int result = map.CountPointsGreaterThan(1);

            Assert.Equal(21104, result);
        }
    }

    class Line
    {
        internal Point Start;
        internal Point End;

        internal static Line Parse(string input)
        {
            string[] parts = input.Split(' ');

            string[] startInput = parts[0].Split(',');
            string[] endInput = parts[2].Split(',');

            return new Line()
            {
                Start = new Point(Int32.Parse(startInput[0]), Int32.Parse(startInput[1])),
                End = new Point(Int32.Parse(endInput[0]), Int32.Parse(endInput[1]))
            };
        }

        internal bool IsVertical => Start.X == End.X;

        internal bool IsHorizontal => Start.Y == End.Y;
    }

    class Map
    {
        private int[,] points = new int[1000, 1000];

        internal void Plot(Line line)
        {
            if (line.IsVertical)
            {
                int x = line.Start.X;

                if (line.Start.Y <= line.End.Y)
                {
                    for (int y = line.Start.Y; y <= line.End.Y; y++)
                    {
                        points[x, y]++;
                    }
                }
                else
                {
                    for (int y = line.End.Y; y <= line.Start.Y; y++)
                    {
                        points[x, y]++;
                    }
                }
            }
            else if (line.IsHorizontal)
            {
                int y = line.Start.Y;

                if (line.Start.X <= line.End.X)
                {
                    for (int x = line.Start.X; x <= line.End.X; x++)
                    {
                        points[x, y]++;
                    }
                }
                else
                {
                    for (int x = line.End.X; x <= line.Start.X; x++)
                    {
                        points[x, y]++;
                    }
                }
            }
            else
            {
                bool xAscending = line.Start.X < line.End.X;
                bool yAscending = line.Start.Y < line.End.Y;

                int x = line.Start.X;
                int y = line.Start.Y;

                while (true)
                {
                    points[x, y]++;

                    if (x == line.End.X)
                    {
                        if (y != line.End.Y)
                        {
                            throw new Exception("Not horizontal");
                        }

                        break;
                    }

                    if (xAscending)
                    {
                        x++;
                    }
                    else
                    {
                        x--;
                    }

                    if (yAscending)
                    {
                        y++;
                    }
                    else
                    {
                        y--;
                    }
                }
            }
        }

        internal int CountPointsGreaterThan(int v)
        {
            int count = 0;

            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    if (points[x, y] > v)
                    {
                        count++;    
                    }
                }
            }

            return count;
        }
    }
}
