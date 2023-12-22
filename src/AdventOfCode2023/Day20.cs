using System.Diagnostics;
using System.Xml.Linq;

namespace AdventOfCode2023;

public class Day20
{
    [Fact]
    public void Part1()
    {
        Dictionary<string, Module> modulesByName = LoadPuzzle();
        Queue<Message> queue = new Queue<Message>();
        int times = 1000;
        long low = 0;
        long high = 0;

        while (times-- > 0)
        {
            queue.Enqueue(new Message() { From = "button", To = "broadcaster" });

            while (queue.TryDequeue(out Message msg))
            {
                if (msg.High)
                {
                    high++;
                }
                else
                {
                    low++;
                }

                if (modulesByName.TryGetValue(msg.To, out Module m))
                {
                    m.Send(msg, queue);
                }
            }

            Debugger.Break();
        }

        long answer = high * low;
        Assert.Equal(731517480, answer);
    }

    [Fact]
    public void Part2()
    {
        Dictionary<string, Module> modulesByName = LoadPuzzle();
        Queue<Message> queue = new Queue<Message>();

        Broadcast broadcaster = (Broadcast)modulesByName["broadcaster"];
        FlipFlop rx = (FlipFlop)modulesByName["rx"];

        foreach (string name in broadcaster.Destinations.ToArray())
        {
            SimplifyCounter(name, broadcaster.Name, modulesByName);
        }

        long answer = modulesByName.Values.OfType<Counter>().Select(c => (long)c.Mask).LeastCommonMultiple();

        Assert.Equal(244178746156661, answer);
    }

    private void SimplifyCounter(string startingName, string parentName, Dictionary<string, Module> modulesByName)
    {
        Conjunction c = null;
        FlipFlop ff = (FlipFlop)modulesByName[startingName];

        int shift = 1;
        int mask = 0;

        List<(string Name, int Mask)> externalInputs = new();

        while (ff != null)
        {
            FlipFlop nextFF = null;
            Conjunction nextC = null;

            // Remove the current flip flop from the puzzle
            var references = modulesByName.Values.Where(m => m.Name != parentName && m.Destinations.Contains(ff.Name)).Select(input => input.Name).ToArray();
            externalInputs.AddRange(references.Select(name => (name, mask)));
            modulesByName.Remove(ff.Name);

            // Extract the branches
            foreach (string name in ff.Destinations)
            {
                if (modulesByName[name] is FlipFlop tempFF) nextFF = (nextFF == null) ? tempFF : throw new Exception("Found multiple branches in counter");
                else if (modulesByName[name] is Conjunction tempC) nextC = (nextC == null) ? tempC : throw new Exception("Found multiple branches in counter"); 
                else throw new Exception();
            }

            // Validate that Conjunction is the same
            if (c == null) c = nextC;
            else if (nextC != null && c != nextC) throw new Exception("Found multiple outputs in counter");

            // Record if there is a connection
            if (nextC != null) mask |= shift;

            ff = nextFF;
            shift <<= 1;
        }

        string counterName = $"counter:{startingName}";
        modulesByName.Add(counterName, new Counter(mask, externalInputs));

        foreach (Module m in modulesByName.Values)
        {
            for (int i = 0; i < m.Destinations.Count; i++)
            {
                if (m.Destinations[i] == startingName)
                {
                    m.Destinations[i] = counterName;
                }
            }
        }
    }

    private Dictionary<string, Module> LoadPuzzle()
    {
        Dictionary<string, Module> modulesByName = new Dictionary<string, Module>();

        foreach (string line in File.ReadAllLines("Day20.txt"))
        {
            string[] split = line.Split(new[] { " -> ", ", " }, StringSplitOptions.None);

            Module module = split[0][0] switch
            {
                '%' => new FlipFlop() { Name = split[0].Substring(1) },
                '&' => new Conjunction() { Name = split[0].Substring(1) },
                _ => (split[0] == "broadcaster") ? 
                    new Broadcast() { Name = "broadcaster", Input = new Dictionary<string, bool?>() { { "button", null } } } : 
                    throw new Exception()
            };

            for (int i = 1; i < split.Length; i++)
            {
                module.Destinations.Add(split[i]);
            }

            modulesByName[module.Name] = module;
        }

        FlipFlop rx = new FlipFlop() { Name = "rx" };
        modulesByName[rx.Name] = rx;

        foreach (Module module in modulesByName.Values)
        {
            foreach (string name in module.Destinations)
            {
                if (modulesByName.TryGetValue(name, out Module m))
                {
                    m.Input[module.Name] = null;
                }
            }
        }

        return modulesByName;
    }

    private abstract class Module
    {
        public string Name;
        public Dictionary<string, bool?> Input = new Dictionary<string, bool?>();
        public List<string> Destinations = new List<string>();

        public abstract string State { get; }

        public void Send(Message msg, Queue<Message> queue)
        {
            if (!Input.ContainsKey(msg.From))
            {
                throw new Exception();
            }

            Input[msg.From] = msg.High;

            DoSend(msg.High, queue);
        }

        protected abstract void DoSend(bool high, Queue<Message> queue);

        protected void Broadcast(bool high, Queue<Message> queue)
        {
            foreach (string dest in Destinations)
            {
                queue.Enqueue(
                    new Message()
                    {
                        From = Name,
                        To = dest,
                        High = high
                    });
            }
        }
    }

    private class FlipFlop : Module
    {
        public bool On = false;

        public override string State => (Name == "rx") ? On ? "ON" : "OFF" : On ? "on" : "off";

        protected override void DoSend(bool high, Queue<Message> queue)
        {
            if (Name == "rx" && !high)
            {
                Debugger.Break();
            }

            if (!high)
            {
                On = !On;
                Broadcast(On, queue);
            }
        }

        public override string ToString()
        {
            return On ? "On" : "Off";
        }
    }

    private class Conjunction : Module
    {
        public override string State => new string(Input.Values.Select(b => (b == true) ? '1' : '0').ToArray());

        protected override void DoSend(bool high, Queue<Message> queue)
        {
            Broadcast(!Input.Values.All(b => b == true), queue);
        }

        public override string ToString()
        {
            return $"{Input.Values.Count(b => b == true)}/{Input.Count}";
        }
    }

    private class Broadcast : Module
    {
        public override string State => "root";

        protected override void DoSend(bool high, Queue<Message> queue)
        {
            Broadcast(high, queue);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    private class Counter : Module
    {
        public Counter(int mask)
        {
            Mask = mask;
        }

        public Counter(int mask, List<(string Name, int Mask)> externalInputs) : this(mask)
        {
            ExternalInputs = externalInputs;
        }

        public override string State => throw new NotImplementedException();

        public int Mask { get; }
        public List<(string Name, int Mask)> ExternalInputs { get; }
        public List<string> FlipFlops = new List<string>();

        protected override void DoSend(bool high, Queue<Message> queue)
        {
            throw new NotImplementedException();
        }
    }

    private struct Message
    {
        public string From;
        public string To;
        public bool High;

        public Message(string from, string to, bool high)
        {
            From = from;
            To = to;
            High = high;
        }
    }
}
