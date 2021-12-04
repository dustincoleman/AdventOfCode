using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode2021
{
    public class Day04
    {
        [Fact]
        public void Part1()
        {
            string[] input = File.ReadAllLines("Day04Input.txt");

            int[] numbers = input[0].Split(',').Select(Int32.Parse).ToArray();

            List<Board> boards = new List<Board>();

            for (int i = 2; i < input.Length; i += 6)
            {
                boards.Add(Board.FromInput(input.Skip(i).Take(5).ToArray()));
            }

            long result = 0;

            foreach (int num in numbers)
            {
                foreach(Board board in boards)
                {
                    board.Mark(num);
                }

                foreach (Board board in boards)
                {
                    if (board.IsWinner())
                    {
                        result = board.ComputeScore();
                        break;
                    }
                }

                if (result > 0)
                {
                    result *= num;
                    break;
                }
            }

            Assert.Equal(87456, result);
        }

        [Fact]
        public void Part2()
        {
            string[] input = File.ReadAllLines("Day04Input.txt");

            int[] numbers = input[0].Split(',').Select(Int32.Parse).ToArray();

            List<Board> boards = new List<Board>();

            for (int i = 2; i < input.Length; i += 6)
            {
                boards.Add(Board.FromInput(input.Skip(i).Take(5).ToArray()));
            }

            long result = 0;

            foreach (int num in numbers)
            {
                foreach (Board board in boards)
                {
                    board.Mark(num);
                }

                foreach (Board board in boards.ToArray())
                {
                    if (board.IsWinner())
                    {
                        if (boards.Count == 1)
                        {
                            result = board.ComputeScore();
                            break;
                        }
                        else
                        {
                            boards.Remove(board);
                        }
                    }
                }

                if (result > 0)
                {
                    result *= num;
                    break;
                }
            }

            Assert.Equal(15561, result);
        }
    }

    class Board
    {
        private int?[,] values = new int?[5, 5];

        public static Board FromInput(string[] lines)
        {
            Board board = new Board();

            for (int i = 0; i < 5; i++)
            {
                int j = 0;
                foreach (int value in lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse))
                {
                    board.values[i, j++] = value;
                }
            }

            return board;
        }

        internal void Mark(int num)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (values[i, j] == num)
                    {
                        values[i, j] = null;
                    }
                }
            }
        }

        internal bool IsWinner()
        {
            for (int i = 0; i < 5; i++)
            {
                if (RowIsWinner(i) || ColumnIsWinner(i))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ColumnIsWinner(int i)
        {
            return (values[i, 0] == null && values[i, 1] == null && values[i, 2] == null && values[i, 3] == null && values[i, 4] == null);
        }

        private bool RowIsWinner(int i)
        {
            return (values[0, i] == null && values[1, i] == null && values[2, i] == null && values[3, i] == null && values[4, i] == null);
        }

        internal long ComputeScore()
        {
            long score = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (values[i, j] != null)
                    {
                        score += values[i, j].Value;
                    }
                }
            }

            return score;
        }
    }
}
