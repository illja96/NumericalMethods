using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace NumericalMethodsTest
{
    [TestClass]
    public class UnitTest_Lab1
    {
        [TestMethod]
        public void Gauss_main_test()
        {
            double[][] P = new double[4][];
            P[0] = new double[] { 52.373, 10, 2.2, 1, 291.0006 };
            P[1] = new double[] { 41.84, 6.5, 1, 0, 239.605 };
            P[2] = new double[] { 37.64, 6.55, 0.5, 0, 220.7825 };
            P[3] = new double[] { 57.56, 10.2, 2, 0, 321.93 };

            double[] lambda = NumericalMethods.Methods.Lab1.Gauss_main(P);

            double[] lambda_done = new double[] { 6, 0.2, -12.735, 2.7616 };

            for (int i = 0; i < lambda.Count(); i++)
                Assert.AreEqual(Math.Round(lambda[i], 1), Math.Round(lambda_done[i], 1));
        }
    }

    [TestClass]
    public class UnitTest_Lab4
    {
        [TestMethod]
        public void Matrix_multiplication_test1()
        {
            double[][] a = new double[2][];
            a[0] = new double[] { 1, 3, 2 };
            a[1] = new double[] { 0, 4, -1 };

            double[][] b = new double[3][];
            b[0] = new double[] { 2, 0, -1, 1 };
            b[1] = new double[] { 3, -2, 1, 2 };
            b[2] = new double[] { 0, 1, 2, 3 };

            double[][] c = NumericalMethods.Methods.Lab3.Matrix_multiplication(a, b);

            double[][] c_done = new double[2][];
            c_done[0] = new double[] { 11, -4, 6, 13 };
            c_done[1] = new double[] { 12, -9, 2, 5 };

            for (int i = 0; i < c.Count(); i++)
            {
                for (int j = 0; j < c[0].Count(); j++)
                    Assert.AreEqual(c[i][j], c_done[i][j]);
            }
        }

        [TestMethod]
        public void Matrix_multiplication_test2()
        {
            double[][] a = new double[3][];
            a[0] = new double[] { 2, 4, 0 };
            a[1] = new double[] { -2, 1, 3 };
            a[2] = new double[] { -1, 0, 1 };

            double[] b = new double[] { 1, 2, -1 };

            double[] c = NumericalMethods.Methods.Lab3.Matrix_multiplication(a, b);

            double[] c_done = new double[] { 10, -3, -2 };

            for (int i = 0; i < c.Count(); i++)
                Assert.AreEqual(c[i], c_done[i]);
        }
    }
}