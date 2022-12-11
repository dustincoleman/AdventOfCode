namespace AdventOfCode2022
{
    public class Day11
    {
        [Fact]
        public void Part1()
        {
            List<Monkey> monkeys = LoadPuzzle();

            for (int i = 0; i < 20; i++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    for (int countdown = monkey.Items.Count; countdown > 0; countdown--)
                    {
                        long item = monkey.Items.Dequeue();

                        item = monkey.Operation switch
                        {
                            InspectOperation.Plus => item + monkey.Operand,
                            InspectOperation.Times => item * monkey.Operand,
                            InspectOperation.Squared => item * item,
                            _ => throw new Exception()
                        };

                        item /= 3;

                        int tossTo = (item % monkey.TestOperand == 0) ? monkey.TrueDestination : monkey.FalseDesitation;
                        monkeys[tossTo].Items.Enqueue(item);
                        monkey.InspectionCount++;
                    }
                }
            }

            long monkeyBusiness = monkeys.Select(m => m.InspectionCount).OrderDescending().Take(2).Aggregate((x, y) => x * y);
            Assert.Equal(121450, monkeyBusiness);
        }

        [Fact]
        public void Part2()
        {
            List<Monkey> monkeys = LoadPuzzle();
            int multiple = monkeys.Select(m => m.TestOperand).Aggregate((x, y) => x * y);

            for (int i = 0; i < 10000; i++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    for (int countdown = monkey.Items.Count; countdown > 0; countdown--)
                    {
                        long item = monkey.Items.Dequeue();

                        item = monkey.Operation switch
                        {
                            InspectOperation.Plus => item + monkey.Operand,
                            InspectOperation.Times => item * monkey.Operand,
                            InspectOperation.Squared => item * item,
                            _ => throw new Exception()
                        };

                        item %= multiple;

                        int tossTo = (item % monkey.TestOperand == 0) ? monkey.TrueDestination : monkey.FalseDesitation;
                        monkeys[tossTo].Items.Enqueue(item);
                        monkey.InspectionCount++;
                    }
                }
            }

            long monkeyBusiness = monkeys.Select(m => m.InspectionCount).OrderDescending().Take(2).Aggregate((x, y) => x * y);
            Assert.Equal(28244037010, monkeyBusiness);
        }

        private List<Monkey> LoadPuzzle()
        {
            List<Monkey> list = new List<Monkey>();

            foreach (string[] lines in PuzzleFile.ReadAllLineGroups("Day11.txt"))
            {
                string[] operationSplit = lines[2].Substring("  Operation: new = old ".Length).Split(' ');

                list.Add(
                    new Monkey()
                    {
                        Items = new Queue<long>(lines[1].Substring("  Starting items: ".Length).Split(", ").Select(long.Parse)),
                        Operation = operationSplit[0] switch
                        {
                            "+" => InspectOperation.Plus,
                            "*" => (operationSplit[1] != "old") ? InspectOperation.Times : InspectOperation.Squared,
                            _ => throw new Exception()
                        },
                        Operand = (operationSplit[1] != "old") ? int.Parse(operationSplit[1]) : 0,
                        TestOperand = int.Parse(lines[3].Substring("  Test: divisible by ".Length)),
                        TrueDestination = int.Parse(lines[4].Substring("    If true: throw to monkey ".Length)),
                        FalseDesitation = int.Parse(lines[5].Substring("    If false: throw to monkey ".Length))
                    });
            }

            return list;
        }

        private class Monkey
        {
            public Queue<long> Items;
            public InspectOperation Operation;
            public int Operand;
            public int TestOperand;
            public int TrueDestination;
            public int FalseDesitation;
            public long InspectionCount;
        }

        private enum InspectOperation
        {
            Plus,
            Times,
            Squared
        }
    }
}