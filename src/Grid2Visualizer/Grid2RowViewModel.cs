using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    public class Grid2RowViewModel : NotifyPropertyChanged
    {
        private readonly int row;
        private readonly Grid2DataProvider provider;
        private NotifyProperty<string> header;
        private Lazy<IReadOnlyCollection<Grid2CellViewModel>> cells;

        public Grid2RowViewModel(int row, Grid2DataProvider provider)
        {
            this.row = row;
            this.provider = provider;
            this.header = new NotifyProperty<string>(nameof(Header), this, row.ToString());
            this.cells = new Lazy<IReadOnlyCollection<Grid2CellViewModel>>(CreateCells);
        }

        public string Header
        {
            get => this.header.Value;
            set => this.header.Value = value;
        }

        public IReadOnlyCollection<Grid2CellViewModel> Cells => this.cells.Value;

        private IReadOnlyCollection<Grid2CellViewModel> CreateCells()
        {
            return new ReadOnlyCollection<Grid2CellViewModel>(
                Enumerable.Range(0, this.provider.Bounds.X)
                    .Select(column => new Grid2CellViewModel(this.row, column, this.provider))
                    .ToList());
        }
    }
}
