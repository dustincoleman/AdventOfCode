namespace AdventOfCode2023;

public class Day25
{
    [Fact]
    public void Part1()
    {
        Dictionary<string, Node> nodesByName = new Dictionary<string, Node>();

        Node getNode(string name)
        {
            if (!nodesByName.TryGetValue(name, out Node node))
            {
                node = new Node() { Name = name };
                nodesByName.Add(name, node);
            }
            return node;
        }

        foreach (string line in File.ReadAllLines("Day25.txt"))
        {
            string[] names = line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries);
            Node l = getNode(names[0]);
            for (int i = 1; i < names.Length; i++)
            {
                Node r = getNode(names[i]);
                l.Edges.Add(r);
                r.Edges.Add(l);
            }
        }

        List<Node> graph = nodesByName.Values.ToList();
        AutoDictionary<string, int> edgeCounts = new AutoDictionary<string, int>();

        foreach (Node source in graph)
        {
            Dijkstra(graph, source);
            foreach (Node start in graph)
            {
                Node n = start;
                while (n.Previous != null)
                {
                    edgeCounts[EdgeKey(n.Previous, n)]++;
                    n = n.Previous;
                }
            }
        }

        Node left = null;
        Node right = null;
        List<string> edgesToRemove = edgeCounts.OrderByDescending(kvp => kvp.Value).Take(3).Select(kvp => kvp.Key).ToList();

        foreach (string edge in edgesToRemove)
        {
            string[] names = edge.Split(',');
            Node node1 = nodesByName[names[0]];
            Node node2 = nodesByName[names[1]];

            if (left == null && right == null)
            {
                left = node1;
                right = node2;
            }

            node1.Edges.Remove(node2);
            node2.Edges.Remove(node1);
        }

        int answer = Count(left) * Count(right);
        Assert.Equal(543256, answer);
    }

    private int Count(Node start)
    {
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.TryDequeue(out Node node))
        {
            foreach (Node v in node.Edges)
            {
                if (visited.Add(v))
                {
                    queue.Enqueue(v);
                }
            }
        }

        return visited.Count;
    }

    private void Dijkstra(List<Node> graph, Node source)
    {
        List<Node> queue = new List<Node>();
        
        foreach (Node n in graph)
        {
            n.Previous = null;
            n.Distance = int.MaxValue;
        }

        source.Distance = 0;
        queue.Add(source);

        while (queue.Count > 0)
        {
            queue.Sort((left, right) => right.Distance.CompareTo(left.Distance));

            Node u = queue[queue.Count - 1];
            queue.RemoveAt(queue.Count - 1);

            foreach (Node v in u.Edges)
            {
                int alt = u.Distance + 1;
                if (alt < v.Distance)
                {
                    v.Previous = u;
                    v.Distance = alt;
                    if (!queue.Contains(v))
                    {
                        queue.Add(v);
                    }
                }
            }
            
        }
    }

    private string EdgeKey(Node node1, Node node2)
    {
        if (node1.Name.CompareTo(node2.Name) < 0)
        {
            return $"{node1.Name},{node2.Name}";
        }
        else
        {
            return $"{node2.Name},{node1.Name}";
        }
    }

    private class Node
    {
        internal string Name;
        internal HashSet<Node> Edges = new HashSet<Node>();
        internal int Distance;
        internal Node Previous;
    }

    private class Path : HashSet<Node>
    {
        internal Node Current { get; private set; }

        internal Path(Node pos)
        {
            Current = pos;
        }

        private Path(Path from, Node next)
            : base(from)
        {
            this.Add(from.Current);
            this.Current = next;
        }

        internal Path MoveTo(Node pos) => new Path(this, pos);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Node n in this)
            {
                sb.Append($"{n.Name}, ");
            }
            sb.Append(Current.Name);
            return sb.ToString();
        }
    }
}
