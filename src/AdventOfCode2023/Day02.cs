using System.Collections.Generic;

namespace AdventOfCode2023;

public class Day02
{
    [Fact]
    public void Part1()
    {
        int answer = LoadGames()
            .Where(game => game.Turns.All(turn => turn.Red <= 12 && turn.Green <= 13 && turn.Blue <= 14))
            .Sum(game => game.Id);

        Assert.Equal(2156, answer);
    }

    [Fact]
    public void Part2()
    {
        long answer = LoadGames()
            .Sum(game => game.Turns.Max(turn => turn.Red) * game.Turns.Max(turn => turn.Green) * game.Turns.Max(turn => turn.Blue));

        Assert.Equal(66909, answer);
    }

    private List<Game> LoadGames()
    {
        return File.ReadAllLines("Day02.txt").Select(line =>
        {
            string[] split = line.Split(": ");
            return new Game()
            {
                Id = int.Parse(split[0].Substring("Game ".Length)),
                Turns = split[1].Split("; ").Select(LoadTurn).ToList()
            };
        }).ToList();
    }

    private CubeSet LoadTurn(string text)
    {
        string[] entries = text.Split(", ");
        CubeSet cubeSet = new CubeSet();

        foreach (string entry in entries)
        {
            string[] split = entry.Split(" ");
            int count = int.Parse(split[0]);
            switch(split[1])
            {
                case "red":
                    cubeSet.Red = count;
                    break;
                case "green":
                    cubeSet.Green = count;
                    break;
                case "blue":
                    cubeSet.Blue = count;
                    break;
                default:
                    throw new Exception("Unexpected entry");
            }
        }

        return cubeSet;
    }

    private struct Game
    {
        public int Id;
        public List<CubeSet> Turns;
    }

    private struct CubeSet
    {
        public int Red;
        public int Green;
        public int Blue;
    }
}