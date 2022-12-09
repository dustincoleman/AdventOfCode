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
            Point2[] knots = new Point2[size];
            HashSet<Point2> tailLocations = new HashSet<Point2>();

            foreach (string line in File.ReadAllLines("Day09.txt"))
            {
                string[] tokens = line.Split(' ');
                string direction = tokens[0];
                int count = int.Parse(tokens[1]);

                while (--count >= 0)
                {
                    Move(direction, knots);
                    tailLocations.Add(knots[size - 1]);
                }
            }

            return tailLocations.Count;
        }

        private void Move(string direction, Point2[] array)
        {
            ref Point2 head = ref array[0];

            head += (direction) switch
            {
                "L" => -Point2.UnitX,
                "R" => Point2.UnitX,
                "U" => Point2.UnitY,
                "D" => -Point2.UnitY,
                _ => throw new Exception()
            };
            
            for (int i = 0; i < array.Length - 1; i++)
            {
                Point2 offset = array[i] - array[i + 1];
                int manhattan = offset.Manhattan();

                // If we need a diagonal move or we are off by two in the same axis
                if (manhattan >= 3 || manhattan == 2 && (array[i].X == array[i + 1].X || array[i].Y == array[i + 1].Y))
                {
                    array[i + 1] += offset.Sign();
                }
            }
        }
    }
}