namespace AdventOfCode2022
{
    public class Day13
    {
        [Fact]
        public void Part1()
        {
            int result = 0;
            int current = 1;

            foreach (string[] pair in PuzzleFile.ReadAllLineGroups("Day13.txt"))
            {
                if (Compare(pair[0], pair[1]) == -1)
                {
                    result += current;
                }

                current++;
            }

            Assert.Equal(5852, result);
        }

        [Fact]
        public void Part2()
        {
            List<string> packets = File.ReadAllLines("Day13.txt").Where(line => !string.IsNullOrEmpty(line)).ToList();
            packets.Add("[[2]]");
            packets.Add("[[6]]");
            packets.Sort(Compare);

            int first = packets.IndexOf("[[2]]") + 1;
            int second = packets.IndexOf("[[6]]") + 1;

            Assert.Equal(24190, first * second);
        }

        private int Compare(string left, string right)
        {
            if (left[0] == '[' && right[0] == '[' || left[0] == ']' && right[0] == ']' || left[0] == ',' && right[0] == ',')
            {
                return Compare(left.Substring(1), right.Substring(1));
            }

            if (left[0] == ']')
            {
                return -1;
            }

            if (right[0] == ']')
            {
                return 1;
            }

            if (left[0] == '[')
            {
                int i = ConsumeInt(ref right);
                right = $"[{i}]{right}";
                return Compare(left, right);
            }
            
            if (right[0] == '[')
            {
                int i = ConsumeInt(ref left);
                left = $"[{i}]{left}";
                return Compare(left, right);
            }

            int leftInt = ConsumeInt(ref left);
            int rightInt = ConsumeInt(ref right);

            if (leftInt < rightInt)
            {
                return -1;
            }

            if (leftInt > rightInt)
            {
                return 1;
            }

            return Compare(left, right);
        }

        private int ConsumeInt(ref string str)
        {
            int count = 0;

            while (str[count] >= '0' && str[count] <= '9')
            {
                count++;
            }

            int i = int.Parse(str.Substring(0, count));
            str = str.Substring(count);

            return i;
        }
    }
}