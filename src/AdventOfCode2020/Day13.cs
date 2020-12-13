using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day13
    {
        public static void Part1()
        {
            string[] lines = File.ReadAllLines("Day13Input.txt");

            int estimatedDeparture = int.Parse(lines[0]);
            int candidateDeparture = estimatedDeparture;
            List<int> busses = lines[1].Split(',').Where(s => s != "x").Select(int.Parse).ToList();

            while (true)
            {
                foreach (int bus in busses)
                {
                    if (candidateDeparture % bus == 0)
                    {
                        int result = bus * (candidateDeparture - estimatedDeparture);
                        Debugger.Break();
                    }
                }

                checked { candidateDeparture++; }
            }
        }

        public static void Part2()
        {
            string[] lines = File.ReadAllLines("Day13Input.txt");

            int estimatedDeparture = int.Parse(lines[0]);
            int candidateDeparture = estimatedDeparture;
            List<int> busses = lines[1].Split(',').Select(s => (s == "x") ? 0 : int.Parse(s)).ToList();

            List<Tuple<long, long>> busOffsets = new List<Tuple<long, long>>();

            for (int i = 0; i < busses.Count; i++)
            {
                if (busses[i] != 0)
                {
                    busOffsets.Add(new Tuple<long, long>(busses[i], i));
                }
            }

            busOffsets.Sort((x, y) => y.Item1.CompareTo(x.Item1));

            // First match for first six busses
            long l = 21252608;

            while (true)
            {
                bool match = true;

                foreach (Tuple<long, long> busOffset in busOffsets)
                {
                    if ((l + busOffset.Item2) % busOffset.Item1 != 0)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    long result = l;
                    Debugger.Break();
                }

                // Gap between first and second match for first six busses
                checked { l += 103627121; }
            }
        }
    }
}
