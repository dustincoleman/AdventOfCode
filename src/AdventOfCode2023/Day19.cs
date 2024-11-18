
namespace AdventOfCode2023;

public class Day19
{
    [Fact]
    public void Part1()
    {
        int answer = 0;
        Puzzle puzzle = LoadPuzzle();

        foreach (Part part in puzzle.Parts)
        {
            string pos = "in";

            while (pos != "A" && pos != "R")
            {
                pos = puzzle.WorkflowsByName[pos].Process(part);
            }

            if (pos == "A")
            {
                answer += part.Total();
            }
        }

        Assert.Equal(389114, answer);
    }

    [Fact]
    public void Part2()
    {
        List<PartRange> completed = new List<PartRange>();
        List<PartRange> rejected = new List<PartRange>();
        Queue<PartRange> queue = new Queue<PartRange>();
        Puzzle puzzle = LoadPuzzle();

        queue.Enqueue(
            new PartRange()
            {
                State = "in",
                X = new Range(1, 4000),
                M = new Range(1, 4000),
                A = new Range(1, 4000),
                S = new Range(1, 4000)
            });

        while (queue.TryDequeue(out PartRange part))
        {
            if (part.State == "A")
            {
                completed.Add(part);
                continue;
            }

            if (part.State == "R")
            {
                rejected.Add(part);
                continue;
            }

            Workflow workflow = puzzle.WorkflowsByName[part.State];

            PartRange temp = part;

            foreach (Rule rule in workflow.Rules)
            {
                PartRange newPart = temp.ScopeBy(rule);
                queue.Enqueue(newPart);

                temp = temp.ScopeBy(rule, inverse: true);
            }

            temp.State = workflow.DefaultResult;
            queue.Enqueue(temp);
        }

        RangeNode root = new RangeNode();

        for (int i = 0; i < completed.Count; i++)
        {
            PartRange range = completed[i];
            List<Range> ranges = new List<Range>() { range.X, range.M, range.A, range.S };
            root.Merge(ranges);
            root.Validate();
        }

        long answer = root.Combinations();

        Assert.Equal(125051049836302, answer);
    }

    private class RangeNode
    {
        public Range Range;
        public List<RangeNode> Children = new List<RangeNode>();

        public RangeNode(Range range = null)
        {
            Range = range;
        }

        internal void Validate()
        {
            int end = -1;

            foreach (RangeNode child in Children)
            {
                if (child.Range.Begin <= end) throw new Exception();
                end = child.Range.End;
            }

            foreach (RangeNode child in Children)
            {
                child.Validate();
            }
        }

        internal long Combinations()
        {
            long combinations = 0;
            long count = Range != null ? Range.End - Range.Begin + 1 : 1;

            if (Children.Count == 0)
            {
                return count;
            }

            foreach (RangeNode child in Children)
            {
                long childCount = child.Combinations();
                combinations += childCount;
            }

            return count * combinations;
        }

        internal void Merge(List<Range> ranges, int pos = 0)
        {
            if (pos >= ranges.Count)
            {
                return;
            }

            Range rangeToMerge = ranges[pos];
            List<RangeNode> addTo = new List<RangeNode>();

            if (Children.Count == 0)
            {
                RangeNode newNode = new RangeNode(rangeToMerge);
                Children.Add(newNode);
                addTo.Add(newNode);
            }
            else
            {
                int begin = rangeToMerge.Begin;
                int end = rangeToMerge.End;

                for (int i = 0; (i < Children.Count && begin <= end); i++)
                {
                    RangeNode child = Children[i];

                    if (begin < child.Range.Begin)
                    {
                        if (i != 0)
                        {

                        }

                        if (end < child.Range.Begin)
                        {
                            // Add the entire range
                            RangeNode newNode = new RangeNode(new Range(begin, end));
                            Children.Insert(i, newNode);
                            addTo.Add(newNode);
                        }
                        else
                        {
                            // Add a range up the current node
                            RangeNode newNode = new RangeNode(new Range(begin, child.Range.Begin - 1));
                            Children.Insert(i++, newNode);
                            addTo.Add(newNode);

                            // Split the current node if needed
                            if (end < child.Range.End)
                            {
                                Children.RemoveAt(i);
                                Children.InsertRange(i, child.SplitAt(end));
                            }

                            // Add the current node
                            addTo.Add(Children[i]);
                        }

                        // We've handled up to the end of the current node
                        begin = Children[i].Range.End + 1;
                    }
                    else if (begin == child.Range.Begin)
                    {
                        // Split the current node if needed
                        if (end < child.Range.End)
                        {
                            Children.RemoveAt(i);
                            Children.InsertRange(i, child.SplitAt(end));
                        }

                        // Add the current node
                        addTo.Add(Children[i]);

                        // We've handled up to the end of the current node
                        begin = Children[i].Range.End + 1;
                    }
                    else if (begin <= child.Range.End)
                    {
                        // Split the current node
                        Children.RemoveAt(i);
                        Children.InsertRange(i, child.SplitAt(begin - 1));
                    }
                }

                if (begin <= end)
                {
                    RangeNode newNode = new RangeNode(new Range(begin, end));
                    Children.Add(newNode);
                    addTo.Add(newNode);
                }
            }

            foreach (RangeNode child in addTo)
            {
                child.Merge(ranges, pos + 1);
            }
        }

