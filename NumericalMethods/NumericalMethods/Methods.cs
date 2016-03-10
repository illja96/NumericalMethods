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
            if (Is_diagonally_dominant(matrix) == false)
                Make_diagonally_dominant(ref matrix);

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

            double maxIter = 0;
            //   

            double[][] B = new double[size][];
            double[][] d = new double[size][];

            for (int i = 0; i < size; i++)
            {
                B[i] = new double[size];
                d[i] = new double[1];
            }

            if (Diagonally_dominant(matrix) == true)
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

                if (Norma(B) < 1)
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

                int k = 0;
                double diff = 0;
                double s = 0;
                double Xi = 0;

                while ((k <= maxIter) && (diff >= accuracy))
                {
                    k = k + 1;
                    for (int i = 0; i < size; i++)
                    {
                        s = 0;
                        for (int j = 0; j < size; j++)
                        {
                            if (i != j)
                            {
                                s += matrix[i][j] * next_solves[j];
                            }
                        }
                        Xi = (prev_solves[i] - s) / matrix[i][i];
                        diff = Math.Abs(Xi - next_solves[i]);
                        next_solves[i] = Xi;
                    }
                }

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

        public static bool Diagonally_dominant(double[][] matrix)
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

        public static bool Is_diagonally_dominant(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (Is_diagonally_dominant(matrix[i], i) == false)
                    return false;
            }

            return true;
        }
        public static bool Is_diagonally_dominant(double[] row, int row_count)
        {
            if (row_count >= row.Length)
                return false;

            double total = 0;
            for (int i = 0; i < row.Length; i++)
            {
                if (i != row_count)
                    total += Math.Abs(row[i]);
            }

            if (total >= Math.Abs(row[row_count]))
                return false;

            return true;
        }
        public static int Diagonally_dominant_element(double[] row)
        {
            double total = 0;
            for (int i = 0; i < row.Length; i++)
                total += Math.Abs(row[i]);

            for (int i = 0; i < row.Length; i++)
            {
                if (Math.Abs(row[i]) > total - Math.Abs(row[i]))
                    return i;
            }

            return -1;
        }

        public static bool Make_diagonally_dominant(ref double[][] matrix)
        {
            if (Is_diagonally_dominant(matrix) == true)
                return true;

            for (int i = 0; i < matrix.Length; i++)
            {
                int now_max = Diagonally_dominant_element(matrix[i]);
                if (now_max == -1)
                    continue;

                if (Is_diagonally_dominant(matrix[now_max], now_max) == false)
                {
                    Replace_row(ref matrix[now_max], ref matrix[i]);
                    i = 0;
                }
            }

            if (Is_diagonally_dominant(matrix) == true)
                return true;

            for (int i = 0; i < matrix.Length; i++)
            {
                if (Is_diagonally_dominant(matrix[i], i) == true)
                    continue;

                for (int j = 0; j < matrix.Length; j++)
                {
                    if (i == j)
                        continue;

                    if (Make_diagonally_dominant(ref matrix[i], i, ref matrix[j]) == true)
                        break;
                }

                if (Is_diagonally_dominant(matrix[i], i) == false)
                    return false;
            }

            return true;
        }
        public static bool Make_diagonally_dominant(ref double[] row, int row_count, ref double[] side_row)
        {
            if (side_row.Length != row.Length)
                return false;

            if (Is_diagonally_dominant(row, row_count) == true)
                return true;

            if (side_row[row_count] == 0)
            {
                if (row[row_count] == 0)
                    return false;
                else
                {
                    for (int i = 0; i < row.Length; i++)
                        side_row[i] += row[i];
                }
            }

            for (int i = 0; i < row.Length; i++)
            {
                if (i == row_count)
                    continue;

                bool reserve = false;
                if (side_row[i] * row[i] < 0)
                    reserve = true;

                double row_multiplier = side_row[i];
                double side_row_multiplier = row[i];
                for (int j = 0; j < row.Length; j++)
                {
                    row[j] *= side_row_multiplier;
                    side_row[j] *= row_multiplier;

                    row[j] += (reserve == true ? -1 : 1) * side_row[j];
                }
            }

            return true;
        }
        public static void Replace_row(ref double[] to_replace, ref double[] from_replace)
        {
            double[] temp = new double[to_replace.Length];
            temp = to_replace;
            to_replace = from_replace;
            from_replace = temp;
        }

        public static bool Make_matrix_whichout_zero(ref double[][] matrix)
        {
            bool[] matrix_element = new bool[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
                matrix_element[i] = false;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (matrix[i][j] != 0)
                        matrix_element[j] = true;
                }
            }
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix_element[i] == false)
                    return false;
            }
            
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        for (int k = 0; k < matrix.Length; k++)
                        {
                            if (k == i)
                                continue;

                            if (matrix[k][j] != 0)
                            {
                                for (int kj = 0; kj < matrix.Length; kj++)
                                    matrix[i][kj] += matrix[k][kj];

                                break;
                            }
                        }

                        i = 0;
                        break;
                    }
                }
            }

            return true;
        }
    }
}
