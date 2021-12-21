using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    public class Grid2CellViewModel : NotifyPropertyChanged
    {
        private readonly int row;
        private readonly int column;
        private readonly Grid2DataProvider provider;

        internal Grid2CellViewModel(int row, int column, Grid2DataProvider provider)
        {
            this.row = row;
            this.column = column;
            this.provider = provider;
        }

        public object RemoteValue => this.provider[this.column, this.row];
    }
}
