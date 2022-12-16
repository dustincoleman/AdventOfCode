namespace AdventOfCode2022
{
    public class Day16
    {
        [Fact]
        public void Part1()
        {
            Dictionary<string, Valve> valvesByName = LoadPuzzle();
            Valve current = valvesByName["AA"];

            int time = 0;
            int total = 0;

            int totalFlow = valvesByName.Values.Select(v => v.FlowRate).Sum();

            while (time < 30)
            {
                total += current.FlowRate * (30 - time);
                totalFlow -= current.FlowRate;

                Destination destination = FindDestination(current, (30 - time), totalFlow);

                if (destination == null)
                {
                    break;
                }

                Destination next = destination;

                for (int i = 0; i < next.Distance; i++)
                {
                    if (next.Path[i].OpenValve)
                    {
                        current = next.Path[i].Valve;
                        current.IsOpened = true;
                        time += i + 2;
                        break;
                    }
                }
            }

            Assert.Equal(1673, total);
        }

        [Fact]
        public void Part2()
        {
            
        }

        private Destination FindDestination(Valve current, int stepsRemaining, int totalFlow)
        {
            Destination destination = null;
            FindDestinationsHelper(totalFlow, stepsRemaining, current, new Stack<Move>(), new HashSet<Valve>(), ref destination);
            return destination;
        }

        private void FindDestinationsHelper(int totalFlow, int stepsRemaining, Valve current, Stack<Move> traveled, HashSet<Valve> visited, ref Destination best, int depth = 0, int runningScore = 0)
        {
            if (stepsRemaining <= 0)
            {
                return;
            }

            if (best != null && runningScore + (totalFlow * stepsRemaining) < best.Score)
            {
                return;
            }    

            foreach (Valve next in current.LeadsTo)
            {
                if (visited.Add(next))
                {
                    if (next.FlowRate > 0 && !next.IsOpened)
                    {
                        next.IsOpened = true;

                        traveled.Push(new Move() { Valve = next, OpenValve = true });

                        int tempScore = runningScore + (next.FlowRate * (stepsRemaining - 2));

                        if (best == null || tempScore > best.Score)
                        {
                            best = new Destination()
                            {
                                Valve = next,
                                Path = new List<Move>(traveled.Reverse()),
                                Score = tempScore
                            };
                        }

                        FindDestinationsHelper(totalFlow - next.FlowRate, stepsRemaining - 2, next, traveled, new HashSet<Valve>(), ref best, depth + 1, tempScore);

                        traveled.Pop();

                        next.IsOpened = false;
                    }

                    traveled.Push(new Move() { Valve = next, OpenValve = false });
                    FindDestinationsHelper(totalFlow, stepsRemaining - 1, next, traveled, visited, ref best, depth, runningScore);
                    traveled.Pop();

                    visited.Remove(next);
                }
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
                    valve.LeadsTo.Add(valvesByName[name]);
                }
            }

            return valvesByName;
        }

        private class Valve
        {
            internal string Name;
            internal bool IsOpened;
            internal int FlowRate;
            internal List<Valve> LeadsTo = new List<Valve>();
            internal string LeadsToString;
        }

        private class Destination
        {
            internal Valve Valve;
            internal List<Move> Path;
            internal int Score;
            internal int Distance => Path.Count;
        }

        private struct Move
        {
            internal Valve Valve;
            internal bool OpenValve;
        }
    }
}