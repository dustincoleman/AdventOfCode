using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2020
{
    public class Day06
    {
        [Fact]
        public void Part1()
        {
            HashSet<char> hashSet = new HashSet<char>();
            int result = 0;

            foreach (string line in File.ReadAllLines("Day06Input.txt"))
            {
                if (line.Length == 0)
                {
                    result += hashSet.Count;
                    hashSet.Clear();
                    continue;
                }

                foreach (char ch in line)
                {
                    hashSet.Add(ch);
                }
            }

            result += hashSet.Count;

            Assert.Equal(6782, result);
        }

        [Fact]
        public void Part2()
        {
            int result = 0;

            HashSet<char> groupAnswers = null;

            foreach (string line in File.ReadAllLines("Day06Input.txt"))
            {
                if (line.Length == 0)
                {
                    result += groupAnswers.Count;
                    groupAnswers = null;
                    continue;
                }

                if (groupAnswers == null)
                {
                    groupAnswers = new HashSet<char>(line);
                }
                else
                {
                    HashSet<char> currentAnswers = new HashSet<char>(line);

                    foreach (char ch in groupAnswers.ToArray())
                    {
                        if (!currentAnswers.Contains(ch))
                        {
                            groupAnswers.Remove(ch);
                        }
                    }
                }
            }

            result += groupAnswers.Count;

            Assert.Equal(3596, result);
        }
    }
}
