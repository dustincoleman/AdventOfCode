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
            long result = RunProblem(ParseInput());

            Assert.Equal(745, result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<Grid2<int>> mapPieces = new Grid2<Grid2<int>>(5, 5);

            mapPieces[Point2.Zero] = ParseInput();

            foreach (Point2 point in mapPieces.Points.Skip(1))
            {
                Point2 source = (point.X > 0) ? point - Point2.UnitX : point - Point2.UnitY;
                mapPieces[point] = mapPieces[source].Transform(i => (i < 9) ? i + 1 : 1);
            }

            long result = RunProblem(Grid2<int>.Combine(mapPieces));

            Assert.Equal(3002, result);
        }

        Grid2<int> ParseInput()
        {
            string[] lines = File.ReadAllLines("Day15Input.txt");
            Grid2<int> map = new Grid2<int>(lines[0].Length, lines.Length);

            foreach (Point2 point in map.Points)
            {
                map[point] = int.Parse(lines[point.Y][point.X].ToString());
            }

            return map;
        }

        long RunProblem(Grid2<int> map)
        {
            PriorityQueue<Node, int> queue = new PriorityQueue<Node, int>();
            Grid2<bool> visited = new Grid2<bool>(map.Bounds);
            Point2 end = map.Bounds - 1;

            queue.Enqueue(new Node(Point2.Zero, 0), 0);

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();

                if (node.Point == map.Bounds - 1)
                {
                    return node.Distance;
                }

                if (visited[node.Point])
                {
                    continue;
                }

                visited[node.Point] = true;

                foreach (Point2 adjacent in node.Point.Adjacent(map.Bounds).Where(p => !visited[p]))
                {
                    int adjacentDistance = node.Distance + map[adjacent];
                    queue.Enqueue(new Node(adjacent, adjacentDistance), adjacentDistance);
                }
            }

            throw new Exception("Didn't find end");
        }

        public struct Node
        {
            public Point2 Point;
            public int Distance;

            public Node(Point2 point, int distance)
            {
                Point = point;
                Distance = distance;
            }
        }
    }
}
