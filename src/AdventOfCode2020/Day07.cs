using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    static class Day07
    {
        private static Regex lineRegex = new Regex(@"^(?<color>.*)\sbags contain (?<contents>.*)$");
        private static Regex contentsRegex = new Regex(@"(?<count>[0-9]+)\s(?<color>[^,.]+)\sbags{0,1}");

        public static void Part1()
        {
            Dictionary<string, HashSet<string>> containingColorsByColor = new Dictionary<string, HashSet<string>>();

            foreach(string line in File.ReadAllLines("Day07Input.txt"))
            {
                Match match = lineRegex.Match(line);
                string outerColor = match.Groups["color"].Value;

                foreach (Match match2 in contentsRegex.Matches(match.Groups["contents"].Value))
                {
                    string color = match2.Groups["color"].Value;
                    int count = int.Parse(match2.Groups["count"].Value);

                    if (!containingColorsByColor.TryGetValue(color, out HashSet<string> containingColors))
                    {
                        containingColors = new HashSet<string>();
                        containingColorsByColor.Add(color, containingColors);
                    }

                    containingColors.Add(outerColor);
                }
            }

            Queue<string> searchQueue = new Queue<string>();
            HashSet<string> visited = new HashSet<string>();

            foreach (string color in containingColorsByColor["shiny gold"])
            {
                visited.Add(color);
                searchQueue.Enqueue(color);
            }

            while (searchQueue.Count > 0)
            {
                string next = searchQueue.Dequeue();

                if (containingColorsByColor.ContainsKey(next))
                {
                    foreach (string color in containingColorsByColor[next])
                    {
                        if (visited.Add(color))
                        {
                            searchQueue.Enqueue(color);
                        }
                    }
                }
            }

            int result = visited.Count;

            Debug.Assert(result == 246);
        }

        public static void Part2()
        {
            Dictionary<string, List<ColorCountPair>> contentsByColor = new Dictionary<string, List<ColorCountPair>>();

            foreach (string line in File.ReadAllLines("Day07Input.txt"))
            {
                Match match = lineRegex.Match(line);
                string outerColor = match.Groups["color"].Value;

                List<ColorCountPair> containedBags = new List<ColorCountPair>();
                contentsByColor.Add(outerColor, containedBags);

                foreach (Match match2 in contentsRegex.Matches(match.Groups["contents"].Value))
                {
                    string color = match2.Groups["color"].Value;
                    int count = int.Parse(match2.Groups["count"].Value);
                    containedBags.Add(new ColorCountPair() { Color = color, Count = count });
                }
            }

            int result = CountContents("shiny gold", contentsByColor);

            Debug.Assert(result == 2976);
        }

        private static int CountContents(string color, Dictionary<string, List<ColorCountPair>> contentsByColor)
        {
            int count = 0;

            foreach (ColorCountPair pair in contentsByColor[color])
            {
                count += pair.Count + pair.Count * CountContents(pair.Color, contentsByColor);
            }

            return count;
        }
    }

    struct ColorCountPair
    {
        public string Color;
        public int Count;
    }
}
