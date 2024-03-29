﻿namespace AdventOfCode2023;

public class Day15
{
    [Fact]
    public void Part1()
    {
        int answer = File.ReadAllText("Day15.txt").Split(',').Sum(Hash);
        Assert.Equal(506869, answer);
    }

    [Fact]
    public void Part2()
    {
        List<Lens>[] boxes = new List<Lens>[256];

        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = new List<Lens>();
        }

        foreach (string step in File.ReadAllText("Day15.txt").Split(','))
        {
            string[] split = step.Split('-', '=');

            string label = split[0];
            int boxIndex = Hash(label);

            if (split[1] == string.Empty)
            {
                boxes[boxIndex].RemoveAll(l => l.Label == label);
            }
            else
            {
                int focalLength = int.Parse(split[1]);
                int lensIndex = boxes[boxIndex].FindIndex(l => l.Label == label);
                
                if (lensIndex >= 0)
                {
                    boxes[boxIndex][lensIndex].FocalLength = focalLength;
                }
                else
                {
                    boxes[boxIndex].Add(new Lens()
                    {
                        Label = label,
                        FocalLength = focalLength
                    });
                }
            }
        }

        int answer = 0;

        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Count; j++)
            {
                answer += (i + 1) * (j + 1) * boxes[i][j].FocalLength;
            }
        }

        Assert.Equal(271384, answer);
    }

    private int Hash(string str)
    {
        int hash = 0;

        foreach (char ch in str)
        {
            hash += ch;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }

    private class Lens
    {
        public string Label;
        public int FocalLength;
    }
}
