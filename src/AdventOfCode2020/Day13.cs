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
            string rawBusRoutes = File.ReadAllLines("Day13Input.txt").Skip(1).First();

            List<BusRoute> busRoutes = File.ReadAllLines("Day13Input.txt")
                .Skip(1).First().Split(',')
                .Select((rawRoute, index) => new BusRoute((rawRoute == "x") ? 0 : int.Parse(rawRoute), index))
                .Where(route => route.Interval != 0)
                .OrderBy(route => route.Interval)
                .ToList();

            long increment = 1;
            long startPosition = 0;

            for (int i = 0; i < busRoutes.Count - 1; i++)
            {
                IEnumerable<BusRoute> busRoutesToSearch = busRoutes.Take(i);

                long firstMatch = FindTimestamp(busRoutesToSearch, startPosition, increment);
                long secondMatch = FindTimestamp(busRoutesToSearch, firstMatch + increment, increment);

                startPosition = firstMatch;
                increment = secondMatch - firstMatch;
            }

            long result = FindTimestamp(busRoutes, startPosition, increment);
            Debugger.Break();
        }

        private static long FindTimestamp(IEnumerable<BusRoute> busRoutesToSearch, long startPosition, long increment)
        {
            long position = startPosition;
            List<BusRoute> routes = new List<BusRoute>(busRoutesToSearch.OrderByDescending(route => route.Interval));

            while (true)
            {
                if (routes.All(route => ((position + route.StartTime) % route.Interval == 0)))
                {
                    return position;
                }

                checked { position += increment; }
            }
        }
    }

    class BusRoute
    {
        public readonly int Interval;
        public readonly int StartTime;

        public BusRoute(int interval, int startTime)
        {
            Interval = interval;
            StartTime = startTime;
        }
    }
}
