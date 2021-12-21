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
    public class Day19
    {
        [Fact]
        public void Part1()
        {
            Scanner merged = RunProblem().Merge();

            Assert.Equal(454, merged.Probes.Count);
        }

        [Fact]
        public void Part2()
        {
            int largestManhattan = RunProblem().ComputeLargestManhattan();

            Assert.Equal(10813, largestManhattan);
        }

        private Scanner RunProblem()
        {
            List<Point3Set> unlocatedScanners = ReadInputFromFile();

            Scanner homeScanner = new Scanner(unlocatedScanners[0]);
            unlocatedScanners.RemoveAt(0);

            Queue<Scanner> toSearch = new Queue<Scanner>();
            toSearch.Enqueue(homeScanner);

            while (toSearch.Count > 0)
            {
                Scanner scanner = toSearch.Dequeue();

                foreach (Point3Set unlocated in unlocatedScanners.ToArray())
                {
                    if (TryAlignProbes(scanner.Probes, unlocated, out NearbyScanner match))
                    {
                        scanner.NearbyScanners.Add(match);
                        unlocatedScanners.Remove(unlocated);
                        toSearch.Enqueue(match.Scanner);
                    }
                }
            }

            return homeScanner;
        }

        private List<Point3Set> ReadInputFromFile()
        {
            List<Point3Set> list = new List<Point3Set>();
            Point3Set set = null;

            foreach (string line in File.ReadAllLines("Day19Input.txt"))
            {
                if (line.StartsWith("---"))
                {
                    set = new Point3Set();
                }
                else if (string.IsNullOrWhiteSpace(line))
                {
                    list.Add(set);
                    set = null;
                }
                else
                {
                    int[] parts = line.Split(',').Select(int.Parse).ToArray();
                    set.Add(new Point3(parts[0], parts[1], parts[2]));
                }
            }

            list.Add(set);

            return list;
        }

        private bool TryAlignProbes(Point3Set leftSet, Point3Set rightSet, out NearbyScanner match)
        {
            foreach (Orientation3 orientation in Enum.GetValues(typeof(Orientation3)))
            {
                Point3Set rightSetOriented = rightSet.Orient(orientation);

                foreach (Point3 leftPoint in leftSet)
                {
                    foreach (Point3 rightPoint in rightSetOriented)
                    {
                        Point3 offset = leftPoint - rightPoint;
                        Point3Set temp = rightSetOriented.Shift(offset);
                        temp.IntersectWith(leftSet);

                        if (temp.Count >= 12)
                        {
                            match = new NearbyScanner(new Scanner(rightSetOriented), offset);
                            return true;
                        }
                    }
                }
            }

            match = null;
            return false;
        }

        private class Scanner
        {
            public Scanner(Point3Set probes)
            {
                Probes = probes;
                NearbyScanners = new List<NearbyScanner>();
            }

            public Point3Set Probes { get; private set; }

            public List<NearbyScanner> NearbyScanners { get; private set; }

            internal int ComputeLargestManhattan()
            {
                HashSet<int> manhattans = new HashSet<int>();
                List<Point3> offsets = new List<Point3>();
                GetOffsetsOfNearbyScanners(offsets, Point3.Zero);

                foreach (Point3 left in offsets)
                {
                    foreach (Point3 right in offsets)
                    {
                        if (left != right)
                        {
                            manhattans.Add((left - right).Manhattan());
                        }
                    }
                }

                return manhattans.Max();
            }

            public Scanner Merge()
            {
                Scanner merged = new Scanner(new Point3Set(Probes)); // Copy

                MergeHelper(merged, this, Point3.Zero);

                return merged;
            }

            private void GetOffsetsOfNearbyScanners(List<Point3> offsets, Point3 currentOffset)
            {
                foreach (NearbyScanner nearby in this.NearbyScanners)
                {
                    Point3 totalOffset = nearby.Offset + currentOffset;
                    offsets.Add(totalOffset);
                    nearby.Scanner.GetOffsetsOfNearbyScanners(offsets, totalOffset);
                }
            }

            private static void MergeHelper(Scanner merged, Scanner current, Point3 offset)
            {
                foreach (NearbyScanner nearby in current.NearbyScanners)
                {
                    Point3 totalOffset = nearby.Offset + offset;
                    Point3Set shift = nearby.Scanner.Probes.Shift(totalOffset);

                    merged.Probes.UnionWith(shift);

                    MergeHelper(merged, nearby.Scanner, totalOffset);
                }
            }
        }

        private class NearbyScanner
        {
            public NearbyScanner(Scanner scanner, Point3 offset)
            {
                Scanner = scanner;
                Offset = offset;
            }

            public Scanner Scanner { get; private set; }

            public Point3 Offset { get; private set; }
        }
    }
}
