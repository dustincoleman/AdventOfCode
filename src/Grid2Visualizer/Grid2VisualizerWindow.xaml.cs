using AdventOfCode.Common;
using System;
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
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            for (int y = 0; y < grid.Bounds.Y; y++)
            {
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            foreach (Point2 p in grid.Points)
            {
                TextBlock textBlock = new TextBlock(new Run(grid[p].ToString()));
                Grid.SetColumn(textBlock, p.X);
                Grid.SetRow(textBlock, p.Y);

                ContentGrid.Children.Add(textBlock);
            }
        }
    }
}
