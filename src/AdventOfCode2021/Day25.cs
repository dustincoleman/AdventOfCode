using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021
{
    public class Day25
    {
        [Fact]
        public void Part1()
        {
            Grid2<char> map = ReadMapFromFile();

            int result = 0;
            bool changed = true;

            for (; changed; result++)
            {
                map = DoStep(map, out changed);
            }

            Assert.Equal(424, result);
        }

        private Grid2<char> ReadMapFromFile()
        {
            string[] input = File.ReadAllLines("Day25Input.txt");

            Grid2<char> map = new Grid2<char>(input[0].Length, input.Length);

            foreach (Point2 point in map.AllPoints)
            {
                char c = input[point.Y][point.X];
                map[point] = (c == '>' || c == 'v') ? c : default(char);
            }

            return map;
        }

        private Grid2<char> DoStep(Grid2<char> map, out bool changed)
        {
            Grid2<char> east = DoEastStepPart(map, out bool eastChanged);
            Grid2<char> south = DoSouthStepPart(east, out bool southChanged);

            changed = eastChanged || southChanged;

            return south;
        }

        private Grid2<char> DoEastStepPart(Grid2<char> map, out bool changed)
        {
            changed = false;

            Grid2<char> newMap = new Grid2<char>(map.Bounds);

            foreach (Point2 source in map.AllPoints)
            {
                if (map[source] == '>')
                {
                    Point2 destination = new Point2((source.X + 1) % map.Bounds.X, source.Y);

                    if (map[destination] == default(char))
                    {
                        newMap[destination] = '>';
                        changed = true;
                    }
                    else
                    {
                        newMap[source] = '>';
                    }
                }
                else if (map[source] == 'v')
                {
                    newMap[source] = 'v';
                }
            }

            return newMap;
        }

        private Grid2<char> DoSouthStepPart(Grid2<char> map, out bool changed)
        {
            changed = false;

            Grid2<char> newMap = new Grid2<char>(map.Bounds);

            foreach (Point2 source in map.AllPoints)
            {
                if (map[source] == 'v')
                {
                    Point2 destination = new Point2(source.X, (source.Y + 1) % map.Bounds.Y);

                    if (map[destination] == default(char))
                    {
                        newMap[destination] = 'v';
                        changed = true;
                    }
                    else
                    {
                        newMap[source] = 'v';
                    }
                }
                else if (map[source] == '>')
                {
                    newMap[source] = '>';
                }
            }

            return newMap;
        }
    }
}
