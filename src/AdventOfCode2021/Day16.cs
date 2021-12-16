using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2021
{
    public class Day16
    {
        [Fact]
        public void Part1()
        {
            long result = ReadPacketFromFile().ComputeVersionSum();

            Assert.Equal(934, result);
        }

        [Fact]
        public void Part2()
        {
            long result = ReadPacketFromFile().Evaluate();

            Assert.Equal(912901337844, result);
        }

        private Packet ReadPacketFromFile()
        {
            return new BitsReader(File.ReadAllText("Day16Input.txt")).ReadPacket();
        }

        class Packet
        {
            internal int Version;
            internal int TypeId;
            internal long LiteralValue;

            internal List<Packet> SubPackets = new List<Packet>();

            internal int ComputeVersionSum()
            {
                return Version + SubPackets.Sum(p => p.ComputeVersionSum());
            }

            internal long Evaluate()
            {
                switch (TypeId)
                {
                    case 0:
                        return SubPackets.Sum(p => p.Evaluate());
                    case 1:
                        return SubPackets.Aggregate(1L, (x, y) => x * y.Evaluate());
                    case 2:
                        return SubPackets.Min(p => p.Evaluate());
                    case 3:
                        return SubPackets.Max(p => p.Evaluate());
                    case 4:
                        return LiteralValue;
                    case 5:
                        return (SubPackets[0].Evaluate() > SubPackets[1].Evaluate()) ? 1L : 0L;
                    case 6:
                        return (SubPackets[0].Evaluate() < SubPackets[1].Evaluate()) ? 1L : 0L;
                    case 7:
                        return (SubPackets[0].Evaluate() == SubPackets[1].Evaluate()) ? 1L : 0L;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }

        class BitsReader
        {
            string binaryString;
            int position;

            internal BitsReader(string hexString)
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in hexString)
                {
                    int i = Convert.ToInt32(c.ToString(), 16);
                    string binary = Convert.ToString(i, 2).PadLeft(4, '0');
                    sb.Append(binary);
                }

                binaryString = sb.ToString();
            }

            internal Packet ReadPacket()
            {
                Packet packet = new Packet()
                {
                    Version = ReadInt(3),
                    TypeId = ReadInt(3),
                };

                if (packet.TypeId == 4)
                {
                    packet.LiteralValue = ReadLiteralValue();
                }
                else
                {
                    ReadSubPackets(packet);
                }

                return packet;
            }

            private void ReadSubPackets(Packet packet)
            {
                bool isLengthCount = ReadBit();
                int length = (isLengthCount) ? ReadInt(11) : ReadInt(15);

                if (isLengthCount)
                {
                    while (length-- > 0)
                    {
                        packet.SubPackets.Add(ReadPacket());
                    }
                }
                else
                {
                    int endPosition = position + length;

                    while (position < endPosition)
                    {
                        packet.SubPackets.Add(ReadPacket());
                    }

                    if (position > endPosition)
                    {
                        throw new Exception("Read too far");
                    }
                }
            }

            internal bool ReadBit()
            {
                return (binaryString[position++] == '1');
            }

            internal int ReadInt(int width)
            {
                string str = binaryString.Substring(position, width);
                position += width;
                return Convert.ToInt32(str, 2);
            }

            internal long ReadLiteralValue()
            {
                StringBuilder sb = new StringBuilder();
                bool more = true;

                while (more)
                {
                    more = ReadBit();
                    sb.Append(binaryString.Substring(position, 4));
                    position += 4;
                }

                return Convert.ToInt64(sb.ToString(), 2);
            }
        }
    }
}
