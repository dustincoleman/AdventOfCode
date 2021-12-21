using AdventOfCode.Common;
using Microsoft.VisualStudio.DebuggerVisualizers;
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
        public Grid2VisualizerWindow(IVisualizerObjectProvider2 objectProvider)
        {
            InitializeComponent();
            DataContext = new Grid2ViewModel(objectProvider);
        }
    }
}
