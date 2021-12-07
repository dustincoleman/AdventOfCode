using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode2021
{
    public class Day07
    {
        [Fact]
        public void Part1()
        {
            int[] input = File.ReadAllLines("Day07Input.txt")[0].Split(',').Select(Int32.Parse).ToArray();

            int max = input.Max();
            long cheapestCost = long.MaxValue;

            for (int i = 0; i <= max; i++)
            {
                long cost = ComputeCost(input, i);

                if (cost < cheapestCost)
                {
                    cheapestCost = cost;
                }
            }

            Assert.Equal(348664, cheapestCost);
        }

        private long ComputeCost(int[] input, int position)
        {
            long cost = 0;

            foreach (int i in input)
            {
                cost += Math.Abs(i - position);
            }

            return cost;
        }

        [Fact]
        public void Part2()
        {
            int[] input = File.ReadAllLines("Day07Input.txt")[0].Split(',').Select(Int32.Parse).ToArray();

            int max = input.Max();
            long cheapestCost = long.MaxValue;

            for (int i = 0; i <= max; i++)
            {
                long cost = ComputeCost2(input, i);

                if (cost < cheapestCost)
                {
                    cheapestCost = cost;
                }
            }

            Assert.Equal(100220525, cheapestCost);
        }

        private long ComputeCost2(int[] input, int position)
        {
            long cost = 0;

            foreach (int i in input)
            {
                int n = Math.Abs(i - position);
                cost += n * (n + 1) / 2;
            }

            return cost;
        }
    }
}
