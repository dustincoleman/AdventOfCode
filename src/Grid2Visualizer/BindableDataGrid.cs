using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Grid2Visualizer
{
    public class BindableDataGrid : DataGrid
    {
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.Register(
                "BindableColumns", 
                typeof(IEnumerable<DataGridColumn>), 
                typeof(BindableDataGrid), 
                new PropertyMetadata(OnBindableColumnsChanged));

        public IEnumerable<DataGridColumn> BindableColumns
        {
            get { return (IEnumerable<DataGridColumn>)GetValue(BindableColumnsProperty); }
            set { SetValue(BindableColumnsProperty, value); }
        }

        private static void OnBindableColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableDataGrid)d).OnBindableColumnsChanged(e);
        }

        private void OnBindableColumnsChanged(DependencyPropertyChangedEventArgs e)
        {
            Columns.Clear();

            if (e.NewValue != null)
            {
                foreach (DataGridColumn column in (IEnumerable<DataGridColumn>)e.NewValue)
                {
                    Columns.Add(column);
                }
            }
        }
    }
}
