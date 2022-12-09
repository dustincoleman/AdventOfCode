namespace AdventOfCode2022
{
    public class Day05
    {
        [Fact]
        public void Part1()
        {
            string[][] parts = PuzzleFile.ReadAllLineGroups("Day05.txt");

            string[] stackLines = parts[0];
            int stackCount = stackLines[stackLines.Length - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            Stack<char>[] stacks = new Stack<char>[stackCount];
            
            for (int i = 0; i < stackCount; i++)
            {
                stacks[i] = new Stack<char>();
            }

            for (int i = stackLines.Length - 2; i >= 0; i--)
            {
                for (int column = 0; column < stackCount; column++)
                {
                    int offset = 4 * column + 1;
                    char ch = stackLines[i][offset];

                    if (ch >= 'A' && ch <= 'Z')
                    {
                        stacks[column].Push(ch);
                    }
                }
            }

            Regex regex = new Regex("move (?<count>\\d+) from (?<from>\\d+) to (?<to>\\d+)");

            foreach (string commandLine in parts[1])
            {
                Match match = regex.Match(commandLine);

                int count = int.Parse(match.Groups["count"].Value);
                int from = int.Parse(match.Groups["from"].Value) - 1;
                int to = int.Parse(match.Groups["to"].Value) - 1;

                for (int i = 0; i < count; i++)
                {
                    stacks[to].Push(stacks[from].Pop());
                }
            }

            string result = string.Empty;

            foreach (Stack<char> stack in stacks)
            {
                result += stack.Peek();
            }

            Assert.Equal("RTGWZTHLD", result);
        }

        [Fact]
        public void Part2()
        {
            string[][] parts = PuzzleFile.ReadAllLineGroups("Day05.txt");

            string[] stackLines = parts[0];
            int stackCount = stackLines[stackLines.Length - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            Stack<char>[] stacks = new Stack<char>[stackCount];

            for (int i = 0; i < stackCount; i++)
            {
                stacks[i] = new Stack<char>();
            }

            for (int i = stackLines.Length - 2; i >= 0; i--)
            {
                for (int column = 0; column < stackCount; column++)
                {
                    int offset = 4 * column + 1;
                    char ch = stackLines[i][offset];

                    if (ch >= 'A' && ch <= 'Z')
                    {
                        stacks[column].Push(ch);
                    }
                }
            }

            Regex regex = new Regex("move (?<count>\\d+) from (?<from>\\d+) to (?<to>\\d+)");

            foreach (string commandLine in parts[1])
            {
                Match match = regex.Match(commandLine);

                int count = int.Parse(match.Groups["count"].Value);
                int from = int.Parse(match.Groups["from"].Value) - 1;
                int to = int.Parse(match.Groups["to"].Value) - 1;

                Stack<char> anotherStack = new Stack<char>();

                for (int i = 0; i < count; i++)
                {
                    anotherStack.Push(stacks[from].Pop());
                }

                for (int i = 0; i < count; i++)
                {
                    stacks[to].Push(anotherStack.Pop());
                }
            }

            string result = string.Empty;

            foreach (Stack<char> stack in stacks)
            {
                result += stack.Peek();
            }

            Assert.Equal("STHGRZZFR", result);
        }

    }
}