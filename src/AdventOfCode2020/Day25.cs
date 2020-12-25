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
    public class Day25
    {
        private static int[] Input = new[] { 9033205, 9281649 };

        [Fact]
        public void Part1()
        {
            int cardLoopSize = GetLoopCount(Input[0]);
            int doorLoopSize = GetLoopCount(Input[1]);

            Assert.Equal(13467729, cardLoopSize);
            Assert.Equal(3020524, doorLoopSize);

            long encryptionKey1 = 1;

            for (long l = 0; l < cardLoopSize; l++)
            {
                encryptionKey1 *= Input[1];
                encryptionKey1 %= 20201227;
            }

            long encryptionKey2 = 1;

            for (long l = 0; l < doorLoopSize; l++)
            {
                encryptionKey2 *= Input[0];
                encryptionKey2 %= 20201227;
            }

            Assert.True(encryptionKey1 == encryptionKey2);
            Assert.Equal(9714832, encryptionKey1);
        }

        [Fact]
        public void Part2()
        {
            Assert.True(true);
        }

        private static int GetLoopCount(long publicKey)
        {
            long value = 1;
            int loopSize = 0;

            while (value != publicKey)
            {
                value *= 7;
                value %= 20201227;
                loopSize++;
            }

            return loopSize;
        }
    }
}
