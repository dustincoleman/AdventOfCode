using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2020
{
    public class Day05
    {
        [Fact]
        public void Part1()
        {
            int result = File.ReadAllLines("Day05Input.txt")
                .Select(line => new BoardingPass(line))
                .Max(pass => pass.SeatId);

            Assert.Equal(885, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 0;
            List<int> seatIds = File.ReadAllLines("Day05Input.txt")
                .Select(line => new BoardingPass(line).SeatId)
                .ToList();

            seatIds.Sort();

            for (int i = 1; i < seatIds.Count; i++)
            {
                if (seatIds[i - 1] + 1 != seatIds[i])
                {
                    result = seatIds[i - 1] + 1;
                    break;
                }
            }

            Assert.Equal(623, result);
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
