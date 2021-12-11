using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using Xunit;

namespace AdventOfCode2020
{
    public class Day20
    {
        [Fact]
        public void Part1()
        {
            string[] lines = File.ReadAllLines("Day20Input.txt").ToArray();
            List<Tile> tiles = new List<Tile>();
            Dictionary<int, List<Tile>> tilesBySide = new Dictionary<int, List<Tile>>();

            void Add(Tile t, int i)
            {
                if (!tilesBySide.TryGetValue(i, out List<Tile> list))
                {
                    list = new List<Tile>();
                    tilesBySide.Add(i, list);
                }

                list.Add(t);
            }

            int pos = 0;
            while (pos < lines.Length)
            {
                Tile tile = new Tile(lines.Skip(pos).Take(11).ToArray());

                Add(tile, tile.Side1A);
                Add(tile, tile.Side1B);
                Add(tile, tile.Side2A);
                Add(tile, tile.Side2B);
                Add(tile, tile.Side3A);
                Add(tile, tile.Side3B);
                Add(tile, tile.Side4A);
                Add(tile, tile.Side4B);

                tiles.Add(tile);
                pos += 12;
            }

            bool NoMatches(int a, int b)
            {
                return (tilesBySide[a].Count == 1 && tilesBySide[b].Count == 1);
            }

            List<Tile> edgeCandidates = new List<Tile>();

            foreach (Tile t in tiles)
            {
                if (NoMatches(t.Side1A, t.Side1B) || NoMatches(t.Side2A, t.Side2B) || NoMatches(t.Side3A, t.Side3B) || NoMatches(t.Side4A, t.Side4B))
                {
                    edgeCandidates.Add(t);
                }
            }

            List<Tile> cornerCandiates = new List<Tile>();

            foreach (Tile t in edgeCandidates)
            {
                int count = 0;

                if (NoMatches(t.Side1A, t.Side1B)) count++;
                if (NoMatches(t.Side2A, t.Side2B)) count++;
                if (NoMatches(t.Side3A, t.Side3B)) count++;
                if (NoMatches(t.Side4A, t.Side4B)) count++;

                if (count > 1)
                {
                    cornerCandiates.Add(t);
                }
            }

            long result = 1;

            foreach (Tile t in cornerCandiates)
            {
                result *= t.Id;
            }

            Assert.Equal(20913499394191, result);
        }

