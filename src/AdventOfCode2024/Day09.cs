namespace AdventOfCode2024
{
    public class Day09
    {
        [Fact]
        public void Part1()
        {
            int[] diskMap = File.ReadAllText("Day09.txt").Select(ch => int.Parse(ch.ToString())).ToArray();
            List<int> disk = new List<int>();

            // Initialize the disk
            int id = 0;
            bool isFile = true;

            for (int i = 0; i < diskMap.Length; i++, isFile = !isFile)
            {
                int data = -1;

                if (isFile)
                {
                    data = id++;
                }

                for (int j = 0; j < diskMap[i]; j++)
                {
                    disk.Add(data);
                }
            }

            // Compress the disk
            int src = disk.Count - 1;

            for (int i = 0; i < disk.Count && src > i; i++)
            {
                if (disk[i] == -1)
                {
                    while (disk[src] == -1)
                    {
                        src--;
                    }

                    if (src > i)
                    {
                        disk[i] = disk[src];
                        disk[src] = -1;
                    }
                }
            }

            // Calculate the Checksum
            long answer = 0;

            for (int i = 0; i < disk.Count; i++)
            {
                if (disk[i] != -1)
                {
                    answer += disk[i] * i;
                }
            }

            Assert.Equal(6366665108136, answer);
        }

        [Fact]
        public void Part2()
        {
            int[] diskMap = File.ReadAllText("Day09.txt").Select(ch => int.Parse(ch.ToString())).ToArray();
            List<int> disk = new List<int>();

            // Initialize the disk
            int id = 0;
            bool isFile = true;

            for (int i = 0; i < diskMap.Length; i++, isFile = !isFile)
            {
                int data = -1;

                if (isFile)
                {
                    data = id++;
                }

                for (int j = 0; j < diskMap[i]; j++)
                {
                    disk.Add(data);
                }
            }

            // Compress the disk
            int[] freeSearchIndexBySize = new int[10];

            while (--id > 0)
            {
                // Find file
                int src = disk.IndexOf(id);

                // Count size
                int size = 0;
                for (int i = src; i < disk.Count && disk[i] == id; i++, size++);

                // Find where to put it
                int dest = -1;

                for (int i = freeSearchIndexBySize[size]; i < src; i++)
                {
                    bool empty = true;

                    for (int j = i; j < i + size; j++)
                    {
                        if (disk[j] != -1)
                        {
                            empty = false;
                            break;
                        }
                    }

                    if (empty)
                    {
                        dest = i;
                        freeSearchIndexBySize[size] = i + size;
                        break;
                    }
                }

                // Move the file
                if (dest >= 0)
                {
                    while(size-- > 0)
                    {
                        disk[dest] = disk[src];
                        disk[src] = -1;
                        dest++;
                        src++;
                    }
                }
            }

            // Calculate the Checksum
            long answer = 0;

            for (int i = 0; i < disk.Count; i++)
            {
                if (disk[i] != -1)
                {
                    answer += disk[i] * i;
                }
            }

            Assert.Equal(6398065450842, answer);
        }
    }
}
