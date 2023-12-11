using AdventOfCode.Common;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    public class Grid2ViewModel : NotifyPropertyChanged
    {
        private Grid2DataProvider provider;
        private NotifyProperty<IReadOnlyCollection<Grid2ColumnViewModel>> columns;
        private NotifyProperty<IReadOnlyCollection<Grid2RowViewModel>> rows;

        public Grid2ViewModel(IVisualizerObjectProvider3 objectProvider)
        {
            IReadOnlyCollection<Grid2ColumnViewModel> initialColumns = new ReadOnlyCollection<Grid2ColumnViewModel>(new List<Grid2ColumnViewModel>());
            IReadOnlyCollection<Grid2RowViewModel> initialRows = new ReadOnlyCollection<Grid2RowViewModel>(new List<Grid2RowViewModel>());

            this.provider = new Grid2DataProvider(objectProvider);
            this.columns = new NotifyProperty<IReadOnlyCollection<Grid2ColumnViewModel>>(nameof(Columns), this, initialColumns);
            this.rows = new NotifyProperty<IReadOnlyCollection<Grid2RowViewModel>>(nameof(Rows), this, initialRows);

            this.provider.Initialize(); //.InitializeAsync().ContinueWith(t =>
            //{
                Columns = CreateColumns();
                Rows = CreateRows();
            //});
        }

        public Grid2DataProvider Provider => this.provider;

        public IReadOnlyCollection<Grid2ColumnViewModel> Columns
        {
            get => this.columns.Value;
            private set => this.columns.Value = value;
        }

        public IReadOnlyCollection<Grid2RowViewModel> Rows
        {
            get => this.rows.Value;
            private set => this.rows.Value = value;
        }

        private IReadOnlyCollection<Grid2ColumnViewModel> CreateColumns()
        {
            return new ReadOnlyCollection<Grid2ColumnViewModel>(
                Enumerable.Range(0, this.provider.Bounds.X)
                    .Select(column => new Grid2ColumnViewModel(column))
                    .ToList());
        }

        private IReadOnlyCollection<Grid2RowViewModel> CreateRows()
        {
            return new ReadOnlyCollection<Grid2RowViewModel>(
                Enumerable.Range(0, this.provider.Bounds.Y)
                    .Select(row => new Grid2RowViewModel(row, this.provider))
                    .ToList());
        }
    }
}
