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
        List<string> trace = new List<string>();
        int times = 1000;
        long low = 0;
        long high = 0;
        int count = 0;

        while (times-- > 0)
        {
            //trace.Add($"--- Iteration: {count++} ---");
            trace.Clear();

            queue.Enqueue(new Message() { From = "button", To = "broadcaster" });

            while (queue.TryDequeue(out Message msg))
            {
                trace.Add($"{msg.From} -{(msg.High ? "high" : "low")}-> {msg.To}");

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
        long answer = 0;
        Dictionary<string, Module> modulesByName = LoadPuzzle();
        Queue<Message> queue = new Queue<Message>();

        Broadcast broadcaster = (Broadcast)modulesByName["broadcaster"];

        FlipFlop rx = (FlipFlop)modulesByName["rx"];
        List<string> history = new List<string>();

        while (!rx.On)
        {
            answer++;

            //List<string> states = GetStates(root: broadcaster, modulesByName);
            List<string> states = GetStatesReverse(rx, modulesByName);
            history.Add(states[3]);

            queue.Enqueue(new Message() { From = "button", To = "broadcaster" });

            while (queue.TryDequeue(out Message msg))
            {
                if (modulesByName.TryGetValue(msg.To, out Module m))
                {
                    m.Send(msg, queue);
                }

                if (rx.On)
                {
                    break;
                }
            }
        }

        Assert.Equal(0, answer);
    }

    private List<string> GetStatesReverse(Module leaf, Dictionary<string, Module> modulesByName)
    {
        List<string> levels = new List<string>();
        StringBuilder sb = new StringBuilder();
        HashSet<string> visited = new HashSet<string>();

        Queue<Module> currentLevel = null;
        Queue<Module> nextLevel = new Queue<Module>();

        nextLevel.Enqueue(leaf);

        while (nextLevel.Count > 0)
        {
            sb.Clear();
            currentLevel = nextLevel;
            nextLevel = new Queue<Module>();

            while (currentLevel.TryDequeue(out Module cur))
            {
                if (visited.Add(cur.Name))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("|");
                    }

                    sb.Append(cur.State);

                    foreach (string name in cur.Input.Keys)
                    {
                        if (modulesByName.ContainsKey(name))
                            nextLevel.Enqueue(modulesByName[name]);
                    }
                }
            }

            if (sb.Length > 0)
                levels.Add(sb.ToString());
        }

        return levels;
    }

    private List<string> GetStates(Module root, Dictionary<string, Module> modulesByName)
    {
        List<string> levels = new List<string>();
        StringBuilder sb = new StringBuilder();
        HashSet<string> visited = new HashSet<string>();

        Queue<Module> currentLevel = null;
        Queue<Module> nextLevel = new Queue<Module>();

        foreach (string name in root.Destinations)
        {
            nextLevel.Enqueue(modulesByName[name]);
        }

        while (nextLevel.Count > 0)
        {
            sb.Clear();
            currentLevel = nextLevel;
            nextLevel = new Queue<Module>();

            while (currentLevel.TryDequeue(out Module cur))
            {
                if (visited.Add(cur.Name))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("|");
                    }

                    sb.Append(cur.State);

                    foreach (string name in cur.Destinations)
                    {
                        nextLevel.Enqueue(modulesByName[name]);
                    }
                }
            }

            if (sb.Length > 0)
                levels.Add(sb.ToString());
        }

        return levels;
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
