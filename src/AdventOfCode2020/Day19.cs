using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2020
{
    public class Day19
    {
        [Fact]
        public static void Part1()
        {
            int result = 0;
            List<Rule> rules = File.ReadAllLines("Day19Rules.txt").Select(line => new Rule(line)).OrderBy(rule => rule.Id).ToList();

            string regexStr = BuildRegex(0, rules);
            Regex regex = new Regex("^" + regexStr + "$");

            foreach(string message in File.ReadAllLines("Day19Messages.txt"))
            {
                if (regex.IsMatch(message))
                {
                    result++;
                }
            }

            Debug.Assert(result == 136);
        }

        [Fact]
        public static void Part2()
        {
            int result = 0;
            List<Rule> rules = File.ReadAllLines("Day19Rules.txt").Select(line => new Rule(line)).OrderBy(rule => rule.Id).ToList();

            //0: 8 11
            rules[8] = new Rule("8: 42 | 42 8");
            rules[11] = new Rule("11: 42 31 | 42 11 31");

            string rule42RegexStr = BuildRegex(42, rules);
            string rule31RegexStr = BuildRegex(31, rules);

            Regex regex = new Regex("^(?<front>" + rule42RegexStr + ")+(?<back>" + rule31RegexStr + ")+$");

            foreach (string message in File.ReadAllLines("Day19Messages.txt"))
            {
                Match match = regex.Match(message);

                if (match.Success && match.Groups["front"].Captures.Count > match.Groups["back"].Captures.Count)
                {
                    result++;
                }
            }

            Debug.Assert(result == 256);
        }

        private static string BuildRegex(int iRule, List<Rule> rules)
        {
            Rule rule = rules[iRule];

            if (rule.Char != 0)
            {
                return new string(rule.Char, 1);
            }

            string s = "";

            foreach (int i in rule.SubRules)
            {
                s += BuildRegex(i, rules);
            }

            if (rule.AltSubRules.Count != 0)
            {
                s = $"({s}|";

                foreach (int i in rule.AltSubRules)
                {
                    s += BuildRegex(i, rules);
                }

                s += ")";
            }

            return s;
        }

        class Rule
        {
            public int Id;
            public char Char;
            public List<int> SubRules = new List<int>();
            public List<int> AltSubRules = new List<int>();

            public Rule(string line)
            {
                string[] split = line.Split(':');
                Debug.Assert(split.Length == 2);

                Id = int.Parse(split[0]);

                if (split[1].Trim() == "\"a\"")
                {
                    Char = 'a';
                }
                else if (split[1].Trim() == "\"b\"")
                {
                    Char = 'b';
                }
                else
                {
                    string[] ruleSplit = split[1].Trim().Split("|");

                    foreach (int i in ruleSplit[0].Trim().Split(' ').Select(int.Parse))
                    {
                        SubRules.Add(i);
                    }

                    if (ruleSplit.Length > 1)
                    {
                        foreach (int i in ruleSplit[1].Trim().Split(' ').Select(int.Parse))
                        {
                            AltSubRules.Add(i);
                        }
                    }
                }
            }
        }
    }
}
