namespace AdventOfCode2023;

public class Day01
{
    private static string[] s_numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    [Fact]
    public void Part1()
    {
        int answer = 0;
        string[] lines = File.ReadAllLines("Day01.txt");

        foreach(string line in lines)
        {
            int first = line.First(char.IsAsciiDigit) - '0';
            int last = line.Last(char.IsAsciiDigit) - '0';
            answer += (first * 10) + last;
        }

        Assert.Equal(53386, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = 0;
        string[] lines = File.ReadAllLines("Day01.txt");

        foreach (string line in lines)
        {
            int first = FirstPart2Digit(line);
            int last = LastPart2Digit(line);
            answer += (first * 10) + last;
        }

        Assert.Equal(53312, answer);
    }

    private int FirstPart2Digit(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            int? digit = GetPart2Digit(line, i);
            if (digit.HasValue)
            {
                return digit.Value;
            }
        }

        throw new Exception("Value not found");
    }

    private int LastPart2Digit(string line)
    {
        for (int i = line.Length - 1; i >= 0; i--)
        {
            int? digit = GetPart2Digit(line, i);
            if (digit.HasValue)
            {
                return digit.Value;
            }
        }

        throw new Exception("Value not found");
    }

    private int? GetPart2Digit(string line, int pos)
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

        return null;
    }
}