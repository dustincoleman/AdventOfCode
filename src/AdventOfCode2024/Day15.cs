namespace AdventOfCode2024
{
    public class Day15
    {
        [Fact]
        public void Part1()
        {
            (Grid2<char> map, List<Direction> moves) = LoadPuzzle();
            Point2 robot = map.AllPoints.First(p => map[p] == '@');

            foreach (Direction d in moves)
            {
                if (Move(map, robot, d))
                {
                    robot += d;
                }
            }

            long result = map.AllPoints.Select(pt => (map[pt] == 'O') ? pt.X + 100 * pt.Y : 0).Sum();
            Assert.Equal(1441031, result);
        }

        [Fact]
        public void Part2()
        {
            (Grid2<char> map, List<Direction> moves) = LoadPuzzle();
            map = Enlarge(map);
            Point2 robot = map.AllPoints.First(p => map[p] == '@');

            foreach (Direction d in moves)
            {
                if (ScaledMove(map, robot, d))
                {
                    robot += d;
                }
            }

            long result = map.AllPoints.Select(pt => (map[pt] == '[') ? pt.X + 100 * pt.Y : 0).Sum();
            Assert.Equal(1425169, result);
        }

        private bool ScaledMove(Grid2<char> map, Point2 p, Direction d)
        {
            if (d == Direction.Left || d == Direction.Right)
            {
                return Move(map, p, d);
            }

            return ScaledMoveHelper(map, p.X, p.X, p.Y, (d == Direction.Up) ? -1 : 1);
        }

        private bool ScaledMoveHelper(Grid2<char> map, int leftX, int rightX, int y, int yIncrement)
        {
            // Find the boxes we are pushing on this row
            if (map[leftX, y] == ']')
            {
                leftX--;
            }
            else while (map[leftX, y] == '.' && leftX <= rightX)
            {
                leftX++;
            }
            if (map[rightX, y] == '[')
            {
                rightX++;
            }
            else while (map[rightX, y] == '.' && rightX >= leftX)
            {
                rightX--;
            }

            // If there aren't any, we've succeeded
            if (leftX > rightX)
            {
                return true;
            }

            // If we're trying to push against a wall, we've failed
            for (int x = leftX; x <= rightX; x++)
            {
                if (map[x, y] == '#')
                {
                    return false;
                }
            }

            // Try to move all the rows in front of us
            int nextY = y + yIncrement;
            if (ScaledMoveHelper(map, leftX, rightX, nextY, yIncrement))
            {
                // They all succeeded, move our row forward
                for (int x = leftX; x <= rightX; x++)
                {
                    (map[x, y], map[x, nextY]) = (map[x, nextY], map[x, y]);
                }

                return true;
            }

            return false;
        }

        private bool Move(Grid2<char> map, Point2 p, Direction d)
        {
            if (map[p] == '#' || map[p] == '.')
            {
                return (map[p] == '.');
            }

            Point2 next = p + d;

            if (Move(map, next, d))
            {
                (map[p], map[next]) = (map[next], map[p]);
                return true;
            }

            return false;
        }

        private Grid2<char> Enlarge(Grid2<char> map)
        {
            Grid2<char> enlarged = new Grid2<char>(map.Bounds + map.Bounds * Point2.UnitX);

            foreach(Point2 source in map.AllPoints)
            {
                Point2 dest = source + source * Point2.UnitX;

                if (map[source] == 'O')
                {
                    enlarged[dest] = '[';
                    enlarged[dest + Point2.UnitX] = ']';
                }
                else if (map[source] == '@')
                {
                    enlarged[dest] = '@';
                    enlarged[dest + Point2.UnitX] = '.';
                }
                else
                {
                    enlarged[dest] = map[source];
                    enlarged[dest + Point2.UnitX] = map[source];
                }
            }

            return enlarged;
        }

        private (Grid2<char> map, List<Direction> moves) LoadPuzzle()
        {
            string[][] groups = PuzzleFile.ReadAllLineGroups("Day15.txt");
            Grid2<char> map = PuzzleFile.ReadLinesAsGrid(groups[0]);
            List<Direction> moves = new List<Direction>();

            foreach (string line in groups[1])
            {
                foreach (char ch in line)
                {
                    moves.Add(Direction.Parse(ch));
                }
            }

            return (map, moves);
        }
    }
}