        [Fact]
        public void Part2()
        {
            string[] lines = File.ReadAllLines("Day20Input.txt").ToArray();
            List<Tile> tiles = new List<Tile>();
            Dictionary<int, List<Tile>> tilesBySide = new Dictionary<int, List<Tile>>();

            void Add(Tile t, int i)
            {
                if (!tilesBySide.TryGetValue(i, out List<Tile> list))
                {
                    list = new List<Tile>();
                    tilesBySide.Add(i, list);
                }

                list.Add(t);
            }

            int pos = 0;
            while (pos < lines.Length)
            {
                Tile tile = new Tile(lines.Skip(pos).Take(11).ToArray());

                Add(tile, tile.Side1A);
                Add(tile, tile.Side1B);
                Add(tile, tile.Side2A);
                Add(tile, tile.Side2B);
                Add(tile, tile.Side3A);
                Add(tile, tile.Side3B);
                Add(tile, tile.Side4A);
                Add(tile, tile.Side4B);

                tiles.Add(tile);
                pos += 12;
            }

            long initialCount = 0;

            foreach (Tile t in tiles)
            {
                for (int x = 1; x < 9; x++)
                {
                    for (int y = 1; y < 9; y++)
                    {
                        if (t.Image[x, y])
                        {
                            initialCount++;
                        }
                    }
                }
            }

            bool NoMatches(int a, int b)
            {
                return (tilesBySide[a].Count == 1 && tilesBySide[b].Count == 1);
            }

            List<Tile> edgePieces = new List<Tile>();

            foreach (Tile t in tiles)
            {
                if (NoMatches(t.Side1A, t.Side1B) || NoMatches(t.Side2A, t.Side2B) || NoMatches(t.Side3A, t.Side3B) || NoMatches(t.Side4A, t.Side4B))
                {
                    edgePieces.Add(t);
                }
            }

            List<Tile> cornerPieces = new List<Tile>();

            foreach (Tile t in edgePieces)
            {
                int count = 0;

                if (NoMatches(t.Side1A, t.Side1B)) count++;
                if (NoMatches(t.Side2A, t.Side2B)) count++;
                if (NoMatches(t.Side3A, t.Side3B)) count++;
                if (NoMatches(t.Side4A, t.Side4B)) count++;

                if (count > 1)
                {
                    cornerPieces.Add(t);
                }
            }

            foreach (Tile t in cornerPieces)
            {
                edgePieces.Remove(t);
            }

            Tile GetMatch(Tile t, int side)
            {
                return tilesBySide[side].Where(t2 => t2 != t).SingleOrDefault();
            }

            foreach (Tile t in tiles)
            {
                t.Top = GetMatch(t, t.Side1A);
                if (GetMatch(t, t.Side1B) != t.Top)
                    throw new Exception();

                t.Left = GetMatch(t, t.Side2A);
                if (GetMatch(t, t.Side2B) != t.Left)
                    throw new Exception();

                t.Bottom = GetMatch(t, t.Side3A);
                if (GetMatch(t, t.Side3B) != t.Bottom)
                    throw new Exception();

                t.Right = GetMatch(t, t.Side4A);
                if (GetMatch(t, t.Side4B) != t.Right)
                    throw new Exception();
            }


            Tile current = cornerPieces[0];
            Tile previous = null;

            // Treat the first corner as top left
            if (current.Right == null)
            {
                current.FlipLR();
            }
            if (current.Bottom == null)
            {
                current.FlipTB();
            }

            Tile currentRow = current;
            Tile nextRow = current.Bottom;

            // Orient all edges in the top row
            while (true)
            {
                previous = current;
                current = current.Right;

                if (current.IsCorner)
                {
                    break;
                }

                if (current == null)
                {
                    throw new Exception();
                }

                if (current.Top != null)
                {
                    current.Rotate();
                }
                if (current.Top != null)
                {
                    current.Rotate();
                }
                if (current.Top != null)
                {
                    current.Rotate();
                }
                if (current.Top != null)
                {
                    throw new Exception();
                }

                if (current.Left != previous)
                {
                    current.FlipLR();
                }
                if (current.Left != previous)
                {
                    throw new Exception();
                }
            }

            // Orient the top right tile
            if (current.Left != previous)
            {
                current.Rotate();
            }
            if (current.Left != previous)
            {
                current.Rotate();
            }
            if (current.Left != previous)
            {
                current.Rotate();
            }
            if (current.Left != previous)
            {
                throw new Exception();
            }

            if (current.Top != null)
            {
                current.FlipTB();
            }
            if (current.Top != null)
            {
                throw new Exception();
            }
            if (current.Right != null)
            {
                throw new Exception();
            }

            for (int i = 0; i < 10; i++)
            {
                // Orient the left piece of the next row
                if (nextRow.Left != null)
                {
                    nextRow.Rotate();
                }
                if (nextRow.Left != null)
                {
                    nextRow.Rotate();
                }
                if (nextRow.Left != null)
                {
                    nextRow.Rotate();
                }
                if (nextRow.Left != null)
                {
                    throw new Exception();
                }

                if (nextRow.Top != currentRow)
                {
                    nextRow.FlipTB();
                }
                if (nextRow.Top != currentRow)
                {
                    throw new Exception();
                }

                current = nextRow;
                currentRow = nextRow;
                nextRow = currentRow.Bottom;

                // Orient all middle pieces in the current row
                while (true)
                {
                    previous = current;
                    current = current.Right;

                    if (current.IsEdge)
                    {
                        break;
                    }

                    if (current == null)
                    {
                        throw new Exception();
                    }

                    if (current.Left != previous)
                    {
                        current.Rotate();
                    }
                    if (current.Left != previous)
                    {
                        current.Rotate();
                    }
                    if (current.Left != previous)
                    {
                        current.Rotate();
                    }
                    if (current.Left != previous)
                    {
                        throw new Exception();
                    }

                    Tile above = previous.Top.Right;

                    if (current.Top != above)
                    {
                        current.FlipTB();
                    }
                    if (current.Top != above)
                    {
                        throw new Exception();
                    }
                }

                // Orient the right tile in the current row
                if (current.Left != previous)
                {
                    current.Rotate();
                }
                if (current.Left != previous)
                {
                    current.Rotate();
                }
                if (current.Left != previous)
                {
                    current.Rotate();
                }
                if (current.Left != previous)
                {
                    throw new Exception();
                }

                Tile above2 = previous.Top.Right;

                if (current.Top != above2)
                {
                    current.FlipTB();
                }
                if (current.Top != above2)
                {
                    throw new Exception();
                }
                if (current.Right != null)
                {
                    throw new Exception();
                }
            }

            // Confirm we are at the bottom left tile
            if (!nextRow.IsCorner)
            {
                throw new Exception();
            }

            // Orient to bottom left tile
            if (nextRow.Top != currentRow)
            {
                nextRow.Rotate();
            }
            if (nextRow.Top != currentRow)
            {
                nextRow.Rotate();
            }
            if (nextRow.Top != currentRow)
            {
                nextRow.Rotate();
            }
            if (nextRow.Top != currentRow)
            {
                throw new Exception();
            }

            if (nextRow.Right == null)
            {
                nextRow.FlipLR();
            }
            if (nextRow.Left != null)
            {
                throw new Exception();
            }
            if (nextRow.Bottom != null)
            {
                throw new Exception();
            }

            current = nextRow;

            // Orient the edge pieces in the bottom row
            while (true)
            {
                previous = current;
                current = current.Right;

                if (current.IsCorner)
                {
                    break;
                }

                if (current == null)
                {
                    throw new Exception();
                }

                if (current.Bottom != null)
                {
                    current.Rotate();
                }
                if (current.Bottom != null)
                {
                    current.Rotate();
                }
                if (current.Bottom != null)
                {
                    current.Rotate();
                }
                if (current.Bottom != null)
                {
                    throw new Exception();
                }

                if (current.Left != previous)
                {
                    current.FlipLR();
                }
                if (current.Left != previous)
                {
                    throw new Exception();
                }
            }

            // Orient the bottom right tile
            // Orient the top right tile
            if (current.Left != previous)
            {
                current.Rotate();
            }
            if (current.Left != previous)
            {
                current.Rotate();
            }
            if (current.Left != previous)
            {
                current.Rotate();
            }
            if (current.Left != previous)
            {
                throw new Exception();
            }

            if (current.Bottom != null)
            {
                current.FlipTB();
            }
            if (current.Bottom != null)
            {
                throw new Exception();
            }
            if (current.Right != null)
            {
                throw new Exception();
            }


            // Create an array of the tiles
            Tile[,] tileArray = new Tile[12, 12];

            currentRow = cornerPieces[0];

            for (int i = 0; i < 12; i++)
            {
                if (currentRow == null)
                {
                    throw new Exception();
                }

                current = currentRow;

                if (!current.IsCorner && !current.IsEdge)
                {
                    throw new Exception();
                }

                for (int j = 0; j < 12; j++)
                {
                    if (current == null)
                    {
                        throw new Exception();
                    }

                    tileArray[i, j] = current;
                    current = current.Right;
                }

                currentRow = currentRow.Bottom;
            }

            // Validate the tile array
            for (int i = 1; i < 12; i++)
            {
                for (int j = 1; j < 12; j++)
                {
                    current = tileArray[i, j];

                    if (current.Left != current.Top.Left.Bottom)
                    {
                        throw new Exception();
                    }
                    if (current.Left.Top != current.Top.Left)
                    {
                        throw new Exception();
                    }
                    if (current.Left.Top.Right != current.Top)
                    {
                        throw new Exception();
                    }
                }
            }

            // Validate the images in the tile array
            for (int i = 1; i < 12; i++)
            {
                for (int j = 1; j < 12; j++)
                {
                    current = tileArray[i, j];

                    if (current.GetLeft() != current.Left.GetRight())
                    {
                        throw new Exception();
                    }
                    if (current.GetTop() != current.Top.GetBottom())
                    {
                        throw new Exception();
                    }
                }
            }

            const int size = 12 * 8;

            // Create the image
            bool[,] image = new bool[size, size];

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Tile tile = tileArray[i, j];

                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            image[i * 8 + x, j * 8 + y] = tile.Image[x + 1, y + 1];
                        }
                    }
                }
            }

            image.FlipTB();
            image.Rotate();

            int linePos = 0;
            int monsterCount = 0;

            // Find the sea monsters
            for (int i = 0; i < size - 2; i++)
            {
                bool found = false;
                int nextPos = linePos;
                linePos = 0;

                bool monsterFound = false;

            myGoto:
                int[] sections = new int[3];

                // Find section 1
                for (int j = nextPos; j < size - 14; j++)
                {
                    if (image[i + 1, j] && image[i + 2, j + 1])
                    {
                        found = true;
                        sections[0] = j;
                        nextPos = j + 4;
                        break;
                    }
                }

                if (!found)
                {
                    continue;
                }

                found = false;

                // Find section 2
                for (int j = nextPos; j < size - 11; j++)
                {
                    if (image[i + 2, j] && image[i + 1, j + 1] && image[i + 1, j + 2] && image[i + 2, j + 3])
                    {
                        found = true;
                        sections[1] = j;
                        nextPos = j + 6;
                        break;
                    }
                }

                if (!found)
                {
                    continue;
                }

                found = false;

                // Find section 3
                for (int j = nextPos; j < size - 7; j++)
                {
                    if (image[i + 2, j] && image[i + 1, j + 1] && image[i + 1, j + 2] && image[i + 2, j + 3])
                    {
                        found = true;
                        sections[2] = j;
                        nextPos = j + 6;
                        break;
                    }
                }

                if (!found)
                {
                    continue;
                }

                // Find section 4
                for (int j = nextPos; j < size - 3; j++)
                {
                    if (image[i + 2, j] && image[i + 1, j + 1] && image[i + 1, j + 2] && image[i + 1, j + 3] && image[i, j + 2])
                    {
                        monsterCount++;

                        //int k = sections[0];
                        //image[i + 1, k] = false;
                        //image[i + 2, k + 1] = false;

                        //k = sections[1];
                        //image[i + 2, k] = false;
                        //image[i + 1, k + 1] = false;
                        //image[i + 1, k + 2] = false;
                        //image[i + 2, k + 3] = false;

                        //k = sections[2];
                        //image[i + 2, k] = false;
                        //image[i + 1, k + 1] = false;
                        //image[i + 1, k + 2] = false;
                        //image[i + 2, k + 3] = false;

                        //image[i + 2, j] = false;
                        //image[i + 1, j + 1] = false;
                        //image[i + 1, j + 2] = false;
                        //image[i + 1, j + 3] = false;
                        //image[i, j + 2] = false;

                        monsterFound = true;
                        nextPos = j + 4;
                        goto myGoto;
                    }
                }
                if (monsterFound)
                {
                    i += 2;
                }
                monsterFound = false;
            }

            long result = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (image[i, j])
                    {
                        result++;
                    }
                }
            }

            result -= (monsterCount * 15);

            Assert.Equal(2209, result);
        }

        class Tile
        {
            public static readonly Regex labelRegex = new Regex(@"^Tile (?<id>[0-9]+):$");

            public long Id;
            public MatrixNN<bool> Image;

            public int Side1A;
            public int Side1B;
            public int Side2A;
            public int Side2B;
            public int Side3A;
            public int Side3B;
            public int Side4A;
            public int Side4B;

            public Tile Left;
            public Tile Top;
            public Tile Right;
            public Tile Bottom;

            public void Test()
            {
                bool[,] copy = new bool[10, 10];

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        copy[i, j] = Image[i, j];
                    }
                }

                FlipLR();
                FlipLR();
                FlipTB();
                FlipTB();
                Rotate();
                Rotate();
                Rotate();
                Rotate();

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Debug.Assert(copy[i, j] == Image[i, j]);
                    }
                }

                Debugger.Break();
            }

            public bool IsEdge
            {
                get
                {
                    int count = 0;

                    if (Left == null) count++;
                    if (Right == null) count++;
                    if (Top == null) count++;
                    if (Bottom == null) count++;

                    if (count > 2) throw new Exception();

                    return (count == 1);
                }
            }

            public bool IsCorner
            {
                get
                {
                    int count = 0;

                    if (Left == null) count++;
                    if (Right == null) count++;
                    if (Top == null) count++;
                    if (Bottom == null) count++;

                    if (count > 2) throw new Exception();

                    return (count == 2);
                }
            }

            public void FlipLR()
            {
                Tile temp = Left;
                Left = Right;
                Right = temp;
                Image.FlipVertically();
            }

            public void FlipTB()
            {
                Tile temp = Top;
                Top = Bottom;
                Bottom = temp;
                Image.FlipHorizontally();
            }

            public void Rotate()
            {
                Tile temp = Left;
                Left = Bottom;
                Bottom = Right;
                Right = Top;
                Top = temp;
                Image.Rotate();
            }

            public int GetLeft()
            {
                int side = 0;

                for (int i = 0; i < 10; i++)
                {
                    side |= (Image[i, 0] ? 1 : 0) << i;
                }

                return side;
            }

            public int GetTop()
            {
                int side = 0;

                for (int i = 0; i < 10; i++)
                {
                    side |= (Image[0, i] ? 1 : 0) << i;
                }

                return side;
            }

            public int GetRight()
            {
                int side = 0;

                for (int i = 0; i < 10; i++)
                {
                    side |= (Image[i, 9] ? 1 : 0) << i;
                }

                return side;
            }

            public int GetBottom()
            {
                int side = 0;

                for (int i = 0; i < 10; i++)
                {
                    side |= (Image[9, i] ? 1 : 0) << i;
                }

                return side;
            }

            public Tile(string[] rawInput)
            {
                if (rawInput.Length != 11)
                {
                    throw new ArgumentException();
                }

                Id = long.Parse(labelRegex.Match(rawInput[0]).Groups["id"].Value);
                Image = new MatrixNN<bool>(10);

                for (int i = 0; i < 10; i++)
                {
                    string line = rawInput[i + 1];

                    if (line.Length != 10)
                    {
                        throw new ArgumentException();
                    }

                    for (int j = 0; j < 10; j++)
                    {
                        if (line[j] != '.' && line[j] != '#')
                        {
                            throw new ArgumentException();
                        }

                        Image[i, j] = (line[j] == '#');
                    }
                }

                for (int i = 0; i < 10; i++)
                {
                    Side1A |= (Image[0, i] ? 1 : 0) << i;
                    Side1B |= (Image[0, i] ? 1 : 0) << (9 - i);
                    Side2A |= (Image[i, 0] ? 1 : 0) << i;
                    Side2B |= (Image[i, 0] ? 1 : 0) << (9 - i);
                    Side3A |= (Image[9, i] ? 1 : 0) << i;
                    Side3B |= (Image[9, i] ? 1 : 0) << (9 - i);
                    Side4A |= (Image[i, 9] ? 1 : 0) << i;
                    Side4B |= (Image[i, 9] ? 1 : 0) << (9 - i);
                }
            }

            public void Print()
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.Write(Image[i, j] ? "# " : ". ");
                    }

                    Console.WriteLine();
                }
            }
        }
    }

    public class MatrixNN<T>
    {
        private int _n;
        private T[,] _array;

        public MatrixNN(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

            _n = n;
            _array = new T[n, n];
        }

        public T this[int x, int y]
        {
            get => _array[x, y];
            set => _array[x, y] = value;
        }

        public void FlipVertically()
        {
            int halfSize = _n / 2;
            int maxIndex = _n - 1;

            for (int x = 0; x < _n; x++)
            {
                for (int y = 0; y < halfSize; y++)
                {
                    T tempBool = _array[x, y];
                    _array[x, y] = _array[x, maxIndex - y];
                    _array[x, maxIndex - y] = tempBool;
                }
            }
        }

        public void FlipHorizontally()
        {
            int halfSize = _n / 2;
            int maxIndex = _n - 1;

            for (int y = 0; y < _n; y++)
            {
                for (int x = 0; x < halfSize; x++)
                {
                    T tempBool = _array[x, y];
                    _array[x, y] = _array[maxIndex - x, y];
                    _array[maxIndex - x, y] = tempBool;
                }
            }
        }

        public void Rotate()
        {
            int halfSize = _n / 2;
            int maxIndex = _n - 1;

            for (int x = 0; x < halfSize; x++)
            {
                for (int y = x; y < maxIndex - x; y++)
                {
                    // Store current cell
                    T tempBool = _array[x, y];

                    // Move left to top
                    _array[x, y] = _array[maxIndex - y, x];

                    // Move right to left
                    _array[maxIndex - y, x] = _array[maxIndex - x, maxIndex - y];

                    // Move top to right
                    _array[maxIndex - x, maxIndex - y] = _array[y, maxIndex - x];

                    // Assign temp to right
                    _array[y, maxIndex - x] = tempBool;
                }
            }
        }
    }
}
