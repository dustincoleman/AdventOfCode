using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day17
    {
        public static void Part1()
        {
            const int size = 24;
            bool[,,] pocket = new bool[size, size, size];

            string[] input = File.ReadAllLines("Day17Input.txt");
            Debug.Assert(input.Length == 8);
            Debug.Assert(input[0].Length == 8);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Debug.Assert(input[x][y] == '.' || input[x][y] == '#');
                    pocket[12, x + 8, y + 8] = (input[x][y] == '#');
                }
            }


            for (int i = 0; i < 6; i++)
            {
                int temp = CountActiveCells(pocket, size);
                bool[,,] nextPocket = new bool[size, size, size];

                for (int x = 1; x < size - 1; x++)
                {
                    for (int y = 1; y < size - 1; y++)
                    {
                        for (int z = 1; z < size - 1; z++)
                        {
                            bool nextState = pocket[x, y, z];
                            int active = CountActiveNeighbors(pocket, x, y, z);

                            if ((nextState && !(active == 2 || active == 3)) || (!nextState && active == 3))
                            {
                                nextState = !nextState;
                            }

                            nextPocket[x, y, z] = nextState;
                        }
                    }
                }

                pocket = nextPocket;
            }

            int result = CountActiveCells(pocket, size);
            Debug.Assert(result == 322);
        }

        public static void Part2()
        {
            const int size = 24;
            bool[,,,] pocket = new bool[size, size, size, size];

            string[] input = File.ReadAllLines("Day17Input.txt");
            Debug.Assert(input.Length == 8);
            Debug.Assert(input[0].Length == 8);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Debug.Assert(input[x][y] == '.' || input[x][y] == '#');
                    pocket[12, 12, x + 8, y + 8] = (input[x][y] == '#');
                }
            }


            for (int i = 0; i < 6; i++)
            {
                bool[,,,] nextPocket = new bool[size, size, size, size];

                for (int x = 1; x < size - 1; x++)
                {
                    for (int y = 1; y < size - 1; y++)
                    {
                        for (int z = 1; z < size - 1; z++)
                        {
                            for (int w = 1; w < size - 1; w++)
                            {
                                bool nextState = pocket[x, y, z, w];
                                int active = CountActiveNeighbors(pocket, x, y, z, w);

                                if ((nextState && !(active == 2 || active == 3)) || (!nextState && active == 3))
                                {
                                    nextState = !nextState;
                                }

                                nextPocket[x, y, z, w] = nextState;
                            }
                        }
                    }
                }

                pocket = nextPocket;
            }

            int result = CountActiveCells(pocket, size);
            Debug.Assert(result == 2000);
        }

        private static int CountActiveCells(bool[,,] pocket, int size)
        {
            int result = 0;

            for (int x = 1; x < size - 1; x++)
            {
                for (int y = 1; y < size - 1; y++)
                {
                    for (int z = 1; z < size - 1; z++)
                    {
                        if (pocket[x, y, z])
                        {
                            result++;
                        }
                    }
                }
            }

            return result;
        }

        private static int CountActiveNeighbors(bool[,,] pocket, int x, int y, int z)
        {
            int result = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        if ((i != 0 || j != 0 || k != 0) && pocket[x+i,y+j,z+k])
                        {
                            result++;
                        }
                    }
                }
            }

            return result;
        }

        private static int CountActiveCells(bool[,,,] pocket, int size)
        {
            int result = 0;

            for (int x = 1; x < size - 1; x++)
            {
                for (int y = 1; y < size - 1; y++)
                {
                    for (int z = 1; z < size - 1; z++)
                    {
                        for (int w = 1; w < size - 1; w++)
                        {
                            if (pocket[x, y, z, w])
                            {
                                result++;
                            }
                        } 
                    }
                }
            }

            return result;
        }

        private static int CountActiveNeighbors(bool[,,,] pocket, int x, int y, int z, int w)
        {
            int result = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if ((i != 0 || j != 0 || k != 0 || l != 0) && pocket[x + i, y + j, z + k, w + l])
                            {
                                result++;
                            }
                        }   
                    }
                }
            }

            return result;
        }
    }
}
