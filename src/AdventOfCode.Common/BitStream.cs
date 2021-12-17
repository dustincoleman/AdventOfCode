using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class BitStream
    {
        private readonly byte[] data;
        private int position; // bit index

        public BitStream(byte[] data)
        {
            this.data = data;
        }

        public static BitStream FromHexString(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new Exception("Expected even number of chars");
            }

            List<byte> bytes = new List<byte>();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes.Add(Convert.ToByte(hexString.Substring(i, 2).ToString(), 16));
            }

            return new BitStream(bytes.ToArray());
        }

        public int Position => this.position;

        public bool ReadBit()
        {
            return (Read(1) != 0);
        }

        public int Read(int bitCount)
        {
            if (bitCount > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount));
            }

            int value = 0;
            
            while (bitCount > 0)
            {
                // Find our position in the data array
                int index = (int)(position / 8);
                byte offset = (byte)(position % 8);

                // Compute how many bytes to read
                byte read = (byte)Math.Min(8 - offset, bitCount);

                // Shift our output value to make room for the next bits
                value <<= read;

                // Read the next bits out of the array
                byte mask = CreateMask(offset, read);
                byte bits = (byte)(data[index] & mask);
                bits >>= (byte)(8 - offset - read);

                // Write the bits into our output value
                value |= bits;

                // Update counters
                bitCount -= read;
                this.position += read;
            }

            return value;
        }

        private byte CreateMask(byte offset, byte length)
        {
            byte mask = byte.MaxValue;

            // Shift left to size to length
            mask <<= (8 - length);

            // Shift right into position
            mask >>= offset;

            return mask;
        }
    }
}
