﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NumericalMethods
{
    public abstract class Methods
    {
        public abstract class Lab1
        {
            public static double[] Gauss_main(double[][] matrix)
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

            public static double[] Gauss_seidel(double[][] x, double[] b, double eps)
            {
                double[] current_solution = new double[x.Length];
                double[] previous_solution = new double[x.Length];

                do
                {
                    for (int i = 0; i < x.Length; i++)
                        previous_solution[i] = current_solution[i];

                    for (int i = 0; i < x.Length; i++)
                    {
                        double var = 0;

                        for (int j = 0; j < i; j++)
                            var += (x[i][j] * current_solution[j]);

                        for (int j = i + 1; j < x.Length; j++)
                            var += (x[i][j] * previous_solution[j]);

                        current_solution[i] = (b[i] - var) / x[i][i];
                    }
                }
                while (!Gauss_seidel_condition(current_solution, previous_solution, eps));

                return current_solution;
            }
            private static bool Gauss_seidel_condition(double[] current_solution, double[] previous_solution, double eps)
            {
                double norm = 0;
                for (int i = 0; i < current_solution.Length; i++)
                    norm += (current_solution[i] - previous_solution[i]) * (current_solution[i] - previous_solution[i]);

                if (Math.Sqrt(norm) >= eps)
                    return false;

                return true;
            }
        }

        public abstract class Lab2
        {
            public static double Chords(Func<double, double> function, double a, double b, double eps)
            {
                while (Math.Abs(b - a) > eps)
                {
                    a = b - (b - a) * function(b) / (function(b) - function(a));
                    b = a + (a - b) * function(a) / (function(a) - function(b));
                }

                return b;
            }

            public static double Newton(Func<double, double> function, Func<double, double> d_function, double x0, double eps)
            {
                double x1 = x0 - function(x0) / d_function(x0);
                while (Math.Abs(x0 - x1) > eps)
                {
                    x0 = x1;
                    x1 = x1 - function(x1) / d_function(x1);
                }

                return x1;
            }
        }

        public static double[] Gauss_with_main_element(double[][] matrix)
        {
            if (Is_diagonally_dominant(matrix) == false)
                return null;

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

        public static double[] Verify_Gauss_with_main_element(double[][] matrix, double[] roots)
        {
            double[] right_side = new double[matrix.Length];

            double sum = 0;

            for (int i = 0; i < matrix.Length; i++)
            {
                sum = 0;
                for (int j = 0; j < matrix.Length; j++)
                {
                    sum += matrix[i][j] * roots[j];
                }

                right_side[i] = sum;
            }

            return right_side;
        }

        public static double[] Gauss_seidel(double[][] matrix, double accuracy)
        {
            int size = matrix.Length;

            double maxIter = 0;
            //   

            double[][] B = new double[size][];
            double[][] d = new double[size][];

            for (int i = 0; i < size; i++)
            {
                B[i] = new double[size];
                d[i] = new double[1];
            }

            if (Is_diagonally_dominant(matrix) == true)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (i == j)
                        {
                            B[i][j] = 0;
                        }
                        else
                        {
                            B[i][j] = (-matrix[i][j]) / matrix[i][i];
                        }
                    }

                    d[i][0] = matrix[i][size] / matrix[i][i];
                }

                if (Norma(B) > 1)
                {
                    maxIter = ((1 / (Math.Log10(Norma(B)))) * (Math.Log10(accuracy) - Math.Log10(Norma(d)) + Math.Log10(1 - Norma(B)))) - 1;
                }
                else
                {
                    return null;
                }

                double[] prev_solves = new double[size];
                double[] next_solves = new double[size];

                for (int i = 0; i < size; i++)
                {
                    prev_solves[i] = 0;
                    next_solves[i] = 0;
                }

                //algorithm


                return next_solves;

            }
            else
            {
                return null;
            }
        }
        public static double Norma(double[][] matrix)
        {
            double norma = 0;
            double max = 0;

            for (int i = 0; i < matrix.Length; i++)
            {
                max = 0;
                for (int j = 0; j < matrix[i].Length - 1; j++)
                {
                    max += Math.Abs(matrix[i][j]);
                }
                if (max > norma)
                {
                    norma = max;
                }

            }
            return norma;
        }

        public static bool Is_diagonally_dominant(double[][] matrix)
        {
            double sum = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                sum = 0;
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (i != j)
                        sum += Math.Abs(matrix[i][j]);
                }

                if (Math.Abs(matrix[i][i]) < sum)
                    return false;
            }

            return true;
        }
    }
}
