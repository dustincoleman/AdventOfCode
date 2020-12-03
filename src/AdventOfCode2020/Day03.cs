using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day03
    {
        public static void Part1()
        {
            Puzzle puzzle = new Puzzle(File.ReadAllLines("Day03Input.txt"));

            // Right 3, down 1.
            long trees = puzzle.CountTrees(3, 1);

            Debugger.Break();
        }

        public static void Part2()
        {
            Puzzle puzzle = new Puzzle(File.ReadAllLines("Day03Input.txt"));

            // Right 1, down 1.
            // Right 3, down 1. (This is the slope you already checked.)
            // Right 5, down 1.
            // Right 7, down 1.
            // Right 1, down 2.
            long answer = 
                new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }
                .Select(pair => puzzle.CountTrees(pair.Item1, pair.Item2))
                .Aggregate((x, y) => x * y);

            Debugger.Break();
        }
    }

    class Puzzle
    {
        private bool[,] map;
        private int inputWidth;
        private int inputLength;

        internal Puzzle(string[] input)
        {
            inputWidth = input[0].Length;
            inputLength = input.Length;

            map = new bool[inputWidth, inputLength];

            for (int y = 0; y < inputLength; y++)
            {
                for (int x = 0; x < inputWidth; x++)
                {
                    char ch = input[y][x];
                    Debug.Assert(ch == '.' || ch == '#');
                    map[x, y] = (ch == '#');
                }
            }
        }

        internal long CountTrees(int xInc, int yInc)
        {
            long count = 0;

            int x = 0;
            int y = 0;

            while ((y += yInc) < inputLength)
            {
                x += xInc;

                if (map[x % inputWidth, y])
                {
                    count++;
                }
            }

            return count;
        }
    }
}
