using AdventOfCode.Common;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Grid2Visualizer
{
    public class Grid2DataProvider : NotifyPropertyChanged
    {
        private readonly IVisualizerObjectProvider2 objectProvider;
        private NotifyProperty<bool> isLoading;
        private NotifyProperty<Point2> bounds;
        private Grid2<RemoteValue> grid;
        private IGrid2 remoteGrid;
        private Grid2<Task> tasks;
        private int runningTasks;

        public Grid2DataProvider(IVisualizerObjectProvider2 objectProvider)
        {
            this.objectProvider = objectProvider;
            this.isLoading = new NotifyProperty<bool>(nameof(IsLoading), this);
            this.bounds = new NotifyProperty<Point2>(nameof(Bounds), this, Point2.Zero);
        }

        public bool IsLoading => this.isLoading.Value;

        public Point2 Bounds => this.bounds.Value;

        public object this[int x, int y] => this.grid[x, y];

        internal async Task InitializeAsync()
        {
            Dispatcher.CurrentDispatcher.VerifyAccess();

            this.isLoading.Value = true;

            await Task.Run(() =>
            {
                // Fetch the bounds of the grid from the remote side
                this.remoteGrid = (IGrid2)this.objectProvider.GetObject();

                // Create the grid of tasks which fetch chunks of data from the remote side
                Point2 bounds = this.remoteGrid.Bounds;
                int tasksX = (bounds.X % 100 > 0) ? (bounds.X / 100) + 2 : (bounds.X / 100) + 1;
                int tasksY = (bounds.Y % 100 > 0) ? (bounds.Y / 100) + 2 : (bounds.Y / 100) + 1;
                this.tasks = new Grid2<Task>(tasksX, tasksY);

                // Create the grid of RemoteValue objects for the view to bind to
                this.grid = new Grid2<RemoteValue>(bounds);

                foreach (Point2 p in this.grid.Points)
                {
                    this.grid[p] = new RemoteValue(this, p);
                }
            })
            .ConfigureAwait(true);

            this.bounds.Value = this.grid.Bounds;
            this.isLoading.Value = false;
        }

        private void EnsureLoading(Point2 point)
        {
            Dispatcher.CurrentDispatcher.VerifyAccess();

            Point2 taskPoint = point / 100;

            if (this.tasks[taskPoint] == null)
            {
                if (++this.runningTasks == 1)
                {
                    this.isLoading.Value = true;
                }

                this.tasks[taskPoint] = Task.Run(() =>
                {
                    Thread.Sleep(3000);
                })
                .ContinueWith(t =>
                {
                    Point2 origin = taskPoint * 100;

                    foreach (Point2 p in Point2.Quadrant(Point2.Zero + 100))
                    {
                        Point2 cell = origin + p;
                        this.grid[cell].Value = this.remoteGrid[cell];
                    }

                    if (--this.runningTasks == 0)
                    {
                        this.isLoading.Value = false;
                    }
                }, 
                TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private class RemoteValue : NotifyPropertyChanged
        {
            private readonly Grid2DataProvider provider;
            private readonly Point2 point;
            private readonly NotifyProperty<object> value;

            internal RemoteValue(Grid2DataProvider provider, Point2 point)
            {
                this.provider = provider;
                this.point = point;
                this.value = new NotifyProperty<object>(nameof(Value), this);
            }

            public object Value
            {
                get
                {
                    this.provider.EnsureLoading(this.point);
                    return this.value.Value;
                }
                set
                {
                    this.value.Value = value;
                }
            }
        }
    }
}
