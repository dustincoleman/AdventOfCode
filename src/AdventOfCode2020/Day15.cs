using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day15
    {
        public static int[] Input = new[] { 6, 19, 0, 5, 7, 13, 1 };

        public static void Part1()
        {
            long result = Solve(Input, 2020);
            Debug.Assert(result == 468);
        }

        public static void Part2()
        {
            long result = Solve(Input, 30000000);
            Debug.Assert(result == 1801753);
        }

        private static long Solve(int[] input, long turnToSolve)
        {
            Dictionary<long, long> lastSpokenByNumber = new Dictionary<long, long>();

            long lastSpoken = 0;

            for (long l = 0; l < input.Length; l++)
            {
                lastSpoken = input[l];
                lastSpokenByNumber[lastSpoken] = l;
            }

            for (long l = input.Length; l < turnToSolve; l++)
            {
                long spoken = 0;
                if (lastSpokenByNumber.ContainsKey(lastSpoken))
                {
                    spoken = l - lastSpokenByNumber[lastSpoken] - 1;
                }

                lastSpokenByNumber[lastSpoken] = l - 1;
                lastSpoken = spoken;
            }

            return lastSpoken;
        }
    }
}
