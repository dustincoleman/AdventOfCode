namespace AdventOfCode2024
{
    public class Day23
    {
        [Fact]
        public void Part1()
        {
            List<Computer> puzzle = LoadPuzzle();
            HashSet<string> sets = new HashSet<string>();

            foreach (Computer comp1 in puzzle)
            {
                List<Computer> connections = comp1.Connections.ToList();

                for (int i = 0; i < connections.Count; i++)
                {
                    Computer comp2 = connections[i];

                    for (int j = i + 1; j < connections.Count; j++)
                    {
                        Computer comp3 = connections[j];

                        if (comp2.Connections.Contains(comp3))
                        {
                            sets.Add(string.Join(',', new[] { comp1.Name, connections[i].Name, connections[j].Name }.OrderBy(s => s)));
                        }
                    }
                }
            }

            long result = sets.Where(set => set.Split(',').Any(name => name.StartsWith('t'))).Count();
            Assert.Equal(1284, result);
        }

        [Fact]
        public void Part2()
        {
            List<Computer> puzzle = LoadPuzzle();
            HashSet<string> sets = new HashSet<string>();

            foreach (Computer computer in puzzle)
            {
                List<Computer> candidates = [computer, .. computer.Connections];

                for (int i = 1; i < candidates.Count; i++)
                {
                    bool connected = true;

                    for (int j = i + 1; j < candidates.Count; j++)
                    {
                        if (!candidates[i].Connections.Contains(candidates[j]))
                        {
                            connected = false;
                            break;
                        }
                    }

                    if (!connected)
                    {
                        candidates[i] = null;
                    }
                }

                sets.Add(string.Join(',', candidates.Where(c => c != null).Select(c => c.Name).OrderBy(n => n)));
            }

            string result = sets.OrderByDescending(s => s.Length).First();
            Assert.Equal("bv,cm,dk,em,gs,jv,ml,oy,qj,ri,uo,xk,yw", result);
        }

        private List<Computer> LoadPuzzle()
        {
            Dictionary<string, Computer> computersByName = new Dictionary<string, Computer>();

            Computer GetComputer(string name)
            {
                if (!computersByName.TryGetValue(name, out Computer computer))
                {
                    computer = new Computer(name);
                    computersByName.Add(name, computer);
                }
                return computer;
            }

            foreach (string line in File.ReadAllLines("Day23.txt"))
            {
                string[] names = line.Split('-');
                Computer left = GetComputer(names[0]);
                Computer right = GetComputer(names[1]);

                left.Connections.Add(right);
                right.Connections.Add(left);
            }

            return computersByName.Values.OrderBy(c => c.Name).ToList();
        }

        private class Computer(string name)
        {
            internal readonly string Name = name;
            internal readonly HashSet<Computer> Connections = new HashSet<Computer>();
        }
    }
}