        private IEnumerable<RangeNode> SplitAt(int end)
        {
            Range originalRange = Range;
            RangeNode first = Clone();
            RangeNode second = Clone();

            first.Range = new Range(originalRange.Begin, end);
            second.Range = new Range(end + 1, originalRange.End);

            yield return first;
            yield return second;
        }

        private RangeNode Clone()
        {
            List<RangeNode> childrenClone = new List<RangeNode>();

            foreach (RangeNode child in Children)
            {
                childrenClone.Add(child.Clone());
            }

            return new RangeNode(Range)
            {
                Children = childrenClone
            };
        }
    }

    private Puzzle LoadPuzzle()
    {
        Puzzle puzzle = new Puzzle();
        string[][] groups = PuzzleFile.ReadAllLineGroups("Day19.txt");

        foreach (string line in groups[0])
        {
            string[] split = line.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            puzzle.WorkflowsByName[split[0]] = Workflow.Parse(split[1]);
        }

        foreach (string line in groups[1])
        {
            puzzle.Parts.Add(Part.Parse(line.Trim('{', '}')));
        }

        return puzzle;
    }

    private class Puzzle
    {
        public Dictionary<string, Workflow> WorkflowsByName = new Dictionary<string, Workflow>();
        public List<Part> Parts = new List<Part>();
    }

    private class Workflow
    {
        public List<Rule> Rules;
        public string DefaultResult;

        internal static Workflow Parse(string line)
        {
            List<Rule> rules = new List<Rule>();
            string[] entries = line.Split(',');

            for (int i = 0; i < entries.Length - 1; i++)
            {
                rules.Add(Rule.Parse(entries[i]));
            }

            return new Workflow()
            {
                Rules = rules,
                DefaultResult = entries[entries.Length - 1]
            };
        }

        internal string Process(Part part)
        {
            foreach (Rule rule in Rules)
            {
                int rating = part.GetRating(rule.Field);

                if (rule.IsGreaterThan)
                {
                    if (rating > rule.Value)
                    {
                        return rule.Result;
                    }
                }
                else
                {
                    if (rating < rule.Value)
                    {
                        return rule.Result;
                    }
                }
            }

            return DefaultResult;
        }
    }

    private class Rule
    {
        public char Field;
        public int Value;
        public bool IsGreaterThan;
        public string Result;

        internal static Rule Parse(string entry)
        {
            string[] split = entry.Split(':');

            return new Rule()
            {
                Field = split[0][0],
                IsGreaterThan = (split[0][1] == '>'),
                Value = int.Parse(split[0].Substring(2)),
                Result = split[1]
            };
        }

        public override string ToString()
        {
            return $"{Field} {(IsGreaterThan ? '>' : '<')} {Value} => {Result}";
        }
    }

    private class PartRange
    {
        public string State;
        public Range X;
        public Range M;
        public Range A;
        public Range S;

        internal long Product() => (long)(X.End - X.Begin + 1) * (M.End - M.Begin + 1) * (A.End - A.Begin + 1) * (S.End - S.Begin + 1);

        internal PartRange ScopeBy(Rule rule, bool inverse = false)
        {
            PartRange newRange = new PartRange()
            {
                State = rule.Result,
                X = X,
                M = M,
                A = A,
                S = S
            };

            int begin;
            int end;

            switch (rule.Field)
            {
                case 'x':
                    begin = X.Begin;
                    end = X.End;
                    break;
                case 'm':
                    begin = M.Begin;
                    end = M.End;
                    break;
                case 'a':
                    begin = A.Begin;
                    end = A.End;
                    break;
                case 's':
                    begin = S.Begin;
                    end = S.End;
                    break;
                default:
                    throw new Exception();
            }

            if (rule.IsGreaterThan)
            {
                if (inverse)
                {
                    end = Math.Min(end, rule.Value);
                }
                else
                {
                    begin = Math.Max(begin, rule.Value + 1);
                }
            }
            else
            {
                if (inverse)
                {
                    begin = Math.Max(begin, rule.Value);
                }
                else
                {
                    end = Math.Min(end, rule.Value - 1);
                }
            }

            switch (rule.Field)
            {
                case 'x':
                    newRange.X = new Range(begin, end);
                    break;
                case 'm':
                    newRange.M = new Range(begin, end);
                    break;
                case 'a':
                    newRange.A = new Range(begin, end);
                    break;
                case 's':
                    newRange.S = new Range(begin, end);
                    break;
                default:
                    throw new Exception();
            }

            return newRange;
        }
    }

    private class Part
    {
        public int X;
        public int M;
        public int A;
        public int S;

        internal static Part Parse(string line)
        {
            string[] split = line.Split(',');

            return new Part()
            {
                X = int.Parse(split[0].Substring(2)),
                M = int.Parse(split[1].Substring(2)),
                A = int.Parse(split[2].Substring(2)),
                S = int.Parse(split[3].Substring(2))
            };
        }

        internal int GetRating(char field) => field switch
        {
            'x' => X,
            'm' => M,
            'a' => A,
            's' => S,
            _ => throw new ArgumentException()
        };

        internal int Total() => X + M + A + S;
    }
}
