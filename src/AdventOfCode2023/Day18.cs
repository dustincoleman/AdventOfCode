using AdventOfCode.Common;

namespace AdventOfCode2023;

public class Day18
{
    [Fact]
    public void Part1()
    {
        List<PuzzleLine> puzzle = LoadPuzzle();
        int minX = 0, minY = 0, maxX = 0, maxY = 0;
        Point2 point = Point2.Zero;

        foreach (PuzzleLine line in puzzle)
        {
            point += line.Direction.Unit * line.Distance;
            minX = Math.Min(minX, point.X);
            minY = Math.Min(minY, point.Y);
            maxX = Math.Max(maxX, point.X);
            maxY = Math.Max(maxY, point.Y);
        }

        point = (-minX, -minY);
        Grid2<string> grid = new Grid2<string>(new Point2(maxX - minX, maxY - minY) + 1);
        grid[point] = string.Empty;

        foreach (PuzzleLine line in puzzle)
        {
            int remaining = line.Distance;

            while (remaining-- > 0)
            {
                point += line.Direction;
                grid[point] = line.RGB;
            }
        }

        foreach (Point2 edge in grid.EdgePoints)
        {
            Fill(grid, edge, string.Empty);
        }

        int answer = grid.Count(s => s != string.Empty); // Border values or interior null points
        Assert.Equal(35244, answer);
    }

    [Fact]
    public void Part2()
    {
        List<PuzzleLine> puzzle = LoadPuzzle();
        Point2 pos = Point2.Zero;
        Point2 min = Point2.Zero;
        Point2 max = Point2.Zero;

        List<Point2> points = new List<Point2<int>>();
        points.Add(pos);

        // Read in the puzzle
        foreach (PuzzleLine line in puzzle)
        {
            int distance = Convert.ToInt32(line.RGB.Substring(1, 5), fromBase: 16);
            Direction direction = line.RGB.Last() switch
            {
                '0' => Direction.Right,
                '1' => Direction.Down,
                '2' => Direction.Left,
                '3' => Direction.Up,
                _ => throw new Exception()
            };

            //pos += line.Direction.Unit * line.Distance;

            pos += direction.Unit * distance;
            points.Add(pos);
            min = Point2.Min(min, pos);
            max = Point2.Max(max, pos);
        }

        // Create a collection of lines from the points
        List<Line2> lines = new List<Line2<int>>();

        for (int i = 1; i < points.Count; i++)
        {
            lines.Add(new Line2(points[i - 1], points[i]));
        }

        // Identify which side of each line is inside the puzzle
        Dictionary<Line2, Direction> insideDirectionByLine = new Dictionary<Line2<int>, Direction>();

        Line2 firstEdge = lines.First(line => line.IsHorizontal ? line.Y == min.Y || line.Y == max.Y : line.X == min.X || line.X == max.X);

        if (firstEdge.IsHorizontal)
        {
            if (firstEdge.Y == min.Y)
            {
                insideDirectionByLine[firstEdge] = Direction.South;
            }
            else
            {
                insideDirectionByLine[firstEdge] = Direction.North;
            }
        }
        else
        {
            if (firstEdge.X == min.X)
            {
                insideDirectionByLine[firstEdge] = Direction.East;
            }
            else
            {
                insideDirectionByLine[firstEdge] = Direction.West;
            }
        }

        Direction inside = null;
        for (int i = 0; true; i++)
        {
            if (inside == null)
            {
                if (insideDirectionByLine.TryGetValue(lines[i], out Direction value))
                {
                    inside = value;
                }
            }
            else
            {
                Line2 line = lines[i % lines.Count];

                if (insideDirectionByLine.ContainsKey(line))
                {
                    // We made a full cycle
                    break;
                }

                if (inside == Direction.North)
                {
                    if (line.Direction == Direction.North) inside = Direction.East;
                    else if (line.Direction == Direction.South) inside = Direction.West;
                    else throw new Exception("Unexpected");
                }
                else if (inside == Direction.South)
                {
                    if (line.Direction == Direction.North) inside = Direction.East;
                    else if (line.Direction == Direction.South) inside = Direction.West;
                    else throw new Exception("Unexpected");
                }
                else if (inside == Direction.East)
                {
                    if (line.Direction == Direction.East) inside = Direction.South;
                    else if (line.Direction == Direction.West) inside = Direction.North;
                    else throw new Exception("Unexpected");
                }
                else if (inside == Direction.West)
                {
                    if (line.Direction == Direction.East) inside = Direction.South;
                    else if (line.Direction == Direction.West) inside = Direction.North;
                    else throw new Exception("Unexpected");
                }
                else throw new Exception("Unexpected");

                insideDirectionByLine[line] = inside;
            }
        }

        // Find the rectangular regions
        HashSet<Line2> connectors = new HashSet<Line2<int>>();
        AutoDictionary<Line2, List<int>> splitPointsByLine = new AutoDictionary<Line2, List<int>>();
        for (int i = 1; i < lines.Count - 1; i++)
        {
            Line2 first = lines[i - 1];
            Line2 second = lines[i];

            Line2? nearest = null;

            if (first.IsHorizontal)
            {
                Direction directionToConnect = second.Direction.Reverse();
                if (directionToConnect == insideDirectionByLine[first])
                {
                    nearest = FindNearestLine(first.Second, lines, directionToConnect);
                }
            }
            else
            {
                Direction directionToConnect = first.Direction;
                if (directionToConnect == insideDirectionByLine[second])
                {
                    nearest = FindNearestLine(first.Second, lines, directionToConnect);
                }
            }

            if (nearest.HasValue)
            {
                Line2 connector = new Line2(first.Second, new Point2(first.Second.X, nearest.Value.Y));

                if (!connectors.Contains(connector.Reverse()))
                {
                    connectors.Add(connector);
                }

                if (first.Second.X != nearest.Value.First.X && first.Second.X != nearest.Value.Second.X)
                {
                    splitPointsByLine[nearest.Value].Add(first.Second.X);
                }
            }
        }

        // Create the rectangular regions
        List<Line2> updatedLines = new List<Line2<int>>();

        foreach (Line2 line in lines)
        {
            if (!splitPointsByLine.TryGetValue(line, out List<int> xValues))
            {
                updatedLines.Add(line);
                continue;
            }

            inside = insideDirectionByLine[line];
            insideDirectionByLine.Remove(line);

            xValues.Add(line.Second.X);
            xValues.Sort();

            if (line.Direction == Direction.West)
            {
                xValues.Reverse();
            }

            Point2 prev = line.First;

            foreach (int x in xValues)
            {
                Point2 next = (x, line.Y);
                Line2 newLine = new Line2(prev, next);
                updatedLines.Add(newLine);
                insideDirectionByLine[newLine] = inside;
                prev = next;
            }
        }

        // Measure the regions
        long answer = 0;

        foreach (Line2 line in updatedLines.Where(line => line.IsHorizontal))
        {
            Direction directionToSearch = insideDirectionByLine[line];
            
            if (directionToSearch == Direction.South)
            {
                Line2? nearest = FindNearestOpposingLine(line, updatedLines, directionToSearch);
                long x = Math.Abs(line.First.X - line.Second.X) - 1;
                long y = Math.Abs(line.Y - nearest.Value.Y) - 1;
                answer += (x * y);
            }
        }

        foreach (Line2 connector in connectors)
        {
            answer += connector.Length - 1;
        }

        foreach (Line2 line in lines)
        {
            if (line.IsVertical)
            {
                answer += line.Length - 1;
            }
            else
            {
                answer += line.Length + 1;
            }
        }

        Assert.Equal(85070763635666, answer);
    }

