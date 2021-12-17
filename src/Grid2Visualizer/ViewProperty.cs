using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid2Visualizer
{
    internal class ViewProperty<T>
    {
        private readonly string name;
        private readonly ViewModelBase owner;
        private T value;

        internal ViewProperty(string name, ViewModelBase owner)
        {
            this.name = name;
            this.owner = owner;
        }

        internal ViewProperty(string name, ViewModelBase owner, T initialValue)
            : this(name, owner)
        {
            this.value = initialValue;
        }

        internal T Value
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
                    this.owner.NotifyPropertyChanged(this.name);
                }
            }
        }
    }
}
