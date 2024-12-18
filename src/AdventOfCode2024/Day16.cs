using System.IO;

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

        private void FindShortestPath(Grid2<Cell> puzzle, Point2 position, Direction direction, Point2 end, long distance = 0)
        {
            Cell cell = puzzle[position];

            if (distance >= cell.MinDistance)
            {
                return;
            }

            cell.MinDistance = distance;

            if (position == end)
            {
                return;
            }

            cell.Visiting = true;

            Point2 next = position + direction;
            if (puzzle[next].CanVisit())
            {
                FindShortestPath(puzzle, next, direction, end, distance + 1);
            }

            next = position + direction.TurnLeft();
            if (puzzle[next].CanVisit())
            {
                FindShortestPath(puzzle, next, direction.TurnLeft(), end, distance + 1001);
            }

            next = position + direction.TurnRight();
            if (puzzle[next].CanVisit())
            {
                FindShortestPath(puzzle, next, direction.TurnRight(), end, distance + 1001);
            }

            cell.Visiting = false;
        }

        private bool MarkShortestPaths(Grid2<Cell> puzzle, Point2 position, Direction direction, Point2 end, long distance = 0)
        {
            Cell cell = puzzle[position];

            if (distance - 1000 > cell.MinDistance)
            {
                return false;
            }
            if (position == end)
            {
                cell.OnShortestPath = true;
                return (distance == cell.MinDistance);
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

            internal bool CanVisit() => (Char != '#' && !Visiting);
        }
    }
}
