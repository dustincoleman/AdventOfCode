namespace AdventOfCode2022
{
    public class Day21
    {
        [Fact]
        public void Part1()
        {
            bool unused = false;
            long result = GetValue("root", LoadPuzzle(), ref unused);
            Assert.Equal(152479825094094, result);
        }

        [Fact]
        public void Part2()
        {
            Dictionary<string, string> puzzle = LoadPuzzle();

            bool leftIsHuman = false, rightIsHuman = false;
            string[] split = puzzle["root"].Split(' ');
            long left = GetValue(split[0], puzzle, ref leftIsHuman);
            long right = GetValue(split[2], puzzle, ref rightIsHuman);
            long current = (leftIsHuman) ? right : left;
            string nextName = (leftIsHuman) ? split[0] : split[2];

            while (nextName != "humn")
            {
                leftIsHuman = rightIsHuman = false;
                split = puzzle[nextName].Split(' ');
                left = GetValue(split[0], puzzle, ref leftIsHuman);
                right = GetValue(split[2], puzzle, ref rightIsHuman);

                if (leftIsHuman)
                {
                    switch (split[1])
                    {
                        case "+":
                            current -= right;
                            break;
                        case "-":
                            current += right;
                            break;
                        case "*":
                            current /= right;
                            break;
                        case "/":
                            current *= right;
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else
                {
                    switch (split[1])
                    {
                        case "+":
                            current -= left;
                            break;
                        case "-":
                            current = -(current - left);
                            break;
                        case "*":
                            current /= left;
                            break;
                        case "/":
                            current = (left / current);
                            break;
                        default:
                            throw new Exception();
                    }
                }

                nextName = (leftIsHuman) ? split[0] : split[2];
            }

            // NOTE: Example and problem data both reach humn - [monkey], so we can just use current as the answer
            Assert.Equal(3360561285172, current);
        }

        private long GetValue(string name, Dictionary<string, string> puzzle, ref bool humn)
        {
            if (name == "humn")
            {
                humn = true;
            }

            string[] split = puzzle[name].Split(' ');

            if (split.Length == 1)
            {
                return int.Parse(split[0]);
            }

            return split[1] switch
            {
                "+" => GetValue(split[0], puzzle, ref humn) + GetValue(split[2], puzzle, ref humn),
                "-" => GetValue(split[0], puzzle, ref humn) - GetValue(split[2], puzzle, ref humn),
                "*" => GetValue(split[0], puzzle, ref humn) * GetValue(split[2], puzzle, ref humn),
                "/" => GetValue(split[0], puzzle, ref humn) / GetValue(split[2], puzzle, ref humn),
                _ => throw new Exception()
            };
        }

        private Dictionary<string, string> LoadPuzzle()
        {
            Dictionary<string, string> puzzle = new Dictionary<string, string>();

            foreach (string line in File.ReadAllLines("Day21.txt"))
            {
                string[] split = line.Split(": ");
                puzzle.Add(split[0], split[1]);
            }

            return puzzle;
        }
    }
}