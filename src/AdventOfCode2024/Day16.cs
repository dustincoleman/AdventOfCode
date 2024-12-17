using System.IO;

namespace AdventOfCode2024
{
    public class Day16
    {
        [Fact]
        public void Part1()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day16.txt");
            Point2 start = puzzle.AllPoints.First(p => puzzle[p] == 'S');
            Point2 end = puzzle.AllPoints.First(p => puzzle[p] == 'E');

            Queue<Path> queue = new Queue<Path>();
            queue.Enqueue(
                new Path()
                {
                    Position = start,
                    Direction = Direction.East,
                    Visited = new HashSet<Point2<int>>()
                });

            long result = long.MaxValue;
            List<Path> paths = new List<Path>();
            Grid2<long> minScores = new Grid2<long>(puzzle.Bounds);

            while (queue.TryDequeue(out Path path))
            {
                if (path.Position == end)
                {
                    result = Math.Min(result, path.Score);
                    paths.Add(path);
                    continue;
                }

                long minScore = minScores[path.Position];
                if (minScore == 0 || path.Score < minScore)
                {
                    minScores[path.Position] = path.Score;
                }
                else
                {
                    continue;
                }

                Point2 next = path.Position + path.Direction;
                if (!path.Visited.Contains(next) && puzzle[next] != '#')
                {
                    queue.Enqueue(path.Move(path.Direction));
                }

                next = path.Position + path.Direction.TurnLeft();
                if (!path.Visited.Contains(next) && puzzle[next] != '#')
                {
                    queue.Enqueue(path.Move(path.Direction.TurnLeft()));
                }

                next = path.Position + path.Direction.TurnRight();
                if (!path.Visited.Contains(next) && puzzle[next] != '#')
                {
                    queue.Enqueue(path.Move(path.Direction.TurnRight()));
                }
            }

            Assert.Equal(95444, actual: result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day16.txt");
            Point2 start = puzzle.AllPoints.First(p => puzzle[p] == 'S');
            Point2 end = puzzle.AllPoints.First(p => puzzle[p] == 'E');

            Queue<Path> queue = new Queue<Path>();
            queue.Enqueue(
                new Path()
                {
                    Position = start,
                    Direction = Direction.East,
                    Visited = new HashSet<Point2<int>>()
                });

            long minScore = long.MaxValue;
            List<Path> paths = new List<Path>();
            Grid2<long> minScoresSoFar = new Grid2<long>(puzzle.Bounds);

            while (queue.TryDequeue(out Path path))
            {
                if (path.Position == end)
                {
                    minScore = Math.Min(minScore, path.Score);
                    paths.Add(path);
                    continue;
                }

                long minScoreSoFar = minScoresSoFar[path.Position];
                if (minScoreSoFar == 0 || path.Score < minScoreSoFar)
                {
                    minScoresSoFar[path.Position] = path.Score;
                }
                else if (path.Score >  minScoreSoFar + 1000)
                {
                    continue;
                }

                Point2 next = path.Position + path.Direction;
                if (!path.Visited.Contains(next) && puzzle[next] != '#')
                {
                    queue.Enqueue(path.Move(path.Direction));
                }

                next = path.Position + path.Direction.TurnLeft();
                if (!path.Visited.Contains(next) && puzzle[next] != '#')
                {
                    queue.Enqueue(path.Move(path.Direction.TurnLeft()));
                }

                next = path.Position + path.Direction.TurnRight();
                if (!path.Visited.Contains(next) && puzzle[next] != '#')
                {
                    queue.Enqueue(path.Move(path.Direction.TurnRight()));
                }
            }

            HashSet<Point2> bestTiles = new HashSet<Point2<int>>();
            Grid2<char> map = PuzzleFile.ReadAsGrid("Day16.txt");

            foreach (Path path in paths)
            {
                if (path.Score == minScore)
                {
                    foreach (Point2 p in path.Visited)
                    {
                        bestTiles.Add(p);
                        map[p] = 'O';
                    }
                }
            }

            long result = bestTiles.Count + 1;
            Assert.Equal(513, actual: result);
        }

        private class Path
        {
            internal Point2 Position;
            internal Direction Direction;
            internal HashSet<Point2> Visited;
            internal int Rotations;

            public long Score => (long)Rotations * 1000 + Visited.Count;

            internal Path Move(Direction d)
            {
                Path path = new Path()
                {
                    Position = Position + d,
                    Direction = d,
                    Visited = new HashSet<Point2>(Visited),
                    Rotations = (d == Direction) ? Rotations : Rotations + 1,
                };
                path.Visited.Add(Position);
                return path;
            }
        }
    }
}
