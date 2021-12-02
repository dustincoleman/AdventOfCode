using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode2021
{
    public class Day02
    {
        [Fact]
        public void Part1()
        {
            int depth = 0;
            int position = 0;

            string[] input = File.ReadAllLines("Day02Input.txt");

            foreach (string line in input)
            {
                string[] parts = line.Split(' ');
                string command = parts[0];
                int value = Int32.Parse(parts[1]);

                if (command == "up")
                {
                    depth -= value;
                }
                else if(command == "down")
                {
                    depth += value;
                }
                else
                {
                    position += value;
                }
            }

            int result = depth * position;

            Assert.Equal(1989265, result);
        }

        [Fact]
        public void Part2()
        {
            int depth = 0;
            int position = 0;
            int aim = 0;

            string[] input = File.ReadAllLines("Day02Input.txt");

            foreach (string line in input)
            {
                string[] parts = line.Split(' ');
                string command = parts[0];
                int value = Int32.Parse(parts[1]);

                if (command == "up")
                {
                    aim -= value;
                }
                else if (command == "down")
                {
                    aim += value;
                }
                else
                {
                    position += value;
                    depth += aim * value;
                }
            }

            long result = depth * position;

            Assert.Equal(2089174012, result);
        }
    }
}
