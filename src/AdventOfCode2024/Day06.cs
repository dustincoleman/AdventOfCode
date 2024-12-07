namespace AdventOfCode2024
{
    public class Day06
    {
        [Fact]
        public void Part1()
        {
            PuzzleTraverser traverser = PuzzleTraverser.Create(PuzzleFile.ReadAsGrid("Day06.txt"));
            int answer = traverser.RoutePositions.Count;
            Assert.Equal(5409, answer);
        }

        [Fact]
        public void Part2()
        {
            PuzzleTraverser traverser = PuzzleTraverser.Create(PuzzleFile.ReadAsGrid("Day06.txt"));
            int answer = 0;

            foreach (Point2 obstruction in traverser.RoutePositions)
            {
                if (traverser.HasCycle(obstruction))
                {
                    answer++;
                }
            }

            Assert.Equal(2022, answer);
        }

        private class PuzzleTraverser
        {
            private readonly Grid2<char> puzzle;
            private readonly Grid2<Position> routes;
            private readonly Point2 startPosition;
            private readonly Direction startDirection;

            private PuzzleTraverser(Grid2<char> puzzle)
            {
                this.puzzle = puzzle;
                this.routes = new Grid2<Position>(puzzle.Bounds);
                this.startPosition = this.puzzle.AllPoints.First(pt => Direction.ParseOrDefault(this.puzzle[pt]) != null);
                this.startDirection = Direction.Parse(this.puzzle[this.startPosition]);
            }

            internal static PuzzleTraverser Create(Grid2<char> puzzle)
            {
                PuzzleTraverser traverser = new PuzzleTraverser(puzzle);
                traverser.Initialize();
                return traverser;
            }

            internal HashSet<Point2> RoutePositions { get; } = new HashSet<Point2>();

            internal bool HasCycle(Point2 obstacle)
            {
                if (obstacle == this.startPosition)
                {
                    return false;
                }

                Position position = new Position(this.startPosition, this.startDirection);
                Grid2<bool> visited = new Grid2<bool>(this.puzzle.Bounds);

                while (true)
                {
                    // Figure out where we are going next
                    EnsureRoute(position);
                    Position next = this.routes[position.Point];

                    // If we've been there, we are done
                    if (visited[next.Point])
                    {
                        return true;
                    }

                    // Check to see if the added obstacle would be hit on our way there
                    Line2 segment = new Line2<int>(position.Point, next.Point);

                    if (segment.Contains(obstacle))
                    {
                        // If so, move to it and figure out our next direction
                        Point2 nextPoint = obstacle - position.Point.SignToward(obstacle);
                        Direction nextDirection = position.Direction.TurnRight();

                        while (this.puzzle[nextPoint + nextDirection] == '#')
                        {
                            nextDirection = nextDirection.TurnRight();
                        }

                        // We don't want this route in the cache, so manually move to the next obstacle or edge
                        while (this.puzzle[nextPoint + nextDirection] != '#')
                        {
                            nextPoint += nextDirection;
                            if (!this.puzzle.InBounds(nextPoint + nextDirection))
                            {
                                return false;
                            }
                        }

                        // If we've been there, we are done
                        if (visited[nextPoint])
                        {
                            return true;
                        }

                        // Manually figure out our next direction, then we can go back to using the cache
                        while (this.puzzle[nextPoint + nextDirection] == '#')
                        {
                            nextDirection = nextDirection.TurnRight();
                            if (!this.puzzle.InBounds(nextPoint + nextDirection))
                            {
                                return false;
                            }
                        }

                        next = new Position(nextPoint, nextDirection);
                    }

                    // If our next direction is null, this means that we have escaped
                    if (next.Direction == null)
                    {
                        return false;
                    }

                    visited[next.Point] = true;
                    position = next;
                }
            }

            // Ensures the cache has an entry from start position to the next obstacle or edge
            private void EnsureRoute(Position start)
            {
                if (this.routes[start.Point].IsInitialized)
                {
                    return;
                }

                Point2 point = start.Point;
                Direction direction = start.Direction;

                while (true)
                {
                    Point2 next = point + direction;

                    // Search for the next obstacle or out of bounds point
                    if (this.puzzle.InBounds(next))
                    {
                        if (puzzle[next] == '#')
                        {
                            // Once we've hit an obstacle, figure out which direction we would head next
                            do
                            {
                                direction = direction.TurnRight();
                                next = point + direction;
                            }
                            while (this.puzzle.InBounds(next) && this.puzzle[next] == '#');

                            // Add the route. As an optimization, consider this point out of bounds (null next direction) if our next step would go out of bounds
                            this.routes[start.Point] = new Position(point, this.puzzle.InBounds(next) ? direction : null);
                            return;
                        }
                    }
                    else
                    {
                        // Add the route. We are actually out of bounds, so there is no next direction.
                        this.routes[start.Point] = new Position(point, Direction: null);
                        return;
                    }

                    point = next;
                }
            }

            // Compute all of the points in the unobstructed route (this warms up the cache a little too)
            private void Initialize()
            {
                Position position = new Position(this.startPosition, this.startDirection);

                while (position.Direction != null)
                {
                    // Figure out where we're going next
                    EnsureRoute(position);
                    Position next = this.routes[position.Point];

                    // Compute all the points
                    Line2 segment = new Line2<int>(position.Point, next.Point);
                    foreach (Point2 pt in segment.AllPoints)
                    {
                        if (this.puzzle.InBounds(pt))
                        {
                            this.RoutePositions.Add(pt);
                        }
                    }

                    position = next;
                }
            }
        }

        private record struct Position(Point2 Point, Direction Direction, bool IsInitialized)
        {
            internal Position(Point2 Point, Direction Direction) : this(Point, Direction, IsInitialized: true) { }
        }
    }
}
