namespace AdventOfCode2024
{
    public class Day21
    {
        [Fact]
        public void Part1()
        {
            string[] puzzle = File.ReadAllLines("Day21.txt");
            Keypad door = Keypad.DoorKeypad();
            Keypad robot = Keypad.RobotKeypad();

            long result = 0;

            foreach (string code in puzzle)
            {
                long min = GetMinButtonPresses(code, door, robot, 2, new Dictionary<ButtonKey, long>());
                result += long.Parse(code.Substring(0, code.Length - 1)) * min;
            }

            Assert.Equal(109758, result);
        }

        [Fact]
        public void Part2()
        {
            string[] puzzle = File.ReadAllLines("Day21.txt");
            Keypad door = Keypad.DoorKeypad();
            Keypad robot = Keypad.RobotKeypad();

            long result = 0;

            foreach (string code in puzzle)
            {
                long min = GetMinButtonPresses(code, door, robot, 25, new Dictionary<ButtonKey, long>());
                result += long.Parse(code.Substring(0, code.Length - 1)) * min;
            }

            Assert.Equal(134341709499296, result);
        }

        private long GetMinButtonPresses(string buttons, Keypad current, Keypad next, int robots, Dictionary<ButtonKey, long> minByButtonKey)
        {
            ButtonKey key = new ButtonKey(buttons, robots);

            if (minByButtonKey.TryGetValue(key, out long cached))
            {
                return cached;
            }

            if (robots < 0)
            {
                return buttons.Length;
            }

            long result = 0;
            char position = 'A';

            for (int i = 0; i < buttons.Length; i++)
            {
                long min = long.MaxValue;

                foreach (string nextCombo in current.GetCombos(position, buttons[i]))
                {
                    long nextMin = GetMinButtonPresses(nextCombo, next, next, robots - 1, minByButtonKey);
                    min = long.Min(min, nextMin);
                }

                result += min;
                position = buttons[i];
            }

            minByButtonKey.Add(key, result);

            return result;
        }

        private record struct ButtonKey(string Buttons, int Robots);

        private class Keypad
        {
            private readonly Dictionary<char, Point2> locationsByKey = new Dictionary<char, Point2<int>>();

            internal Point2 this[char ch] => this.locationsByKey[ch];

            internal Point2 Gap { get; private init; }

            internal Point2 Bounds { get; private init; }

            internal static Keypad DoorKeypad()
            {
                return new Keypad()
                {
                    locationsByKey =
                    {
                        { '7', (0,0) }, { '8', (1,0) }, { '9', (2,0) },
                        { '4', (0,1) }, { '5', (1,1) }, { '6', (2,1) },
                        { '1', (0,2) }, { '2', (1,2) }, { '3', (2,2) },
                                        { '0', (1,3) }, { 'A', (2,3) },
                    },
                    Gap = (0,3),
                    Bounds = (3,4)
                };
            }

            internal static Keypad RobotKeypad()
            {
                return new Keypad()
                {
                    locationsByKey =
                    {
                                        { '^', (1,0) }, { 'A', (2,0) },
                        { '<', (0,1) }, { 'v', (1,1) }, { '>', (2,1) },
                    },
                    Gap = (0,0),
                    Bounds = (3, 2)
                };
            }
            internal List<string> GetCombos(char position, char target)
            {
                List<string> combos = new List<string>();
                GetCombosHelper(this[position], this[target], string.Empty, combos);
                return combos;
            }

            internal void GetCombosHelper(Point2 position, Point2 target, string sequence, List<string> sequences)
            {
                // Are we at the button?
                if (position == target)
                {
                    sequences.Add(sequence + 'A');
                    return;
                }

                // Figure out how far away we are
                int remaining = (target - position).Manhattan();

                // Recurse
                foreach (Direction d in Direction.All())
                {
                    Point2 next = position + d;
                    if (next >= Point2.Zero && next < Bounds && next != Gap)
                    {
                        int nextRemaining = (target - next).Manhattan();
                        if (nextRemaining < remaining)
                        {
                            GetCombosHelper(next, target, sequence + d.ToArrow(), sequences);
                        }
                    }
                }
            }
        }
    }
}
