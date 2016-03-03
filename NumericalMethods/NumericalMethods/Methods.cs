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

        public static double[] Verify_Gauss_with_main_element(double[][] matrix, double[] roots)
        {
            double[] right_side = new double[matrix.Length];

            double sum = 0;

            for(int i = 0; i < matrix.Length; i++)
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
             

            double[][] B = new double[size][];
            double[][] d = new double[size][];

            for (int i = 0; i < size; i++)
            {
                B[i] = new double[size];
                d[i] = new double[1];
            }         

            if (Diagonally_dominant(matrix) == true)
            {
                for(int i=0;i< size; i++)
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

            for(int i = 0; i < matrix.Length; i++)
            {
                max = 0;
                for (int j = 0; j < matrix[i].Length-1; j++)
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
    }
}
