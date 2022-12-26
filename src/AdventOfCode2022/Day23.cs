using System.Linq;

namespace AdventOfCode2022
{
    public class Day23
    {
        [Fact]
        public void Part1()
        {
            VirtualGrid2<Elf> grid = LoadPuzzle();
            LinkedList<MoveGenerator> moves = LoadMoves();

            for (int i = 0; i < 10; i++)
            {
                MakeNextMoves(grid, moves);

                var node = moves.First;
                moves.Remove(node);
                moves.AddLast(node);
            }

            Point2 min = grid.Points.Aggregate(Point2.Min);
            Point2 max = grid.Points.Aggregate(Point2.Max);

            long result = ((max - min) + 1).Product() - grid.Points.Count();
            Assert.Equal(4336, result);
        }

        [Fact]
        public void Part2()
        {
            long result = 0;
            VirtualGrid2<Elf> grid = LoadPuzzle();
            LinkedList<MoveGenerator> moves = LoadMoves();

            for (int i = 0; ; i++)
            {
                if (!MakeNextMoves(grid, moves))
                {
                    result = i + 1;
                    break;
                }

                var node = moves.First;
                moves.Remove(node);
                moves.AddLast(node);
            }

            Assert.Equal(1005, result);
        }

        private bool MakeNextMoves(VirtualGrid2<Elf> grid, LinkedList<MoveGenerator> availableMoves)
        {
            bool moved = false;
            HashSet<Point2> proposedMoves = new HashSet<Point2>();
            HashSet<Point2> collisions = new HashSet<Point2>();

            foreach (Point2 p in grid.Points)
            {
                Elf elf = grid[p];
                elf.ProposedMove = null;

                if (p.Surrounding().All(p => grid[p] == null))
                {
                    continue;
                }

                foreach (MoveGenerator gen in availableMoves)
                {
                    if (gen.CanMove(p, grid))
                    {
                        Point2 move = gen.Move(p);
                        elf.ProposedMove = move;

                        if (!proposedMoves.Add(move))
                        {
                            collisions.Add(move);
                        }

                        break;
                    }
                }
            }

            foreach (Point2 p in grid.Points)
            {
                Elf elf = grid[p];

                if (elf.ProposedMove.HasValue && collisions.Contains(elf.ProposedMove.Value))
                {
                    elf.ProposedMove = null;
                }
            }

            foreach (Point2 p in grid.Points.ToArray())
            {
                Elf elf = grid[p];

                if (elf.ProposedMove.HasValue)
                {
                    grid.Remove(p);
                    grid.Add(elf.ProposedMove.Value, elf);
                    elf.ProposedMove = null;
                    moved = true;
                }
            }

            return moved;
        }

        private VirtualGrid2<Elf> LoadPuzzle()
        {
            string[] lines = File.ReadAllLines("Day23.txt");
            VirtualGrid2<Elf> grid = new VirtualGrid2<Elf>();

            foreach (Point2 p in Point2.Quadrant(new Point2(lines[0].Length, lines.Length)))
            {
                if (lines[p.Y][p.X] == '#')
                {
                    grid.Add(p, new Elf());
                }
            }

            return grid;
        }

        private LinkedList<MoveGenerator> LoadMoves()
        {
            LinkedList<MoveGenerator> list = new LinkedList<MoveGenerator>();
            list.AddLast(new MoveGenerator() { CanMove = CanMoveNorth, Move = MoveNorth });
            list.AddLast(new MoveGenerator() { CanMove = CanMoveSouth, Move = MoveSouth });
            list.AddLast(new MoveGenerator() { CanMove = CanMoveWest, Move = MoveWest });
            list.AddLast(new MoveGenerator() { CanMove = CanMoveEast, Move = MoveEast });
            return list;
        }

        private struct MoveGenerator
        {
            internal MoveValidator CanMove;
            internal MoveComputation Move;
        }

        private delegate bool MoveValidator(Point2 point, VirtualGrid2<Elf> grid);
        private delegate Point2 MoveComputation(Point2 point);

        private static bool CanMoveNorth(Point2 point, VirtualGrid2<Elf> grid)
        {
            return
                grid[point.X - 1, point.Y - 1] == null &&
                grid[point.X, point.Y - 1] == null &&
                grid[point.X + 1, point.Y - 1] == null;
        }

        private static Point2 MoveNorth(Point2 point) => point - Point2.UnitY;

        private static bool CanMoveSouth(Point2 point, VirtualGrid2<Elf> grid)
        {
            return
                grid[point.X - 1, point.Y + 1] == null &&
                grid[point.X, point.Y + 1] == null &&
                grid[point.X + 1, point.Y + 1] == null;
        }

        private static Point2 MoveSouth(Point2 point) => point + Point2.UnitY;

        private static bool CanMoveWest(Point2 point, VirtualGrid2<Elf> grid)
        {
            return
                grid[point.X - 1, point.Y - 1] == null &&
                grid[point.X - 1, point.Y] == null &&
                grid[point.X - 1, point.Y + 1] == null;
        }

        private static Point2 MoveWest(Point2 point) => point - Point2.UnitX;

        private static bool CanMoveEast(Point2 point, VirtualGrid2<Elf> grid)
        {
            return
                grid[point.X + 1, point.Y - 1] == null &&
                grid[point.X + 1, point.Y] == null &&
                grid[point.X + 1, point.Y + 1] == null;
        }

        private static Point2 MoveEast(Point2 point) => point + Point2.UnitX;

        private class Elf
        {
            internal Point2? ProposedMove;
        }
    }
}