using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021
{
    public class Day19
    {
        [Fact]
        public void Part1()
        {
            SnailfishNumber number = ReadNumbersFromFile().Aggregate((left, right) => SnailfishNumber.Add(left, right));

            int result = number.ComputeMagnitude();

            Assert.Equal(3216, result);
        }

        [Fact]
        public void Part2()
        {
            int maxMagnitude = 0;
            List<SnailfishNumber> numbers = ReadNumbersFromFile();

            foreach (SnailfishNumber n1 in numbers)
            {
                foreach (SnailfishNumber n2 in numbers)
                {
                    if (n1 != n2)
                    {
                        int magnitude = SnailfishNumber.Add(n1.Clone(), n2.Clone()).ComputeMagnitude();
                        maxMagnitude = Math.Max(maxMagnitude, magnitude);
                    }
                }
            }

            Assert.Equal(4643, maxMagnitude);
        }

        private List<SnailfishNumber> ReadNumbersFromFile()
        {
            return File.ReadAllLines("Day18Input.txt")
                       .Select(line => SnailfishNumber.Parse(line))
                       .ToList();

        }

        private class SnailfishNumber
        {
            public int? Value { get; set; }
            public SnailfishNumber Left { get; set; }
            public SnailfishNumber Right { get; set; }
            public SnailfishNumber Parent { get; set; }
            public override string ToString()
            {
                if (Value != null)
                {
                    return Value.Value.ToString();
                }

                return $"[{Left.ToString()},{Right.ToString()}]";
            }

            internal static SnailfishNumber Parse(string input, SnailfishNumber parent = null)
            {
                if (input.StartsWith('['))
                {
                    (string left, string right) = GetComponents(input.Substring(1, input.Length - 2));

                    SnailfishNumber number = new SnailfishNumber();

                    number.Left = Parse(left, number);
                    number.Right = Parse(right, number);
                    number.Parent = parent;

                    return number;
                }

                return new SnailfishNumber() 
                { 
                    Value = int.Parse(input),
                    Parent = parent,
                };
            }

            internal static SnailfishNumber Add(SnailfishNumber left, SnailfishNumber right)
            {
                if (left.Parent != null || right.Parent != null)
                {
                    throw new Exception();
                }

                SnailfishNumber result = new SnailfishNumber();

                result.Left = left;
                result.Right = right;
                left.Parent = result;
                right.Parent = result;

                result.Reduce();

                return result;
            }

            internal void Reduce()
            {
                while (true)
                {
                    if (!Explode() && !Split())
                    {
                        return;
                    }
                }

                throw new Exception();
            }

            internal int ComputeMagnitude()
            {
                if (Value == null)
                {
                    return (Left.ComputeMagnitude() * 3) + (Right.ComputeMagnitude() * 2);
                }

                return Value.Value;
            }

            internal SnailfishNumber Clone(SnailfishNumber parent = null)
            {
                SnailfishNumber number = new SnailfishNumber();
                
                number.Value = Value;
                number.Left = Left?.Clone(number);
                number.Right = Right?.Clone(number);
                number.Parent = parent;

                return number;
            }

            private bool Explode(int depth = 0)
            {
                if (Value == null)
                {
                    if (Left.Explode(depth + 1))
                    {
                        return true;
                    }

                    if (depth >= 4 && Left.Value != null && Right.Value != null)
                    {
                        SnailfishNumber previous = Parent.GetPreviousNumber(this);

                        if (previous != null)
                        {
                            previous.Value += Left.Value;
                        }

                        SnailfishNumber next = Parent.GetNextNumber(this);

                        if (next != null)
                        {
                            next.Value += Right.Value;
                        }

                        Left = null;
                        Right = null;
                        Value = 0;

                        return true;
                    }

                    if (Right.Explode(depth + 1))
                    {
                        return true;
                    }
                }

                return false;
            }

            private SnailfishNumber GetPreviousNumber(SnailfishNumber requestingChild)
            {
                if (requestingChild == Left)
                {
                    return (Parent != null) ? Parent.GetPreviousNumber(this) : null;
                }

                if (requestingChild == Right)
                {
                    SnailfishNumber next = Left;

                    while (next.Value == null)
                    {
                        next = next.Right;
                    }

                    return next;
                }

                throw new Exception();
            }

            private SnailfishNumber GetNextNumber(SnailfishNumber requestingChild)
            {
                if (requestingChild == Right)
                {
                    return (Parent != null) ? Parent.GetNextNumber(this) : null;
                }

                if (requestingChild == Left)
                {
                    SnailfishNumber next = Right;

                    while (next.Value == null)
                    {
                        next = next.Left;
                    }

                    return next;
                }

                throw new Exception();
            }

            private bool Split()
            {
                if (Value == null)
                {
                    if (Left.Split())
                    {
                        return true;
                    }

                    if (Right.Split())
                    {
                        return true;
                    }
                }
                else if (Value.Value >= 10)
                {
                    int mod = Value.Value % 2;
                    int div = Value.Value / 2;

                    Left = new SnailfishNumber()
                    {
                        Value = div,
                        Parent = this,
                    };

                    Right = new SnailfishNumber()
                    {
                        Value = div + mod,
                        Parent = this,
                    };

                    Value = null;

                    return true;
                }

                return false;
            }

            private static (string left, string right) GetComponents(string input)
            {
                int pos = 0;
                int bracketCount = 0;

                while (true)
                {
                    if (input[pos] == '[')
                    {
                        bracketCount++;
                    }
                    else if (input[pos] == ']')
                    {
                        bracketCount--;
                    }
                    else if (input[pos] == ',' && bracketCount == 0)
                    {
                        break;
                    }

                    pos++;
                }

                return (input.Substring(0, pos), input.Substring(pos + 1));
            }
        }
    }
}
