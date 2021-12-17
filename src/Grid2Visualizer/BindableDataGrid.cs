using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Grid2Visualizer
{
    public class BindableDataGrid : DataGrid
    {
        public static readonly DependencyProperty ColumnsBindingProperty =
            DependencyProperty.Register(
                "ColumnsBinding", 
                typeof(IEnumerable<DataGridColumn>), 
                typeof(BindableDataGrid), 
                new PropertyMetadata(OnColumnsBindingChanged));

        public IEnumerable<DataGridColumn> ColumnsBinding
        {
            get { return (IEnumerable<DataGridColumn>)GetValue(ColumnsBindingProperty); }
            set { SetValue(ColumnsBindingProperty, value); }
        }

        private static void OnColumnsBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableDataGrid)d).OnColumnsBindingChanged(e);
        }

        private void OnColumnsBindingChanged(DependencyPropertyChangedEventArgs e)
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
