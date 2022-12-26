using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class Cube<T>
    {
        private readonly Grid2<T> _front;
        private readonly Grid2<T> _bottom;
        private readonly Grid2<T> _back;
        private readonly Grid2<T> _top;
        private readonly Grid2<T> _left;
        private readonly Grid2<T> _right;

        public Cube(int order)
        {
            _front = new Grid2<T>(order, order);
            _bottom = new Grid2<T>(order, order);
            _back = new Grid2<T>(order, order);
            _top = new Grid2<T>(order, order);
            _left = new Grid2<T>(order, order);
            _right = new Grid2<T>(order, order);

            Order = order;
        }

        private Cube(Grid2<T> front, Grid2<T> bottom, Grid2<T> back, Grid2<T> top, Grid2<T> left, Grid2<T> right)
        {
            _front = front;
            _bottom = bottom;
            _back = back;
            _top = top;
            _left = left;
            _right = right;

            Order = front.Bounds.X;
        }

        public T this[int x, int y]
        {
            get => _front[x, y];
            set => _front[x, y] = value;
        }

        public T this[Point2 point]
        {
            get => _front[point];
            set => _front[point] = value;
        }

        public Grid2<T> Face => _front;

        public int Order { get; }

        public Cube<T> RotateLeft()
        {
            return new Cube<T>(
                front: _right,
                bottom: _bottom.RotateCCW(),
                back: _left,
                top: _top.Rotate(),
                left: _front,
                right: _back
            );
        }

        public Cube<T> RotateRight()
        {
            return new Cube<T>(
                front: _left,
                bottom: _bottom.Rotate(),
                back: _right,
                top: _top.RotateCCW(),
                left: _back,
                right: _front
            );
        }

        public Cube<T> RotateUp()
        {
            return new Cube<T>(
                front: _bottom,
                bottom: _back.Rotate180(),
                back: _top.Rotate180(),
                top: _front,
                left: _left.RotateCCW(),
                right: _right.Rotate()
            );
        }

        public Cube<T> RotateDown()
        {
            return new Cube<T>(
                front: _top,
                bottom: _front,
                back: _bottom.Rotate180(),
                top: _back.Rotate180(),
                left: _left.Rotate(),
                right: _right.RotateCCW()
            );
        }
    }
}
