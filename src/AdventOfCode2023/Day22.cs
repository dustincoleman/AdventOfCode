namespace AdventOfCode2023;

public class Day22
{
    [Fact]
    public void Part1()
    {
        List<Brick> bricks = LoadPuzzle();
        SettleBricks(bricks);

        int answer = bricks.Count(b => b.CountSupported() == 0);
        Assert.Equal(505, answer);
    }

    [Fact]
    public void Part2()
    {
        List<Brick> bricks = LoadPuzzle();
        SettleBricks(bricks);

        int answer = bricks.Sum(b => b.CountSupported());
        Assert.Equal(71002, answer);
    }

    private List<Brick> LoadPuzzle()
    {
        return File.ReadAllLines("Day22.txt").Select(Brick.Parse).ToList();
    }

    private void SettleBricks(List<Brick> bricks)
    {
        bricks.Sort((x, y) => x.Bottom.CompareTo(y.Bottom));

        Point2 max = new Point2<int>(
            bricks.Max(b => b.CrossSection.Upper.X),
            bricks.Max(b => b.CrossSection.Upper.Y));

        Grid2<BrickTop> tops = new Grid2<BrickTop>(max + 1);

        foreach (Point2 pt in tops.Points)
        {
            tops[pt] = new BrickTop();
        }

        for (int i = 0; i < bricks.Count; i++)
        {
            Brick current = bricks[i];
            bool falling = true;

            while (falling)
            {
                foreach (Point2 pt in current.CrossSection.Points())
                {
                    if (tops[pt].Top > current.Bottom)
                    {
                        throw new Exception("Corrupt Tops Grid");
                    }

                    if (tops[pt].Top == current.Bottom)
                    {
                        Brick restingOn = tops[pt].Brick;
                        falling = false;

                        if (restingOn != null)
                        {
                            restingOn.Supports.Add(current);
                            current.SupportedBy.Add(restingOn);
                        }
                    }
                }

                if (falling)
                {
                    current.Drop();
                }
                else
                {
                    foreach (Point2 pt in current.CrossSection.Points())
                    {
                        tops[pt].Top = current.Top;
                        tops[pt].Brick = current;
                    }
                }
            }
        }
    }

    private class Brick
    {
        public Rect2 CrossSection;
        public int Bottom;
        public int Top;
        public HashSet<Brick> Supports = new HashSet<Brick>();
        public HashSet<Brick> SupportedBy = new HashSet<Brick>();

        public static Brick Parse(string line)
        {
            int[] values = line.Split(',', '~').Select(int.Parse).ToArray();
            if (values[2] > values[5]) throw new Exception("Unexpected Input");
            return new Brick()
            {
                CrossSection = new Rect2(new Point2(values[0], values[1]), new Point2(values[3], values[4]) + 1),
                Bottom = values[2] - 1,
                Top = values[5]
            };
        }

        public void Drop()
        {
            if (Bottom <= 0) throw new InvalidOperationException();
            Bottom--;
            Top--;
        }

        public int CountSupported()
        {
            HashSet<Brick> stack = new HashSet<Brick>();
            Queue<Brick> queue = new Queue<Brick>();

            stack.Add(this);
            queue.Enqueue(this);

            // NOTE: Chuck pointed out that this should be a priority queue based on Brick.Top to avoid
            //       a case where a tall brick causes us to have supports we have not yet reached.
            while (queue.TryDequeue(out Brick current))
            {
                foreach (Brick supported in current.Supports)
                {
                    if (supported.SupportedBy.Count <= 0)
                    {
                        throw new Exception("Corrupt Support List");
                    }
                    if (supported.SupportedBy.All(s => stack.Contains(s)))
                    {
                        if (stack.Add(supported))
                        {
                            queue.Enqueue(supported);
                        }
                    }
                }
            }

            return (stack.Count - 1);
        }
    }

    private class BrickTop
    {
        public Brick Brick;
        public int Top;
    }
}
