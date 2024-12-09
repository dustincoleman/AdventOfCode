namespace AdventOfCode2024
{
    public class Day08
    {
        [Fact]
        public void Part1()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day08.txt");
            HashSet<Point2> antinodes = new HashSet<Point2>();

            foreach (char frequency in puzzle.Where(ch => ch != '.').Distinct())
            {
                List<Point2> antennas = puzzle.AllPoints.Where(pt => puzzle[pt] == frequency).ToList();
                
                for (int i = 0; i < antennas.Count - 1; i++)
                {
                    for (int j = i + 1; j < antennas.Count; j++)
                    {
                        Point2 first = antennas[i];
                        Point2 second = antennas[j];
                        Point2 distance = second - first;

                        Point2 antinode = first - distance;
                        if (puzzle.InBounds(antinode))
                        {
                            antinodes.Add(antinode);
                        }

                        antinode = second + distance;
                        if (puzzle.InBounds(antinode))
                        {
                            antinodes.Add(antinode);
                        }
                    }
                }
            }

            int answer = antinodes.Count;
            Assert.Equal(327, answer);
        }

        [Fact]
        public void Part2()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day08.txt");
            HashSet<Point2> antinodes = new HashSet<Point2>();

            foreach (char frequency in puzzle.Where(ch => ch != '.').Distinct())
            {
                List<Point2> antennas = puzzle.AllPoints.Where(pt => puzzle[pt] == frequency).ToList();

                for (int i = 0; i < antennas.Count - 1; i++)
                {
                    for (int j = i + 1; j < antennas.Count; j++)
                    {
                        Point2 first = antennas[i];
                        Point2 second = antennas[j];
                        Point2 distance = second - first;

                        Point2 antinode = first;
                        while (puzzle.InBounds(antinode))
                        {
                            antinodes.Add(antinode);
                            antinode -= distance;
                        }

                        antinode = second;
                        while (puzzle.InBounds(antinode))
                        {
                            antinodes.Add(antinode);
                            antinode += distance;
                        }
                    }
                }
            }

            int answer = antinodes.Count;
            Assert.Equal(1233, answer);
        }
    }
}
