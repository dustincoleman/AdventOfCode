using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    public class Grid2ColumnViewModel : NotifyPropertyChanged
    {
        private readonly int column;
        private NotifyProperty<string> header;

        internal Grid2ColumnViewModel(int column)
        {
            this.column = column;
            this.header = new NotifyProperty<string>(nameof(Header), this, column.ToString());
        }

        public int Column => this.column;

        public string Header
        {
            get => this.header.Value;
            set => this.header.Value = value;
        }
    }
}
