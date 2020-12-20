using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    static class Day12
    {
        public static void Part1()
        {
            Ship ship = new Ship();

            foreach (string line in File.ReadAllLines("Day12Input.txt"))
            {
                char command = line[0];
                int arg = int.Parse(line.Substring(1));

                switch(command)
                {
                    case 'N':
                        ship.Y += arg;
                        break;
                    case 'S':
                        ship.Y -= arg;
                        break;
                    case 'E':
                        ship.X += arg;
                        break;
                    case 'W':
                        ship.X -= arg;
                        break;
                    case 'L':
                        ship.Heading += arg;
                        while (ship.Heading >= 360) ship.Heading -= 360;
                        break;
                    case 'R':
                        ship.Heading -= arg;
                        while (ship.Heading < 0) ship.Heading += 360;
                        break;
                    case 'F':
                        switch (ship.Heading)
                        {
                            case 0:
                                ship.X += arg;
                                break;
                            case 90:
                                ship.Y += arg;
                                break;
                            case 180:
                                ship.X -= arg;
                                break;
                            case 270:
                                ship.Y -= arg;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            int result = Math.Abs(ship.X) + Math.Abs(ship.Y);

            Debug.Assert(result == 879);
        }

        public static void Part2()
        {
            Ship ship = new Ship();

            string[] sample =
            {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"
            };

            foreach (string line in File.ReadAllLines("Day12Input.txt"))
            {
                char command = line[0];
                int arg = int.Parse(line.Substring(1));

                switch (command)
                {
                    case 'N':
                        ship.WaypointY += arg;
                        break;
                    case 'S':
                        ship.WaypointY -= arg;
                        break;
                    case 'E':
                        ship.WaypointX += arg;
                        break;
                    case 'W':
                        ship.WaypointX -= arg;
                        break;
                    case 'L':
                        int tempL = ship.WaypointX;
                        switch (arg)
                        {
                            case 90:
                                ship.WaypointX = -ship.WaypointY;
                                ship.WaypointY = tempL;
                                break;
                            case 180:
                                ship.WaypointX = -ship.WaypointX;
                                ship.WaypointY = -ship.WaypointY;
                                break;
                            case 270:
                                ship.WaypointX = ship.WaypointY;
                                ship.WaypointY = -tempL;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case 'R':
                        int tempR = ship.WaypointX;
                        switch (arg)
                        {
                            case 90:
                                ship.WaypointX = ship.WaypointY;
                                ship.WaypointY = -tempR;
                                break;
                            case 180:
                                ship.WaypointX = -ship.WaypointX;
                                ship.WaypointY = -ship.WaypointY;
                                break;
                            case 270:
                                ship.WaypointX = -ship.WaypointY;
                                ship.WaypointY = tempR;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case 'F':
                        ship.X += ship.WaypointX * arg;
                        ship.Y += ship.WaypointY * arg;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            int result = Math.Abs(ship.X) + Math.Abs(ship.Y);

            Debug.Assert(result == 18107);
        }
    }

    class Ship
    {
        public int Heading = 0;
        public int X = 0;
        public int Y = 0;
        public int WaypointX = 10;
        public int WaypointY = 1;
    }
}
