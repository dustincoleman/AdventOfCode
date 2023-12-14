namespace AdventOfCode2023;

public class Day08
{
    [Fact]
    public void Part1()
    {
        Puzzle puzzle = LoadPuzzle();

        int answer = 0;
        string pos = "AAA";

        while (pos != "ZZZ")
        {
            foreach (char ch in puzzle.Instructions)
            {
                pos = ch switch
                {
                    'L' => puzzle.Map[pos].Left,
                    'R' => puzzle.Map[pos].Right,
                    _ => throw new Exception("Invalid char")
                };
                answer++;

                if (pos == "ZZZ")
                {
                    break;
                }
            }
        }

        Assert.Equal(12083, answer);
    }

    [Fact]
    public void Part2()
    {
        Puzzle puzzle = LoadPuzzle();

        long count = 0;
        List<string> pos = puzzle.Map.Keys.Where(k => k.EndsWith('A')).ToList();
        HashSet<long> counts = new HashSet<long>();

        while (pos.Count > 0)
        {
            foreach (char ch in puzzle.Instructions)
            {
                for (int i = 0; i < pos.Count; i++)
                {
                    pos[i] = ch switch
                    {
                        'L' => puzzle.Map[pos[i]].Left,
                        'R' => puzzle.Map[pos[i]].Right,
                        _ => throw new Exception("Invalid char")
                    };
                }

                count++;

                for (int i = pos.Count - 1; i >= 0; i--)
                {
                    if (pos[i].EndsWith('Z'))
                    {
                        pos.RemoveAt(i);
                        counts.Add(count);
                    }
                }

                if (pos.Count == 0)
                {
                    break;
                }
            }
        }

        long answer = counts.LeastCommonMultiple();

        Assert.Equal(13385272668829, answer);
    }

    private Puzzle LoadPuzzle()
    {
        string[][] lineGroups = PuzzleFile.ReadAllLineGroups("Day08.txt");
        Dictionary<string, Node> map = new Dictionary<string, Node>();

        foreach (string line in lineGroups[1])
        {
            string[] split = line.Split(new[] { ' ', '=', '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
            map[split[0]] = new Node(split[1], split[2]);
        }

        return new Puzzle()
        {
            Instructions = lineGroups[0][0].ToCharArray(),
            Map = map
        };
    }

    private class Puzzle
    {
        public char[] Instructions;
        public Dictionary<string, Node> Map;
    }

    private record Node(string Left, string Right);
}
