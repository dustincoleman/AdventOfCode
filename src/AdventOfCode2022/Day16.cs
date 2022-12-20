using System;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2022
{
    public class Day16
    {
        [Fact]
        public void Part1()
        {
            Puzzle puzzle = LoadPuzzle(30);
            int score = Solve(puzzle);
            Assert.Equal(1673, score);
        }

        [Fact]
        public void Part2()
        {
            Assert.Equal(1707, 0); // Test data result
        }

        private Dictionary<Puzzle, int> scoresByPuzzle = new Dictionary<Puzzle, int>();

        private int Solve(Puzzle puzzle)
        {
            if (scoresByPuzzle.TryGetValue(puzzle, out int cached))
            {
                return cached;
            }

            int score = 0;

            foreach (Destination destination in puzzle.GetNextMoves())
            {
                if (puzzle.RemainingValves.Contains(destination.Valve))
                {
                    Puzzle openValve = puzzle.Without(destination, openValve: true);
                    int pointsForThisMove = destination.Valve.FlowRate * openValve.RemainingSteps;
                    score = Math.Max(pointsForThisMove + Solve(openValve), score);
                }

                Puzzle justMove = puzzle.Without(destination, openValve: false);
                score = Math.Max(Solve(justMove), score);
            }

            scoresByPuzzle.Add(puzzle, score);

            return score;
        }

        private Puzzle LoadPuzzle(int stepsRemaining)
        {
            Regex regex = new Regex(@"^Valve (?<name>\w+) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<paths>.+)$");
            Dictionary<string, Valve> valvesByName = new Dictionary<string, Valve>();
            Dictionary<Valve, string> leadsToStringByValve = new Dictionary<Valve, string>();

            foreach (string line in File.ReadAllLines("Day16.txt"))
            {
                Match match = regex.Match(line);

                Valve valve = new Valve()
                {
                    Name = match.Groups["name"].Value,
                    FlowRate = int.Parse(match.Groups["rate"].Value)
                };

                valvesByName.Add(valve.Name, valve);
                leadsToStringByValve.Add(valve, match.Groups["paths"].Value);
            }

            HashSet<Valve> visited = new HashSet<Valve>();

            List<Destination> GetDestinations(Valve current)
            {
                List<Destination> list = new List<Destination>();

                visited.Add(current);

                foreach (Valve next in leadsToStringByValve[current].Split(", ").Select(name => valvesByName[name]))
                {
                    if (!visited.Contains(next))
                    {
                        if (next.FlowRate > 0)
                        {
                            list.Add(new Destination() { Valve = next, Distance = visited.Count });
                        }
                        else
                        {
                            list.AddRange(GetDestinations(next));
                        }
                    }
                }

                visited.Remove(current);

                return list;
            }

            foreach (Valve valve in valvesByName.Values)
            {
                valve.LeadsTo = GetDestinations(valve);
            }

            return new Puzzle(valvesByName["AA"], stepsRemaining, valvesByName.Values.Where(v => v.FlowRate > 0));
        }

        private class Puzzle : IEquatable<Puzzle>
        {
            private Valve _position;
            private int _remainingSteps;
            private HashSet<Valve> _remainingValves;
            private Lazy<int> _hashCode;

            public Puzzle(Valve position, int remainingSteps, IEnumerable<Valve> remainingValves)
            {
                _position = position;
                _remainingSteps = remainingSteps;
                _remainingValves = new HashSet<Valve>(remainingValves);
                _hashCode = new Lazy<int>(ComputeHashCode);
            }

            public Valve Position => _position;

            public int RemainingSteps => _remainingSteps;

            public IEnumerable<Valve> RemainingValves => _remainingValves;

            internal IEnumerable<Destination> GetNextMoves()
            {
                foreach (Destination destination in _position.LeadsTo)
                {
                    if (_remainingSteps > destination.Distance)
                    {
                        yield return destination;
                    }
                }
            }

            internal Puzzle Without(Destination destination, bool openValve)
            {
                return (openValve) ?
                    new Puzzle(destination.Valve, _remainingSteps - destination.Distance - 1, _remainingValves.Where(v => v != destination.Valve)) :
                    new Puzzle(destination.Valve, _remainingSteps - destination.Distance, _remainingValves);
            }

            public bool Equals(Puzzle other)
            {
                return
                    _position == other._position &&
                    _remainingSteps == other._remainingSteps &&
                    _remainingValves.SetEquals(other._remainingValves);
            }

            public override bool Equals(object obj)
            {
                return obj is Puzzle && Equals((Puzzle)obj);
            }

            public override int GetHashCode()
            {
                return _hashCode.Value;
            }

            private int ComputeHashCode()
            {
                HashCode hashcode = new HashCode();

                hashcode.Add(_position);
                hashcode.Add(_remainingSteps);

                foreach (Valve v in _remainingValves)
                {
                    hashcode.Add(v);
                }

                return hashcode.ToHashCode();
            }
        }

        private class Valve
        {
            internal string Name;
            internal int FlowRate;
            internal List<Destination> LeadsTo;
        }

        private struct Destination
        {
            internal Valve Valve;
            internal int Distance;
        }
    }
}