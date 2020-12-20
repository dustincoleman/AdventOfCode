using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    static class Day02
    {
        public static void Part1()
        {
            int valid = File.ReadAllLines("Day02Input.txt").Select(line => new PasswordInfo(line)).Count(IsValidPart1);
            Debug.Assert(valid == 580);
        }

        public static void Part2()
        {
            int valid = File.ReadAllLines("Day02Input.txt").Select(line => new PasswordInfo(line)).Count(IsValidPart2);
            Debug.Assert(valid == 611);
        }

        private static bool IsValidPart1(PasswordInfo passwordInfo)
        {
            int count = passwordInfo.Password.Count(c => c == passwordInfo.Letter);
            return (count >= passwordInfo.Min && count <= passwordInfo.Max);
        }

        private static bool IsValidPart2(PasswordInfo passwordInfo)
        {
            return ((passwordInfo.Password[passwordInfo.Min - 1] == passwordInfo.Letter) ^ passwordInfo.Password[passwordInfo.Max - 1] == passwordInfo.Letter);
        }
    }

    class PasswordInfo
    {
        public int Min { get; }
        public int Max { get; }
        public char Letter { get; }
        public string Password { get; }

        private static Regex regex = new Regex(@"^(?<Min>[0-9]+)-(?<Max>[0-9]+)\s+(?<Letter>[A-Za-z]):\s+(?<Password>[A-Za-z]+)$");

        public PasswordInfo(string input)
        {
            Match match = regex.Match(input);

            Min = int.Parse(match.Groups["Min"].Value);
            Max = int.Parse(match.Groups["Max"].Value);
            Letter = match.Groups["Letter"].Value.Single();
            Password = match.Groups["Password"].Value;
        }
    }
}
