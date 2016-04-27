using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace NumericalMethodsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Gauss_seidel_test()
        {
            double[][] x = new double[3][];
            x[0] = new double[] { 8, 4, 2 };
            x[1] = new double[] { 3, 5, 1 };
            x[2] = new double[] { 3, -2, 10 };

            double[] b = new double[] { 10, 5, 4 };

            double eps = 0.001;

            double[] roots = NumericalMethods.Methods.Lab1.Gauss_seidel(x, b, eps);

            Assert.IsNotNull(roots);
            Assert.IsFalse(roots.Length == 0);
            Assert.IsFalse(roots.Contains(double.NegativeInfinity) || roots.Contains(double.PositiveInfinity) || roots.Contains(double.NaN));
        }
    }
}