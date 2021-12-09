using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace AdventOfCode2021
{
    public class Day09
    {
        [Fact]
        public void Part1()
        {
            string[] input = File.ReadAllLines("Day09Input.txt");

            int width = input[0].Length;
            int height = input.Length;

            int[,] map = new int[width, height];
            List<int> lowPoints = new List<int>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = int.Parse(input[y][x].ToString());
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (IsLowPoint(map, x, y, width, height))
                    {
                        lowPoints.Add(map[x, y]);
                    }
                }
            }

            long result = lowPoints.Sum(x => x + 1);

            Assert.Equal(462, result);
        }

        [Fact]
        public void Part2()
        {
            string[] input = File.ReadAllLines("Day09Input.txt");

            int width = input[0].Length;
            int height = input.Length;

            int[,] map = new int[width, height];
            List<long> basins = new List<long>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = int.Parse(input[y][x].ToString());
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (IsLowPoint(map, x, y, width, height))
                    {
                        basins.Add(SizeOfBasin(map, x, y, width, height, new bool[width, height]));
                    }
                }
            }

            long result = basins.OrderByDescending(x => x).Take(3).Aggregate((x, y) => x * y);

            Assert.Equal(1397760, result);
        }

        private bool IsLowPoint(int[,] map, int x, int y, int width, int height)
        {
            int current = map[x, y];

            // left
            if (x > 0 && map[x - 1, y] <= current)
            {
                return false;
            }

            // right
            if (x < width - 1 && map[x + 1, y] <= current)
            {
                return false;
            }

            // top
            if (y > 0 && map[x, y - 1] <= current)
            {
                return false;
            }

            // bottom
            if (y < height - 1 && map[x, y + 1] <= current)
            {
                return false;
            }

            return true;
        }

        private int SizeOfBasin(int[,] map, int x, int y, int width, int height, bool[,] visited)
        {
            if (x < 0 || x >= width || y < 0 || y >= height || map[x, y] == 9 || visited[x, y])
            {
                return 0;
            }

            visited[x, y] = true;

            int size = 1;

            size += SizeOfBasin(map, x - 1, y, width, height, visited);
            size += SizeOfBasin(map, x + 1, y, width, height, visited);
            size += SizeOfBasin(map, x, y - 1, width, height, visited);
            size += SizeOfBasin(map, x, y + 1, width, height, visited);

            return size;
        }
    }
}
