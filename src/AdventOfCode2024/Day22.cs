namespace AdventOfCode2024
{
    public class Day22
    {
        [Fact]
        public void Part1()
        {
            List<long> puzzle = File.ReadAllLines("Day22.txt").Select(long.Parse).ToList();
            long result = 0;

            foreach (int seed in puzzle)
            {
                long secret = Generate(seed, 2000);
                result += secret;
            }

            Assert.Equal(19150344884, result);
        }

        [Fact]
        public void Part2()
        {
            List<long> puzzle = File.ReadAllLines("Day22.txt").Select(long.Parse).ToList();
            HashSet<int> allSequences = new HashSet<int>();
            List<int[]> buyers = new List<int[]>();

            foreach (long num in puzzle)
            {
                int[] priceBySequence = new int[SellSequence.MaxKey];
                SellSequence sequence = new SellSequence();
                long last = num;
                int remaining = 2000;

                while (remaining-- > 1997)
                {
                    long next = Next(last);
                    sequence = sequence.Next((sbyte)((next % 10) - (last % 10)));
                    last = next;
                }

                while (remaining-- > 0)
                {
                    long next = Next(last);
                    sequence = sequence.Next((sbyte)((next % 10) - (last % 10)));

                    int key = sequence.Key();
                    if (priceBySequence[key] == 0)
                    {
                        priceBySequence[key] = (int)(next % 10);
                        allSequences.Add(key);
                    }
                    last = next;
                }

                buyers.Add(priceBySequence);
            }

            long result = 0;

            foreach (int sequence in allSequences)
            {
                long total = 0;

                foreach (int[] priceBySequence in buyers)
                {
                    total += priceBySequence[sequence];
                }

                if (total > result)
                {
                    result = total;
                }
            }

            Assert.Equal(2121, result);
        }

        private long Generate(long seed, int n)
        {
            while (n-- > 0)
            {
                seed = Next(seed);
            }

            return seed;
        }

        private long Next(long num)
        {
            num = (num ^ (num * 64)) % 16777216;
            num = (num ^ (num / 32)) % 16777216;
            num = (num ^ (num * 2048)) % 16777216;
            return num;
        }

        private record struct SellSequence(sbyte First, sbyte Second, sbyte Third, sbyte Fourth)
        {
            internal static int MaxKey => 1 << 20;

            internal int Key() => (First + 10) << 15 | (Second + 10) << 10 | (Third + 10) << 5 | (Fourth + 10);

            internal SellSequence Next(sbyte next) => new SellSequence(Second, Third, Fourth, next);
        }
    }
}
