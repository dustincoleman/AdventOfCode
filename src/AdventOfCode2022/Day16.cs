namespace AdventOfCode2022
{
    public class Day16
    {
        [Fact]
        public void Part1()
        {
            Dictionary<string, Valve> valvesByName = LoadPuzzle();
            int maxFlow = FindMaxFlow(valvesByName["AA"], 30, valvesByName.Values.Select(v => v.FlowRate).Sum());
            Assert.Equal(1673, maxFlow);
        }

        [Fact]
        public void Part2()
        {
            
        }

        private int FindMaxFlow(Valve start, int stepsRemaining, int totalFlow)
        {
            int maxFlow = 0;
            FindMaxFlowHelper(totalFlow, stepsRemaining, start, new HashSet<Valve>(), ref maxFlow);
            return maxFlow;
        }

        private void FindMaxFlowHelper(int totalFlow, int stepsRemaining, Valve current, HashSet<Valve> visited, ref int best, int runningScore = 0)
        {
            if (stepsRemaining <= 0 || (runningScore + (totalFlow * stepsRemaining) < best))
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

                        int tempScore = runningScore + (next.FlowRate * (stepsRemaining - 2));

                        if (tempScore > best)
                        {
                            best = tempScore;
                        }

                        FindMaxFlowHelper(totalFlow - next.FlowRate, stepsRemaining - 2, next, new HashSet<Valve>(), ref best, tempScore);

                        next.IsOpened = false;
                    }

                    FindMaxFlowHelper(totalFlow, stepsRemaining - 1, next, visited, ref best, runningScore);

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