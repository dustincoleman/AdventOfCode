using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Common
{
    public static class MathHelpers
    {
        public static int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static int LeastCommonMultiple(int a, int b) => a / GreatestCommonDivisor(a, b) * b;
        public static int LeastCommonMultiple(this IEnumerable<int> values) => values.Aggregate(LeastCommonMultiple);
        public static long LeastCommonMultiple(long a, long b) => a / GreatestCommonDivisor(a, b) * b;
        public static long LeastCommonMultiple(this IEnumerable<long> values) => values.Aggregate(LeastCommonMultiple);
        public static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b) => a / BigInteger.GreatestCommonDivisor(a, b) * b;
        public static BigInteger LeastCommonMultiple(this IEnumerable<BigInteger> values) => values.Aggregate(LeastCommonMultiple);
    }
}
