using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2020
{
    public class Day08
    {
        [Fact]
        public void Part1()
        {
            Machine machine = new Machine(ParseProgram("Day08Input.txt"));
            bool completed = machine.TryRunToEnd(null, out int result);

            Assert.Equal(1331, result);
        }

        [Fact]
        public void Part2()
        {
            Instruction[] program = ParseProgram("Day08Input.txt");

            bool completed = false;
            int result = 0;

            for (int i = 0; i < program.Length && !completed; i++)
            {
                completed = new Machine(program).TryRunToEnd(i, out result);
            }

            Assert.Equal(1121, result);
        }

        private static Instruction[] ParseProgram(string filename)
        {
            return 
                File.ReadAllLines(filename)
                    .Select(line => new Instruction(line))
                    .ToArray();
        }
    }

    enum OpCode
    {
        acc,
        jmp,
        nop
    }

    class Instruction
    {
        public readonly OpCode op;
        public readonly int arg;

        public Instruction(string rawInput)
        {
            op = Enum.Parse<OpCode>(rawInput.Substring(0, 3));
            arg = int.Parse(rawInput.Substring(4));
        }
    }

    class Machine
    {
        Instruction[] program;
        bool[] visited;
        int position;
        int accumulator;

        public Machine(Instruction[] program)
        {
            this.program = program;
            visited = new bool[program.Length];
            position = 0;
            accumulator = 0;
        }

        public bool TryRunToEnd(int? flipIndex, out int accumulator)
        {
            while (true)
            {
                if (position == program.Length)
                {
                    accumulator = this.accumulator;
                    return true;
                }
                if (visited[position])
                {
                    accumulator = this.accumulator;
                    return false;
                }

                visited[position] = true;

                Instruction instruction = program[position];

                switch (instruction.op)
                {
                    case OpCode.acc:
                        this.accumulator += instruction.arg;
                        position++;
                        break;
                    case OpCode.jmp:
                        position += (position == flipIndex) ? 1 : instruction.arg;
                        break;
                    case OpCode.nop:
                        position += (position == flipIndex) ? instruction.arg : 1;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
