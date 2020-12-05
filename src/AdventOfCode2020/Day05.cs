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
        public int Row;
        public int Column;
        public int SeatId;

        public BoardingPass(string rawData)
        {
            string rawRow = rawData.Substring(0, 7);
            string rawColumn = rawData.Substring(7, 3);

            for (int i = 0; i < 7; i++)
            {
                Debug.Assert(rawRow[i] == 'F' || rawRow[i] == 'B');
                Row <<= 1;

                if (rawRow[i] == 'B')
                {
                    Row |= 1;
                }
            }


            for (int i = 0; i < 3; i++)
            {
                Debug.Assert(rawColumn[i] == 'L' || rawColumn[i] == 'R');
                Column <<= 1;

                if (rawColumn[i] == 'R')
                {
                    Column |= 1;
                }
            }

            SeatId = Row * 8 + Column;
        }
    }
}
