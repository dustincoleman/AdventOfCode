using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    static class Day14
    {
        private static Regex s_maskRegex = new Regex(@"^mask\s=\s(?<mask>.*)$");
        private static Regex s_assignmentRegex = new Regex(@"^mem\[(?<address>[^\]]*)\]\s=\s(?<value>.*)$");

        public static void Part1()
        {
            long result = RunMachine(new Part1Machine());
            Debug.Assert(result == 12610010960049);
        }

        public static void Part2()
        {
            long result = RunMachine(new Part2Machine());
            Debug.Assert(result == 3608464522781);
        }

        private static long RunMachine(Machine machine)
        {
            foreach (string line in File.ReadAllLines("Day14Input.txt"))
            {
                Match match;

                if ((match = s_maskRegex.Match(line)).Success)
                {
                    machine.SetMask(match.Groups["mask"].Value);
                }
                else if ((match = s_assignmentRegex.Match(line)).Success)
                {
                    machine.SetValue(long.Parse(match.Groups["address"].Value), long.Parse(match.Groups["value"].Value));
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return machine.SumOfValues;
        }

        abstract class Machine
        {
            protected long maskA;
            protected long maskB;
            protected long maskC;

            protected readonly Dictionary<long, long> valuesByAddress = new Dictionary<long, long>();

            public long SumOfValues => valuesByAddress.Values.Sum();

            public void SetMask(string mask)
            {
                maskA = 0;
                maskB = 0;
                maskC = 0;

                foreach (char ch in mask)
                {
                    maskA <<= 1;
                    maskB <<= 1;
                    maskC <<= 1;

                    switch (ch)
                    {
                        case '0':
                            maskA |= 1;
                            break;
                        case '1':
                            maskB |= 1;
                            break;
                        case 'X':
                            maskC |= 1;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public abstract void SetValue(long address, long value);
        }

        class Part1Machine : Machine
        {
            public override void SetValue(long address, long value)
            {
                valuesByAddress[address] = (value & ~maskA) | maskB;
            }
        }

        class Part2Machine : Machine
        {
            public override void SetValue(long address, long value)
            {
                long maskedAddress = ((address & maskA) | maskB) & ~maskC;

                foreach (long combination in GetCombinations(maskC))
                {
                    valuesByAddress[maskedAddress | combination] = value;
                }
            }

            private IEnumerable<long> GetCombinations(long inputMask)
            {
                int bitCount = 0;
                int combinationCount = 1;
                long validMask = 0xFFFFFFFFF;

                // Count the number of bits in the input mask
                for (long bit = 1; (bit & validMask) != 0; bit <<= 1)
                {
                    if ((inputMask & bit) != 0)
                    {
                        bitCount++;
                        combinationCount <<= 1;
                    }
                }

                // Iterate over each combination of bits to be used in the output mask
                for (int i = 0; i < combinationCount; i++)
                {
                    long sourceBit = 1;
                    long targetBit = 1;
                    long outputMask = 0;

                    // Copy the bits from the combination into the output mask
                    for (int j = 0; j < bitCount; j++)
                    {
                        // Find the next bit position in the input mask
                        while ((targetBit & inputMask) == 0)
                        {
                            Debug.Assert(targetBit < validMask);
                            targetBit <<= 1;
                        }

                        // Set the bit in that position in the output mask
                        if ((i & sourceBit) != 0)
                        {
                            outputMask |= targetBit;
                        }

                        sourceBit <<= 1;
                        targetBit <<= 1;
                    }

                    yield return outputMask;
                }
            }
        }
    }
}

