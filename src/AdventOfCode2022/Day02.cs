using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode2022
{
    public class Day02
    {
        [Fact]
        public void Part1()
        {
            int score = 0;

            foreach (string line in File.ReadAllLines("Day02.txt"))
            {
                int p1 = line[0] - 'A';
                int p2 = line[2] - 'X';

                score += Score(p1, p2);
            }

            Assert.Equal(10404, score);
        }

        [Fact]
        public void Part2()
        {
            int score = 0;

            foreach (string line in File.ReadAllLines("Day02.txt"))
            {
                int p1 = line[0] - 'A';
                int p2 = (p1 + (line[2] - 'Y') + 3) % 3; // Use 'Y' to get -1, 0, 1

                score += Score(p1, p2);
            }

            Assert.Equal(10334, score);
        }

        private int Score(int p1, int p2)
        {
            // (p2 - p1 + 3) % 3 give -1, 0, 1 for loss, tie, win.
            // Use '+ 4' to get our multiple of 6 points
            return ((p2 - p1 + 4) % 3) * 3 + p2 + 1;
        }
    }
}