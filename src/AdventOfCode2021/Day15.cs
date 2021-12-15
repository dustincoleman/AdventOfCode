using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace AdventOfCode2021
{
    public class Day15
    {
        [Fact]
        public void Part1()
        {
            string[] lines = File.ReadAllLines("Day15Input.txt");

            Point2 bounds = new Point2(lines[0].Length, lines.Length);

            long result = RunProblem(bounds, point => int.Parse(lines[point.Y][point.X].ToString()));

            Assert.Equal(745, result);
        }

        [Fact]
        public void Part2()
        {
            string[] lines = File.ReadAllLines("Day15Input.txt");

            // Load the input map
            Grid2<int> inputMap = new Grid2<int>(lines[0].Length, lines.Length);

            foreach (Point2 point in inputMap.Points)
            {
                inputMap[point] = int.Parse(lines[point.Y][point.X].ToString());
            }

            // Create the larger map 
            Grid2<Grid2<int>> enlargedMapPieces = new Grid2<Grid2<int>>(5, 5);
            enlargedMapPieces[Point2.Zero] = inputMap;

            foreach (Point2 point in enlargedMapPieces.Points.Skip(1))
            {
                Point2 source = (point.X > 0) ? point - Point2.UnitX : point - Point2.UnitY;
                enlargedMapPieces[point] = enlargedMapPieces[source].Transform(i => (i < 9) ? i + 1 : 1);
            }

            Grid2<int> enlargedMap = Grid2<int>.Combine(enlargedMapPieces);

            long result = RunProblem(enlargedMap.Bounds, point => enlargedMap[point]);

            Assert.Equal(3002, result);
        }

        long RunProblem(Point2 bounds, Func<Point2, int> getValue)
        {
            PriorityQueue queue = new PriorityQueue();
            Grid2<Position> map = new Grid2<Position>(bounds);

            foreach (Point2 point in map.Points)
            {
                Position position = new Position()
                {
                    Point = point,
                    Risk = getValue(point),
                    LowestCost = (point == Point2.Zero) ? 0 : int.MaxValue
                };

                map[point] = position;
                queue.Enqueue(position);
            }

            while (queue.Any())
            {
                Position position = queue.Dequeue();

                foreach (Point2 adjacent in position.Point.Adjacent(map.Bounds))
                {
                    Position adjacentPosition = map[adjacent];
                    int currentCost = position.LowestCost + adjacentPosition.Risk;

                    if (currentCost < adjacentPosition.LowestCost)
                    {
                        adjacentPosition.LowestCost = currentCost;
                        queue.Requeue(adjacentPosition);
                    }
                }
            }

            return map[map.Bounds - 1].LowestCost;
        }

        public class Position
        {
            public int Risk;
            public int LowestCost;
            public Point2 Point;
        }

        public class PriorityQueue
        {
            private LinkedList<Position> list = new LinkedList<Position>();
            Dictionary<Position, LinkedListNode<Position>> map = new Dictionary<Position, LinkedListNode<Position>>();

            internal bool Any()
            {
                return list.Any();
            }

            internal void Enqueue(Position position)
            {
                if (list.Count == 0 || position.LowestCost >= list.Last.Value.LowestCost)
                {
                    map[position] = list.AddLast(position);
                }
                else
                {
                    throw new Exception();
                }
            }

            internal Position Dequeue()
            {
                Position position = list.First.Value;
                list.RemoveFirst();
                map.Remove(position);
                return position;
            }

            internal void Requeue(Position p)
            {
                if (map.ContainsKey(p) && list.Count > 1)
                {
                    LinkedListNode<Position> node = map[p];
                    list.Remove(node);

                    if (p.LowestCost < list.First.Value.LowestCost)
                    {
                        list.AddFirst(p);
                    }
                    else
                    {
                        LinkedListNode<Position> insertAfter = list.First;
                        while (insertAfter.Next != null && p.LowestCost > insertAfter.Next.Value.LowestCost)
                        {
                            insertAfter = insertAfter.Next;
                        }

                        list.AddAfter(insertAfter, node);
                    }
                }
            }
        }
    }
}
