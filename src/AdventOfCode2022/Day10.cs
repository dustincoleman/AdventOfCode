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

            int xRegister = 1;
            int cycle = 0;

            Queue<Instruction> instructions = LoadPuzzle();
            Instruction currentInstruction = new Instruction();
            int remaining = 0;
            
            while (instructions.Any())
            {
                if (remaining == 0)
                {
                    if (currentInstruction.Code == "addx")
                    {
                        xRegister += currentInstruction.Operand;
                    }

                    currentInstruction = instructions.Dequeue();

                    remaining = currentInstruction.Code switch
                    {
                        "noop" => 1,
                        "addx" => 2,
                        _ => throw new Exception()
                    };
                }

                // Start of cycle

                if ((cycle + 1 - 20) % 40 == 0)
                {
                    totalSignalStrength += xRegister * (cycle + 1);
                }

                // End of cycle

                remaining--;
                cycle++;
            }

            Assert.Equal(12640, totalSignalStrength);
        }

        [Fact]
        public void Part2()
        {
            Grid2<char> screen = new Grid2<char>(40, 6);
            foreach (Point2 p in screen.Points)
            {
                screen[p] = ' ';
            }

            int xRegister = 1;
            int cycle = 0;

            Queue<Instruction> instructions = LoadPuzzle();
            Instruction currentInstruction = new Instruction();
            int remaining = 0;

            while (instructions.Any())
            {
                string spritePosition = string.Empty;
                for (int i = 0; i < 40; i++)
                {
                    if (i == xRegister - 1 || i == xRegister || i == xRegister + 1)
                    {
                        spritePosition += '#';
                    }
                    else
                    {
                        spritePosition += '.';
                    }
                }

                if (remaining == 0)
                {
                    if (currentInstruction.Code == "addx")
                    {
                        xRegister += currentInstruction.Operand;
                    }

                    currentInstruction = instructions.Dequeue();

                    remaining = currentInstruction.Code switch
                    {
                        "noop" => 1,
                        "addx" => 2,
                        _ => throw new Exception()
                    };
                }

                // Start of cycle

                Point2 screenPoint = new Point2(cycle % 40, (cycle % 240) / 40);

                if (screenPoint.X == xRegister - 1 || screenPoint.X == xRegister || screenPoint.X == xRegister + 1)
                {
                    screen[screenPoint] = '#';
                }
                else
                {
                    screen[screenPoint] = '.';
                }

                // End of cycle

                remaining--;
                cycle++;
            }

            StringBuilder builder = new StringBuilder();

            foreach (Grid2Row<char> row in screen.Rows)
            {
                builder.AppendLine(new string(row.ToArray()));
            }

            StringBuilder expected = new StringBuilder();
            expected.AppendLine("####.#..#.###..####.#....###....##.###..");
            expected.AppendLine("#....#..#.#..#....#.#....#..#....#.#..#.");
            expected.AppendLine("###..####.###....#..#....#..#....#.#..#.");
            expected.AppendLine("#....#..#.#..#..#...#....###.....#.###..");
            expected.AppendLine("#....#..#.#..#.#....#....#.#..#..#.#.#..");
            expected.AppendLine("####.#..#.###..####.####.#..#..##..#..#.");

            Assert.Equal(expected.ToString(), builder.ToString());
            
        }

        private Queue<Instruction> LoadPuzzle()
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

            return instructions;
        }

        struct Instruction
        {
            public string Code;
            public int Operand;
        }
    }
}