using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Grid2Visualizer
{
    public class Grid2DataGridColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(IEnumerable<DataGridColumn>))
            {
                throw new ArgumentException("Invalid target type");
            }

            List<DataGridColumn> columns = new List<DataGridColumn>();

            if (value != null)
            {
                if (!(value is IEnumerable<Grid2ColumnViewModel> viewModels))
                {
                    throw new ArgumentException("Invalid value");
                }

                foreach (Grid2ColumnViewModel viewModel in viewModels)
                {
                    DataGridColumn column = new DataGridTextColumn()
                    {
                        Binding = new Binding($"{ nameof(Grid2RowViewModel.Cells) }[{ viewModel.Column }].Value")
                    };

                    BindingOperations.SetBinding(
                        column, 
                        DataGridColumn.HeaderProperty,
                        new Binding(nameof(Grid2ColumnViewModel.Header))
                        {
                            Source = viewModel,
                        });

                    columns.Add(column);
                }
            }

            return columns;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
