using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day05
    {
        public static void Part1()
        {
            int result = File.ReadAllLines("Day05Input.txt")
                .Select(line => new BoardingPass(line))
                .Max(pass => pass.SeatId);
            Debugger.Break();
        }

        public static void Part2()
        {
            List<int> seatIds = File.ReadAllLines("Day05Input.txt")
                .Select(line => new BoardingPass(line).SeatId)
                .ToList();

            seatIds.Sort();

            for (int i = 1; i < seatIds.Count; i++)
            {
                if (seatIds[i - 1] + 1 != seatIds[i])
                {
                    int result = seatIds[i - 1] + 1;
                    Debugger.Break();
                }
            }
        }
    }

    class BoardingPass
    {
        public int SeatId;

        public BoardingPass(string rawData)
        {
            for (int i = 0; i < 10; i++)
            {
                SeatId <<= 1;

                if (rawData[i] == 'B' || rawData[i] == 'R')
                {
                    SeatId |= 1;
                }
            }
        }
    }
}
