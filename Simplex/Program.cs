﻿using System;
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
            height = ruleCount + 1;
            array = new double[width * height];
            for (int i = 0; i < ruleCount; i++)
            {
                this[varCount + i, i] = 1; // initialize e variables
            }
        }

        public int GetPivotY(int pivotX)
        {
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
        public int GetPivotX()
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

        /// <summary>
        /// adds a rule at line y
        /// </summary>
        /// <param name="y"></param>
        /// <param name="equation"></param>
        /// <param name="answer"></param>
        public void SetRule(int y, double[] equation, double answer)
        {
            if (equation.Length != varCount) throw new Exception("rule must have one value per variable");
            for (int i = 0; i < varCount; i++)
            {
                this[i, y] = equation[i];
            }
            this[width - 1, y] = answer;
        }
        public void SetObjective(double[] objective)
        {
            if (objective.Length != varCount) throw new Exception("objective must have one value per variable");
            for (int i = 0; i < varCount; i++)
            {
                this[i, height - 1] = objective[i];
            }
        }
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
    }
    class Program
    {
        static bool ShouldContinue(SimplexObject t)
        {
            for (int i = 0; i < t.width; i++)
            {
                if (t[i, t.height - 1] > 0) return true;
            }
            return false;
        }
        static void Main(string[] args)
        {
            var tab = new SimplexObject(2, 2);
            tab.SetRule(0, new double[] { 1, -1 }, -1);
            tab.SetRule(1, new double[] { 1, 1 }, 4);
            tab.SetObjective(new double[] { 3, 1 });

            Console.WriteLine(tab);
            Console.ReadKey(true);

            while (ShouldContinue(tab))
            {
                Iterate(tab);
                Console.WriteLine(tab);
                Console.ReadKey(true);
            }


            Console.WriteLine(tab);
            Console.ReadKey(true);

            Console.WriteLine("la réponse est : " + (-tab[tab.width - 1, tab.height - 1]));
            Console.ReadKey(true);
        }
        
        
        static void Iterate(SimplexObject tab)
        {
            int pivotX = tab.GetPivotX();
            int pivotY = tab.GetPivotY(pivotX);
           
            //étape 3
            double pivotN = tab[pivotX, pivotY];
            for (int i = 0; i < tab.width; i++)
            {
                tab[i, pivotY] /= pivotN;
            }

            //étape 4
            for (int j = 0; j < tab.height; j++)
            {
                if (j == pivotY) continue;

                double m = tab[pivotX, j];
                for (int i = 0; i < tab.width; i++)
                {

                    tab[i, j] -= m * tab[i, pivotY];
                }
            }
        }
    }

}