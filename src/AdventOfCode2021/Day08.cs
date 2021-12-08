using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace AdventOfCode2021
{
    public class Day08
    {
        [Fact]
        public void Part1()
        {
            int result = 0;
            List<Entry> input = File.ReadAllLines("Day08Input.txt").Select(line => new Entry(line)).ToList();

            foreach (Entry entry in input)
            {
                result += entry.Outputs.Count(x => x.Length is 2 or 3 or 4 or 7);
            }

            Assert.Equal(440, result);
        }

        [Fact]
        public void Part2()
        {
            long result = 0;
            List<Entry> input = File.ReadAllLines("Day08Input.txt").Select(line => new Entry(line)).ToList();

            foreach (Entry entry in input)
            {
                long localResult = 0;
                Decoder decoder = new Decoder(entry.Inputs);

                foreach (string output in entry.Outputs)
                {
                    localResult *= 10;
                    localResult += decoder.Decode(output);
                }

                result += localResult;
            }

            Assert.Equal(1046281, result);
        }
    }

    public class Entry
    {
        public List<string> Inputs;
        public List<string> Outputs;

        public Entry(string line)
        {
            string[] parts = line.Split('|');
            Inputs = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            Outputs = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }

    public class Decoder
    {
        //   aaaa
        //  b    c
        //  b    c
        //   dddd
        //  e    f
        //  e    f
        //   gggg

        List<HashSet<char>> sets = new List<HashSet<char>>();

        public Decoder(List<string> inputs)
        {
            for (int i = 0; i < 10; i++)
            {
                sets.Add(new HashSet<char>());
            }

            // 1
            foreach (char ch in inputs.Where(x => x.Length is 2).Single().ToCharArray())
            {
                sets[1].Add(ch);
            }

            // 7
            foreach (char ch in inputs.Where(x => x.Length is 3).Single().ToCharArray())
            {
                sets[7].Add(ch);
            }

            // 4
            foreach (char ch in inputs.Where(x => x.Length is 4).Single().ToCharArray())
            {
                sets[4].Add(ch);
            }

            // 3
            foreach (char ch in inputs.Where(x => x.Length is 5 && Contains(x, 7)).Single().ToCharArray())
            {
                sets[3].Add(ch);
            }

            // 9
            foreach (char ch in inputs.Where(x => x.Length is 6 && Contains(x, 3)).Single().ToCharArray())
            {
                sets[9].Add(ch);
            }

            // 0
            foreach (char ch in inputs.Where(x => x.Length is 6 && !Contains(x, 3) && Contains(x, 7)).Single().ToCharArray())
            {
                sets[0].Add(ch);
            }

            // 6
            foreach (char ch in inputs.Where(x => x.Length is 6 && !Contains(x, 3) && !Contains(x, 7)).Single().ToCharArray())
            {
                sets[6].Add(ch);
            }

            // 5
            foreach (char ch in inputs.Where(x => x.Length is 5 && !Contains(x, 7) && IsSubsetOf(x, 6)).Single().ToCharArray())
            {
                sets[5].Add(ch);
            }

            // 2
            foreach (char ch in inputs.Where(x => x.Length is 5 && !Contains(x, 7) && !IsSubsetOf(x, 6)).Single().ToCharArray())
            {
                sets[2].Add(ch);
            }

            // 8
            foreach (char ch in inputs.Where(x => x.Length is 7).Single().ToCharArray())
            {
                sets[8].Add(ch);
            }
        }

        public int Decode(string output)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Matches(output, i))
                {
                    return i;
                }
            }

            throw new Exception();
        }

        private bool Contains(string input, int number)
        {
            HashSet<char> inputChars = new HashSet<char>(input.ToCharArray());

            foreach (char ch in sets[number])
            {
                if (!inputChars.Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSubsetOf(string input, int number)
        {
            foreach (char ch in input)
            {
                if (!sets[number].Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private bool Matches(string input, int number)
        {
            if (input.Length != sets[number].Count)
            {
                return false;
            }

            foreach (char ch in input)
            {
                if (!sets[number].Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
