namespace AdventOfCode2023;

public class Day01
{
    private static string[] s_numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    [Fact]
    public void Part1()
    {
        int answer = File.ReadAllLines("Day01.txt").Sum(line => 
            Decode(line, pos =>
            {
                if (char.IsAsciiDigit(line[pos]))
                {
                    return line[pos] - '0';
                }

                return 0;
            }));

        Assert.Equal(53386, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = File.ReadAllLines("Day01.txt").Sum(line => 
            Decode(line, pos =>
            {
                if (char.IsAsciiDigit(line[pos]))
                {
                    return line[pos] - '0';
                }

                for (int i = 0; i < s_numbers.Length; i++)
                {
                    if (pos + s_numbers[i].Length <= line.Length && line.IndexOf(s_numbers[i], pos, s_numbers[i].Length) == pos)
                    {
                        return i + 1;
                    }
                }

                return 0;
            }));

        Assert.Equal(53312, answer);
    }

    private int Decode(string line, Func<int, int> decode)
    {
        int first = 0;
        int last = 0;

        for (int i = 0; i < line.Length && (first == 0 || last == 0); i++)
        {
            if (first == 0)
            {
                first = decode(i);
            }
            if (last == 0)
            {
                last = decode(line.Length - i - 1);
            }
        }

        return (first * 10) + last;
    }
}