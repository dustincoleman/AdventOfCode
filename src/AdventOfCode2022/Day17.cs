using System;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2022
{
    public class Day17
    {
        [Fact]
        public void Part1()
        {
            int result = Puzzle.Load().Run(2022);
            Assert.Equal(3141, result);
        }

        [Fact]
        public void Part2()
        {
            Puzzle puzzle = Puzzle.Load();
            long totalPieces = 1000000000000;

            (int pos, int len)? FindPattern(List<int> list, int minLen)
            {
                for (int len = list.Count / 2; len > minLen; len--)
                {
                    bool found = true;
                    int pos = list.Count - (len * 2);

                    for(int i = 0; i < len; i++)
                    {
                        if (list[pos + i] != list[pos + len + i])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        return (pos, len);
                    }
                }

                return null;
            }

            List<int> list = new List<int>();
            int lastRun = 0;
            int minLen = puzzle.JetPatternLength * puzzle.PieceCount;
            (int pos, int len)? match = null;

            for (int i = 0; i < 1000000; i++)
            {
                int run = puzzle.Run(1);
                list.Add(run - lastRun);
                lastRun = run;

                match = FindPattern(list, minLen);
                if (match != null)
                {
                    break;
                }
            }

            int position = match.Value.pos;
            int length = match.Value.len;

            totalPieces -= position;
            long times = totalPieces / length;
            long remainder = totalPieces % length;

            long result = (long)list.Take(position).Sum() + ((long)list.Skip(position).Take(length).Sum() * times) + puzzle.Run((int)remainder) - lastRun;
            Assert.Equal(1561739130391, result);
        }

        private class Puzzle
        {
            private VirtualGrid2<bool> _grid;
            private IEnumerator<VirtualGrid2<bool>> _pieceEnumerator;
            private IEnumerator<Point2> _moveEnumerator;
            private List<Point2> _jetPattern;
            private VirtualGrid2<bool>[] _pieces;

            internal Puzzle(string jetPattern)
            {
                _grid = new VirtualGrid2<bool>();
                _grid.AddVirtualRow(0, true);
                _grid.AddVirtualColumn(0, true);
                _grid.AddVirtualColumn(8, true);

                _pieceEnumerator = GetPieces().GetEnumerator();
                _moveEnumerator = GetMoves().GetEnumerator();

                _jetPattern = jetPattern.Select(c => c switch
                {
                    '<' => -Point2.UnitX,
                    '>' => Point2.UnitX,
                    _ => throw new Exception()
                })
                .ToList();

                _pieces = new VirtualGrid2<bool>[]
                {
                    BuildPiece1(),
                    BuildPiece2(),
                    BuildPiece3(),
                    BuildPiece4(),
                    BuildPiece5()
                };
            }

            internal static Puzzle Load() => new Puzzle(File.ReadAllText("Day17.txt"));

            internal int JetPatternLength => _jetPattern.Count;

            internal int PieceCount => _pieces.Length;

            internal int Run(int pieceCount)
            {
                while (pieceCount-- > 0)
                {
                    var piece = NextPiece();

                    while (true)
                    {
                        var shifted = piece.Shift(NextMove());
                        if (!shifted.Intersects(_grid))
                        {
                            piece = shifted;
                        }

                        var lowered = piece.Shift(-Point2.UnitY);
                        if (!lowered.Intersects(_grid))
                        {
                            piece = lowered;
                        }
                        else
                        {
                            _grid.Add(piece);
                            break;
                        }
                    }
                }

                return _grid.MaxPoint.Y;
            }

            internal void Skip(int pieceCount)
            {
                while (pieceCount-- >= 0)
                {
                    _ = NextPiece();
                }
            }

            private VirtualGrid2<bool> NextPiece()
            {
                _pieceEnumerator.MoveNext();
                return _pieceEnumerator.Current.Shift((_grid.MaxPoint * Point2.UnitY) + new Point2(3, 4));
            }

            private Point2 NextMove()
            {
                _moveEnumerator.MoveNext();
                return _moveEnumerator.Current;
            }

            private IEnumerable<VirtualGrid2<bool>> GetPieces()
            {
                while (true)
                {
                    foreach (VirtualGrid2<bool> p in _pieces)
                    {
                        yield return p;
                    }
                }
            }

            private IEnumerable<Point2> GetMoves()
            {
                while (true)
                {
                    foreach (Point2 p in _jetPattern)
                    {
                        yield return p;
                    }
                }
            }

            private static VirtualGrid2<bool> BuildPiece1()
            {
                VirtualGrid2<bool> grid = new VirtualGrid2<bool>();
                grid[0, 0] = true;
                grid[1, 0] = true;
                grid[2, 0] = true;
                grid[3, 0] = true;
                return grid;
            }

            private static VirtualGrid2<bool> BuildPiece2()
            {
                VirtualGrid2<bool> grid = new VirtualGrid2<bool>();
                grid[0, 1] = true;
                grid[1, 0] = true;
                grid[1, 1] = true;
                grid[1, 2] = true;
                grid[2, 1] = true;
                return grid;
            }

            private static VirtualGrid2<bool> BuildPiece3()
            {
                VirtualGrid2<bool> grid = new VirtualGrid2<bool>();
                grid[0, 0] = true;
                grid[1, 0] = true;
                grid[2, 0] = true;
                grid[2, 1] = true;
                grid[2, 2] = true;
                return grid;
            }

            private static VirtualGrid2<bool> BuildPiece4()
            {
                VirtualGrid2<bool> grid = new VirtualGrid2<bool>();
                grid[0, 0] = true;
                grid[0, 1] = true;
                grid[0, 2] = true;
                grid[0, 3] = true;
                return grid;
            }

            private static VirtualGrid2<bool> BuildPiece5()
            {
                VirtualGrid2<bool> grid = new VirtualGrid2<bool>();
                grid[0, 0] = true;
                grid[0, 1] = true;
                grid[1, 0] = true;
                grid[1, 1] = true;
                return grid;
            }
        }
    }
}