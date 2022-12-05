using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.Common
{
    public static class PuzzleFile
    {
        public static string[][] ReadAllLineGroups(string filename)
        {
            List<string> lines = new List<string>();
            List<string[]> groups = new List<string[]>();

            foreach (string line in File.ReadAllLines(filename))
            {
                if (line == string.Empty)
                {
                    if (lines.Count > 0)
                    {
                        groups.Add(lines.ToArray());
                        lines.Clear();
                    }
                }
                else
                {
                    lines.Add(line);
                }
            }

            if (lines.Count > 0)
            {
                groups.Add(lines.ToArray());
            }

            return groups.ToArray();
        }
    }
}
