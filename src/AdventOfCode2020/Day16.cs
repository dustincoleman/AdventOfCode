using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    static class Day16
    {
        public static void Part1()
        {
            List<Rule> rules = File.ReadAllLines("Day16Rules.txt").Select(input => new Rule(input)).ToList();
            HashSet<int> validNumbers = new HashSet<int>();

            foreach (Rule rule in rules)
            {
                for (int i = rule.Range1.Item1; i <= rule.Range1.Item2; i++)
                {
                    validNumbers.Add(i);
                }
                for (int i = rule.Range2.Item1; i <= rule.Range2.Item2; i++)
                {
                    validNumbers.Add(i);
                }
            }

            int[] fieldValues = File.ReadAllText("Day16OtherTickets.txt").Replace(Environment.NewLine, ",").Split(',').Select(int.Parse).ToArray();
            int result = fieldValues.Sum(i => !validNumbers.Contains(i) ? i : 0);
            Debug.Assert(result == 23054);
        }

        public static void Part2()
        {
            List<Rule> rules = File.ReadAllLines("Day16Rules.txt").Select(input => new Rule(input)).ToList();

            HashSet<int> validNumbers = new HashSet<int>();

            foreach (Rule rule in rules)
            {
                for (int i = rule.Range1.Item1; i <= rule.Range1.Item2; i++)
                {
                    validNumbers.Add(i);
                }
                for (int i = rule.Range2.Item1; i <= rule.Range2.Item2; i++)
                {
                    validNumbers.Add(i);
                }
            }

            List<int[]> tickets = File.ReadAllLines("Day16OtherTickets.txt").Select(line => line.Split(',')).Select(arr => arr.Select(int.Parse).ToArray()).Where(ticket => ticket.All(i => validNumbers.Contains(i))).ToList();

            int ticketFields = tickets[0].Length;

            foreach (Rule rule in rules)
            {
                for (int i = 0; i < ticketFields; i++)
                {
                    if (tickets.All(ticket => (ticket[i] >= rule.Range1.Item1 && ticket[i] <= rule.Range1.Item2) || (ticket[i] >= rule.Range2.Item1 && ticket[i] <= rule.Range2.Item2)))
                    {
                        rule.CandidateColumns.Add(i);
                    }
                }
            }

            HashSet<int> matchedColumns = new HashSet<int>();

            foreach (Rule rule in rules.OrderBy(rule => rule.CandidateColumns.Count))
            {
                rule.Column = rule.CandidateColumns.Single(col => !matchedColumns.Contains(col));

                foreach (int col in rule.CandidateColumns)
                {
                    matchedColumns.Add(col);
                }
            }

            int[] myTicket = File.ReadAllText("Day16MyTicket.txt").Split(',').Select(int.Parse).ToArray();

            long result = 1;

            foreach (Rule rule in rules.Where(rule => rule.Field.StartsWith("departure")))
            {
                result *= myTicket[rule.Column];
            }

            Debug.Assert(result == 51240700105297);
        }
    }

    class Rule
    {
        private static Regex ruleRegex = new Regex(@"^(?<field>.*):\s(?<range1>.*)\sor\s(?<range2>.*)$");

        public string Field;
        public Tuple<int, int> Range1;
        public Tuple<int, int> Range2;
        public List<int> CandidateColumns = new List<int>();

        public int Column;

        public Rule(string rawInput)
        {
            Match match = ruleRegex.Match(rawInput);

            Field = match.Groups["field"].Value;
            Range1 = ParseRange(match.Groups["range1"].Value);
            Range2 = ParseRange(match.Groups["range2"].Value);
        }

        private static Tuple<int, int> ParseRange(string rawInput)
        {
            string[] split = rawInput.Split('-');
            return new Tuple<int, int>(int.Parse(split[0]), int.Parse(split[1]));
        }
    }
}
