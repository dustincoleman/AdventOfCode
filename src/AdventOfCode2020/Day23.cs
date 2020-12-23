using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using Xunit;

using Node = System.Collections.Generic.LinkedListNode<int>;

namespace AdventOfCode2020
{
    public class Day23
    {
        const string Input = "624397158";

        [Fact]
        public void Part1()
        {
            LinkedList<int> list = new LinkedList<int>(Input.Select(ch => int.Parse(new string(ch, 1))));
            Dictionary<int, Node> map = new Dictionary<int, Node>();
            Node current = list.First;

            // Build a map to the nodes by their values
            while (current != null)
            {
                map.Add(current.Value, current);
                current = current.Next;
            }

            // Play the game
            current = list.First;
            
            for (int round = 0; round < 100; round++)
            {
                // Pick up three cups
                List<Node> pickedUpCups = new List<Node>();
                
                for (int i = 0; i < 3; i++)
                {
                    Node cup = current.Next ?? list.First;
                    pickedUpCups.Add(cup);
                    list.Remove(cup);
                }

                // Select the destination cup
                int destination = current.Value - 1;
                if (destination < 1)
                {
                    destination = 9;
                }

                while (pickedUpCups.Any(c => c.Value == destination))
                {
                    destination--;
                    if (destination < 1)
                    {
                        destination = 9;
                    }
                }

                Node destinationCup = map[destination];

                // Place the cups after the destination
                foreach (Node cup in pickedUpCups)
                {
                    list.AddAfter(destinationCup, cup);
                    destinationCup = cup;
                }

                // Move to the next cup
                current = current.Next ?? list.First;
            }

            string result = "";
            current = map[1];

            for (int i = 0; i < 8; i++)
            {
                current = current.Next ?? list.First;
                result += current.Value.ToString();
            }

            Assert.Equal("74698532", result);
        }

        [Fact]
        public void Part2()
        {
            LinkedList<int> list = new LinkedList<int>(Input.Select(ch => int.Parse(new string(ch, 1))));
            Dictionary<int, Node> map = new Dictionary<int, Node>();
            Node current = list.First;

            // Build a map to the nodes by their values
            while (current != null)
            {
                map.Add(current.Value, current);
                current = current.Next;
            }

            // Make it Part 2!!!
            for (int i = 10; i <= 1000000; i++)
            {
                map.Add(i, list.AddLast(i));
            }

            // Play the game
            List<Node> pickedUpCups = new List<Node>();
            current = list.First;

            for (long round = 0; round < 10000000; round++)
            {
                // Pick up three cups
                pickedUpCups.Clear();

                for (int i = 0; i < 3; i++)
                {
                    Node cup = current.Next ?? list.First;
                    pickedUpCups.Add(cup);
                    list.Remove(cup);
                }

                // Select the destination cup
                int destination = current.Value - 1;
                if (destination < 1)
                {
                    destination = 1000000;
                }

                while (pickedUpCups.Any(c => c.Value == destination))
                {
                    destination--;
                    if (destination < 1)
                    {
                        destination = 1000000;
                    }
                }

                Node destinationCup = map[destination];

                // Place the cups after the destination
                foreach (Node cup in pickedUpCups)
                {
                    list.AddAfter(destinationCup, cup);
                    destinationCup = cup;
                }

                // Move to the next cup
                current = current.Next ?? list.First;
            }

            current = map[1];

            long result = (long)current.Next.Value * current.Next.Next.Value;

            Assert.Equal(286194102744, result);
        }
    }
}
