using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<Range> MergeOverlap(IEnumerable<Range> input)
        {
            List<Range> list = input.ToList();

            for (int pos = list.Count - 1; pos >= 0; pos--)
            {
                Range cur = list[pos];
                int match = list.FindIndex(0, count: pos, r => r.Overlaps(cur));

                if (match >= 0)
                {
                    list[match] = list[match].Merge(cur);
                    list.RemoveAt(pos);
                }
            }

            return list;
        }

        public bool Contains(int i)
        {
            return (i >= Begin && i <= End);
        }

        public bool Contains(Range other)
        {
            return (other.Begin >= Begin && other.End <= End);
        }

        public bool Overlaps(Range other)
        {
            return (Math.Max(Begin, other.Begin) <= Math.Min(End, other.End));
        }

        public Range Merge(Range other)
        {
            if (!Overlaps(other)) throw new ArgumentException();
            return new Range(Math.Min(Begin, other.Begin), Math.Max(End, other.End));
        }
    }
}
