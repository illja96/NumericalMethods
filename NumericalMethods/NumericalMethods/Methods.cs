using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NumericalMethods
{
    public abstract class Methods
    {
        public static double[] Gauss_with_main_element(double[][] matrix)
        {
            int size = matrix.Length;

            double main = 0;
            int m = 0;
            int i = 0;
            int j = 0;
            double bb = 0;

            //Поиск максимального элемента в первом столбце
            for (int k = 0; k < size; k++)
            {
                main = Math.Abs(matrix[k][k]);
                i = k;

                for (m = k + 1; m < size; m++)
                {
                    if (Math.Abs(matrix[m][k]) > main)
                    {
                        i = m;
                        main = Math.Abs(matrix[m][k]);
                    }
                }

                //Проверка на совместность
                if (main == 0)
                    return null;

                //Перестановка i-ой строки, содержащей главный элемент k-ой строки
                if (i != k)
                {
                    for (j = k; j < size + 1; j++)
                    {
                        bb = matrix[k][j];
                        matrix[k][j] = matrix[i][j];
                        matrix[i][j] = bb;
                    }
                }

                //Преобразование k-ой строки
                main = matrix[k][k];
                matrix[k][k] = 1;

                for (j = k + 1; j < size + 1; j++)
                    matrix[k][j] = matrix[k][j] / main;

                //Преобразование строк с помощью k-ой строки
                for (i = k + 1; i < size; i++)
                {
                    bb = matrix[i][k];
                    matrix[i][k] = 0;

                    if (bb != 0)
                    {
                        for (j = k + 1; j < size + 1; j++)
                            matrix[i][j] = matrix[i][j] - bb * matrix[k][j];
                    }
                }
            }

            //Поиск корней
            double[] roots = new double[size];
            for (i = size - 1; i >= 0; i--)
            {
                roots[i] = 0;
                main = matrix[i][size];

                for (j = size - 1; j > i; j--)
                    main = main - matrix[i][j] * roots[j];

                roots[i] = main;
            }

            return roots;
        }

        public static double[] Gauss_seidel(double[][] matrix, double accuracy)
        {
            int size = matrix.Length;

            double[] prev_solves = new double[matrix.Length];
            double[] next_solves = new double[matrix.Length];
            double[] b = new double[matrix.Length];

            for (int i = 0; i < size; i++)
            {
                next_solves[i] = 0;
            }

            for (int i = 0; i < size; i++)
            {
                b[i] = matrix[i][size + 1];
            }

            do
            {
                for (int i = 0; i < size + 1; i++)
                    prev_solves[i] = next_solves[i];

                for (int i = 0; i < size + 1; i++)
                {
                    double var = 0;
                    for (int j = 0; j < i; j++)
                        var += (matrix[i][j] * next_solves[j]);
                    for (int j = i + 1; j < size; j++)
                        var += (matrix[i][j] * prev_solves[j]);
                    next_solves[i] = (b[i] - var) / matrix[i][i];
                }
            }
            while (!Сonverge(next_solves, prev_solves, size, accuracy));


            //  double[] roots = new double[matrix.Length];

            return next_solves;
        }

        public static bool Сonverge(double[] next_solves, double[] prev_solves, int size, double accuracy)
        {
            double norm = 0;
            for (int i = 0; i < size; i++)
            {
                norm += (next_solves[i] - prev_solves[i]) * (next_solves[i] - prev_solves[i]);
            }
            if (Math.Sqrt(norm) >= accuracy)
                return false;
            return true;
        }

        public static bool DiagonallyDominant(double[][] matrix)
        {
            bool diagonal = true;
            for (int i = 0; i < matrix.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (i != j)
                    {
                        sum += Math.Abs(matrix[i][j]);
                    }
                }
                if (Math.Abs(matrix[i][i]) < sum)
                {
                    diagonal = false;
                    break;
                }
            }
            return diagonal;
        }
    }
}
