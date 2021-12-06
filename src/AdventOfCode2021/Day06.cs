using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode2021
{
    public class Day06
    {
        [Fact]
        public void Part1()
        {
            long[] counts = new long[9];

            foreach (int i in File.ReadAllLines("Day06Input.txt")[0].Split(',').Select(Int32.Parse).ToArray())
            {
                counts[i]++;
            }

            for (int i = 0; i < 80; i++)
            {
                long newFish = counts[0];

                for (int j = 0; j < 8; j++)
                {
                    counts[j] = counts[j + 1];
                }

                counts[8] = newFish;
                counts[6] += newFish;
            }

            long result = counts.Sum();

            Assert.Equal(355386, result);
        }

        [Fact]
        public void Part2()
        {
            long[] counts = new long[9];

            foreach (int i in File.ReadAllLines("Day06Input.txt")[0].Split(',').Select(Int32.Parse).ToArray())
            {
                counts[i]++;
            }

            for (int i = 0; i < 256; i++)
            {
                long newFish = counts[0];

                for (int j = 0; j < 8; j++)
                {
                    counts[j] = counts[j + 1];
                }

                counts[8] = newFish;
                counts[6] += newFish;
            }

            long result = counts.Sum();

            Assert.Equal(1613415325809, result);
        }
    }
}
