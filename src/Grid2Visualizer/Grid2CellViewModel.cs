using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    public class Grid2CellViewModel : ViewModelBase
    {
        private readonly int row;
        private readonly int column;
        private readonly IGrid2 grid;

        internal Grid2CellViewModel(int row, int column, IGrid2 grid)
        {
            this.row = row;
            this.column = column;
            this.grid = grid;
        }

        public object Value => this.grid[this.column, this.row];
    }
}
