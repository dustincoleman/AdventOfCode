using AdventOfCode.Common;
using Grid2Visualizer.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer.TestConsole
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Grid2<int> grid = new Grid2<int>(500, 500);

            foreach (Point2 point in grid.Points)
            {
                grid[point] = point.Sum();
            }

            StringGrid2 proxy = new StringGrid2(grid);

            Grid2DialogVisualizer.TestShow(proxy);
        }
    }
}
