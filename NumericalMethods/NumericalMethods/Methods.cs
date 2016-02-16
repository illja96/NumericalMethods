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
        public static double[] Gauss_with_main_element(double[,] matrix, int size)
        {
            bool has_solve = true;

            double[] roots = new double[size];

            double main = 0;

            int m = 0;int i = 0;int j = 0;

            double bb = 0;

            for (int k = 0; k < size; k++) //searching of max element absolute value in the first column
            {
                main = Math.Abs(matrix[k,k]);
                i = k;
                for (m = k + 1; m < size; m++)
                    if (Math.Abs(matrix[m,k]) > main)
                    {
                        i = m;
                        main = Math.Abs(matrix[m,k]);
                    }

                if (main == 0)  //does the system has solves
                {
                    has_solve = false;
                }

                if (i != k)  //  перестановка i-ой строки, содержащей главный элемент k-ой строки
                {
                    for (j = k; j < size + 1; j++)
                    {
                        bb = matrix[k,j];
                        matrix[k,j] = matrix[i,j];
                        matrix[i,j] = bb;
                    }
                }
                main = matrix[k,k];//преобразование k-ой строки
                matrix[k,k] = 1;

                for (j = k + 1; j < size + 1; j++)
                    matrix[k,j] = matrix[k,j] / main;

                for (i = k + 1; i < size; i++)//преобразование строк с помощью k-ой строки
                {
                    bb = matrix[i,k];
                    matrix[i,k] = 0;
                    if (bb != 0)
                        for (j = k + 1; j < size + 1; j++)
                            matrix[i,j] = matrix[i,j] - bb * matrix[k,j];
                }
            }        

            for (i = size-1; i >= 0; i--)   //Search roots
            {
                roots[i] = 0;
                main = matrix[i,size];
                for (j = size-1; j > i; j--)
                    main = main - matrix[i,j] * roots[j];
                roots[i] = main;
            }

            if (has_solve == true)
                return roots; //roots
            else return null; //system hasn`t got any solves
        }

        public static void Gauss_seidel()
        {

        }
    }
}
