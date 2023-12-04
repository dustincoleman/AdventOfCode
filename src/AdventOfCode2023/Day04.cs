namespace AdventOfCode2023;

public class Day04
{
    [Fact]
    public void Part1()
    {
        int answer = 0;

        foreach (ScratchCard card in LoadPuzzle())
        {
            int matches = card.YourNumbers.Count(i => card.WinningNumbers.Contains(i));
            if (matches > 0)
            {
                answer += 1 << (matches - 1);
            }
        }

        Assert.Equal(19855, answer);
    }

    [Fact]
    public void Part2()
    {
        List<ScratchCard> cards = LoadPuzzle();
        int[] counts = new int[cards.Count];

        for (int i = 0; i < cards.Count; i++)
        {
            int cardCount = ++counts[i];
            int matches = cards[i].YourNumbers.Count(j => cards[i].WinningNumbers.Contains(j));

            for (int j = i + 1; matches > 0; matches--)
            {
                counts[j++] += cardCount;
            }
        }

        int answer = counts.Sum();

        Assert.Equal(10378710, answer);
    }

    private List<ScratchCard> LoadPuzzle()
    {
        List<ScratchCard> list = new List<ScratchCard>();

        foreach (string row in File.ReadAllLines("Day04.txt").Select(line => line.Split(":")[1]))
        {
            string[] split = row.Split(" | ");
            list.Add(
                new ScratchCard()
                {
                    WinningNumbers = ParseNumbers(split[0]),
                    YourNumbers = ParseNumbers(split[1])
                });
        }

        return list;
    }

    private List<int> ParseNumbers(string str) => str.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

    private struct ScratchCard
    {
        public List<int> WinningNumbers;
        public List<int> YourNumbers;
    }
}
