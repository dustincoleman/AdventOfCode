

using AdventOfCode.Common;

namespace AdventOfCode2024
{
    public class Day04
    {
        [Fact]
        public void Part1()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day04.txt");

            bool IsMatch(Point2<int> pt, Point2<int> direction, Grid2<char> puzzle)
            {
                return
                    puzzle[pt] == 'X' &&
                    puzzle.InBounds(pt + direction) && puzzle[pt + direction] == 'M' &&
                    puzzle.InBounds(pt + (direction * 2)) && puzzle[pt + (direction * 2)] == 'A' &&
                    puzzle.InBounds(pt + (direction * 3)) && puzzle[pt + (direction * 3)] == 'S';
            }

            int answer = puzzle.AllPoints.Sum(pt => Point2.Zero.Surrounding().Count(dir => IsMatch(pt, dir, puzzle)));
            Assert.Equal(2685, answer);
        }

        

        [Fact]
        public void Part2()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day04.txt");

            bool IsCrossMatch(Point2<int> pt, Grid2<char> puzzle)
            {
                return
                    puzzle[pt] == 'A' && !puzzle.IsEdge(pt) &&
                    (puzzle[pt - 1] == 'M' && puzzle[pt + 1] == 'S' || puzzle[pt - 1] == 'S' && puzzle[pt + 1] == 'M') &&
                    (puzzle[pt + (-1, 1)] == 'M' && puzzle[pt - (-1, 1)] == 'S' || puzzle[pt + (-1, 1)] == 'S' && puzzle[pt - (-1, 1)] == 'M');
            }

            int answer = puzzle.AllPoints.Count(pt => IsCrossMatch(pt, puzzle));
            Assert.Equal(2048, answer);
        }
    }
}
