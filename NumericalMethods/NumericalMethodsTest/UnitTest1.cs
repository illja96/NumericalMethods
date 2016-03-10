using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NumericalMethodsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1()
        {
            double[][] matrix = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                matrix[i] = new double[3];
                for (int j = 0; j < 3; j++)
                    matrix[i][j] = 0;

                matrix[i][i] = 1;
            }

            Assert.IsTrue(NumericalMethods.Methods.Is_diagonally_dominant(matrix));
        }

        [TestMethod]
        public void Test2()
        {
            double[][] matrix = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                matrix[i] = new double[3];
                for (int j = 0; j < 3; j++)
                    matrix[i][j] = 0;

                matrix[i][i] = 1;
            }

            NumericalMethods.Methods.Replace_row(ref matrix[0], ref matrix[2]);

            Assert.IsTrue(matrix[0][2] == 1 && matrix[2][0] == 1);
        }

        [TestMethod]
        public void Test3()
        {
            double[][] matrix = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                matrix[i] = new double[3];
                for (int j = 0; j < 3; j++)
                    matrix[i][j] = 0;

                matrix[i][i] = 1;
            }

            Assert.IsTrue(NumericalMethods.Methods.Diagonally_dominant_element(matrix[1]) == 1);
        }

        [TestMethod]
        public void Test4()
        {
            double[][] matrix = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                matrix[i] = new double[3];
                for (int j = 0; j < 3; j++)
                    matrix[i][j] = 0;

                matrix[i][2 - i] = 1;
            }

            NumericalMethods.Methods.Make_diagonally_dominant(ref matrix);

            Assert.IsTrue(NumericalMethods.Methods.Is_diagonally_dominant(matrix));
        }

        [TestMethod]
        public void Test5()
        {
            return;

            Random rand = new Random();

            double[][] matrix = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                matrix[i] = new double[3];
                for (int j = 0; j < 3; j++)
                    matrix[i][j] = rand.Next(0, 10) * rand.NextDouble();
            }

            while (NumericalMethods.Methods.Is_diagonally_dominant(matrix) == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    matrix[i] = new double[3];
                    for (int j = 0; j < 3; j++)
                        matrix[i][j] = rand.Next(0, 10) * rand.NextDouble();
                }
            }

            NumericalMethods.Methods.Make_diagonally_dominant(ref matrix);

            Assert.IsTrue(NumericalMethods.Methods.Is_diagonally_dominant(matrix));
        }

        [TestMethod]
        public void Test6()
        {
            Random rand = new Random();

            double[][] matrix = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                matrix[i] = new double[3];
                for (int j = 0; j < 3; j++)
                    matrix[i][j] = rand.Next(0, 5);
            }

            if (NumericalMethods.Methods.Make_matrix_whichout_zero(ref matrix) == false)
                return;

            //Делаем диагональное доминирование
            double o_row_multipler, s_row_multipler;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (i == j)
                        continue;

                    o_row_multipler = matrix[i][i];
                    s_row_multipler = matrix[j][i];

                    bool reverse_multipler = false;
                    if (o_row_multipler * s_row_multipler < 0)
                        reverse_multipler = true;

                    for (int k = 0; k < matrix.Length; k++)
                    {
                        matrix[i][k] *= s_row_multipler;
                        matrix[j][k] *= o_row_multipler;
                        matrix[j][k] -= matrix[i][k] * (reverse_multipler == true ? -1 : 1);
                    }

                    if (matrix[j][j] == 0)
                        NumericalMethods.Methods.Make_matrix_whichout_zero(ref matrix);
                }
            }
        }
    }
}
