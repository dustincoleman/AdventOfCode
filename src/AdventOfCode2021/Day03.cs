using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode2021
{
    public class Day03
    {
        [Fact]
        public void Part1()
        {
            string[] input = File.ReadAllLines("Day03Input.txt");

            int[] counts = new int[12];

            foreach (string line in input)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (line[i] == '1')
                    {
                        counts[i]++;
                    }
                }
            }

            int gammaRate = 0;
            int epsilonRate = 0;

            for (int i = 0; i < 12; i++)
            {
                gammaRate <<= 1;
                epsilonRate <<= 1;

                if (counts[i] >= 500)
                {
                    gammaRate++;
                }
                else
                {
                    epsilonRate++;
                }
            }

            long result = gammaRate * epsilonRate;

            Assert.Equal(2261546, result);
        }

        [Fact]
        public void Part2()
        {
            string[] input = File.ReadAllLines("Day03Input.txt");

            List<string> oxygenList = new List<string>(input);
            List<string> co2List = new List<string>(input);

            for (int i = 0; i < 12; i++)
            {
                if (oxygenList.Count > 1)
                {
                    oxygenList = Refine(oxygenList, i, true);
                }
                if (co2List.Count > 1)
                {
                    co2List = Refine(co2List, i, false);
                }
            }

            int oxygenRate = Convert.ToInt32(oxygenList[0], 2);
            int co2Rate = Convert.ToInt32(co2List[0], 2);
            long result = oxygenRate * co2Rate;

            Assert.Equal(6775520, result);
        }

        private List<string> Refine(List<string> input, int pos, bool mostCommon)
        {
            int count = input.Count(s => s[pos] == '1');

            char criteria = (mostCommon) ?
                ((count * 2) >= input.Count) ? '1' : '0' :
                ((count * 2) >= input.Count) ? '0' : '1';

            return input.Where(s => s[pos] == criteria).ToList();
        }
    }
}
