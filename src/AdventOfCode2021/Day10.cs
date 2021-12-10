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
                              .Sum(x => (long)invalidCharScores[Parse(x).FirstInvalidChar]);

            Assert.Equal(339411, result);
        }

        [Fact]
        public void Part2()
        {
            List<long> scores = File.ReadAllLines("Day10Input.txt")
                                    .Select(line => Parse(line))
                                    .Where(result => result.FirstInvalidChar == 0)
                                    .Select(result => result.RestOfLine.Aggregate(0L, (score, ch) => (score * 5) + completionCharScores[ch]))
                                    .OrderBy(x => x)
                                    .ToList();

            long result = scores[scores.Count / 2];

            Assert.Equal(2289754624, result);
        }

        private struct ParseResult
        {
            internal char FirstInvalidChar;
            internal IEnumerable<char> RestOfLine;
        }

        private ParseResult Parse(string line)
        {
            Stack<char> stack = new Stack<char>();

            foreach (char ch in line)
            {
                if (closingChars.TryGetValue(ch, out char closingChar))
                {
                    stack.Push(closingChar);
                }
                else if (ch != stack.Pop())
                {
                    return new ParseResult() { FirstInvalidChar = ch };
                }
            }

            return new ParseResult() { RestOfLine = stack };
        }
    }
}
