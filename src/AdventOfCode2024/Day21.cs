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
                long min = long.MaxValue;

                foreach (string doorSequence in GetPermutations(door.GetSequences(code)))
                {
                    foreach (string robot1Sequence in GetPermutations(robot.GetSequences(doorSequence)))
                    {
                        List<List<string>> robot2Sequence = robot.GetSequences(robot1Sequence);
                        int sequenceLength = robot2Sequence.Sum(list => list.Select(s => s.Length).Min());

                        if (sequenceLength < min)
                        {
                            min = sequenceLength;
                        }
                    }
                }

                result += long.Parse(code.Substring(0, code.Length - 1)) * min;
            }

            Assert.Equal(109758, actual: result);
        }

        [Fact]
        public void Part2()
        {
            string[] puzzle = File.ReadAllLines("Day21.txt");
            Keypad door = Keypad.DoorKeypad();
            Keypad robot = Keypad.RobotKeypad();

            long result = 0;

            

            Assert.Equal(0, actual: result);
        }

        private IEnumerable<string> GetPermutations(List<List<string>> list)
        {
            int[] positions = new int[list.Count];
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append(list[i][positions[i]]);
                }

                yield return sb.ToString();
                sb.Clear();

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (positions[i] < list[i].Count - 1)
                    {
                        positions[i]++;
                        break;
                    }

                    if (i == 0)
                    {
                        yield break;
                    }

                    positions[i] = 0;
                }
            }
        }

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

            internal List<List<string>> GetSequences(string buttons)
            {
                List<List<string>> sequences = new List<List<string>>();
                Point2 position = this['A'];
                foreach (char button in buttons)
                {
                    List<string> subSequences = new List<string>();
                    GetSequencesHelper(this[button], position, string.Empty, subSequences);
                    sequences.Add(subSequences);
                    position = this[button];
                }
                return sequences;
            }

            internal void GetSequencesHelper(Point2 buttonPosition, Point2 position, string sequence, List<string> sequences)
            {
                // Are we at the button?
                if (position == buttonPosition)
                {
                    sequences.Add(sequence + 'A');
                    return;
                }

                // Figure out how far away we are
                int remaining = (buttonPosition - position).Manhattan();

                // Recurse
                foreach (Direction d in Direction.All())
                {
                    Point2 next = position + d;
                    if (next >= Point2.Zero && next < Bounds && next != Gap)
                    {
                        int nextRemaining = (buttonPosition - next).Manhattan();
                        if (nextRemaining < remaining)
                        {
                            GetSequencesHelper(buttonPosition, next, sequence + d.ToArrow(), sequences);
                        }
                    }
                }
            }
        }
    }
}
