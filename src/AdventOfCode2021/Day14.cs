using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2021
{
    public class Day14
    {
        [Fact]
        public void Part1()
        {
            long result = RunProblem(10);
            Assert.Equal(2375, result);
        }

        [Fact]
        public void Part2()
        {
            long result = RunProblem(40);
            Assert.Equal(1976896901756, result);
        }

        long RunProblem(int cycles)
        {
            ParseResult parseResult = ParseInput();

            IDictionary<string, long> polymer = new AutoDictionary<string, long>();

            for (int i = 0; i < parseResult.Template.Length - 1; i++)
            {
                string pair = string.Concat(parseResult.Template.Skip(i).Take(2));
                polymer[pair]++;
            }

            for (int i = 0; i < cycles; i++)
            {
                polymer = ApplyRules(polymer, parseResult.Rules);
            }

            long[] counts = new long[256];

            foreach (string pair in polymer.Keys)
            {
                counts[pair[0]] += polymer[pair];
                counts[pair[1]] += polymer[pair];
            }

            counts[parseResult.Template[0]]++;
            counts[parseResult.Template[parseResult.Template.Length - 1]]++;

            long min = counts.Where(c => c > 0).Min() / 2;
            long max = counts.Where(c => c > 0).Max() / 2;

            return max - min;
        }

        ParseResult ParseInput()
        {
            string[] input = File.ReadAllLines("Day14Input.txt");
            Dictionary<string, string> rules = new Dictionary<string, string>();

            foreach (string line in input.Skip(2))
            {
                string[] split = line.Split(" -> ");
                rules.Add(split[0], split[1]);
            }

            return new ParseResult()
            {
                Template = input[0],
                Rules = rules
            };
        }

        private IDictionary<string, long> ApplyRules(IDictionary<string, long> polymer, Dictionary<string, string> rules)
        {
            AutoDictionary<string, long> newPolymer = new AutoDictionary<string, long>();

            foreach (string pair in polymer.Keys)
            {
                long count = polymer[pair];
                string insert = rules[pair];

                newPolymer[string.Concat(pair[0], insert)] += count;
                newPolymer[string.Concat(insert, pair[1])] += count;
            }

            return newPolymer;
        }

        struct ParseResult
        {
            public string Template;
            public Dictionary<string, string> Rules;
        }
    }
}
