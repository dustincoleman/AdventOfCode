using System.Diagnostics;

namespace AdventOfCode2022
{
    public class Day09
    {
        [Fact]
        public void Part1()
        {
            int result = RunPuzzle(2);
            Assert.Equal(6067, result);
        }

        [Fact]
        public void Part2()
        {
            int result = RunPuzzle(10);
            Assert.Equal(2471, result);
        }

        private int RunPuzzle(int size)
        {
            Knot[] knots = new Knot[size];
            for (int i = 0; i < size; i++)
            {
                knots[i] = new Knot();
            }

            HashSet<Point2> tailLocations = new HashSet<Point2>();

            foreach (string line in File.ReadAllLines("Day09.txt"))
            {
                string[] tokens = line.Split(' ');
                string direction = tokens[0];
                int count = int.Parse(tokens[1]);

                while (--count >= 0)
                {
                    Move(direction, knots);
                    tailLocations.Add(knots[size - 1].Point);
                }
            }

            return tailLocations.Count;
        }

        private void Move(string direction, Knot[] array, int position = 0)
        {
            Knot head = array[position];
            
            switch (direction)
            {
                case "L":
                    head.Point -= Point2.UnitX;
                    break;
                case "R":
                    head.Point += Point2.UnitX;
                    break;
                case "U":
                    head.Point += Point2.UnitY;
                    break;
                case "D":
                    head.Point -= Point2.UnitY;
                    break;
                case "UR":
                    head.Point += 1;
                    break;
                case "DR":
                    head.Point -= Point2.UnitY;
                    head.Point += Point2.UnitX;
                    break;
                case "UL":
                    head.Point += Point2.UnitY;
                    head.Point -= Point2.UnitX;
                    break;
                case "DL":
                    head.Point -= 1;
                    break;
                default:
                    throw new Exception();
            }

            if (position == array.Length - 1)
                return;

            Knot tail = array[position + 1];

            if (direction.Length == 2)
            {
                if (tail.Orientation != Orientation.Stacked)
                {
                    if (head.Point.X == tail.Point.X || head.Point.Y == tail.Point.Y)
                    {
                        if (head.Point.X == tail.Point.X && head.Point.Y == tail.Point.Y)
                        {
                            tail.Orientation = Orientation.Stacked;
                        }
                        else if (Math.Abs(head.Point.X - tail.Point.X) == 2)
                        {
                            if (head.Point.X > tail.Point.X)
                            {
                                Move("R", array, position + 1);
                            }
                            else
                            {
                                Move("L", array, position + 1);
                            }

                            tail.Orientation = Orientation.Adjacent;
                        }
                        else if (Math.Abs(head.Point.Y - tail.Point.Y) == 2)
                        {
                            if (head.Point.Y > tail.Point.Y)
                            {
                                Move("U", array, position + 1);
                            }
                            else
                            {
                                Move("D", array, position + 1);
                            }

                            tail.Orientation = Orientation.Adjacent;
                        }
                        else
                        {
                            tail.Orientation = Orientation.Adjacent;
                        }
                    }
                    else
                    {
                        Move(direction, array, position + 1);
                    }
                }
                else
                {
                    tail.Orientation = Orientation.Diagonal;
                }

                return;
            }

            if (tail.Orientation == Orientation.Diagonal)
            {
                // Check if they are adjacent now
                if (head.Point.X == tail.Point.X || head.Point.Y == tail.Point.Y)
                {
                    tail.Orientation = Orientation.Adjacent;
                }
                else
                {
                    string move = string.Empty;

                    if (head.Point.Y > tail.Point.Y)
                    {
                        move += "U";
                    }
                    else
                    {
                        move += "D";
                    }
                    if (head.Point.X > tail.Point.X)
                    {
                        move += "R";
                    }
                    else
                    {
                        move += "L";
                    }

                    Move(move, array, position + 1);

                    tail.Orientation = Orientation.Adjacent;
                }
            }
            else if (tail.Orientation == Orientation.Adjacent)
            {
                if (head.Point == tail.Point)
                {
                    tail.Orientation = Orientation.Stacked;
                }
                else if (head.Point.X == tail.Point.X || head.Point.Y == tail.Point.Y)
                {
                    // Move the tail the same direction
                    Move(direction, array, position + 1);

                    tail.Orientation = Orientation.Adjacent;
                }
                else
                {
                    tail.Orientation = Orientation.Diagonal;
                }
            }
            else // Stacked
            {
                tail.Orientation = Orientation.Adjacent;
            }
        }

        private class Knot
        {
            public Point2 Point = Point2.Zero;
            public Orientation Orientation = Orientation.Stacked;
        }

        private enum Orientation
        {
            Stacked,
            Adjacent,
            Diagonal
        }
    }
}