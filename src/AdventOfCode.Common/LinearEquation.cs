using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public class LinearEquation
    {
        private double[] coefficients;

        public LinearEquation(params double[] coefficients)
        {
            if (coefficients.Length == 0) throw new ArgumentException("At least one coefficient is required.");
            this.coefficients = coefficients;
        }

        public double this[int index]
        {
            get => this.coefficients[index];
            set => this.coefficients[index] = value;
        }

        public int Length => this.coefficients.Length;

        public double Answer
        {
            get => this.coefficients[this.coefficients.Length - 1];
            set => this.coefficients[this.coefficients.Length - 1] = value;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            char name = 'A';

            for (int i = 0; i < this.coefficients.Length - 1; i++)
            {
                if (name > 'Z')
                {
                    name = '?';
                }
                if (i < this.coefficients.Length - 2)
                {
                    sb.Append($"{name}:{this.coefficients[i]:0.0000}, ");
                }
                else
                {
                    sb.Append($"{name}:{this.coefficients[i]:0.0000} = ");
                }

                name++;
            }

            sb.Append($"{this.coefficients[this.coefficients.Length - 1]:0.0000}");
            return sb.ToString();
        }
    }
}
