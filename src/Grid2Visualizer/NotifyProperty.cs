using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    internal class NotifyProperty<T>
    {
        private readonly string name;
        private readonly NotifyPropertyChanged owner;
        private T value;

        internal NotifyProperty(string name, NotifyPropertyChanged owner)
        {
            this.name = name;
            this.owner = owner;
        }

        internal NotifyProperty(string name, NotifyPropertyChanged owner, T initialValue)
            : this(name, owner)
        {
            this.value = initialValue;
        }

        internal virtual T Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(this.value, value))
                {
                    this.value = value;
                    this.owner.OnPropertyChanged(this.name);
                }
            }
        }
    }
}
