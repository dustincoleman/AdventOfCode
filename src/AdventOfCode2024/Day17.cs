
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode2024
{
    public class Day17
    {
        [Fact]
        public void Part1()
        {
            Computer puzzle = LoadPuzzle();
            string result = puzzle.Run();
            Assert.Equal("1,6,7,4,3,0,5,0,6", result);
        }

        [Fact]
        public void Part2()
        {
            Computer puzzle = LoadPuzzle();
            BigInteger result = puzzle.Solve();
            Assert.Equal(216148338630253, result);
        }

        private static Computer LoadPuzzle()
        {
            string[] lines = File.ReadAllLines("Day17.txt");
            return new Computer
            (
                program: lines[4].Replace("Program: ", "").Split(',').Select(int.Parse).ToArray(),
                a: int.Parse(lines[0].Replace("Register A: ", "")),
                b: int.Parse(lines[1].Replace("Register B: ", "")),
                c: int.Parse(lines[2].Replace("Register C: ", ""))
            );
        }

        private class Computer
        {
            private readonly int[] program;
            private readonly BigInteger initB;
            private readonly BigInteger initC;

            private int IP;
            private BigInteger A;
            private BigInteger B;
            private BigInteger C;

            internal Computer(int[] program, BigInteger a, BigInteger b, BigInteger c)
            {
                this.program = program;
                A = a;
                B = this.initB = b;
                C = this.initC = c;
            }

            internal string Run()
            {
                List<int> output = new List<int>();

                while (Go(out int next))
                {
                    output.Add(next);
                }

                return string.Join(',', output);
            }

            internal BigInteger Solve(BigInteger solution = default, int solved = 0)
            {
                if (solved == this.program.Length)
                {
                    return solution;
                }

                int toFind = this.program[this.program.Length - 1 - solved];

                for (int i = 0; i < 8; i++)
                {
                    Reset((solution << 3) + i);

                    if (Go(out int next) && next == toFind && A == solution)
                    {
                        BigInteger result = Solve((solution << 3) + i, solved + 1);
                        if (result > 0)
                        {
                            return result;
                        }
                    }
                }

                return -1;
            }

            private bool Go(out int output)
            {
                bool success = false;
                output = 0;

                while (!success && IP < this.program.Length)
                {
                    int instruction = this.program[IP];
                    int operand = this.program[IP + 1];

                    switch (instruction)
                    {
                        case 0: // adv
                            int result = (1 << (int)Combo(operand));
                            A = A / result;
                            break;

                        case 1: // bxl
                            B = B ^ operand;
                            break;

                        case 2: // bst
                            B = Combo(operand) % 8;
                            break;

                        case 3: // jnz
                            if (A != 0)
                            {
                                IP = operand;
                                continue;
                            }
                            break;

                        case 4: // bxc
                            B = B ^ C;
                            break;

                        case 5: // out
                            output = (int)(Combo(operand) % 8);
                            success = true;
                            break;

                        case 6: // bdv
                            B = A / (1 << (int)Combo(operand));
                            break;

                        case 7: // cdv
                            C = A / (1 << (int)Combo(operand));
                            break;
                    }

                    IP += 2;
                }

                return success;
            }

            private BigInteger Combo(int operand) => operand switch
            {
                1 or 2 or 3 => operand,
                4 => A,
                5 => B,
                6 => C,
                _ => throw new ArgumentException()
            };

            private void Reset(BigInteger a)
            {
                A = a;
                B = this.initB;
                C = this.initC;
                IP = 0;
            }
        }
    }
}
