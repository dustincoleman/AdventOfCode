namespace AdventOfCode2022
{
    public class Day07
    {
        [Fact]
        public void Part1()
        {
            long result = LoadPuzzleFileSystem().Sum(d => d.GetSize() <= 100000 ? d.GetSize() : 0);
            Assert.Equal(1325919, result);
        }

        [Fact]
        public void Part2()
        {
            List<Directory7> dirs = LoadPuzzleFileSystem();
            long sizeNeeded = dirs[0].GetSize() - 40000000;
            long result = dirs.OrderBy(d => d.GetSize()).First(d => d.GetSize() > sizeNeeded).GetSize();
            Assert.Equal(2050735, result);
        }

        private List<Directory7> LoadPuzzleFileSystem()
        {
            List<Directory7> dirs = new List<Directory7>()
            {
                new Directory7() { Name = "/" }
            };
            Directory7 cd = dirs[0];

            foreach (string line in System.IO.File.ReadAllLines("Day07.txt"))
            {
                string[] tokens = line.Split(' ');

                if (tokens[0] == "$")
                {
                    if (tokens[1] == "cd")
                    {
                        if (tokens[2] == "/")
                        {
                            cd = dirs[0];
                        }
                        else if (tokens[2] == "..")
                        {
                            cd = cd.Parent;
                        }
                        else
                        {
                            cd = cd.Directories.First(d => d.Name == tokens[2]);
                        }
                    }
                }
                else if (tokens[0] == "dir")
                {
                    Directory7 d = new Directory7() { Name = tokens[1], Parent = cd };
                    cd.Directories.Add(d);
                    dirs.Add(d);
                }
                else // file
                {
                    cd.Files.Add(new File7() { Name = tokens[1], Size = int.Parse(tokens[0]) });
                }
            }

            return dirs;
        }

        public class Directory7
        {
            public string Name;
            public Directory7 Parent;
            public List<File7> Files = new List<File7>();
            public List<Directory7> Directories = new List<Directory7>();

            private long? cachedSize;

            public long GetSize()
            {
                return cachedSize ?? (cachedSize = Files.Sum(f => f.Size) + Directories.Sum(d => d.GetSize())).Value;
            }
        }

        public class File7
        {
            public string Name;
            public int Size;
        }
    }
}