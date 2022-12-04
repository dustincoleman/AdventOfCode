using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class Range
    {
        public readonly int Begin;
        public readonly int End;

        public Range(int begin, int end)
        {
            if (begin > end) throw new ArgumentOutOfRangeException();
            Begin = begin;
            End = end;
        }

        public bool Contains(Range other)
        {
            return (other.Begin >= Begin && other.End <= End);
        }

        public bool Overlaps(Range other)
        {
            return (Math.Min(End, other.End) - Math.Max(Begin, other.Begin) >= 0);
        }
    }
}
