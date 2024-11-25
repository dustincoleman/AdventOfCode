namespace AdventOfCode2023;

public class Day23
{
    [Fact]
    public void Part1()
    {
        Puzzle puzzle = Puzzle.Load("Day23.txt");
        Graph graph = puzzle.BuildGraph(ignoreDirections: false);
        int answer = Solve(graph);
        Assert.Equal(2326, answer);
    }

    [Fact]
    public void Part2()
    {
        Puzzle puzzle = Puzzle.Load("Day23.txt");
        Graph graph = puzzle.BuildGraph(ignoreDirections: true);
        int answer = Solve(graph);
        Assert.Equal(6574, answer);
    }

    private int Solve(Graph graph)
    {
        int maxLength = 0;
        Queue<Route> queue = new Queue<Route>();

        queue.Enqueue(new Route() { Position = graph.Start });

        while (queue.TryDequeue(out Route current))
        {
            foreach (Edge edge in current.Position.Edges)
            {
                if (!current.Visited[edge.Destination.Id])
                {
                    Route next = new Route()
                    {
                        Position = edge.Destination,
                        Visited = current.Visited,
                        Distance = current.Distance + edge.Distance
                    };

                    next.Visited[current.Position.Id] = true;

                    if (next.Position == graph.End)
                    {
                        maxLength = Math.Max(maxLength, next.Distance);
                    }
                    else
                    {
                        queue.Enqueue(next);
                    }
                }
            }
        }

        return maxLength;
    }

    private class Puzzle : Grid2<Cell>
    {
        internal Point2 Start;
        internal Point2 End;

        private Puzzle(Point2 bounds)
            : base(bounds)
        {
        }

        internal static Puzzle Load(string filename)
        {
            Puzzle puzzle = PuzzleFile.ReadAsGrid(
                filename, 
                (ch, pt) => 
                    new Cell()
                    {
                        IsWall = (ch == '#'),
                        Direction = Direction.ParseOrDefault(ch),
                        Point = pt
                    }, 
                bounds => new Puzzle(bounds));

            puzzle.Start = puzzle.Rows.First().Points.FirstOrDefault(pt => !puzzle[pt].IsWall);
            puzzle.End = puzzle.Rows.Last().Points.FirstOrDefault(pt => !puzzle[pt].IsWall);

            return puzzle;
        }

        internal Graph BuildGraph(bool ignoreDirections)
        {
            int nextId = 0;
            Dictionary<Point2, Node> nodesByLocation = new Dictionary<Point2, Node>();
            Queue<Point2> waypoints = new Queue<Point2>();

            nodesByLocation.Add(this.Start, new Node() { Id = nextId++ });
            waypoints.Enqueue(this.Start);

            while (waypoints.TryDequeue(out Point2 pt))
            {
                Node current = nodesByLocation[pt];

                foreach (Direction direction in PathsFrom(pt))
                {
                    if (Traverse(pt, direction, ignoreDirections, out Point2 destinationPt, out int distance))
                    {
                        if (!nodesByLocation.TryGetValue(destinationPt, out Node destination))
                        {
                            destination = new Node() { Id = nextId++ };
                            nodesByLocation.Add(destinationPt, destination);
                            waypoints.Enqueue(destinationPt);
                        }

                        current.Edges.Add(new Edge() { Destination = destination, Distance = distance });
                    }
                }
            }

            return new Graph()
            {
                Start = nodesByLocation[this.Start],
                End = nodesByLocation[this.End]
            };
        }

        private bool Traverse(Point2 start, Direction direction, bool ignoreDirections, out Point2 destination, out int distance)
        {
            destination = start + direction;
            distance = 1;

            if (!ignoreDirections)
            {
                Cell cell = this[destination];
                if (cell.Direction != null && cell.Direction != direction)
                {
                    return false;
                }
            }

            Direction[] directions = PathsFrom(destination).Where(d => d != direction.Reverse()).ToArray();

            while (directions.Length == 1)
            {
                direction = directions[0];
                destination += direction;
                distance++;

                directions = PathsFrom(destination).Where(d => d != direction.Reverse()).ToArray();
            }

            return true;
        }

        private IEnumerable<Direction> PathsFrom(Point2 point)
        {
            foreach (Direction d in Direction.All())
            {
                Point2 next = point + d;
                if (this.InBounds(next) && !this[next].IsWall)
                {
                    yield return d;
                }
            }
        }
    }

    private class Cell
    {
        internal bool IsWall;
        internal Direction Direction;
        internal Point2 Point;
    }

    private class Graph
    {
        internal Node Start;
        internal Node End;
    }

    private class Node
    {
        internal int Id;
        internal List<Edge> Edges = new List<Edge>();
    }

    private class Edge
    {
        internal Node Destination;
        internal int Distance;
    }

    private struct Route
    {
        internal Node Position;
        internal BitVector Visited;
        internal int Distance;
    }
}
