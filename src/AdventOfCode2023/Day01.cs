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
            int first = line.First(c => c >= '0' && c <= '9') - '0';
            int last = line.Last(c => c >= '0' && c <= '9') - '0';
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
            int? digit = GetPart2Digit(line.Substring(i));
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
            int? digit = GetPart2Digit(line.Substring(i));
            if (digit.HasValue)
            {
                return digit.Value;
            }
        }

        throw new Exception("Value not found");
    }

    private int? GetPart2Digit(string subStr)
    {
        if (subStr[0] >= '0' && subStr[0] <= '9')
        {
            return subStr[0] - '0';
        }

        for (int i = 0; i < s_numbers.Length; i++)
        {
            if (subStr.StartsWith(s_numbers[i]))
            {
                return i + 1;
            }
        }

        return null;
    }
}