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
                if (function(a) * function(b) > 0)
                    return double.NaN;

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

        public abstract class Lab3
        {

            public static double DiagonallySum(double[][] matrix)
            {
                double sum = 0;

                for(int i = 0; i < matrix.Count(); i++)
                {
                    for (int j = 0; j < matrix.Count(); j++)
                    {
                        if (i == j) sum += matrix[i][j];
                    }
                }

                return sum;
            }
        }

        public static double[][] Make_diagonaly_dominant(double[][] matrix)
        {
            if (Is_diagonaly_dominant(matrix) == true)
                return matrix;

            for (int i = 0; i < matrix.Count(); i++)
            {
                int diagonaly_dominant_element = Diagonaly_dominant_element(matrix[i]);

                if (diagonaly_dominant_element == -1 || diagonaly_dominant_element == i)
                    continue;
                else
                {
                    if (Is_diagonaly_dominant(matrix[diagonaly_dominant_element], diagonaly_dominant_element) == true)
                        continue;
                    else
                    {
                        Swap_rows(ref matrix[diagonaly_dominant_element], ref matrix[i]);

                        i = -1;
                    }
                }
            }

            if (Is_diagonaly_dominant(matrix) == true)
                return matrix;

            for (int i = 0; i < matrix.Count(); i++)
            {
                if (Is_diagonaly_dominant(matrix[i], i) == false)
                {
                    if (i != matrix.Count() - 1)
                        Make_diagonaly_dominant(ref matrix[i], matrix[i + 1], i);
                    else
                        Make_diagonaly_dominant(ref matrix[i], matrix[i - 1], i);
                }
            }

            if (Is_diagonaly_dominant(matrix) == false)
                return null;

            return matrix;
        }
        public static bool Is_diagonaly_dominant(double[][] matrix)
        {
            if (matrix == null || matrix.Count() == 0 || matrix.LongCount() == 0)
                return false;

            for (int i = 0; i < matrix.Count(); i++)
            {
                if (Is_diagonaly_dominant(matrix[i], i) == false)
                    return false;
            }

            return true;
        }
        private static bool Is_diagonaly_dominant(double[] row, int count)
        {
            if (row.Count() <= count)
                return false;

            double sum = 0;
            foreach (var e in row)
                sum += Math.Abs(e);

            if (Math.Abs(row[count]) >= sum - Math.Abs(row[count]))
                return true;
            else
                return false;
        }
        private static int Diagonaly_dominant_element(double[] row)
        {
            if (row == null || row.Count() == 0)
                return -1;

            double sum = 0;
            foreach (var e in row)
                sum += Math.Abs(e);

            for (int i = 0; i < row.Count(); i++)
            {
                if (Math.Abs(row[i]) >= sum - Math.Abs(row[i]))
                    return i;
            }

            return -1;
        }
        private static void Swap_rows(ref double[] row_1, ref double[] row_2)
        {
            if (row_1 == null || row_2 == null || row_1.Count() == 0 || row_2.Count() == 0 || row_1.Count() != row_2.Count())
                return;

            double[] row_temp = new double[row_1.Count()];

            for (int i = 0; i < row_1.Count(); i++)
            {
                row_temp[i] = row_1[i];
                row_1[i] = row_2[i];
                row_2[i] = row_temp[i];
            }
        }
        private static void Make_diagonaly_dominant(ref double[] row_to_make, double[] row_from_make, int diagonaly_element)
        {
            if (row_to_make == null || row_from_make == null || row_to_make.Count() == 0 || row_from_make.Count() == 0 || row_to_make.Count() != row_from_make.Count())
                return;

            for (int i = 0; i < row_to_make.Count(); i++)
            {
                if (i == diagonaly_element)
                    continue;

                double row_to_make_multipler = row_from_make[i];
                double row_from_make_multipler = row_to_make[i];

                bool is_minus;
                if ((row_to_make[i] * row_to_make_multipler) - (row_from_make[i] * row_from_make_multipler) == 0)
                    is_minus = true;
                else
                    is_minus = false;

                for (int j = 0; j < row_to_make.Count(); j++)
                {
                    if (is_minus == true)
                        row_to_make[j] = (row_to_make[j] * row_to_make_multipler) - (row_from_make[j] * row_from_make_multipler);
                    else
                        row_to_make[j] = (row_to_make[j] * row_to_make_multipler) + (row_from_make[j] * row_from_make_multipler);
                }
            }
        }
    }
}
