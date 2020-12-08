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
            bool completed = machine.TryRunToEnd(null, out int result);

            Debug.Assert(!completed);
            Debugger.Break();
        }

        public static void Part2()
        {
            string[] lines = File.ReadAllLines("Day08Input.txt");
            Machine machine = new Machine(lines);

            bool completed = false;
            int result;

            for (int i = 0; i < lines.Length && !completed; i++)
            {
                completed = machine.TryRunToEnd(i, out result);
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

public bool TryRunToEnd(int? flipIndex, out int accumulator)
{
    while (true)
    {
        if (position == program.Length)
        {
            accumulator = this.accumulator;
            return true;
        }

        Instruction instruction = program[position];

        if (instruction.hasRun)
        {
            accumulator = this.accumulator;
            return false;
        }

        instruction.hasRun = true;

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

        public void Reset()
        {
            position = 0;
            accumulator = 0;

            foreach (Instruction i in program)
            {
                i.hasRun = false;
            }
        }
    }
}
