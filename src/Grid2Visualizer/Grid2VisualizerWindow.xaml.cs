using AdventOfCode.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grid2Visualizer
{
    /// <summary>
    /// Interaction logic for Grid2VisualizerWindow.xaml
    /// </summary>
    public partial class Grid2VisualizerWindow : Window
    {
        private IGrid2 grid;

        public Grid2VisualizerWindow(IGrid2 grid)
        {
            this.grid = grid;

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            for (int x = 0; x < grid.Bounds.X; x++)
            {
                ContentGrid.Columns.Add(new DataGridTextColumn() { Binding = new Binding($"Columns[{x}]"), Header = x });
            }

            ContentGrid.ItemsSource = new Grid2RowsPresenter(grid);
        }
    }

    public class Grid2RowsPresenter : IEnumerable<Grid2Row>
    {
        private IGrid2 grid;

        internal Grid2RowsPresenter(IGrid2 grid)
        {
            this.grid = grid;
        }

        public IEnumerator<Grid2Row> GetEnumerator()
        {
            return Enumerable
                   .Range(0, grid.Bounds.Y)
                   .Select(row => new Grid2Row(grid, row))
                   .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Grid2Row
    {
        private IGrid2 grid;
        private int row;

        private Lazy<object[]> columns;

        internal Grid2Row(IGrid2 grid, int row)
        {
            this.grid = grid;
            this.row = row;
            this.columns = new Lazy<object[]>(LoadColumns);
        }

        public int Row => row;

        public object[] Columns => columns.Value;

        private object[] LoadColumns()
        {
            return Enumerable
                   .Range(0, grid.Bounds.X)
                   .Select(column => grid[column, row])
                   .ToArray();
        }
    }
}
