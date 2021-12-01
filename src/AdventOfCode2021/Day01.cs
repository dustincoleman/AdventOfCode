using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode2021
{
    public class Day01
    {
        [Fact]
        public void Part1()
        {
            int result = 0;
            int[] input = File.ReadAllLines("Day01Input.txt").Select(Int32.Parse).ToArray();

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i + 1] > input[i])
                {
                    result++;
                }
            }

            Assert.Equal(1228, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 0;
            int[] input = File.ReadAllLines("Day01Input.txt").Select(Int32.Parse).ToArray();

            for (int i = 0; i < input.Length - 3; i++)
            {
                if (input[i + 3] /* + input[i + 2] + input[i + 1] */ > /* input[i + 2] + input[i + 1] + */ input[i])
                {
                    result++;
                }
            }

            Assert.Equal(1257, result);
        }
    }
}