    private Line2? FindNearestLine(Point2<int> point, List<Line2> lines, Direction direction)
    {
        IEnumerable<Line2> filtered =  lines.Where(l => l.IsHorizontal);

        if (direction == Direction.North)
        {
            filtered = filtered.Where(l => l.Y < point.Y);
        }
        else if (direction == Direction.South)
        {
            filtered = filtered.Where(l => l.Y > point.Y);
        }
        else
        {
            throw new Exception("Unexpected");
        }

        return filtered.Where(l => Math.Min(l.First.X, l.Second.X) <= point.X && Math.Max(l.First.X, l.Second.X) >= point.X).OrderBy(l => Math.Abs(l.Y - point.Y)).Cast<Line2?>().FirstOrDefault();
    }

    private Line2? FindNearestOpposingLine(Line2 line, List<Line2> lines, Direction direction)
    {
        IEnumerable<Line2> filtered = lines.Where(l => l.IsHorizontal);

        if (direction == Direction.North)
        {
            filtered = filtered.Where(l => l.Y < line.Y);
        }
        else if (direction == Direction.South)
        {
            filtered = filtered.Where(l => l.Y > line.Y);
        }
        else
        {
            throw new Exception("Unexpected");
        }

        return filtered.Where(l => l.Second.X == line.First.X && l.First.X == line.Second.X).OrderBy(l => Math.Abs(l.Y - line.Y)).Cast<Line2?>().FirstOrDefault();
    }

    private void Fill(Grid2<string> grid, Point2 startPoint, string value)
    {
        Queue<Point2> queue = new Queue<Point2>();
        queue.Enqueue(startPoint);

        while (queue.TryDequeue(out Point2 point))
        {
            if (grid.InBounds(point) && grid[point] == null)
            {
                grid[point] = value;

                foreach (Point2 next in grid.AdjacentPoints(point))
                {
                    queue.Enqueue(next);
                }
            }
        }
    }

    private List<PuzzleLine> LoadPuzzle()
    {
        List<PuzzleLine> list = new List<PuzzleLine>();

        foreach (string line in File.ReadAllLines("Day18.txt"))
        {
            string[] split = line.Split(' ');

            list.Add(
                new PuzzleLine()
                {
                    Direction = Direction.Parse(split[0]),
                    Distance = int.Parse(split[1]),
                    RGB = split[2].Trim('(', ')')
                });
        }

        return list;
    }

    private class PuzzleLine
    {
        public Direction Direction;
        public int Distance;
        public string RGB;
    }
}
