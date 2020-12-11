using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    enum Seat
    {
        Edge = 0,
        Floor, // .
        Empty, // L
        Occupied // #
    }

    static class Day11
    {
        public static void Part1()
        {
            FloorPlan floorPlan = new FloorPlan(File.ReadAllLines("Day11Input.txt"));

            while (floorPlan.UpdatePart1()) { }

            int result = floorPlan.CountOccupiedSeats();

            Debugger.Break();
        }

        public static void Part2()
        {
            FloorPlan floorPlan = new FloorPlan(File.ReadAllLines("Day11Input.txt"));

            while (floorPlan.UpdatePart2()) { }

            int result = floorPlan.CountOccupiedSeats();

            Debugger.Break();
        }
    }

    class FloorPlan
    {
        private Seat[,] seats;
        int width;
        int height;

        public FloorPlan(string[] rawInput)
        {
            width = rawInput[0].Length;
            height = rawInput.Length;
            seats = new Seat[height + 2, width + 2];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Seat s;

                    switch (rawInput[i][j])
                    {
                        case '.':
                            s = Seat.Floor;
                            break;
                        case 'L':
                            s = Seat.Empty;
                            break;
                        case '#':
                            s = Seat.Occupied;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    seats[i + 1, j + 1] = s;
                }
            }
        }

        internal bool UpdatePart1()
        {
            return Update(CountAdjacentOccupiedSeats, 4);
        }

        internal bool UpdatePart2()
        {
            return Update(CountAdjacentVisibleOccupiedSeats, 5);
        }

        internal bool Update(Func<int, int, int> countAdjacentSeats, int seatCount)
        {
            bool changed = false;
            Seat[,] newSeats = new Seat[height + 2, width + 2];

            for (int i = 1; i < height + 1; i++)
            {
                for (int j = 1; j < width + 1; j++)
                {
                    Seat newSeat = seats[i, j];
                    int count = countAdjacentSeats(i, j);

                    if (seats[i, j] == Seat.Empty && count == 0)
                    {
                        newSeat = Seat.Occupied;
                        changed = true;
                    }
                    else if (seats[i, j] == Seat.Occupied && count >= seatCount)
                    {
                        newSeat = Seat.Empty;
                        changed = true;
                    }

                    newSeats[i, j] = newSeat;
                }
            }

            seats = newSeats;

            return changed;
        }

        internal int CountOccupiedSeats()
        {
            int count = 0;

            for (int i = 1; i < height + 1; i++)
            {
                for (int j = 1; j < width + 1; j++)
                {
                    if (seats[i, j] == Seat.Occupied)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountAdjacentOccupiedSeats(int i, int j)
        {
            if (i < 1 || i > height + 1) throw new IndexOutOfRangeException();
            if (j < 1 || j > width + 1) throw new IndexOutOfRangeException();

            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if ((x != 0 || y != 0) && seats[i + x, j + y] == Seat.Occupied)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountAdjacentVisibleOccupiedSeats(int i, int j)
        {
            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        count += IsAdjacentVisibleSeatOccupied(i, j, x, y) ? 1 : 0;
                    }
                }
            }

            return count;
        }

        private bool IsAdjacentVisibleSeatOccupied(int i, int j, int iInc, int jInc)
        {
            int x = i;
            int y = j;

            while (true)
            {
                x += iInc;
                y += jInc;

                if (seats[x, y] == Seat.Edge || seats[x, y] == Seat.Empty)
                {
                    return false;
                }
                if (seats[x, y] == Seat.Occupied)
                {
                    return true;
                }
            }
        }
    }
}
