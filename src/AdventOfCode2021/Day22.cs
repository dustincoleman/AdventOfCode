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
    public class Day22
    {
        [Fact]
        public void Part1()
        {
            VirtualGrid3<bool> reactor = new VirtualGrid3<bool>();

            foreach (VirtualGrid3Region<bool> step in ReadStepsFromFile().Where(step => IsWithinBounds(step, Point3.One * -50, Point3.One * 50)))
            {
                reactor.Set(step);
            }

            long result = reactor.Where(r => r.Value).Sum(Size);

            Assert.Equal(612714, result);
        }

        [Fact]
        public void Part2()
        {
            VirtualGrid3<bool> reactor = new VirtualGrid3<bool>();

            foreach (VirtualGrid3Region<bool> step in ReadStepsFromFile())
            {
                reactor.Set(step);
            }

            long result = reactor.Where(r => r.Value).Sum(Size);

            Assert.Equal(1311612259117092, result);
        }

        private List<VirtualGrid3Region<bool>> ReadStepsFromFile()
        {
            Regex regex = new Regex(@"^(?<command>\S+)\sx=(?<x1>-?\d+)..(?<x2>-?\d+),y=(?<y1>-?\d+)..(?<y2>-?\d+),z=(?<z1>-?\d+)..(?<z2>-?\d+)$");

            List<VirtualGrid3Region<bool>> list = new List<VirtualGrid3Region<bool>>();

            foreach (string line in File.ReadAllLines("Day22Input.txt"))
            {
                Match match = regex.Match(line);

                int x1 = int.Parse(match.Groups["x1"].Value);
                int x2 = int.Parse(match.Groups["x2"].Value);
                int y1 = int.Parse(match.Groups["y1"].Value);
                int y2 = int.Parse(match.Groups["y2"].Value);
                int z1 = int.Parse(match.Groups["z1"].Value);
                int z2 = int.Parse(match.Groups["z2"].Value);
                bool on = (match.Groups["command"].Value == "on");

                list.Add(new VirtualGrid3Region<bool>(Rect3.Normalize((x1, y1, z1), (x2, y2, z2)), on));
            }

            return list;
        }

        private bool IsWithinBounds(VirtualGrid3Region<bool> region, Point3 lower, Point3 upper)
        {
            return region.Bounds.Lower >= lower && region.Bounds.Upper <= upper;
        }

        private long Size(VirtualGrid3Region<bool> region)
        {
            return (region.Bounds.Upper - region.Bounds.Lower + Point3.One).As<long>().Product();
        }
    }
}
