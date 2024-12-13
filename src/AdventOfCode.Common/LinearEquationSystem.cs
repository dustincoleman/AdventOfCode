using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Solve()
        {
            if (this.equations.Count + 1 != this.equationLength)
            {
                throw new InvalidOperationException("Equation count does not match unknowns");
            }

            // Reduce to row echelon form
            int pivotRow = 0;
            int pivotColumn = 0;

            while (pivotRow < this.equationLength - 1 && pivotColumn < this.equationLength)
            {
                int rowMax = RowMax(pivotRow, pivotColumn, equations);

                if (equations[rowMax][pivotColumn] == 0)
                {
                    pivotColumn++;
                }
                else
                {
                    (equations[pivotRow], equations[rowMax]) = (equations[rowMax], equations[pivotRow]);

                    for (int row = pivotRow + 1; row < this.equations.Count; row++)
                    {
                        double div = equations[row][pivotColumn] / equations[pivotRow][pivotColumn];

                        equations[row][pivotColumn] = 0;

                        for (int col = pivotColumn + 1; col < this.equationLength; col++)
                        {
                            equations[row][col] = equations[row][col] - equations[pivotRow][col] * div;
                        }
                    }

                    pivotRow++;
                    pivotColumn++;
                }
            }

            // Solve
            for (int pivot = this.equations.Count - 1; pivot >= 0; pivot--)
            {
                LinearEquation eq = equations[pivot];

                for (int col = this.equations.Count - 1; col > pivot; col--)
                {
                    double subst = eq[col] * equations[col].Answer;
                    eq.Answer -= subst;
                    eq[col] = 0;
                }

                long answer = Convert.ToInt64(eq.Answer / eq[pivot]);
                eq[pivot] = 1;
                eq.Answer = Convert.ToDouble(answer);
            }
        }
        private int RowMax(int pivotRow, int pivotColumn, List<LinearEquation> equations)
        {
            int rowMax = pivotRow;
            double max = 0;

            for (int row = pivotRow; row < equations.Count; row++)
            {
                double value = Math.Abs(equations[row][pivotColumn]);
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
