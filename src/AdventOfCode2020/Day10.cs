using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day10
    {
        public static void Part1()
        {
            List<int> input = File.ReadAllLines("Day10Input.txt").Select(int.Parse).ToList();
            input.Add(0); // The port
            input.Sort();

            int oneVoltCount = 0;
            int threeVoltCount = 1; // The built in supply

            for (int i = 0; i < input.Count - 1; i++)
            {
                switch(input[i + 1] - input[i])
                {
                    case 1:
                        oneVoltCount++;
                        break;
                    case 2:
                        break;
                    case 3:
                        threeVoltCount++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            int result = oneVoltCount * threeVoltCount;
            Debugger.Break();
        }

        public static void Part2()
        {
            List<int> input = File.ReadAllLines("Day10Input.txt").Select(int.Parse).ToList();
            input.Add(0);
            input.Sort();

            int[] nextOptions = new int[input.Count];

            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = i + 1; j < input.Count && j <= i + 3; j++)
                {
                    if (input[j] - input[i] <= 3)
                    {
                        nextOptions[i]++;
                    }
                }
            }

            long result = 1;

            for (int i = 0; i < nextOptions.Length - 1; i++)
            {
                if (i < nextOptions.Length - 3 && nextOptions[i] == 3 && nextOptions[i + 1] == 3 && nextOptions[i + 2] == 2)
                {
                    result *= 7;
                    i += 2;
                }
                else if (i < nextOptions.Length - 2 && nextOptions[i] == 3 && nextOptions[i + 1] == 2)
                {
                    result *= 4;
                    i += 1;
                }
                else if (nextOptions[i] == 2)
                {
                    result *= 2;
                }
            }

            Debugger.Break();
        }
    }
}
