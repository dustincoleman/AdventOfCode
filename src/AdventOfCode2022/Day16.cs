using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2022
{
    public class Day16
    {
        Dictionary<string, Valve> valvesByName;
        List<Valve> valves;
        List<string> print;

        [Fact]
        public void Part1()
        {
            print = new List<string>();
            valvesByName = LoadPuzzle();
            valves = valvesByName.Values.ToList();
            valves.Sort((v1, v2) => v1.Name.CompareTo(v2.Name));

            for (int i = 0; i < 10; i++)
            {
                foreach (Valve v in valves)
                {
                    v.Reduce();
                }
            }

            int maxFlow = FindMaxFlow2(valvesByName["AA"], valvesByName["AA"], 30, 0, valvesByName.Values.Select(v => v.FlowRate).Sum());

            print.Sort((left, right) => new string(left.Reverse().ToArray()).CompareTo(new string(right.Reverse().ToArray())));

            Assert.Equal(1673, maxFlow);
        }


        [Fact]
        public void Part2()
        {
            print = new List<string>();
            valvesByName = LoadPuzzle();
            valves = valvesByName.Values.ToList();
            valves.Sort((v1, v2) => v1.Name.CompareTo(v2.Name));

            for (int i = 0; i < 10; i++)
            {
                foreach (Valve v in valves)
                {
                    v.Reduce();
                }
            }

            int maxFlow = FindMaxFlow2(valvesByName["AA"], valvesByName["AA"], 26, 26, valvesByName.Values.Select(v => v.FlowRate).Sum());

            print.Sort((left, right) => new string(left.Reverse().ToArray()).CompareTo(new string(right.Reverse().ToArray())));

            Assert.Equal(1707, maxFlow);
        }

        private int FindMaxFlow(Valve start, int stepsRemaining, int totalFlow)
        {
            int maxFlow = 0;
            FindMaxFlowHelper(totalFlow, stepsRemaining, new Edge() { Valve = start }, new HashSet<Valve>(), new Stack<Move>(), ref maxFlow);
            return maxFlow;
        }

        Dictionary<string, int> scoresByKey = new Dictionary<string, int>();

        private void FindMaxFlowHelper(int totalFlow, int stepsRemaining, Edge current, HashSet<Valve> visited, Stack<Move> moves, ref int best, int runningScore = 0)
        {
            if (stepsRemaining <= 0 || (runningScore + (totalFlow * stepsRemaining) < best))
            {
                print.Add(string.Concat(moves.Reverse().Select(m => m.Print())));
                return;
            }

            foreach (Edge next in current.Valve.LeadsTo)
            {
                if (visited.Add(next.Valve))
                {
                    if (next.Valve.FlowRate > 0 && !next.Valve.IsOpened)
                    {
                        next.Valve.IsOpened = true;
                        moves.Push(new Move() { Edge = next, OpenValve = true });

                        int tempScore = runningScore + (next.Valve.FlowRate * (stepsRemaining - next.Distance - 1));

                        if (tempScore > best)
                        {
                            best = tempScore;
                        }

                        FindMaxFlowHelper(totalFlow - next.Valve.FlowRate, stepsRemaining - next.Distance - 1, next, new HashSet<Valve>(), moves, ref best, tempScore);

                        moves.Pop();
                        next.Valve.IsOpened = false;
                    }

                    moves.Push(new Move() { Edge = next, OpenValve = false });
                    FindMaxFlowHelper(totalFlow, stepsRemaining - next.Distance, next, visited, moves, ref best, runningScore);
                    moves.Pop();

                    visited.Remove(next.Valve);
                }
            }
        }

        private int FindMaxFlow2(Valve start1, Valve start2, int stepsRemaining1, int stepsRemaining2, int totalFlow)
        {
            int maxFlow = 0;
            FindMaxFlowHelper2(totalFlow, stepsRemaining1, stepsRemaining2, new Edge() { Valve = start1 }, new Edge() { Valve = start2 }, new HashSet<Valve>(), new HashSet<Valve>(), new Stack<Move>(), new Stack<Move>(), ref maxFlow);
            return maxFlow;
        }

        private void FindMaxFlowHelper2(int totalFlow, int stepsRemaining1, int stepsRemaining2, Edge current1, Edge current2, HashSet<Valve> visited1, HashSet<Valve> visited2, Stack<Move> moves1, Stack<Move> moves2, ref int best, int runningScore = 0)
        {
            if ((stepsRemaining1 < 2 && stepsRemaining2 < 2) || (runningScore + (totalFlow * Math.Max(stepsRemaining1, stepsRemaining2)) < best))
            {
                //print.Add(string.Concat(moves1.Reverse().Select(m => m.Print())) + " | " + string.Concat(moves2.Reverse().Select(m => m.Print())));
                print.Add(string.Concat(moves1.Reverse().Select(m => m.Print())));
                print.Add(string.Concat(moves2.Reverse().Select(m => m.Print())));
                return;
            }

            List<(Edge next1, Edge next2)> combos = new List<(Edge next1, Edge next2)>();

            if (stepsRemaining1 >= 2 && stepsRemaining2 >= 2)
            {
                foreach (Edge next1 in current1.Valve.LeadsTo.Where(e => !visited1.Contains(e.Valve)))
                {
                    foreach (Edge next2 in current2.Valve.LeadsTo.Where(e => !visited2.Contains(e.Valve)))
                    {
                        combos.Add((next1, next2));
                    }
                }
            }
            else if (stepsRemaining1 >= 2)
            {
                FindMaxFlowHelper(totalFlow, stepsRemaining1, current1, visited1, moves1, ref best, runningScore);
                return;
            }
            else
            {
                FindMaxFlowHelper(totalFlow, stepsRemaining2, current2, visited2, moves2, ref best, runningScore);
                return;
            }

            foreach ((Edge next1, Edge next2) in combos)
            {
                visited1.Add(next1.Valve);
                visited2.Add(next2.Valve);

                if (next1.Valve.FlowRate > 0 && !next1.Valve.IsOpened && next2.Valve.FlowRate > 0 && !next2.Valve.IsOpened && next1.Valve != next2.Valve)
                {
                    next1.Valve.IsOpened = true;
                    next2.Valve.IsOpened = true;

                    moves1.Push(new Move() { Edge = next1, OpenValve = true });
                    moves2.Push(new Move() { Edge = next2, OpenValve = true });

                    int tempScore = runningScore + (next1.Valve.FlowRate * (stepsRemaining1 - next1.Distance - 1)) + (next2.Valve.FlowRate * (stepsRemaining2 - next2.Distance - 1));

                    if (tempScore > best)
                    {
                        best = tempScore;
                    }

                    FindMaxFlowHelper2(totalFlow - next1.Valve.FlowRate - next2.Valve.FlowRate, stepsRemaining1 - next1.Distance - 1, stepsRemaining2 - next2.Distance - 1, next1, next2, new HashSet<Valve>(), new HashSet<Valve>(), moves1, moves2, ref best, tempScore);

                    moves1.Pop();
                    moves2.Pop();

                    next1.Valve.IsOpened = false;
                    next2.Valve.IsOpened = false;
                }

                if (next1.Valve.FlowRate > 0 && !next1.Valve.IsOpened)
                {
                    next1.Valve.IsOpened = true;

                    moves1.Push(new Move() { Edge = next1, OpenValve = true });
                    moves2.Push(new Move() { Edge = next2, OpenValve = false });

                    int tempScore = runningScore + (next1.Valve.FlowRate * (stepsRemaining1 - next1.Distance - 1));

                    if (tempScore > best)
                    {
                        best = tempScore;
                    }

                    FindMaxFlowHelper2(totalFlow - next1.Valve.FlowRate, stepsRemaining1 - next1.Distance - 1, stepsRemaining2 - next2.Distance, next1, next2, new HashSet<Valve>(), visited2, moves1, moves2, ref best, tempScore);

                    moves1.Pop();
                    moves2.Pop();

                    next1.Valve.IsOpened = false;
                }
                
                if (next2.Valve.FlowRate > 0 && !next2.Valve.IsOpened)
                {
                    next2.Valve.IsOpened = true;

                    moves1.Push(new Move() { Edge = next1, OpenValve = false });
                    moves2.Push(new Move() { Edge = next2, OpenValve = true });

                    int tempScore = runningScore + (next2.Valve.FlowRate * (stepsRemaining2 - next2.Distance - 1));

                    if (tempScore > best)
                    {
                        best = tempScore;
                    }

                    FindMaxFlowHelper2(totalFlow - next2.Valve.FlowRate, stepsRemaining1 - next1.Distance, stepsRemaining2 - next2.Distance - 1, next1, next2, visited1, new HashSet<Valve>(), moves1, moves2, ref best, tempScore);

                    moves1.Pop();
                    moves2.Pop();

                    next2.Valve.IsOpened = false;
                }

                moves1.Push(new Move() { Edge = next1, OpenValve = false });
                moves2.Push(new Move() { Edge = next2, OpenValve = false });

                FindMaxFlowHelper2(totalFlow, stepsRemaining1 - next1.Distance, stepsRemaining2 - next2.Distance, next1, next2, visited1, visited2, moves1, moves2, ref best, runningScore);

                moves1.Pop();
                moves2.Pop();

                visited1.Remove(next1.Valve);
                visited2.Remove(next2.Valve);
            }
        }

        private Dictionary<string, Valve> LoadPuzzle()
        {
            Regex regex = new Regex(@"^Valve (?<name>\w+) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<paths>.+)$");
            Dictionary<string, Valve> valvesByName = new Dictionary<string, Valve>();

            foreach (string line in File.ReadAllLines("Day16.txt"))
            {
                Match match = regex.Match(line);
                valvesByName.Add(
                    match.Groups["name"].Value,
                    new Valve()
                    {
                        Name = match.Groups["name"].Value,
                        FlowRate = int.Parse(match.Groups["rate"].Value),
                        LeadsToString = match.Groups["paths"].Value
                    });
            }

            foreach (Valve valve in valvesByName.Values)
            {
                foreach (string name in valve.LeadsToString.Split(", "))
                {
                    valve.LeadsTo.Add(new Edge() { Valve = valvesByName[name], Distance = 1 });
                }
            }

            return valvesByName;
        }

        private class Valve
        {
            internal string Name;
            internal bool IsOpened;
            internal int FlowRate;
            internal List<Edge> LeadsTo = new List<Edge>();
            internal string LeadsToString;

            internal void Reduce()
            {
                List<Edge> newList = new List<Edge>();
                HashSet<string> set = new HashSet<string>() { Name };

                foreach (Edge e in LeadsTo)
                {
                    foreach (Edge e2 in e.Reduce())
                    {
                        if (set.Add(e2.Valve.Name))
                        {
                            newList.Add(e2);
                        }
                        else if (e2.Valve.Name != Name)
                        {
                            for (int i = 0; i < newList.Count; i++)
                            {
                                if (newList[i].Valve.Name == e2.Valve.Name)
                                {
                                    Edge e3 = newList[i];
                                    e3.Distance = Math.Min(newList[i].Distance, e2.Distance);
                                    newList[i] = e3;
                                    break;
                                }
                            }
                        }
                    }
                }

                LeadsTo = newList;
            }
        }

        private class Destination
        {
            internal Valve Valve;
            internal List<Move> Path;
            internal int Score;
            internal int Distance => Path.Count;

            internal int Travel = 1;
            internal bool IsReduced;
        }

        private struct Move
        {
            internal Edge Edge;
            internal bool OpenValve;

            internal string Print()
            {
                return Edge.Valve.Name + (OpenValve ? "1" : "0");
            }
        }

        private struct Edge
        {
            internal Valve Valve;
            internal int Distance;

            internal IEnumerable<Edge> Reduce()
            {
                if (Valve.FlowRate > 0)
                {
                    yield return this;
                }
                else
                {
                    foreach (Edge e in Valve.LeadsTo)
                    {
                        yield return new Edge() { Valve = e.Valve, Distance = Distance + e.Distance };
                    }
                }
            }
        }
    }
}