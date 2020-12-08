using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day08
    {
        public static void Part1()
        {
            Machine machine = new Machine(File.ReadAllLines("Day08Input.txt"));
            int result = machine.Run(null, null, out bool completed);

            Debug.Assert(!completed);
            Debugger.Break();
        }

        public static void Part2()
        {
            string[] lines = File.ReadAllLines("Day08Input.txt");
            Machine machine = new Machine(lines);

            bool completed = false;
            int result = 0;

            for (int i = 0; i < lines.Length && !completed; i++)
            {
                result = machine.Run(i, 10000, out completed);
                machine.Reset();
            }
            
            Debug.Assert(completed);
            Debugger.Break();
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
        public OpCode op;
        public int arg;
        public bool hasRun;

        public Instruction(string rawInput)
        {
            op = Enum.Parse<OpCode>(rawInput.Substring(0, 3));
            arg = int.Parse(rawInput.Substring(4));
            hasRun = false;
        }
    }

    class Machine
    {
        Instruction[] program;
        int position;
        int accumulator;

        public Machine(string[] rawInput)
        {
            program = rawInput
                .Select(line => new Instruction(line))
                .ToArray();
            position = 0;
            accumulator = 0;
        }

        public int Run(int? flipIndex, int? instructionsToRun, out bool completed)
        {
            int count = 0;

            while (true)
            {
                if (position == program.Length)
                {
                    completed = true;
                    return accumulator;
                }

                Instruction instruction = program[position];

                if ((instruction.hasRun && instructionsToRun == null) || count == instructionsToRun)
                {
                    completed = false;
                    return accumulator;
                }

                instruction.hasRun = true;

                bool flip = (position == flipIndex);

                switch (instruction.op)
                {
                    case OpCode.acc:
                        accumulator += instruction.arg;
                        position++;
                        break;
                    case OpCode.jmp:
                        position += (flip) ? 1 : instruction.arg;
                        break;
                    case OpCode.nop:
                        position += (flip) ? instruction.arg : 1;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                count++;
            }
        }

        public void Reset()
        {
            position = 0;
            accumulator = 0;
        }
    }
}
