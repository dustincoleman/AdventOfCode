using System.Diagnostics;
using System.Text;

namespace AdventOfCode2022
{
    public class Day10
    {
        [Fact]
        public void Part1()
        {
            long totalSignalStrength = 0;

            LoadPuzzle().Run((clock, xRegister) =>
            {
                if ((clock + 1 - 20) % 40 == 0)
                {
                    totalSignalStrength += xRegister * (clock + 1);
                }
            });

            Assert.Equal(12640, totalSignalStrength);
        }

        [Fact]
        public void Part2()
        {
            Grid2<char> screen = new Grid2<char>(40, 6);

            LoadPuzzle().Run((clock, xRegister) =>
            {
                Point2 screenPoint = new Point2(clock % 40, (clock % 240) / 40);

                if (screenPoint.X == xRegister - 1 || screenPoint.X == xRegister || screenPoint.X == xRegister + 1)
                {
                    screen[screenPoint] = '#';
                }
                else
                {
                    screen[screenPoint] = '.';
                }
            });

            StringBuilder builder = new StringBuilder();
            StringBuilder expected = new StringBuilder();

            foreach (Grid2Row<char> row in screen.Rows)
            {
                builder.AppendLine(new string(row.ToArray()));
            }

            expected.AppendLine("####.#..#.###..####.#....###....##.###..");
            expected.AppendLine("#....#..#.#..#....#.#....#..#....#.#..#.");
            expected.AppendLine("###..####.###....#..#....#..#....#.#..#.");
            expected.AppendLine("#....#..#.#..#..#...#....###.....#.###..");
            expected.AppendLine("#....#..#.#..#.#....#....#.#..#..#.#.#..");
            expected.AppendLine("####.#..#.###..####.####.#..#..##..#..#.");

            Assert.Equal(expected.ToString(), builder.ToString());
            
        }

        private Scanner LoadPuzzle()
        {
            Queue<Instruction> instructions = new Queue<Instruction>();

            foreach (string line in File.ReadAllLines("Day10.txt"))
            {
                if (line == "noop")
                {
                    instructions.Enqueue(new Instruction() { Code = line });
                }
                else
                {
                    string[] split = line.Split(' ');
                    instructions.Enqueue(new Instruction() { Code = split[0], Operand = int.Parse(split[1]) });
                }
            }

            return new Scanner(instructions);
        }

        private class Scanner
        {
            private Instruction _instruction;
            private int _remaining = 0;
            private int _clock = 0;
            private int _xRegister = 1;

            private Queue<Instruction> _program;

            internal delegate void ClockTickHandler(int clock, int xRegister);

            internal Scanner(Queue<Instruction> program)
            {
                _program = program;
            }

            internal void Run(ClockTickHandler onClockTick)
            {
                while (_program.Any())
                {
                    if (_remaining == 0)
                    {
                        if (_instruction.Code == "addx")
                        {
                            _xRegister += _instruction.Operand;
                        }

                        _instruction = _program.Dequeue();

                        _remaining = _instruction.Code switch
                        {
                            "noop" => 1,
                            "addx" => 2,
                            _ => throw new Exception()
                        };
                    }

                    onClockTick(_clock, _xRegister);

                    _clock++;
                    _remaining--;
                }
            }
        }

        struct Instruction
        {
            public string Code;
            public int Operand;
        }
    }
}