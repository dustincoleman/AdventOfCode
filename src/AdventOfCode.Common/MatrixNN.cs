using System;

namespace AdventOfCode.Common
{
    public class MatrixNN<T>
    {
        private int _n;
        private T[,] _array;

        public MatrixNN(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

            _n = n;
            _array = new T[n, n];
        }

        public T this[int x, int y]
        {
            get => _array[x, y];
            set => _array[x, y] = value;
        }

        public void FlipVertically()
        {
            int halfSize = _n / 2;
            int maxIndex = _n - 1;

            for (int x = 0; x < _n; x++)
            {
                for (int y = 0; y < halfSize; y++)
                {
                    T tempBool = _array[x, y];
                    _array[x, y] = _array[x, maxIndex - y];
                    _array[x, maxIndex - y] = tempBool;
                }
            }
        }

        public void FlipHorizontally()
        {
            int halfSize = _n / 2;
            int maxIndex = _n - 1;

            for (int y = 0; y < _n; y++)
            {
                for (int x = 0; x < halfSize; x++)
                {
                    T tempBool = _array[x, y];
                    _array[x, y] = _array[maxIndex - x, y];
                    _array[maxIndex - x, y] = tempBool;
                }
            }
        }

        public void Rotate()
        {
            int halfSize = _n / 2;
            int maxIndex = _n - 1;

            for (int x = 0; x < halfSize; x++)
            {
                for (int y = x; y < maxIndex - x; y++)
                {
                    // Store current cell
                    T tempBool = _array[x, y];

                    // Move left to top
                    _array[x, y] = _array[maxIndex - y, x];

                    // Move right to left
                    _array[maxIndex - y, x] = _array[maxIndex - x, maxIndex - y];

                    // Move top to right
                    _array[maxIndex - x, maxIndex - y] = _array[y, maxIndex - x];

                    // Assign temp to right
                    _array[y, maxIndex - x] = tempBool;
                }
            }
        }
    }
}
