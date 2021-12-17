using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace AdventOfCode2021
{
    public class Day12
    {
        [Fact]
        public void Part1()
        {
            Dictionary<string, HashSet<string>> graph = LoadGraph();
            List<string> paths = new List<string>();

            FindPaths("start", graph, paths);

            long result = paths.Count;

            Assert.Equal(5254, result);
        }

        [Fact]
        public void Part2()
        {
            Dictionary<string, HashSet<string>> graph = LoadGraph();
            List<string> paths = new List<string>();

            FindPaths("start", graph, paths, canDoubleVisit: true);

            long result = paths.Count;

            Assert.Equal(149385, result);
        }

        private Dictionary<string, HashSet<string>> LoadGraph()
        {
            AutoDictionary<string, HashSet<string>> graph = new AutoDictionary<string, HashSet<string>>();

            foreach (string[] entry in File.ReadAllLines("Day12Input.txt").Select(s => s.Split('-')))
            {
                graph[entry[0]].Add(entry[1]);
                graph[entry[1]].Add(entry[0]);
            }

            return graph.Dictionary;
        }

        private void FindPaths(string currentPath, Dictionary<string, HashSet<string>> graph, List<string> paths, bool canDoubleVisit = false)
        {
            string node = currentPath.Split(',').Last();

            foreach (string next in graph[node])
            {
                bool nextCanDoubleVisit = canDoubleVisit;

                if (next == "start")
                {
                    continue;
                }

                if (next == next.ToLower() && currentPath.Split(",").Contains(next))
                {
                    if (canDoubleVisit)
                    {
                        nextCanDoubleVisit = false;
                    }
                    else
                    {
                        continue;
                    }
                }

                string nextPath = $"{currentPath},{next}";

                if (next == "end")
                {
                    paths.Add(nextPath);
                }
                else
                {
                    FindPaths(nextPath, graph, paths, nextCanDoubleVisit);
                }
            }
        }
    }
}
