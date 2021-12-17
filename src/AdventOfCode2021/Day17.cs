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
    public class Day17
    {
        [Fact]
        public void Part1()
        {
            Target target = Target.ReadFromFile();
            int yMax = 0;

            for (int x = 1; x < target.XMax; x++)
            {
                for (int y = -999; y < 1000; y++)
                {
                    Probe probe = new Probe(x, y, target);

                    if (probe.HitsTarget())
                    {
                        yMax = Math.Max(yMax, probe.YMax);
                    }
                }
            }

            Assert.Equal(7626, yMax);
        }

        [Fact]
        public void Part2()
        {
            Target target = Target.ReadFromFile();
            int velocityCounts = 0;

            for (int x = 1; x < 1000; x++)
            {
                for (int y = -999; y < 1000; y++)
                {
                    Probe probe = new Probe(x, y, target);

                    if (probe.HitsTarget())
                    {
                        velocityCounts++;
                    }
                }
            }

            Assert.Equal(2032, velocityCounts);
        }

        private class Probe
        {
            private Point2 position;
            private Point2 velocity;
            private Target target;

            internal Probe(int xVelocity, int yVelocity, Target target)
            {
                this.position = Point2.Zero;
                this.velocity = new Point2(xVelocity, yVelocity);
                this.target = target;
            }

            internal int YMax { get; private set; }

            internal bool HitsTarget()
            {
                while (position.X <= target.XMax && (velocity.Y >= 0 || position.Y >= target.YMin))
                {
                    position = new Point2(position.X + Math.Max(velocity.X - 1, 0), position.Y + velocity.Y);
                    velocity -= 1;

                    YMax = Math.Max(YMax, position.Y);

                    if (position.X >= target.XMin && position.X <= target.XMax && position.Y >= target.YMin && position.Y <= target.YMax)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private class Target
        {
            // HACK: Assume positive x values
            private static readonly Regex regex = new Regex(@"^target area: x=(?<x1>\d+)..(?<x2>\d+), y=(?<y1>-?\d+)..(?<y2>-?\d+)$");

            public int XMin;
            public int XMax;
            public int YMin;
            public int YMax;

            internal static Target ReadFromFile()
            {
                Match match = regex.Match(File.ReadAllText("Day17Input.txt"));

                int x1 = int.Parse(match.Groups["x1"].Value);
                int x2 = int.Parse(match.Groups["x2"].Value);
                int y1 = int.Parse(match.Groups["y1"].Value);
                int y2 = int.Parse(match.Groups["y2"].Value);

                return new Target()
                {
                    XMin = Math.Min(x1, x2),
                    XMax = Math.Max(x1, x2),
                    YMin = Math.Min(y1, y2),
                    YMax = Math.Max(y1, y2),
                };
            }
        }
    }
}
