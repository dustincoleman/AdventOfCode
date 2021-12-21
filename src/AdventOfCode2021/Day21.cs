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
            DiceGame game = ReadGameFromFile();
            int nextDieRoll = 0;

            int NextRoll()
            {
                int rollSum = ((nextDieRoll % 100) + 2) * 3;
                nextDieRoll += 3;
                return rollSum;
            }

            while (!(game = game.Player1Rolls(NextRoll())).IsComplete(1000) &&
                   !(game = game.Player2Rolls(NextRoll())).IsComplete(1000)) { }

            long result = nextDieRoll * Math.Min(game.P1Score, game.P2Score);

            Assert.Equal(897798, result);
        }

        [Fact]
        public void Part2()
        {
            int[] rollOutcomes = new int[] { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };

            long p1Wins = 0;
            long p2Wins = 0;

            Queue<DiceGame> inProgressGames = new Queue<DiceGame>() {  };

            inProgressGames.Enqueue(ReadGameFromFile());

            while (inProgressGames.Count > 0)
            {
                DiceGame previousRound = inProgressGames.Dequeue();

                for (int p1Roll = 3; p1Roll < 10; p1Roll++)
                {
                    DiceGame p1Round = previousRound.Player1Rolls(p1Roll, rollOutcomes[p1Roll]);

                    if (p1Round.IsComplete(21))
                    {
                        p1Wins += p1Round.Combos;
                        continue;
                    }

                    for (int p2Roll = 3; p2Roll < 10; p2Roll++)
                    {
                        DiceGame p2Round = p1Round.Player2Rolls(p2Roll, rollOutcomes[p2Roll]);
                        
                        if (p2Round.IsComplete(21))
                        {
                            p2Wins += p2Round.Combos;
                            continue;
                        }

                        inProgressGames.Enqueue(p2Round);
                    }
                }
            }
            long result = Math.Max(p1Wins, p2Wins);

            Assert.Equal(48868319769358, result);
        }

        private DiceGame ReadGameFromFile()
        {
            string[] input = File.ReadAllLines("Day21Input.txt");

            return new DiceGame()
            {
                P1Position = int.Parse(input[0].Split(" starting position: ")[1]),
                P2Position = int.Parse(input[1].Split(" starting position: ")[1]),
                Combos = 1
            };
        }

        private struct DiceGame
        {
            public int P1Position;
            public int P1Score;
            public int P2Position;
            public int P2Score;
            public long Combos;

            internal bool IsComplete(int winningScore) => (P1Score >= winningScore || P2Score >= winningScore);

            internal DiceGame Player1Rolls(int roll, long combos = 1)
            {
                int newPos = (P1Position + roll) % 10;
                int newScore = P1Score + ((newPos == 0) ? 10 : newPos);

                return new DiceGame()
                {
                    P1Position = newPos,
                    P1Score = newScore,
                    P2Position = P2Position,
                    P2Score = P2Score,
                    Combos = Combos * combos
                };
            }

            internal DiceGame Player2Rolls(int roll, long combos = 1)
            {
                int newPos = (P2Position + roll) % 10;
                int newScore = P2Score + ((newPos == 0) ? 10 : newPos);

                return new DiceGame()
                {
                    P1Position = P1Position,
                    P1Score = P1Score,
                    P2Position = newPos,
                    P2Score = newScore,
                    Combos = Combos * combos
                };
            }
        }
    }
}
