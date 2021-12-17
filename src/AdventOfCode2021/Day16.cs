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
            private BitStream bitStream;

            internal BitsReader(string hexString)
            {
                this.bitStream = BitStream.FromHexString(hexString);
            }

            internal Packet ReadPacket()
            {
                Packet packet = new Packet()
                {
                    Version = this.bitStream.Read(3),
                    TypeId = this.bitStream.Read(3),
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
                bool isLengthCount = this.bitStream.ReadBit();
                int length = (isLengthCount) ? this.bitStream.Read(11) : this.bitStream.Read(15);

                if (isLengthCount)
                {
                    while (length-- > 0)
                    {
                        packet.SubPackets.Add(ReadPacket());
                    }
                }
                else
                {
                    int endPosition = this.bitStream.Position + length;

                    while (this.bitStream.Position < endPosition)
                    {
                        packet.SubPackets.Add(ReadPacket());
                    }

                    if (this.bitStream.Position > endPosition)
                    {
                        throw new Exception("Read too far");
                    }
                }
            }

            internal long ReadLiteralValue()
            {
                bool more = true;
                long value = 0;

                while (more)
                {
                    more = this.bitStream.ReadBit();
                    value <<= 4;
                    value |= (long)this.bitStream.Read(4);
                }

                return value;
            }
        }
    }
}
