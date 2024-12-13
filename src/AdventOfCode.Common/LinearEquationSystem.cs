using System.Collections;
using System.Numerics;

namespace AdventOfCode.Common
{
    public class LinearEquationSystem : IEnumerable<LinearEquation>
    {
        private List<LinearEquation> equations = new List<LinearEquation>();
        private int equationLength;

        public LinearEquation this[int index] => this.equations[index];

        public void Add(LinearEquation equation)
        {
            if (this.equations.Count == 0)
            {
                this.equationLength = equation.Length;
            }
            if (this.equationLength != equation.Length)
            {
                throw new ArgumentException("Equations must all be of the same length");
            }
            this.equations.Add(equation);
        }

        public bool SolveAsInteger()
        {
            if (this.equations.Count + 1 != this.equationLength)
            {
                throw new InvalidOperationException("Equation count does not match unknowns");
            }

            // Reduce to row echelon form
            int pivot = 0;

            while (pivot < this.equationLength - 1 && pivot < this.equationLength)
            {
                int rowMax = RowMax(pivot, pivot, equations);

                if (equations[rowMax][pivot] == 0)
                {
                    return false;
                }
                else
                {
                    // Swap in the row with the maximum coefficient for the current column
                    (equations[pivot], equations[rowMax]) = (equations[rowMax], equations[pivot]);

                    // Find the least common multiple of all the coefficients in the current column
                    List<BigInteger> coefficients = new List<BigInteger>();
                    for (int row = pivot; row < this.equations.Count; row++)
                    {
                        coefficients.Add(equations[row][pivot]);
                    }
                    BigInteger lcm = MathHelpers.LeastCommonMultiple(coefficients.Where(c => c != 0));

                    // Scale all the rows such that the current column coefficients are the same
                    for (int row = pivot; row < this.equations.Count; row++)
                    {
                        if (equations[row][pivot] != 0)
                        {
                            BigInteger rowMultiple = lcm / equations[row][pivot];
                            for (int col = pivot; col < this.equationLength; col++)
                            {
                                equations[row][col] = equations[row][col] * rowMultiple;
                            }
                        }
                    }

                    // Perform row subtraction to zero the remaining coefficients in the current row
                    for (int row = pivot + 1; row < this.equations.Count; row++)
                    {
                        if (equations[row][pivot] != 0)
                        {
                            for (int col = pivot; col < this.equationLength; col++)
                            {
                                equations[row][col] = equations[row][col] - equations[pivot][col];
                            }
                        }
                    }

                    pivot++;
                }
            }

            // Solve
            for (pivot = this.equations.Count - 1; pivot >= 0; pivot--)
            {
                LinearEquation eq = equations[pivot];

                for (int col = this.equations.Count - 1; col > pivot; col--)
                {
                    BigInteger subst = eq[col] * equations[col].Answer;
                    eq.Answer -= subst;
                    eq[col] = 0;
                }

                if (eq.Answer % eq[pivot] != 0)
                {
                    return false; // Can't solve as an integer
                }

                BigInteger answer = eq.Answer / eq[pivot];
                eq[pivot] = 1;
                eq.Answer = answer;
            }

            return true;
        }
        private int RowMax(int pivotRow, int pivotColumn, List<LinearEquation> equations)
        {
            int rowMax = pivotRow;
            BigInteger max = 0;

            for (int row = pivotRow; row < equations.Count; row++)
            {
                BigInteger value = BigInteger.Abs(equations[row][pivotColumn]);
                if (value > max)
                {
                    rowMax = row;
                    max = value;
                }
            }

            return rowMax;
        }


        public IEnumerator<LinearEquation> GetEnumerator()
        {
            return equations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
