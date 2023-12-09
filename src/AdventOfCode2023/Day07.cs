using Newtonsoft.Json.Linq;

namespace AdventOfCode2023;

public class Day07
{
    [Fact]
    public void Part1()
    {
        List<Hand> hands = LoadPuzzle();
        Hand.JIsJoker = false;
        hands.Sort();

        long answer = 0;

        for (int i = 0; i < hands.Count; i++)
        {
            answer += hands[i].Bid * (i + 1);
        }

        Assert.Equal(252295678, answer);
    }

    [Fact]
    public void Part2()
    {
        List<Hand> hands = LoadPuzzle();
        Hand.JIsJoker = true;
        hands.Sort();

        long answer = 0;

        for (int i = 0; i < hands.Count; i++)
        {
            answer += hands[i].Bid * (i + 1);
        }

        Assert.Equal(250577259, answer);
    }

    private List<Hand> LoadPuzzle()
    {
        List<Hand> list = new List<Hand>();

        foreach (string line in File.ReadLines("Day07.txt"))
        {
            string[] split = line.Split(" ");
            list.Add(
                new Hand()
                {
                    Cards = split[0].ToCharArray(),
                    Bid = int.Parse(split[1])
                });
        }

        return list;
    }

    private class Hand : IComparable<Hand>
    {
        public static bool JIsJoker = false;

        public char[] Cards;
        public int Bid;

        private Lazy<HandType> _handType;

        public Hand()
        {
            _handType = new Lazy<HandType>(ComputeHandType);
        }

        public int CompareTo(Hand other)
        {
            int cmp = _handType.Value.CompareTo(other._handType.Value);

            if (cmp != 0)
            {
                return cmp;
            }
            
            for (int i = 0; i < 5; i++)
            {
                cmp = Rank(Cards[i]).CompareTo(Rank(other.Cards[i]));

                if (cmp != 0)
                {
                    return cmp;
                }
            }

            return 0;
        }

        private HandType ComputeHandType()
        {
            int jokerCount = 0;
            AutoDictionary<char, int> countsByCard = new AutoDictionary<char, int>();

            foreach (char ch in Cards)
            {
                if (JIsJoker && ch is 'J')
                {
                    jokerCount++;
                }
                else
                {
                    countsByCard[ch]++;
                }
            }

            if (jokerCount == 5)
            {
                return HandType.FiveOfAKind;
            }

            if (jokerCount > 0)
            {
                char ch = countsByCard.OrderByDescending(kvp => kvp.Value).First().Key;
                countsByCard[ch] += jokerCount;
            }
            
            if (countsByCard.Count is 1)
            {
                return HandType.FiveOfAKind;
            }

            if (countsByCard.Count is 2)
            {
                if (countsByCard.Values.First() is 1 or 4)
                {
                    return HandType.FourOfAKind;
                }

                return HandType.FullHouse;
            }

            if (countsByCard.Count is 3)
            {
                if (countsByCard.Values.Any(count => count is 3))
                {
                    return HandType.ThreeOfAKind;
                }

                return HandType.TwoPair;
            }

            if (countsByCard.Count is 4)
            {
                return HandType.OnePair;
            }

            return HandType.HighCard;
        }

        private static int Rank(char ch) => ch switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => JIsJoker ? 1 : 11,
            'T' => 10,
            _ => ch - '0'
        };
    }

    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
}
