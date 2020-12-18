using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    static class Day18
    {
        private static Regex regex = new Regex(@"\((?<content>[^()]*)\)");
        private static Regex addRegex = new Regex(@"(?<a>[0-9]+) \+ (?<b>[0-9]+)");
        private static Regex multiplyRegex = new Regex(@"(?<a>[0-9]+) \* (?<b>[0-9]+)");

        public static void Part1()
        {
            string[] lines = File.ReadAllLines("Day18Input.txt");

            long result = 0;

            foreach (string line in lines)
            {
                result += CalculatePart1(line);
            }

            Debugger.Break();
        }

        public static void Part2()
        {
            string[] lines = File.ReadAllLines("Day18Input.txt");

            long result = 0;

            foreach (string line in lines)
            {
                result += CalculatePart2(line);
            }

            Debugger.Break();
        }

        private static long CalculatePart1(string line)
        {
            Match match = regex.Match(line);

            while (match.Success)
            {
                line = line.Replace(match.Value, DoCalculatePart1(match.Groups["content"].Value));
                match = regex.Match(line);
            }

            return long.Parse(DoCalculatePart1(line));
        }

        private static string DoCalculatePart1(string value)
        {
            string[] split = value.Split(' ');

            string result = split[0];

            for (int i = 1; i < split.Length; i += 2)
            {
                long left = long.Parse(result);
                long right = long.Parse(split[i + 1]);

                switch (split[i].Single())
                {
                    case '+':
                        result = (left + right).ToString();
                        break;
                    case '*':
                        result = (left * right).ToString();
                        break;
                    default:
                        throw new Exception();
                }
            }

            return result;
        }

        private static long CalculatePart2(string line)
        {
            Match match = regex.Match(line);

            while (match.Success)
            {
                line = ReplaceFirst(line, match.Value, DoCalculatePart2(match.Groups["content"].Value).ToString());
                match = regex.Match(line);
            }

            return DoCalculatePart2(line);
        }

        private static long DoCalculatePart2(string value)
        {
            Match match = addRegex.Match(value);

            while (match.Success)
            {
                value = ReplaceFirst(value, match.Value, (long.Parse(match.Groups["a"].Value) + long.Parse(match.Groups["b"].Value)).ToString());
                match = addRegex.Match(value);
            }

            match = multiplyRegex.Match(value);

            while (match.Success)
            {
                value = ReplaceFirst(value, match.Value, (long.Parse(match.Groups["a"].Value) * long.Parse(match.Groups["b"].Value)).ToString());
                match = multiplyRegex.Match(value);
            }

            return long.Parse(value);
        }

        private static string ReplaceFirst(string input, string searchTerm, string newTerm)
        {
            int i = input.IndexOf(searchTerm);
            return input.Substring(0, i) + newTerm + input.Substring(i + searchTerm.Length);
        }
    }
}
