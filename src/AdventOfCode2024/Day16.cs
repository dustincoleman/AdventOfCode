namespace AdventOfCode2024
{
    public class Day16
    {
        [Fact]
        public void Part1()
        {
            Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day16.txt", ch => new Cell(ch));
            Point2 start = puzzle.AllPoints.First(p => puzzle[p].Char == 'S');
            Point2 end = puzzle.AllPoints.First(p => puzzle[p].Char == 'E');

            FindShortestPath(puzzle, start, Direction.East, end);

            long result = puzzle[end].MinDistance;
            Assert.Equal(95444, result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day16.txt", ch => new Cell(ch));
            Point2 start = puzzle.AllPoints.First(p => puzzle[p].Char == 'S');
            Point2 end = puzzle.AllPoints.First(p => puzzle[p].Char == 'E');

            FindShortestPath(puzzle, start, Direction.East, end);
            MarkShortestPaths(puzzle, start, Direction.East, end);

            long result = puzzle.Where(c => c.OnShortestPath).Count();
            Assert.Equal(513, result);
        }

        private void FindShortestPath(Grid2<Cell> puzzle, Point2 startPos, Direction startDir, Point2 end)
        {
            int minDistance = int.MaxValue;
            PriorityQueue<Step, int> queue = new PriorityQueue<Step, int>();
            queue.Enqueue(new Step (startPos, startDir), 0);

            while (queue.TryDequeue(out Step step, out int distance))
            {
                if (distance > minDistance)
                {
                    continue;
                }

                if (step.Point == end)
                {
                    minDistance = distance;
                    continue;
                }

                Cell c = puzzle[step.Point];
                int directions = 0;

                // Straight ahead
                Point2 next = step.Point + step.Direction;
                Cell nextCell = puzzle[next];
                int alt = distance + 1;

                if (nextCell.Char != '#')
                {
                    if (alt < nextCell.MinDistance)
                    {
                        nextCell.MinDistance = alt;
                        queue.Enqueue(new Step(next, step.Direction), alt);
                    }

                    directions++;
                }

                // Turn left
                next = step.Point + step.Direction.TurnLeft();
                nextCell = puzzle[next];
                alt = distance + 1001;

                if (nextCell.Char != '#')
                {
                    if (alt < nextCell.MinDistance)
                    {
                        nextCell.MinDistance = alt;
                        queue.Enqueue(new Step(next, step.Direction.TurnLeft()), alt);
                    }

                    directions++;
                }

                // Turn right
                next = step.Point + step.Direction.TurnRight();
                nextCell = puzzle[next];
                alt = distance + 1001;

                if (nextCell.Char != '#')
                {
                    if (alt < nextCell.MinDistance)
                    {
                        nextCell.MinDistance = alt;
                        queue.Enqueue(new Step(next, step.Direction.TurnRight()), alt);
                    }

                    directions++;
                }

                c.IsIntersection = (directions > 1);
            }
        }

        private bool MarkShortestPaths(Grid2<Cell> puzzle, Point2 position, Direction direction, Point2 end, long distance = 0)
        {
            Cell cell = puzzle[position];
            long totalDistance = puzzle[end].MinDistance;

            if (position == end)
            {
                cell.OnShortestPath = true;
                return (distance == cell.MinDistance);
            }

            if (distance > totalDistance || distance > cell.MinDistance)
            {
                // Alternate routes may have crossed with a lesser distance
                if (!cell.IsIntersection || distance - cell.MinDistance != 1000)
                {
                    return false;
                }
            }

            cell.Visiting = true;
            bool onShortestPath = false;

            Point2 next = position + direction;
            if (puzzle[next].CanVisit())
            {
                onShortestPath |= MarkShortestPaths(puzzle, next, direction, end, distance + 1);
            }

            next = position + direction.TurnLeft();
            if (puzzle[next].CanVisit())
            {
                onShortestPath |= MarkShortestPaths(puzzle, next, direction.TurnLeft(), end, distance + 1001);
            }

            next = position + direction.TurnRight();
            if (puzzle[next].CanVisit())
            {
                onShortestPath |= MarkShortestPaths(puzzle, next, direction.TurnRight(), end, distance + 1001);
            }

            cell.Visiting = false;

            if (onShortestPath)
            {
                cell.OnShortestPath = true;
            }

            return onShortestPath;
        }

        private record Cell(char Char)
        {
            internal long MinDistance = long.MaxValue;
            internal bool Visiting = false;
            internal bool OnShortestPath = false;
            internal bool IsIntersection = false;

            internal bool CanVisit() => (Char != '#' && !Visiting);
        }

        private record struct Step(Point2 Point, Direction Direction);
    }
}
