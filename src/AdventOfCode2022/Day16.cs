using System;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2022
{
    public class Day16
    {
        [Fact]
        public void Part1()
        {
            Puzzle puzzle = LoadPuzzle(30, 0);
            int score = Solve(puzzle);
            Assert.Equal(1673, score);
        }

        [Fact(Skip = "Poor Performance")]
        public void Part2()
        {
            Puzzle puzzle = LoadPuzzle(26, 26);
            int score = Solve(puzzle);
            Assert.Equal(2343, score); // Test data result
        }

        private List<int[]> scoresByRemainingByPuzzle = new List<int[]>(UInt16.MaxValue);
        private List<Dictionary<Puzzle, int>> bigScoresByPuzzleByRemaining = new List<Dictionary<Puzzle, int>>();

        private long filterHits = 0;
        private long cacheHits = 0;
        private long cache2Hits = 0;
        private long cacheMisses = 0;
        private int maxScore = 0;

        private int Solve(Puzzle puzzle, int runningScore = 0)
        {
            if (runningScore + puzzle.RemainingPoints < maxScore)
            {
                filterHits++;
                return 0;
            }

            int key = (int)puzzle.PositionIndex;

            if (key > 0)
            {
                int cached = scoresByRemainingByPuzzle[(int)puzzle.RemainingValves][key];

                if (cached > 0)
                {
                    cacheHits++;
                    return cached - 1;
                }
            }
            else if (bigScoresByPuzzleByRemaining[(int)puzzle.RemainingValves].TryGetValue(puzzle, out int cached2))
            {
                cache2Hits++;
                return cached2;
            }

            cacheMisses++;

            int score = 0;

            foreach (Move move in puzzle.GetNextMoves())
            {
                score = Math.Max(move.Points + Solve(move.Puzzle, runningScore + move.Points), score);
            }

            if (key > 0)
            {
                scoresByRemainingByPuzzle[(int)puzzle.RemainingValves][key] = score + 1;
            }
            else
            {
                bigScoresByPuzzleByRemaining[(int)puzzle.RemainingValves].Add(puzzle, score);
            }

            maxScore = Math.Max(score, maxScore);

            return score;
        }

        private Puzzle LoadPuzzle(int stepsRemaining1, int stepsRemaining2)
        {
            for (int i = 0; i < UInt16.MaxValue; i++)
            {
                scoresByRemainingByPuzzle.Add(new int[UInt16.MaxValue]);
            }

            for (int i = 0; i < UInt16.MaxValue; i++)
            {
                bigScoresByPuzzleByRemaining.Add(new Dictionary<Puzzle, int>());
            }

            Regex regex = new Regex(@"^Valve (?<name>\w+) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<paths>.+)$");
            Dictionary<string, Valve> valvesByName = new Dictionary<string, Valve>();
            Dictionary<Valve, string> leadsToStringByValve = new Dictionary<Valve, string>();

            uint bit = 1U;
            uint valveBits = 0;
            uint id = 0;

            foreach (string line in File.ReadAllLines("Day16.txt"))
            {
                Match match = regex.Match(line);

                Valve valve = new Valve()
                {
                    Name = match.Groups["name"].Value,
                    FlowRate = int.Parse(match.Groups["rate"].Value)
                };

                if (valve.FlowRate > 0)
                {
                    valve.Id = id++;
                    valve.Bit = bit;
                    valveBits |= bit;
                    bit <<= 1;
                }

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

            return new Puzzle(valvesByName["AA"], stepsRemaining1, valvesByName["AA"], stepsRemaining2, valveBits, valvesByName.Values.Select(v => v.FlowRate).Sum());
        }

        private class Puzzle : IEquatable<Puzzle>
        {
            private Valve _position1;
            private int _remainingSteps1;
            private Valve _position2;
            private int _remainingSteps2;
            private uint _remainingValves;
            private int _remainingPoints;
            private int _hashCode;

            private static readonly Destination NoDestination = new Destination();

            public Puzzle(Valve position1, int remainingSteps1, Valve position2, int remainingSteps2, uint remainingValves, int remainingPoints)
            {
                _position1 = position1;
                _remainingSteps1 = remainingSteps1;
                _position2 = position2;
                _remainingSteps2 = remainingSteps2;
                _remainingValves = remainingValves;
                _remainingPoints = remainingPoints;
                _hashCode = HashCode.Combine(_position1, _remainingSteps1, _position2, _remainingSteps2, _remainingValves);
            }

            internal int RemainingPoints => _remainingPoints * Math.Max(_remainingSteps1, _remainingSteps2);

            internal uint RemainingValves => _remainingValves;

            internal uint PositionIndex => (_remainingSteps1 < 16 && _remainingSteps2 < 16) ? (uint)(((uint)_position1.Id << 12) | ((uint)_position2.Id << 8) | ((uint)_remainingSteps1 << 4) | (uint)_remainingSteps2) : 0;

            internal IEnumerable<Move> GetNextMoves()
            {
                int p1Remaining = _remainingSteps1;
                int p2Remaining = _remainingSteps2;

                IEnumerable<Destination> p1Moves = _position1?.LeadsTo.Where(d => p1Remaining > d.Distance).ToArray();
                IEnumerable<Destination> p2Moves = _position2?.LeadsTo.Where(d => p2Remaining > d.Distance).ToArray();

                if ((p1Moves?.Any() ?? false) && (p2Moves?.Any() ?? false))
                {
                    foreach (Destination p1Destination in p1Moves)
                    {
                        foreach (Destination p2Destination in p2Moves)
                        {
                            bool p1CanOpenValve = (_remainingValves & p1Destination.Valve.Bit) != 0;
                            bool p2CanOpenValve = (_remainingValves & p2Destination.Valve.Bit) != 0;

                            if (p1CanOpenValve && p2CanOpenValve && p1Destination.Valve == p2Destination.Valve)
                            {
                                if (_remainingSteps1 > _remainingSteps2)
                                {
                                    p2CanOpenValve = false;
                                }
                                else
                                {
                                    p1CanOpenValve = false;
                                }
                            }

                            if (p1CanOpenValve && p2CanOpenValve)
                            {
                                yield return MakeMove(p1Destination, true, p2Destination, true);
                            }

                            if (p1CanOpenValve)
                            {
                                yield return MakeMove(p1Destination, true, p2Destination, false);
                            }

                            if (p2CanOpenValve)
                            {
                                yield return MakeMove(p1Destination, false, p2Destination, true);
                            }

                            yield return MakeMove(p1Destination, false, p2Destination, false);
                        }
                    }
                }
                else if (p1Moves?.Any() ?? false)
                {
                    foreach (Destination p1Destination in p1Moves)
                    {
                        if ((_remainingValves & p1Destination.Valve.Bit) != 0)
                        {
                            yield return MakeMove(p1Destination, true, NoDestination, false);
                        }

                        yield return MakeMove(p1Destination, false, NoDestination, false);
                    }
                }
                else if (p2Moves?.Any() ?? false)
                {
                    foreach (Destination p2Destination in p2Moves)
                    {
                        if ((_remainingValves & p2Destination.Valve.Bit) != 0)
                        {
                            yield return MakeMove(NoDestination, false, p2Destination, true);
                        }

                        yield return MakeMove(NoDestination, false, p2Destination, false);
                    }
                }
            }

            private Move MakeMove(Destination p1Destination, bool p1OpenValve, Destination p2Destination, bool p2OpenValve)
            {
                int p1NewRemaining = _remainingSteps1 - p1Destination.Distance;
                int p2NewRemaining = _remainingSteps2 - p2Destination.Distance;
                uint newRemaining = _remainingValves;
                int newRemainingPoints = _remainingPoints;

                int points = 0;

                if (p1OpenValve)
                {
                    newRemainingPoints -= p1Destination.Valve.FlowRate;
                    newRemaining &= ~p1Destination.Valve.Bit;
                    points += p1Destination.Valve.FlowRate * --p1NewRemaining;
                }

                if (p2OpenValve)
                {
                    newRemainingPoints -= p2Destination.Valve.FlowRate;
                    newRemaining &= ~p2Destination.Valve.Bit;
                    points += p2Destination.Valve.FlowRate * --p2NewRemaining;
                }

                Puzzle withMove = new Puzzle(
                    p1Destination.Valve ?? _position1, 
                    p1NewRemaining,
                    p2Destination.Valve ?? _position2, 
                    p2NewRemaining,
                    newRemaining,
                    newRemainingPoints);

                return new Move()
                {
                    Puzzle = withMove,
                    Points = points
                };
            }

            public bool Equals(Puzzle other)
            {
                return
                    _position1 == other._position1 &&
                    _remainingSteps1 == other._remainingSteps1 &&
                    _position2 == other._position2 &&
                    _remainingSteps2 == other._remainingSteps2 &&
                    _remainingValves == other._remainingValves;
            }

            public override bool Equals(object obj)
            {
                return obj is Puzzle && Equals((Puzzle)obj);
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }
        }

        private class Valve
        {
            internal string Name;
            internal int FlowRate;
            internal List<Destination> LeadsTo;
            internal uint Bit;
            internal uint Id;
        }

        private struct Destination
        {
            internal Valve Valve;
            internal int Distance;
        }

        private struct Move
        {
            internal Puzzle Puzzle;
            internal int Points;
        }
    }
}