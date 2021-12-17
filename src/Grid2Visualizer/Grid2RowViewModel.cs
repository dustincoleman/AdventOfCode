using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    public class Grid2RowViewModel : ViewModelBase
    {
        private readonly int row;
        private readonly IGrid2 grid;
        private ViewProperty<string> header;
        private Lazy<IReadOnlyCollection<Grid2CellViewModel>> cells;

        internal Grid2RowViewModel(int row, IGrid2 grid)
        {
            this.row = row;
            this.grid = grid;
            this.header = new ViewProperty<string>(nameof(Header), this, row.ToString());
            this.cells = new Lazy<IReadOnlyCollection<Grid2CellViewModel>>(CreateColumns);
        }

        public string Header
        {
            get => this.header.Value;
            set => this.header.Value = value;
        }

        public IReadOnlyCollection<Grid2CellViewModel> Cells => this.cells.Value;

        private IReadOnlyCollection<Grid2CellViewModel> CreateColumns()
        {
            return new ReadOnlyCollection<Grid2CellViewModel>(
                Enumerable.Range(0, grid.Bounds.X)
                    .Select(column => new Grid2CellViewModel(this.row, column, this.grid))
                    .ToList());
        }
    }
}
