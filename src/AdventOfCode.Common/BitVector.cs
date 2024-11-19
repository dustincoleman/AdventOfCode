namespace AdventOfCode.Common
{
    public struct BitVector
    {
        private long data;

        public BitVector()
        {
        }

        private BitVector(long data)
        {
            this.data = data;
        }

        public bool this[int shift]
        {
            get
            {
                if (shift >= 64) throw new ArgumentOutOfRangeException();
                return (this.data & 1L << shift) != 0;
            }
            set
            {
                if (shift >= 64) throw new ArgumentOutOfRangeException();
                if (value)
                {
                    this.data |= (1L << shift);
                }
                else
                {
                    this.data &= ~(1L << shift);
                }
            }
        }

        public static implicit operator long(BitVector vector) => vector.data;
        public static implicit operator BitVector(long data) => new BitVector(data);
    }
}
