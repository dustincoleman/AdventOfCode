namespace AdventOfCode2024
{
    public class Day24
    {
        [Fact]
        public void Part1()
        {
            bool pendingGate = true;
            (Dictionary<string, bool> valuesByWire, List<Gate> gates) = LoadPuzzle();

            while (pendingGate)
            {
                pendingGate = false;

                foreach (Gate gate in gates)
                {
                    if (!valuesByWire.ContainsKey(gate.Out))
                    {
                        if (valuesByWire.TryGetValue(gate.In1, out bool val1) && valuesByWire.TryGetValue(gate.In2, out bool val2))
                        {
                            bool outVal = gate.Type switch
                            {
                                "AND" => val1 && val2,
                                "OR" => val1 || val2,
                                "XOR" => val1 ^ val2,
                                _ => throw new Exception()
                            };
                            valuesByWire.Add(gate.Out, outVal);
                        }
                        else
                        {
                            pendingGate = true;
                        }
                    }
                }
            }

            long result = 0;

            foreach (bool bit in valuesByWire.Where(kvp => kvp.Key.StartsWith('z')).OrderByDescending(kvp => kvp.Key).Select(kvp => kvp.Value))
            {
                result <<= 1;
                result |= Convert.ToInt64(bit);
            }

            Assert.Equal(52956035802096, result);
        }

        [Fact]
        public void Part2()
        {
            (Dictionary<string, bool> valuesByWire, List<Gate> gates) = LoadPuzzle(corrected: true);

            Adder[] adders = new Adder[valuesByWire.Count / 2];

            // Find all the input gates for the bits from the two numbers
            for (int i = gates.Count -1; i >= 0; i--)
            {
                Gate gate = gates[i];

                if (gate.In1.StartsWith('x') && gate.In2.StartsWith('y'))
                {
                    if (int.TryParse(gate.In1.Substring(1), out int r1) && int.TryParse(gate.In2.Substring(1), out int r2))
                    {
                        Assert.Equal(r1, r2);
                        if (adders[r1] == null)
                        {
                            adders[r1] = new Adder();
                        }
                        if (gate.Type == "XOR")
                        {
                            adders[r1].InBitsXor = gate;
                        }
                        else if (gate.Type == "AND")
                        {
                            adders[r1].InBitsAnd = gate;
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        gates.RemoveAt(i);
                    }
                }
            }

            // Find the remaining components to build a full adder
            for (int i = 0; i < adders.Length; i++)
            {
                Adder adder = adders[i];

                int idx = gates.FindIndex(g => g.Type == "XOR" && (g.In1 == adder.InBitsXor.Out || g.In2 == adder.InBitsXor.Out));
                if (idx >= 0)
                {
                    adder.CarryXor = gates[idx];
                    gates.RemoveAt(idx);
                }

                idx = gates.FindIndex(g => g.Type == "AND" && (g.In1 == adder.InBitsXor.Out || g.In2 == adder.InBitsXor.Out));
                if (idx >= 0)
                {
                    adder.CarryAnd = gates[idx];
                    gates.RemoveAt(idx);
                }

                idx = gates.FindIndex(g => g.Type == "OR" && (g.In1 == adder.InBitsAnd.Out || g.In2 == adder.InBitsAnd.Out));
                if (idx >= 0)
                {
                    adder.CarryOr = gates[idx];
                    gates.RemoveAt(idx);
                }
            }

            // At this point, I used the following loop to debug the circuit
            string carry = null;
            for (int i = 0; i < adders.Length; i++)
            {
                Adder adder = adders[i];

                Assert.Equal(carry, adder.CarryIn);
                Assert.Equal($"z{i:00}", adder.OutBit);
                Assert.NotNull(adder.CarryOut);

                carry = adder.CarryOut;
            }

            // I left a comment after each output in the puzzle file that I swapped
            List<string> swappedOutputs = new List<string>();
            foreach (string line in File.ReadAllLines("Day24.Fixed.txt"))
            {
                int i = line.IndexOf("//");
                if (i > 0)
                {
                    swappedOutputs.Add(line.Substring(i + "//".Length));
                }
            }

            string result = string.Join(',', swappedOutputs.OrderBy(s => s));
            Assert.Equal("hnv,hth,kfm,tqr,vmv,z07,z20,z28", result);
        }

        private (Dictionary<string, bool> valuesByWire, List<Gate> gates) LoadPuzzle(bool corrected = false)
        {
            string[][] groups = PuzzleFile.ReadAllLineGroups(corrected ? "Day24.Fixed.txt" : "Day24.txt");
            Dictionary<string, bool> valuesByWire = new Dictionary<string, bool>();
            List<Gate> gates = new List<Gate>();

            foreach (string line in groups[0])
            {
                string[] split = line.Split(": ");
                valuesByWire.Add(split[0], Convert.ToBoolean(int.Parse(split[1])));
            }

            foreach (string line in groups[1])
            {
                string[] split = line.Replace(" -> ", " ").Split(' ');

                if (split[0].StartsWith('y') && split[2].StartsWith('x'))
                {
                    gates.Add(new Gate(split[2], split[0], split[3], split[1]));
                }
                else
                {
                    gates.Add(new Gate(split[0], split[2], split[3], split[1]));
                }
            }

            return (valuesByWire, gates);
        }

        private record Gate(string In1, string In2, string Out, string Type);

        private class Adder
        {
            internal Gate InBitsXor;
            internal Gate InBitsAnd;
            internal Gate CarryXor;
            internal Gate CarryAnd;
            internal Gate CarryOr;

            internal string CarryIn => CarryAnd == null ? null : CarryAnd.In1 != InBitsXor.Out ? CarryAnd.In1 : CarryAnd.In2;
            internal string OutBit => CarryXor?.Out ?? InBitsXor?.Out;
            internal string CarryOut => CarryOr?.Out ?? InBitsAnd?.Out;
        }
    }
}
