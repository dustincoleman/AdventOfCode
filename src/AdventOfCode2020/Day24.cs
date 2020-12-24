using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using Xunit;

namespace AdventOfCode2020
{
    public class Day24
    {
        [Fact]
        public void Part1()
        {
            Floor floor = new Floor();

            foreach (string path in File.ReadAllLines("Day24Input.txt"))
            {
                floor.FlipTile(path);
            }

            int result = floor.CountBlackTiles();

            Assert.Equal(473, result);
        }

        [Fact]
        public void Part2()
        {
            Floor floor = new Floor();

            foreach (string path in File.ReadAllLines("Day24Input.txt"))
            {
                floor.FlipTile(path);
            }

            for (int i = 0; i < 100; i++)
            {
                floor.CycleTiles();
                int temp = floor.CountBlackTiles();
            }

            int result = floor.CountBlackTiles();

            Assert.Equal(4070, result);
        }

        class Floor
        {
            Dictionary<Tuple<int, int>, Tile> map;
            Tile referenceTile;

            public Floor()
            {
                map = new Dictionary<Tuple<int, int>, Tile>();
                referenceTile = new Tile(0, 0, map);
            }

            public void FlipTile(string pathToTile)
            {
                Tile t = referenceTile;

                while (!string.IsNullOrEmpty(pathToTile))
                {
                    if (pathToTile.StartsWith("e") || pathToTile.StartsWith("w"))
                    {
                        t = t.GetTile(pathToTile.Substring(0, 1));
                        pathToTile = pathToTile.Substring(1);
                    }
                    else
                    {
                        t = t.GetTile(pathToTile.Substring(0, 2));
                        pathToTile = pathToTile.Substring(2);
                    }
                }

                t.Flip();
            }

            internal int CountBlackTiles()
            {
                return map.Values.Count(t => t.IsBlack);
            }

            internal void CycleTiles()
            {
                foreach (Tile t in map.Values.ToArray())
                {
                    // Create any adjacent white tiles
                    _ = t.GetAdjacentTiles(create: true).ToArray();
                }

                List<Tile> toFlip = new List<Tile>();

                foreach (Tile t in map.Values)
                {
                    // Mark the tiles that need to be flipped
                    int count = t.GetAdjacentTiles(create: false).Where(t => t != null).Count(t => t.IsBlack);

                    if ((t.IsBlack && (count == 0 || count > 2)) || (!t.IsBlack && count == 2))
                    {
                        toFlip.Add(t);
                    }
                }

                foreach (Tile t in toFlip)
                {
                    // Flip them
                    t.Flip();
                }
            }
        }

        class Tile
        {
            bool isBlack;
            int x;
            int y;
            Dictionary<Tuple<int, int>, Tile> map;

            public Tile(int x, int y, Dictionary<Tuple<int, int>, Tile> map)
            {
                this.x = x;
                this.y = y;
                this.map = map;
                this.isBlack = false;

                map.Add(new Tuple<int, int>(x, y), this);
            }

            public bool IsBlack => isBlack;

            public Tile GetTile(string direction, bool create = true)
            {
                Tuple<int, int> requestedTile;

                if (direction == "e")
                {
                    requestedTile = new Tuple<int, int>(x + 1, y);
                }
                else if (direction == "w")
                {
                    requestedTile = new Tuple<int, int>(x - 1, y);
                }
                else if (direction == "se")
                {
                    requestedTile = new Tuple<int, int>(x, y + 1);
                }
                else if (direction == "sw")
                {
                    requestedTile = new Tuple<int, int>(x - 1, y + 1);
                }
                else if (direction == "ne")
                {
                    requestedTile = new Tuple<int, int>(x + 1, y - 1);
                }
                else if (direction == "nw")
                {
                    requestedTile = new Tuple<int, int>(x, y - 1);
                }
                else
                {
                    throw new Exception();
                }

                if (!map.TryGetValue(requestedTile, out Tile t) && create)
                {
                    t = new Tile(requestedTile.Item1, requestedTile.Item2, map);
                }

                return t;
            }

            public void Flip()
            {
                isBlack = !isBlack;
            }

            internal IEnumerable<Tile> GetAdjacentTiles(bool create = false)
            {
                yield return GetTile("e", create);
                yield return GetTile("w", create);
                yield return GetTile("se", create);
                yield return GetTile("sw", create);
                yield return GetTile("ne", create);
                yield return GetTile("nw", create);
            }
        }
    }
}
