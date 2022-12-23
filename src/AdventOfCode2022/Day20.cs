using System.Drawing;

namespace AdventOfCode2022
{
    public class Day20
    {
        [Fact]
        public void Part1()
        {
            LinkedList<long> linkedList = LoadPuzzle();
            List<LinkedListNode<long>> nodes = new List<LinkedListNode<long>>();
            LinkedListNode<long> node = linkedList.First;

            while (node != null)
            {
                nodes.Add(node);
                node = node.Next;
            }

            foreach (LinkedListNode<long> current in nodes)
            {
                long toMove = Math.Abs(current.Value) % (linkedList.Count - 1);
                bool forward = (current.Value >= 0);

                if (toMove != 0)
                {
                    if (forward)
                    {
                        node = current.Next ?? linkedList.First;
                        linkedList.Remove(current);

                        while (--toMove > 0)
                        {
                            node = node.Next ?? linkedList.First;
                        }

                        linkedList.AddAfter(node, current);
                    }
                    else
                    {
                        node = current.Previous ?? linkedList.Last;
                        linkedList.Remove(current);

                        while (--toMove > 0)
                        {
                            node = node.Previous ?? linkedList.Last;
                        }

                        linkedList.AddBefore(node, current);
                    }
                }
            }

            List<long> list = linkedList.ToList();
            int idx = list.IndexOf(0);

            long i1 = list[(1000 + idx) % list.Count];
            long i2 = list[(2000 + idx) % list.Count];
            long i3 = list[(3000 + idx) % list.Count];

            long result = i1 + i2 + i3;
            Assert.Equal(8372, result);
        }

        [Fact]
        public void Part2()
        {
            LinkedList<long> linkedList = LoadPuzzle(811589153);
            List<LinkedListNode<long>> nodes = new List<LinkedListNode<long>>();
            LinkedListNode<long> node = linkedList.First;

            while (node != null)
            {
                nodes.Add(node);
                node = node.Next;
            }

            for (int i = 0; i < 10; i++)
            {
                foreach (LinkedListNode<long> current in nodes)
                {
                    long toMove = Math.Abs(current.Value) % (linkedList.Count - 1);
                    bool forward = (current.Value >= 0);

                    if (toMove != 0)
                    {
                        if (forward)
                        {
                            node = current.Next ?? linkedList.First;
                            linkedList.Remove(current);

                            while (--toMove > 0)
                            {
                                node = node.Next ?? linkedList.First;
                            }

                            linkedList.AddAfter(node, current);
                        }
                        else
                        {
                            node = current.Previous ?? linkedList.Last;
                            linkedList.Remove(current);

                            while (--toMove > 0)
                            {
                                node = node.Previous ?? linkedList.Last;
                            }

                            linkedList.AddBefore(node, current);
                        }
                    }
                }
            }

            List<long> list = linkedList.ToList();
            int idx = list.IndexOf(0);

            long i1 = list[(1000 + idx) % list.Count];
            long i2 = list[(2000 + idx) % list.Count];
            long i3 = list[(3000 + idx) % list.Count];

            long result = i1 + i2 + i3;
            Assert.Equal(7865110481723, result);
        }

        private LinkedList<long> LoadPuzzle(long key = 1)
        {
            return new LinkedList<long>(File.ReadAllLines("Day20.txt").Select(x => long.Parse(x) * key));
        }
    }
}