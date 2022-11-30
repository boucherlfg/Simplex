using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    // a1x  b1y  c1z  e1  0   0   u 
    // a2x  b2y  c2z  0   e2  0   v        
    // a3x  b3y  c3z  0   0   e3  w
    public class SimplexObject
    {
        public readonly int width, height, ruleCount, varCount;
        readonly double[] array;
        public SimplexObject(int ruleCount, int varCount)
        {
            this.ruleCount = ruleCount;
            this.varCount = varCount;
            width = ruleCount + varCount + 1; //one e per rule + variable count + answer
            height = ruleCount + 1; //one line per rule + maximizing equation
            array = new double[width * height];
            for (int i = 0; i < ruleCount; i++)
            {
                this[varCount + i, i] = 1; // initialize e variables
            }
        }

        /// <summary>
        /// this is the pivot row (y axis refers to a row)
        /// </summary>
        public int PivotY
        {
            get
            {
                int pivotX = PivotX;
                int bestIndex = 0;
                double bestValue = double.MaxValue;
                for (int i = 0; i < height - 1; i++)
                {
                    double a = this[width - 1, i];
                    double b = this[pivotX, i];
                    double value = a / b;
                    if (value < bestValue && value > 0)
                    {
                        bestIndex = i;
                        bestValue = value;
                    }
                }
                return bestIndex;
            }
        }

        /// <summary>
        /// this is the pivot column (x axis refers to a column)
        /// </summary>
        public int PivotX
        {
            get
            {
                int bestIndex = 0;
                double bestValue = double.MinValue;
                for (int i = 0; i < width - 1; i++)
                {
                    if (this[i, height - 1] > bestValue)
                    {
                        bestValue = this[i, height - 1];
                        bestIndex = i;
                    }
                }
                return bestIndex;
            }
        }

        
        /// <summary>
        /// gets or sets the value of a specific line (useful for defining constraints)
        /// </summary>
        public double[] this[int line]
        {
            get
            {
                double[] theLine = new double[width];
                for (int i = 0; i < width; i++)
                {
                    theLine[i] = this[i, line];
                }
                return theLine;
            }
            set
            {
                for (int i = 0; i < width; i++)
                {
                    this[i, line] = value[i];
                }
            }
        }
        /// <summary>
        /// sets the maximizing equation (last line of the matrix)
        /// </summary>
        public double[] Max
        {
            get
            {
                double[] maximizing = new double[width];
                for (int i = 0; i < width; i++)
                {
                    maximizing[i] = this[i, height - 1];
                }
                return maximizing;
            }
            set
            {
                for (int i = 0; i < width; i++)
                {
                    this[i, height - 1] = value[i];
                }
            }
        }

        /// <summary>
        /// sets or gets value of element at position (i, j)
        /// </summary>
        public double this[int i, int j]
        {
            get => array[i + j * width];
            set => array[i + j * width] = value;
        }

        public override string ToString()
        {
            string ret = "";

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    ret += this[i, j] + " ";
                }
                ret += "\n";
            }

            return ret;
        }

        /// <summary>
        /// check if there exists an element with positive value on maximizing line
        /// </summary>
        public bool ShouldContinue()
        {
            for (int i = 0; i < this.width; i++)
            {
                if (this[i, this.height - 1] > 0) return true;
            }
            return false;
        }

        /// <summary>
        /// 1. get pivot column, 2. get pivot row, 3. divide elements of pivot row by pivot number, 4. watch the code. Kind of hard to explain in a one liner.
        /// </summary>
        public void Iterate()
        {
            //étape 1
            int pivotX = PivotX;

            //étape 2
            int pivotY = PivotY;
            
            //étape 3
            double pivotN = this[pivotX, pivotY];
            for (int i = 0; i < this.width; i++)
            {
                this[i, pivotY] /= pivotN;
            }

            //étape 4
            for (int j = 0; j < this.height; j++)
            {
                if (j == pivotY) continue;

                double m = this[pivotX, j];
                for (int i = 0; i < this.width; i++)
                {
                    this[i, j] -= m * this[i, pivotY];
                }
            }
        }

    }
}
