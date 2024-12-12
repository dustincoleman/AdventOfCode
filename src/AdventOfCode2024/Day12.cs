namespace AdventOfCode2024
{
    public class Day12
    {
        [Fact]
        public void Part1()
        {
            Grid2<Plot> puzzle = PuzzleFile.ReadAsGrid("Day12.txt", (ch, pt) => new Plot(pt, ch));
            long result = 0;

            foreach (Plot plot in puzzle)
            {
                if (!plot.Visited)
                {
                    List<Plot> plotGroup = VisitPlotGroup(puzzle, plot);
                    int perimeter = MeasurePerimeter(puzzle, plotGroup);
                    result += perimeter * plotGroup.Count;
                }
            }

            Assert.Equal(1363682, result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<Plot> puzzle = PuzzleFile.ReadAsGrid("Day12.txt", (ch, pt) => new Plot(pt, ch));
            long result = 0;

            foreach (Plot plot in puzzle)
            {
                if (!plot.Visited)
                {
                    List<Plot> plotGroup = VisitPlotGroup(puzzle, plot);
                    int sides = CountSides(puzzle, plotGroup);
                    result += sides * plotGroup.Count;
                }
            }

            Assert.Equal(787680, result);
        }

        private List<Plot> VisitPlotGroup(Grid2<Plot> puzzle, Plot start)
        {
            List<Plot> plots = new List<Plot>();
            Queue<Plot> queue = new Queue<Plot>();

            queue.Enqueue(start);

            while (queue.TryDequeue(out Plot current))
            {
                if (!current.Visited && current.Plant == start.Plant)
                {
                    plots.Add(current);
                    current.Visited = true;

                    foreach (Plot adj in puzzle.Adjacent(current.Location))
                    {
                        queue.Enqueue(adj);
                    }
                }
            }

            return plots;
        }

        private int MeasurePerimeter(Grid2<Plot> puzzle, List<Plot> plotGroup)
        {
            int perimeter = 0;

            foreach (Plot plot in plotGroup)
            {
                foreach (Point2 pt in plot.Location.Adjacent())
                {
                    if (!puzzle.InBounds(pt) || puzzle[pt].Plant != plot.Plant)
                    {
                        perimeter++;
                    }
                }
            }

            return perimeter;
        }

        private int CountSides(Grid2<Plot> puzzle, List<Plot> plotGroup)
        {
            int sides = 0;
            List<Plot>[] fencePointsBySide = [new List<Plot>(), new List<Plot>(), new List<Plot>(), new List<Plot>()];

            // Find all of the fence segments for each side of the plots
            foreach (Plot plot in plotGroup)
            {
                foreach (Direction side in Direction.All())
                {
                    Point2 pt = plot.Location + side;
                    if (!puzzle.InBounds(pt) || puzzle[pt].Plant != plot.Plant)
                    {
                        fencePointsBySide[(int)side].Add(new Plot(plot.Location, plot.Plant));
                    }
                }
            }

            // Count the fences
            foreach (Direction side in Direction.All())
            {
                List<Plot> fencePoints = fencePointsBySide[(int)side];
                Direction direction = side.TurnRight();

                foreach (Plot plot in fencePoints)
                {
                    // Find the next segment we haven't visited
                    if (!plot.Visited)
                    {
                        sides++;

                        // Walk the fence each direction and mark it visited
                        Plot next = plot;
                        while (next != null)
                        {
                            next.Visited = true;
                            next = fencePoints.Find(plot => plot.Location == next.Location + direction);
                        }

                        next = plot;
                        while (next != null)
                        {
                            next.Visited = true;
                            next = fencePoints.Find(plot => plot.Location == next.Location - direction);
                        }
                    }
                }
            }

            return sides;
        }

        private record Plot(Point2 Location, char Plant)
        {
            internal bool Visited { get; set; }
        }
    }
}
