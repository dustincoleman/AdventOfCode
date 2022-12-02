namespace AdventOfCode2022
{
    public enum Moves : int
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public class Day02
    {
        [Fact]
        public void Part1()
        {
            int score = 0;

            foreach (string line in File.ReadAllLines("Day02.txt"))
            {
                string[] moves = line.Split(' ');

                Moves p1 = moves[0] == "A" ? Moves.Rock :
                           moves[0] == "B" ? Moves.Paper :
                           moves[0] == "C" ? Moves.Scissors :
                           throw new Exception();

                Moves p2 = moves[1] == "X" ? Moves.Rock :
                           moves[1] == "Y" ? Moves.Paper :
                           moves[1] == "Z" ? Moves.Scissors :
                           throw new Exception();

                score += (int)p2;

                if (p1 == p2)
                {
                    score += 3;
                }
                else if (p1 == Moves.Rock && p2 == Moves.Paper || p1 == Moves.Paper && p2 == Moves.Scissors || p1 == Moves.Scissors && p2 == Moves.Rock)
                {
                    score += 6;
                }
            }

            Assert.Equal(10404, score);
        }

        [Fact]
        public void Part2()
        {
            int score = 0;

            foreach (string line in File.ReadAllLines("Day02.txt"))
            {
                string[] moves = line.Split(' ');

                Moves p1 = moves[0] == "A" ? Moves.Rock :
                           moves[0] == "B" ? Moves.Paper :
                           moves[0] == "C" ? Moves.Scissors :
                           throw new Exception();

                Moves p2 = moves[1] == "X" ? (Moves)((((int)p1 + 1) % 3) + 1) :
                           moves[1] == "Y" ? p1 :
                           moves[1] == "Z" ? (Moves)((((int)p1) % 3) + 1) :
                           throw new Exception();

                score += (int)p2;

                if (p1 == p2)
                {
                    score += 3;
                }
                else if (p1 == Moves.Rock && p2 == Moves.Paper || p1 == Moves.Paper && p2 == Moves.Scissors || p1 == Moves.Scissors && p2 == Moves.Rock)
                {
                    score += 6;
                }
            }

            Assert.Equal(-1, score);
        }
    }
}