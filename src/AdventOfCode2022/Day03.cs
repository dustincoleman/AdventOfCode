namespace AdventOfCode2022
{
    public class Day03
    {
        [Fact]
        public void Part1()
        {
            int result = 0;

            foreach (string line in File.ReadLines("Day03.txt"))
            {
                if (line.Length % 2 != 0) throw new Exception();

                string firstHalf = line.Substring(0, line.Length / 2);
                string secondHalf = line.Substring(line.Length / 2, line.Length / 2);

                foreach (char ch in firstHalf)
                {
                    if (secondHalf.Contains(ch))
                    {
                        if (ch >= 'a' && ch <= 'z')
                        {
                            result += ch - 'a' + 1;
                        }
                        else if (ch >= 'A' && ch <= 'Z')
                        {
                            result += ch - 'A' + 27;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        break;
                    }
                }
            }

            Assert.Equal(7990, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 0;

            string[] lines = File.ReadAllLines("Day03.txt");

            if (lines.Length % 3 != 0) throw new Exception();

            for (int i = 0; i < lines.Length; i+= 3)
            {
                foreach (char ch in lines[i])
                {
                    if (lines[i + 1].Contains(ch) && lines[i + 2].Contains(ch))
                    {
                        if (ch >= 'a' && ch <= 'z')
                        {
                            result += ch - 'a' + 1;
                        }
                        else if (ch >= 'A' && ch <= 'Z')
                        {
                            result += ch - 'A' + 27;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        break;
                    }
                }
            }

            Assert.Equal(2602, result);
        }
    }
}