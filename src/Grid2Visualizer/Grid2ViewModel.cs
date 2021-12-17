using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    public class Grid2ViewModel
    {
        private readonly IGrid2 grid;
        private Lazy<IReadOnlyCollection<Grid2ColumnViewModel>> columns;
        private Lazy<IReadOnlyCollection<Grid2RowViewModel>> rows;

        internal Grid2ViewModel(IGrid2 grid)
        {
            this.grid = grid;
            this.columns = new Lazy<IReadOnlyCollection<Grid2ColumnViewModel>>(CreateColumns);
            this.rows = new Lazy<IReadOnlyCollection<Grid2RowViewModel>>(CreateRows);
        }

        public IEnumerable<Grid2ColumnViewModel> Columns => this.columns.Value;

        public IEnumerable<Grid2RowViewModel> Rows => this.rows.Value;

        private IReadOnlyCollection<Grid2ColumnViewModel> CreateColumns()
        {
            return new ReadOnlyCollection<Grid2ColumnViewModel>(
                Enumerable.Range(0, grid.Bounds.X)
                    .Select(column => new Grid2ColumnViewModel(column))
                    .ToList());
        }

        private IReadOnlyCollection<Grid2RowViewModel> CreateRows()
        {
            return new ReadOnlyCollection<Grid2RowViewModel>(
                Enumerable.Range(0, grid.Bounds.Y)
                    .Select(row => new Grid2RowViewModel(row, this.grid))
                    .ToList());
        }
    }
}
