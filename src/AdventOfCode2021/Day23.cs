using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021
{
    public class Day23
    {
        [Fact]
        public void Part1()
        {
            Puzzle puzzle = new Puzzle("CC", "AA", "BD", "DB");

            long result = puzzle.Solve();

            Assert.Equal(11536, result);
        }

        [Fact]
        public void Part2()
        {
            Puzzle puzzle = new Puzzle("CDDC", "ACBA", "BBAD", "DACB");

            long result = puzzle.Solve();

            Assert.Equal(55136, result);
        }

        private enum HallId : int { LL, L, AB, BC, CD, R, RR, Count };

        private enum RoomId : int { A, B, C, D, Count };

        private class Puzzle
        {
            private long bestSolution = long.MaxValue;
            private Stack<Move> moves = new Stack<Move>();

            internal Puzzle(params string[] rooms)
            {
                Hallway = new Hallway();
                Rooms = new Room[(int)RoomId.Count];

                for (int i = 0; i < rooms.Length; i++)
                {
                    Rooms[i] = new Room(rooms[i], (RoomId)i);
                }
            }

            internal Hallway Hallway { get; }

            internal Room[] Rooms { get; }

            internal long Cost { get; private set; }

            internal bool IsSolved => Rooms.All(r => r.IsSolved);

            internal long Solve()
            {
                if (Cost < this.bestSolution)
                {
                    foreach (Move move in GetAvailableMoves())
                    {
                        PushMove(move);

                        if (IsSolved)
                        {
                            this.bestSolution = Math.Min(Cost, this.bestSolution);
                        }
                        else
                        {
                            Solve();
                        }

                        PopMove();
                    }
                }

                return this.bestSolution;
            }

            private IEnumerable<Move> GetAvailableMoves()
            {
                for (RoomId roomId = 0; roomId != RoomId.Count; roomId++)
                {
                    Room room = Rooms[(int)roomId];

                    if (room.CanMoveOut)
                    {
                        Amphipod occupant = room.Peek();

                        RoomId destinationRoomId = occupant.Home;
                        Room destinationRoom = Rooms[(int)destinationRoomId];

                        if (destinationRoom.CanMoveIn && Hallway.IsRoomReachable(roomId, destinationRoomId, out int hallDistance))
                        {
                            yield return new Move()
                            {
                                FromRoom = roomId,
                                ToRoom = destinationRoomId,
                                Cost = (room.MoveOutDistance + destinationRoom.MoveInDistance + hallDistance) * occupant.MoveCost,
                            };
                        }
                        else
                        {
                            foreach ((HallId hallId, int distance) in Hallway.HallsAvailableFrom(roomId))
                            {
                                yield return new Move()
                                {
                                    FromRoom = roomId,
                                    ToHall = hallId,
                                    Cost = (room.MoveOutDistance + distance) * occupant.MoveCost,
                                };
                            }
                        }
                    }
                }

                for (HallId hallId = 0; hallId < HallId.Count; hallId++)
                {
                    Amphipod? occupantNullable = Hallway.Positions[(int)hallId];

                    if (occupantNullable.HasValue)
                    {
                        Amphipod occupant = occupantNullable.Value;
                        RoomId roomId = occupant.Home;
                        Room room = Rooms[(int)roomId];

                        if (room.CanMoveIn && Hallway.IsRoomReachable(hallId, roomId, out int hallDistance))
                        {
                            yield return new Move()
                            {
                                FromHall = hallId,
                                ToRoom = roomId,
                                Cost = (hallDistance + room.MoveInDistance) * occupant.MoveCost,
                            };
                        }
                    }
                }
            }

            private void PushMove(Move move)
            {
                Amphipod amphipod;

                if (move.FromRoom != null)
                {
                    amphipod = Rooms[(int)move.FromRoom.Value].Pop();
                }
                else
                {
                    amphipod = Hallway.Positions[(int)move.FromHall.Value].Value;
                    Hallway.Positions[(int)move.FromHall.Value] = null;
                }

                if (move.ToRoom != null)
                {
                    Rooms[(int)move.ToRoom.Value].Push(amphipod);
                }
                else
                {
                    Hallway.Positions[(int)move.ToHall.Value] = amphipod;
                }

                Cost += move.Cost;

                this.moves.Push(move);
            }

            private void PopMove()
            {
                Move move = this.moves.Pop();

                Amphipod amphipod;

                if (move.ToRoom != null)
                {
                    amphipod = Rooms[(int)move.ToRoom.Value].Pop();
                }
                else
                {
                    amphipod = Hallway.Positions[(int)move.ToHall.Value].Value;
                    Hallway.Positions[(int)move.ToHall.Value] = null;
                }

                if (move.FromRoom != null)
                {
                    Rooms[(int)move.FromRoom.Value].Push(amphipod);
                }
                else
                {
                    Hallway.Positions[(int)move.FromHall.Value] = amphipod;
                }

                Cost -= move.Cost;
            }
        }

        private class Hallway
        {
            internal Amphipod?[] Positions = new Amphipod?[(int)HallId.Count];

            internal IEnumerable<(HallId id, int distance)> HallsAvailableFrom(RoomId roomId)
            {
                int distance = 2;

                for (HallId left = HallId.L + (int)roomId; left >= HallId.LL; left--)
                {
                    if (Positions[(int)left] != null)
                    {
                        break;
                    }

                    yield return (left, distance);

                    distance += (left == HallId.L) ? 1 : 2;
                }

                distance = 2;

                for (HallId right = HallId.AB + (int)roomId; right <= HallId.RR; right++)
                {
                    if (Positions[(int)right] != null)
                    {
                        break;
                    }

                    yield return (right, distance);

                    distance += (right == HallId.R) ? 1 : 2;
                }
            }

            internal bool IsRoomReachable(RoomId begin, RoomId end, out int distance)
            {
                if ((int)begin > (int)end)
                {
                    (begin, end) = (end, begin);
                }

                HallId hallBegin = HallId.AB + (int)begin;
                HallId hallEnd = HallId.L + (int)end;

                distance = 2;

                for (; hallBegin <= hallEnd; hallBegin++, distance += 2)
                {
                    if (Positions[(int)hallBegin] != null)
                    {
                        distance = 0;
                        return false;
                    }
                }

                return true;
            }

            internal bool IsRoomReachable(HallId hallId, RoomId roomId, out int distance)
            {
                HallId left = HallId.L + (int)roomId;
                HallId right = left + 1;

                distance = 2;

                while (hallId < left)
                {
                    hallId++;
                    distance += (hallId == HallId.L) ? 1 : 2;

                    if (Positions[(int)hallId] != null)
                    {
                        distance = 0;
                        return false;
                    }
                }

                while (hallId > right)
                {
                    hallId--;
                    distance += (hallId == HallId.R) ? 1 : 2;

                    if (Positions[(int)hallId] != null)
                    {
                        distance = 0;
                        return false;
                    }
                }

                return true;
            }
        }

        private class Room
        {
            private readonly RoomId roomId;
            private readonly int initialSize;
            private Stack<Amphipod> occupants;
            private bool? canMoveIn;
            private bool? isSolved;

            internal Room(string initial, RoomId roomId)
            {
                this.occupants = new Stack<Amphipod>(
                    initial.Reverse().Select(ch => new Amphipod(ch)));

                this.roomId = roomId;
                this.initialSize = this.occupants.Count;
            }

            private Room(IEnumerable<Amphipod> occupants, RoomId roomId, int initialSize)
            {
                this.occupants = new Stack<Amphipod>(occupants.Reverse());

                this.roomId = roomId;
                this.initialSize = initialSize;
            }

            internal int MoveOutDistance => this.initialSize - this.occupants.Count;

            internal int MoveInDistance => MoveOutDistance - 1;

            internal bool CanMoveIn => this.canMoveIn ?? (this.canMoveIn = this.occupants.Count == 0 || this.occupants.All(a => a.Home == this.roomId)).Value;

            internal bool CanMoveOut => !CanMoveIn;

            internal bool IsSolved => this.isSolved ?? (this.isSolved = this.occupants.Count == this.initialSize && this.occupants.All(a => a.Home == this.roomId)).Value;

            internal Amphipod Peek() => this.occupants.Peek();

            internal void Push(Amphipod amphipod)
            {
                this.canMoveIn = null;
                this.isSolved = null;
                this.occupants.Push(amphipod);
            }

            internal Amphipod Pop()
            {
                this.canMoveIn = null;
                this.isSolved = null;
                return this.occupants.Pop();
            }
        }

        private struct Amphipod
        {
            private static readonly Dictionary<RoomId, int> moveCostByHome = new Dictionary<RoomId, int>()
            {
                {RoomId.A, 1},
                {RoomId.B, 10},
                {RoomId.C, 100},
                {RoomId.D, 1000},
            };

            internal Amphipod(char ch)
            {
                Home = Enum.Parse<RoomId>(ch.ToString());
            }

            internal RoomId Home { get; }

            internal int MoveCost => moveCostByHome[Home];
        }

        private struct Move
        {
            internal RoomId? FromRoom;
            internal HallId? FromHall;
            internal RoomId? ToRoom;
            internal HallId? ToHall;
            internal int Cost;
        }
    }
}
