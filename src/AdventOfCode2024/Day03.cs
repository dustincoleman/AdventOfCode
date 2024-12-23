﻿namespace AdventOfCode2024
{
    public class Day03
    {
        [Fact]
        public void Part1()
        {
            string input = File.ReadAllText("Day03.txt");
            Regex regex = new Regex(@"mul\((\d+,\d+)\)");
            int answer = 0;

            foreach (Match match in regex.Matches(input))
            {
                answer += Point2.Parse(match.Groups[1].Value).Product();
            }

            Assert.Equal(expected: 166357705, answer);
        }

        [Fact]
        public void Part2()
        {
            string input = File.ReadAllText("Day03.txt");
            Regex regex = new Regex(@"mul\((\d+,\d+)\)|do\(\)|don't\(\)");
            int answer = 0;
            bool enabled = true;

            foreach (Match match in regex.Matches(input))
            {
                if (match.Value == "do()")
                {
                    enabled = true;
                }
                else if (match.Value == "don't()")
                {
                    enabled = false;
                }
                else if (enabled)
                {
                    answer += Point2.Parse(match.Groups[1].Value).Product();
                }
            }

            Assert.Equal(expected: 88811886, answer);
        }
    }
}
