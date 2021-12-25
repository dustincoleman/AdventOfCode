using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021
{
    public class Day24
    {
        // See notes in Day24Input.txt
        //
        //  input[0] - 8 == input[13]
        //  input[1] + 7 == input[12]
        //  input[2] - 7 == input[3]
        //  input[4] + 1 == input[5]
        //  input[6] + 8 == input[11]
        //  input[7] + 5 == input[8]
        //  input[9] == input[10]

        [Fact]
        public void Part1()
        {
            ALU alu = new ALU(File.ReadAllLines("Day24Input.txt"));

            List<int> modelNumber = new List<int>() { 9, 2, 9, 2, 8, 9, 1, 4, 9, 9, 9, 9, 9, 1 };

            alu.Run(modelNumber.ToArray());

            Assert.Equal(0, alu.Z);
        }

        [Fact]
        public void Part2()
        {
            ALU alu = new ALU(File.ReadAllLines("Day24Input.txt"));

            List<int> modelNumber = new List<int>() { 9, 1, 8, 1, 1, 2, 1, 1, 6, 1, 1, 9, 8, 1 };

            alu.Run(modelNumber.ToArray());

            Assert.Equal(0, alu.Z);
        }

        private class ALU
        {
            private string[] program;
            private int pos;
            private long[] variables = new long[4];

            internal ALU(string[] program)
            {
                this.program = program
                    .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
                    .ToArray();
            }

            internal long W => this.variables[0];
            internal long X => this.variables[1];
            internal long Y => this.variables[2];
            internal long Z => this.variables[3];

            public void Reset()
            {
                this.pos = 0;
                this.variables = new long[4];
            }

            public void Run(params int[] input)
            {
                Queue<int> inputQueue = new Queue<int>(input);

                for (; this.pos < this.program.Length; this.pos++)
                {
                    string[] parts = program[pos].Split(' ');

                    int aPos = parts[1].Single() - 'w';
                    long a = this.variables[aPos];
                    long b = (parts.Length == 3) ? GetArg(parts[2]) : 0;

                    switch (parts[0])
                    {
                        case "inp":
                            if (inputQueue.Count == 0)
                            {
                                return;
                            }

                            this.variables[aPos] = inputQueue.Dequeue();
                            break;
                        case "add":
                            this.variables[aPos] = a + b;
                            break;
                        case "mul":
                            this.variables[aPos] = a * b;
                            break;
                        case "div":
                            this.variables[aPos] = a / b;
                            break;
                        case "mod":
                            this.variables[aPos] = a % b;
                            break;
                        case "eql":
                            this.variables[aPos] = (a == b) ? 1 : 0;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }

            private long GetArg(string arg)
            {
                if (arg.Length == 1 && arg[0] >= 'w' && arg[0] <= 'z')
                {
                    return this.variables[arg[0] - 'w'];
                }

                return int.Parse(arg);
            }
        }
    }
}
