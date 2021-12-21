using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021
{
    public class Day21
    {
        [Fact]
        public void Part1()
        {
            string[] input = File.ReadAllLines("Day21Input.txt");

            int player1Pos = int.Parse(input[0].Split(" starting position: ")[1]);
            int player2Pos = int.Parse(input[1].Split(" starting position: ")[1]);

            int player1Score = 0;
            int player2Score = 0;
            int nextDieRoll = 0;

            while (true)
            {
                int rollSum = ((nextDieRoll % 100) + 2) * 3;
                nextDieRoll += 3;

                player1Pos = (player1Pos + rollSum) % 10;
                player1Score += (player1Pos == 0) ? 10 : player1Pos;

                if (player1Score >= 1000)
                {
                    break;
                }

                rollSum = ((nextDieRoll % 100) + 2) * 3;
                nextDieRoll += 3;

                player2Pos = (player2Pos + rollSum) % 10;
                player2Score += (player2Pos == 0) ? 10 : player2Pos;

                if (player2Score >= 1000)
                {
                    break;
                }
            }

            long result = nextDieRoll * Math.Min(player1Score, player2Score);

            Assert.Equal(897798, result);
        }

        [Fact]
        public void Part2()
        {
            string[] input = File.ReadAllLines("Day21Input.txt");

            int player1Pos = int.Parse(input[0].Split(" starting position: ")[1]);
            int player2Pos = int.Parse(input[1].Split(" starting position: ")[1]);

            int[] rollOutcomes = new int[] { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };

            List<List<DiceGame>> outcomes = new List<List<DiceGame>>();

            outcomes.Add(new List<DiceGame>() { new DiceGame() { P1Position = player1Pos, P2Position = player2Pos, Combos = 1 } });

            for (int i = 1; true; i++)
            {
                List<DiceGame> current = new List<DiceGame>();

                foreach (DiceGame info in outcomes[i - 1])
                {
                    if (info.P1Score < 21 && info.P2Score < 21)
                    {
                        for (int p1Roll = 3; p1Roll < 10; p1Roll++)
                        {
                            int newP1Pos = (info.P1Position + p1Roll) % 10;
                            int newP1Score = info.P1Score + ((newP1Pos == 0) ? 10 : newP1Pos);
                            int p1RollOutcomes = rollOutcomes[p1Roll];

                            if (newP1Score >= 21)
                            {
                                current.Add(new DiceGame()
                                {
                                    P1Position = newP1Pos,
                                    P1Score = newP1Score,
                                    P2Position = info.P2Position,
                                    P2Score = info.P2Score,
                                    Combos = info.Combos * p1RollOutcomes,
                                });
                            }
                            else
                            {
                                for (int p2Roll = 3; p2Roll < 10; p2Roll++)
                                {
                                    int newP2Pos = (info.P2Position + p2Roll) % 10;
                                    int newP2Score = info.P2Score + ((newP2Pos == 0) ? 10 : newP2Pos);
                                    int p2RollOutcomes = rollOutcomes[p2Roll];

                                    current.Add(new DiceGame()
                                    {
                                        P1Position = newP1Pos,
                                        P1Score = newP1Score,
                                        P2Position = newP2Pos,
                                        P2Score = newP2Score,
                                        Combos = info.Combos * p1RollOutcomes * p2RollOutcomes,
                                    });
                                }
                            }
                        }
                    }
                }

                if (!current.Any())
                {
                    break;
                }

                outcomes.Add(current);
            }

            List<DiceGame> completedGames = outcomes.SelectMany(list => list)
                                                    .Where(game => game.P1Score >= 21 || game.P2Score >= 21)
                                                    .ToList();

            long p1Wins = completedGames.Where(game => game.P1Score >= 21).Sum(game => game.Combos);
            long p2Wins = completedGames.Where(game => game.P2Score >= 21).Sum(game => game.Combos);

            long result = Math.Max(p1Wins, p2Wins);

            Assert.Equal(48868319769358, result);
        }

        private struct DiceGame
        {
            public int P1Position;
            public int P1Score;
            public int P2Position;
            public int P2Score;
            public long Combos;
        }
    }
}
