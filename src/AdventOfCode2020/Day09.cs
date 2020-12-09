using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day09
    {
        public static void Part1()
        {
            long[] input = File.ReadAllLines("Day09Input.txt").Select(l => long.Parse(l)).ToArray();
            
            for (int i = 0; i < input.Length - 25; i++)
            {
                bool matchFound = false;
                long current = input[i + 25];
                HashSet<long> hashSet = new HashSet<long>(input.Skip(i).Take(25));

                foreach (long candidate in hashSet)
                {
                    long other = current - candidate;
                    if (other > 0 && other != candidate && hashSet.Contains(other))
                    {
                        matchFound = true;
                    }

                }

                if (!matchFound)
                {
                    long result = current;
                    Debugger.Break();
                }
            }
        }

        public static void Part2()
        {
            const long part1Result = 26134589;

            long[] input = File.ReadAllLines("Day09Input.txt").Select(l => long.Parse(l)).ToArray();

            for (int length = 2; length < input.Length; length++)
            {
                for (int i = 0; i < input.Length - length + 1; i++)
                {
                    long[] candidate = input.Skip(i).Take(length).ToArray();
                    if (candidate.Sum() == part1Result)
                    {
                        long result = candidate.Min() + candidate.Max();
                        Debugger.Break();
                    }
                }
            }
        }
    }
}
