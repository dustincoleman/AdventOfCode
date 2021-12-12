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
            Dictionary<string, HashSet<string>> graph = new Dictionary<string, HashSet<string>>();

            foreach (string[] entry in File.ReadAllLines("Day12Input.txt").Select(s => s.Split('-')))
            {
                HashSet<string> set;

                if (!graph.TryGetValue(entry[0], out set))
                {
                    set = new HashSet<string>();
                    graph.Add(entry[0], set);
                }

                set.Add(entry[1]);

                if (!graph.TryGetValue(entry[1], out set))
                {
                    set = new HashSet<string>();
                    graph.Add(entry[1], set);
                }

                set.Add(entry[0]);
            }

            return graph;
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
