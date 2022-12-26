using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Common
{
    public class VirtualGrid2<T>
    {
        private readonly Dictionary<Point2, T> grid;
        private readonly Dictionary<int, T> virtualRows;
        private readonly Dictionary<int, T> virtualColumns;
        private Point2 min = Point2.Zero;
        private Point2 max = Point2.Zero;
        private Point2 offset = Point2.Zero;

        public VirtualGrid2()
        {
            this.grid = new Dictionary<Point2, T>();
            this.virtualRows = new Dictionary<int, T>();
            this.virtualColumns = new Dictionary<int, T>();
        }

        private VirtualGrid2(VirtualGrid2<T> other, Point2 offset)
        {
            this.grid = other.grid;
            this.virtualRows = other.virtualRows;
            this.virtualColumns = other.virtualColumns;
            this.offset = other.offset + offset;
            this.min = other.min + offset;
            this.max = other.max + offset;
        }

        public T this[int x, int y]
        {
            get => this[new Point2(x, y)];
            set => this[new Point2(x, y)] = value;
        }

        public T this[Point2 point]
        {
            get
            {
                T t;
                point -= this.offset;
                return
                    (this.virtualRows.TryGetValue(point.Y, out t)) ? t :
                    (this.virtualColumns.TryGetValue(point.X, out t)) ? t :
                    (this.grid.TryGetValue(point, out t)) ? t : 
                        default(T);
            }
            set
            {
                point -= this.offset;
                if (this.grid.ContainsKey(point))
                {
                    this.grid[point] = value;
                }
                else
                {
                    this.grid.Add(point, value);
                }

                this.min = Point2.Min(this.min, point);
                this.max = Point2.Max(this.max, point);
            }
        }

        public IEnumerable<Point2> Points => this.grid.Keys.Select(k => k + offset);

        public Point2 MinPoint => this.min;

        public Point2 MaxPoint => this.max;

        public void AddVirtualRow(int y, T value) => this.virtualRows.Add(y, value);

        public void AddVirtualColumn(int x, T value) => this.virtualColumns.Add(x, value);

        public VirtualGrid2<T> Shift(Point2 offset) => new VirtualGrid2<T>(this, offset);

        private bool HasPoint(Point2 point)
        {
            return this.virtualRows.ContainsKey(point.Y) || this.virtualColumns.ContainsKey(point.X) || this.grid.ContainsKey(point);
        }

        public bool Intersects(VirtualGrid2<T> grid)
        {
            if (this.virtualRows.Any() || this.virtualColumns.Any())
            {
                throw new InvalidOperationException();
            }

            foreach (Point2 point in this.Points)
            {
                if (grid.HasPoint(point))
                {
                    return true;
                }
            }

            return false;
        }

        public void Add(VirtualGrid2<T> other)
        {
            if (other.virtualRows.Any() || other.virtualColumns.Any())
            {
                throw new InvalidOperationException();
            }

            foreach (Point2 point in other.Points)
            {
                this[point] = other[point];
            }
        }

        public void Add(Point2 point, T value) => this.grid.Add(point, value);
        public bool Remove(Point2 point) => this.grid.Remove(point);
    }
}
