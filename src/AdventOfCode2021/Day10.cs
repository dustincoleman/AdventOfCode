using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode2021
{
    public class Day10
    {
        Dictionary<char, char> closingChars = new Dictionary<char, char>() { { '(' , ')' } , { '[', ']' }, { '{', '}' }, { '<', '>' } };

        Dictionary<char, int> invalidCharScores = new Dictionary<char, int>() { { (char)0, 0 }, { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };

        Dictionary<char, int> completionCharScores = new Dictionary<char, int>() { { (char)0, 0 }, { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 } };

        [Fact]
        public void Part1()
        {
            long result = File.ReadAllLines("Day10Input.txt")
                              .Sum(x => (long)invalidCharScores[GetFirstInvalidChar(x)]);

            Assert.Equal(339411, result);
        }

        [Fact]
        public void Part2()
        {
            List<long> scores = File.ReadAllLines("Day10Input.txt")
                                    .Where(x => GetFirstInvalidChar(x) == 0)
                                    .Select(line => GetRestOfLine(line).Aggregate(0L, (score, ch) => (score * 5) + completionCharScores[ch]))
                                    .OrderBy(x => x)
                                    .ToList();

            long result = scores[scores.Count / 2];

            Assert.Equal(2289754624, result);
        }

        private char GetFirstInvalidChar(string line)
        {
            Stack<char> stack = new Stack<char>();

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (closingChars.TryGetValue(ch, out char closingChar))
                {
                    stack.Push(closingChar);
                }
                else if (ch != stack.Pop())
                {
                    return ch;
                }
            }

            return (char)0;
        }

        private IEnumerable<char> GetRestOfLine(string line)
        {
            Stack<char> stack = new Stack<char>();

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (closingChars.TryGetValue(ch, out char closingChar))
                {
                    stack.Push(closingChar);
                }
                else
                {
                    stack.Pop();
                }
            }

            return stack;
        }
    }
}
