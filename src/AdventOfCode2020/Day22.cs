using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using Xunit;

namespace AdventOfCode2020
{
    public class Day22
    {
        [Fact]
        public void Part1()
        {
            Queue<int> player1 = new Queue<int>(File.ReadAllLines("Day22Player1.txt").Select(int.Parse));
            Queue<int> player2 = new Queue<int>(File.ReadAllLines("Day22Player2.txt").Select(int.Parse));

            while (player1.Count > 0 && player2.Count > 0)
            {
                int p1 = player1.Dequeue();
                int p2 = player2.Dequeue();

                if (p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else if (p1 < p2)
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }

            long result = 0;
            int[] winningCards = ((player1.Count > 0) ? player1 : player2).Reverse().ToArray();

            for (int i = 0; i < winningCards.Length; i++)
            {
                result += winningCards[i] * (i + 1);
            }

            Assert.Equal(35013, result);
        }

        [Fact]
        public void Part2()
        {
            Queue<int> player1 = new Queue<int>();
            Queue<int> player2 = new Queue<int>();

            RecursiveCombatGame game = new RecursiveCombatGame(File.ReadAllLines("Day22Player1.txt").Select(int.Parse), File.ReadAllLines("Day22Player2.txt").Select(int.Parse));

            game.PlayToEnd();

            long result = game.GetFinalScore();

            Assert.Equal(32806, result);
        }

        class RecursiveCombatGame
        {
            private Queue<int> player1;
            private Queue<int> player2;

            HashSet<string> previoushands = new HashSet<string>();

            public RecursiveCombatGame(IEnumerable<int> player1, IEnumerable<int> player2)
            {
                this.player1 = new Queue<int>(player1);
                this.player2 = new Queue<int>(player2);
            }

            public bool PlayToEnd()
            {
                while (player1.Count > 0 && player2.Count > 0)
                {
                    if (!previoushands.Add(ComputeHandString()))
                    {
                        return true; // Player1 Wins
                    }

                    int p1 = player1.Dequeue();
                    int p2 = player2.Dequeue();

                    bool player1Wins;

                    if (p1 <= player1.Count && p2 <= player2.Count)
                    {
                        player1Wins = new RecursiveCombatGame(player1.Take(p1), player2.Take(p2)).PlayToEnd();
                    }
                    else
                    {
                        player1Wins = (p1 > p2);
                    }

                    if (player1Wins)
                    {
                        player1.Enqueue(p1);
                        player1.Enqueue(p2);
                    }
                    else
                    {
                        player2.Enqueue(p2);
                        player2.Enqueue(p1);
                    }
                }

                return (player1.Count > 0);
            }

            public long GetFinalScore()
            {
                long result = 0;
                int[] winningCards = ((player1.Count > 0) ? player1 : player2).Reverse().ToArray();

                for (int i = 0; i < winningCards.Length; i++)
                {
                    result += winningCards[i] * (i + 1);
                }

                return result;
            }

            private string ComputeHandString()
            {
                string str = "P1:" + string.Join(",", player1.Select(i => i.ToString()));
                str += "P2:" + string.Join(",", player2.Select(i => i.ToString()));
                return str;
            }
        }
    }
}
