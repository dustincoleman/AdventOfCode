using System.Numerics;

namespace AdventOfCode2023;

public class Day05
{
    [Fact]
    public void Part1()
    {
        Puzzle puzzle = LoadPuzzle();
        BigInteger answer = long.MaxValue;

        foreach (long seed in puzzle.Seeds)
        {
            BigInteger l = seed;

            foreach (PuzzleMap map in puzzle.Maps)
            {
                l = map.MapValue(l);
            }

            answer = BigInteger.Min(answer, l);
        }

        Assert.Equal(289863851, answer);
    }

    [Fact]
    public void Part2()
    {
        Puzzle puzzle = LoadPuzzle();
        PuzzleMap flatMap = FlattenMaps(puzzle.Maps);
        BigInteger answer = ulong.MaxValue << 64 | ulong.MaxValue;

        for (int i = 0; i < puzzle.Seeds.Count; i += 2)
        {
            PuzzleMapRange seedRange = new PuzzleMapRange(puzzle.Seeds[i], puzzle.Seeds[i] + puzzle.Seeds[i + 1] - 1);
            PuzzleMapEntry seedEntry = new PuzzleMapEntry()
            {
                Source = seedRange,
                Destination = seedRange
            };

            foreach (PuzzleMapEntry entry in flatMap.MapRange(seedEntry))
            {
                answer = BigInteger.Min(answer, entry.Destination.Begin);
            }
        }

        Assert.Equal(60568880, answer);
    }

    private PuzzleMap FlattenMaps(List<PuzzleMap> maps)
    {
        PuzzleMap flatMap = maps[0].SortAndFillGaps();

        for (int i = 1; i < maps.Count; i++)
        {
            flatMap = FlattenMaps(flatMap, maps[i].SortAndFillGaps());
        }

        return flatMap;
    }

    private PuzzleMap FlattenMaps(PuzzleMap left, PuzzleMap right)
    {
        List<PuzzleMapEntry> newEntries = new List<PuzzleMapEntry>();

        foreach (PuzzleMapEntry leftEntry in left.Entries)
        {
            newEntries.AddRange(right.MapRange(leftEntry));
        }

        newEntries.Sort((x, y) =>
        {
            int comp = x.Source.Begin.CompareTo(y.Source.Begin);
            if (comp != 0)
            {
                return comp;
            }

            return y.Source.End.CompareTo(x.Source.End);
        });

        for (int i = 0; i < newEntries.Count; i++)
        {
            for (int j = i + 1; j < newEntries.Count;)
            {
                if (newEntries[i].Source.Contains(newEntries[j].Source))
                {
                    newEntries.RemoveAt(j);
                }
                else
                {
                    j++;
                }
            }
        }

        PuzzleMap newMap = new PuzzleMap();
        newMap.Entries = newEntries;

        return newMap;
    }

    private Puzzle LoadPuzzle()
    {
        string[][] groups = PuzzleFile.ReadAllLineGroups("Day05.txt");
        Puzzle puzzle = new Puzzle();

        puzzle.Seeds.AddRange(groups[0][0].Substring("seeds: ".Length).Split(" ").Select(long.Parse));

        for (int i = 1; i < groups.Length; i++)
        {
            PuzzleMap map = new PuzzleMap();

            foreach (string line in groups[i].Skip(1))
            {
                long[] values = line.Split(" ").Select(long.Parse).ToArray();
                map.AddEntry(values[0], values[1], values[2]);
            }

            puzzle.Maps.Add(map);
        }

        return puzzle;
    }

    private class Puzzle
    {
        public List<long> Seeds = new List<long>();
        public List<PuzzleMap> Maps = new List<PuzzleMap>();
    }

    private class PuzzleMap
    {
        public List<PuzzleMapEntry> Entries = new List<PuzzleMapEntry>();

        public void AddEntry(long destinationStart, long sourceStart, long length)
        {
            Entries.Add(
                new PuzzleMapEntry()
                {
                    Source = new PuzzleMapRange(sourceStart, sourceStart + length - 1),
                    Destination = new PuzzleMapRange(destinationStart, destinationStart + length - 1)
                });
        }

        public PuzzleMap SortAndFillGaps()
        {
            List<PuzzleMapEntry> newEntries = new List<PuzzleMapEntry>(Entries);

            newEntries.Sort((x, y) => x.Source.Begin.CompareTo(y.Source.Begin));

            if (newEntries[0].Source.Begin > 0)
            {
                PuzzleMapRange beginRange = new PuzzleMapRange(0, newEntries[0].Source.Begin - 1);
                newEntries.Insert(0,
                    new PuzzleMapEntry()
                    {
                        Source = beginRange,
                        Destination = beginRange
                    });
            }

            for (int pos = 1; pos < newEntries.Count; pos++)
            {
                if (newEntries[pos - 1].Source.End + 1 < newEntries[pos].Source.Begin)
                {
                    PuzzleMapRange range = new PuzzleMapRange(newEntries[pos - 1].Source.End + 1, newEntries[pos].Source.Begin - 1);
                    newEntries.Insert(pos,
                        new PuzzleMapEntry()
                        {
                            Source = range,
                            Destination = range
                        });
                }
            }

            PuzzleMapRange endRange = new PuzzleMapRange(newEntries[newEntries.Count - 1].Source.End + 1, long.MaxValue);
            newEntries.Add(
                new PuzzleMapEntry()
                {
                    Source = endRange,
                    Destination = endRange
                });

            PuzzleMap newMap = new PuzzleMap();
            newMap.Entries = newEntries;

            return newMap;
        }

        public BigInteger MapValue(BigInteger value)
        {
            foreach (var entry in Entries)
            {
                if (value >= entry.Source.Begin && value <= entry.Source.End)
                {
                    return entry.Destination.Begin + (value - entry.Source.Begin);
                }
            }

            return value;
        }

        internal IEnumerable<PuzzleMapEntry> MapRange(PuzzleMapEntry source)
        {
            List<PuzzleMapEntry> list = new List<PuzzleMapEntry>();

            BigInteger sourcePos = source.Source.Begin;
            BigInteger destPos = source.Destination.Begin;

            while (sourcePos <= source.Source.End)
            {
                PuzzleMapEntry match = Entries.First(e => e.Source.Begin <= destPos && e.Source.End >= destPos);
                BigInteger matchLength = BigInteger.Min(match.Source.End - destPos + 1, source.Source.End - sourcePos + 1);
                BigInteger offset = destPos - match.Source.Begin;

                PuzzleMapEntry mappedEntry = new PuzzleMapEntry()
                {
                    Source = new PuzzleMapRange(sourcePos, sourcePos + matchLength - 1),
                    Destination = new PuzzleMapRange(match.Destination.Begin + offset, match.Destination.Begin + offset + matchLength - 1)
                };

                list.Add(mappedEntry);

                if (destPos + matchLength - 1 == long.MaxValue)
                {
                    break;
                }

                sourcePos += matchLength;
                destPos += matchLength;
            }

            return list;
        }
    }

    private class PuzzleMapEntry
    {
        public PuzzleMapRange Source;
        public PuzzleMapRange Destination;
    }

    private class PuzzleMapRange
    {
        public BigInteger Begin;
        public BigInteger End;

        public PuzzleMapRange(BigInteger begin, BigInteger end)
        {
            Begin = begin;
            End = end;
        }

        public bool Contains(PuzzleMapRange other)
        {
            return (other.Begin >= Begin && other.End <= End);
        }
    }
}
