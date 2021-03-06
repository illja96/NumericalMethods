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
                int size = matrix.Count();
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
                double[] current_solution = new double[x.Count()];
                double[] previous_solution = new double[x.Count()];

                do
                {
                    for (int i = 0; i < x.Count(); i++)
                        previous_solution[i] = current_solution[i];

                    for (int i = 0; i < x.Count(); i++)
                    {
                        double var = 0;

                        for (int j = 0; j < i; j++)
                            var += (x[i][j] * current_solution[j]);

                        for (int j = i + 1; j < x.Count(); j++)
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
                for (int i = 0; i < current_solution.Count(); i++)
                    norm += (current_solution[i] - previous_solution[i]) * (current_solution[i] - previous_solution[i]);

                if (Math.Sqrt(norm) >= eps)
                    return false;

                return true;
            }
        }

        public abstract class Lab2
        {
            public static double[] Chords_multi(Func<double, double> function, int roots_count, double a, double b, double eps)
            {
                List<double> roots = new List<double>();

                for (double start = a; start < b; start += eps)
                {
                    if (function(start) * function(start + eps) > 0)
                        continue;

                    roots.Add(Chords(function, start, start + eps, eps * 2));
                }

                if (roots.Count() != roots_count)
                    return Chords_multi(function, roots_count, a * 2, b * 2, eps);

                return roots.ToArray();
            }
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
            private static double[][] Matrix_multiplication(double[][] a, double[][] b)
            {
                if (a == null || a.Count() == 0)
                    return null;

                if (b == null || b.Count() == 0)
                    return null;

                if (a[0].Count() != b.Count())
                    return null;

                int m = a.Count();
                int n = a[0].Count();
                int q = b[0].Count();

                double[][] c = new double[m][];
                for (int i = 0; i < c.Count(); i++)
                    c[i] = new double[q];

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        c[i][j] = 0;

                        for (int r = 0; r < n; r++)
                            c[i][j] += a[i][r] * b[r][j];
                    }
                }

                return c;
            }
            private static double[] Matrix_multiplication(double[][] a, double[] b)
            {
                if (a == null || a.Count() == 0 || a.Count() != a[0].Count())
                    return null;

                if (b == null || b.Count() == 0 || a.Count() != b.Count())
                    return null;

                double[] c = new double[b.Count()];

                for (int i = 0; i < c.Count(); i++)
                {
                    c[i] = 0;

                    for (int j = 0; j < c.Count(); j++)
                        c[i] += a[i][j] * b[j];
                }

                return c;
            }
            private static double[][] Matrix_multiplication(double[][] a, double b)
            {
                if (a == null || a.Count() == 0)
                    return null;

                if (double.IsNaN(b) == true)
                    return null;

                double[][] c = new double[a.Count()][];

                for (int i = 0; i < a.Count(); i++)
                {
                    c[i] = new double[a[i].Count()];

                    for (int j = 0; j < a[i].Count(); j++)
                        c[i][j] = a[i][j] * b;
                }

                return c;
            }
            private static double[] Matrix_multiplication(double[] a, double b)
            {
                if (a == null || a.Count() == 0)
                    return null;

                if (double.IsNaN(b) == true)
                    return null;

                double[] c = new double[a.Count()];

                for (int i = 0; i < a.Count(); i++)
                    c[i] = a[i] * b;

                return c;
            }

            private static double[][] Matrix_addition(double[][] a, double[][] b)
            {
                if (a == null || a.Count() == 0)
                    return null;

                if (b == null || b.Count() == 0)
                    return null;

                if (a.LongCount() != b.LongCount())
                    return null;

                double[][] c = new double[a.Count()][];

                for (int i = 0; i < c.Count(); i++)
                {
                    c[i] = new double[a[i].Count()];

                    for (int j = 0; j < c[i].Count(); j++)
                        c[i][j] = a[i][j] + b[i][j];
                }

                return c;
            }
            private static double[] Matrix_addition(double[] a, double[] b)
            {
                if (a == null || a.Count() == 0)
                    return null;

                if (b == null || b.Count() == 0)
                    return null;

                if (a.Count() != b.Count())
                    return null;

                double[] c = new double[a.Count()];

                for (int i = 0; i < c.Count(); i++)
                    c[i] = a[i] + b[i];

                return c;
            }
            private static double[][] Matrix_subtraction(double[][] a, double[][] b)
            {
                if (a == null || a.Count() == 0)
                    return null;

                if (b == null || b.Count() == 0)
                    return null;

                if (a.LongCount() != b.LongCount())
                    return null;

                double[][] c = new double[a.Count()][];

                for (int i = 0; i < c.Count(); i++)
                {
                    c[i] = new double[a[i].Count()];

                    for (int j = 0; j < c[i].Count(); j++)
                        c[i][j] = a[i][j] - b[i][j];
                }

                return c;
            }
            private static double[] Matrix_subtraction(double[] a, double[] b)
            {
                if (a == null || a.Count() == 0)
                    return null;

                if (b == null || b.Count() == 0)
                    return null;

                if (a.Count() != b.Count())
                    return null;

                double[] c = new double[a.Count()];

                for (int i = 0; i < c.Count(); i++)
                    c[i] = a[i] - b[i];

                return c;
            }

            public static void Krylov(double[][] A, double start, double end, double eps, out double[] self_values, out double[][] self_vectors)
            {
                self_values = null;
                self_vectors = null;

                if (A == null || A.Count() == 0)
                    return;

                if (A.Count() != A[0].Count())
                    return;

                double[][] y = new double[A.Count() + 1][];
                for (int i = 0; i < y.Count(); i++)
                {
                    y[i] = new double[A.Count()];

                    for (int j = 0; j < y[i].Count(); j++)
                    {
                        if (i == 0)
                        {
                            if (j == 0)
                                y[i][j] = 1;
                            else
                                y[i][j] = 0;
                        }
                        else
                            y[i] = Matrix_multiplication(A, y[i - 1]);
                    }
                }

                double[][] p_matrix = new double[y.Count() - 1][];
                for (int i = 0; i < p_matrix.Count(); i++)
                {
                    p_matrix[i] = new double[p_matrix.Count() + 1];

                    for (int j = 0; j < p_matrix.Count(); j++)
                    {
                        p_matrix[i][j] = y[y.Count() - 2 - j][i];
                    }

                    p_matrix[i][p_matrix.Count()] = y[y.Count() - 1][i];
                }
                double[] p = Lab1.Gauss_main(p_matrix);

                Func<double, double> lambda_function = delegate (double lambda)
                {
                    double lambda_root = Math.Pow(lambda, p.Count());

                    for (int i = 0; i < p.Count() - 1; i++)
                        lambda_root -= p[i] * Math.Pow(lambda, p.Count() - 1 - i);

                    lambda_root -= p[p.Count() - 1];

                    return lambda_root;
                };

                self_values = Lab2.Chords_multi(lambda_function, p.Count(), start, end, eps);

                double[][] q = new double[self_values.Count()][];
                for (int i = 0; i < q.Count(); i++)
                {
                    q[i] = new double[q.Count()];

                    for (int j = 0; j < q[i].Count(); j++)
                    {
                        if (j == 0)
                            q[i][j] = 1;
                        else
                            q[i][j] = self_values[i] * q[i][j - 1] - p[j - 1];
                    }
                }

                double[][] x = new double[self_values.Count()][];
                for (int i = 0; i < x.Count(); i++)
                {
                    x[i] = y[y.Count() - 2];

                    for (int j = 0; j < x[i].Count() - 1; j++)
                        x[i] = Matrix_addition(x[i], Matrix_multiplication(y[y.Count() - 3 - j], q[i][j + 1]));
                }

                self_vectors = x;
            }

            public static void Verrier(double[][] A, double start, double end, double eps, out double[] self_values, out double[][] self_vectors)
            {
                self_values = null;
                self_vectors = null;

                if (A == null || A.Count() == 0)
                    return;

                if (A.Count() != A[0].Count())
                    return;

                double[][][] A_pows = new double[A.Count()][][];
                A_pows[0] = A;
                for (int i = 1; i < A_pows.Count(); i++)
                    A_pows[i] = Matrix_multiplication(A_pows[i - 1], A);

                double[] S = new double[A_pows.Count()];
                for (int i = 0; i < S.Count(); i++)
                {
                    S[i] = 0;

                    for (int j = 0; j < A_pows[i].Count(); j++)
                        S[i] += A_pows[i][j][j];
                }

                double[] p = new double[A_pows.Count()];
                p[0] = S[0];
                for (int i = 1; i < p.Count(); i++)
                {
                    p[i] = 1.0 / (i + 1.0);

                    double pi = S[i];
                    for (int j = 0; j <= i - 1; j++)
                        pi -= (p[j] * S[i - 1 - j]);
                    p[i] *= pi;
                }

                Func<double, double> lambda_function = delegate (double lambda)
                {
                    double lambda_root = Math.Pow(lambda, p.Count());

                    for (int i = 0; i < p.Count() - 1; i++)
                        lambda_root -= p[i] * Math.Pow(lambda, p.Count() - 1 - i);

                    lambda_root -= p[p.Count() - 1];

                    return lambda_root;
                };

                self_values = Lab2.Chords_multi(lambda_function, p.Count(), start, end, eps);


            }


        }

        public abstract class Lab4
        {
            public static double Lagrange_polynom(List<double> x_list, List<double> y_list, double x)
            {
                if (x_list == null || y_list == null || x_list.Count == 0 || y_list.Count == 0 || x_list.Count != y_list.Count || double.IsNaN(x) == true)
                    return double.NaN;

                List<double> L = new List<double>();
                for (int i = 0; i < x_list.Count; i++)
                {
                    double Li = 1;
                    for (int j = 0; j < x_list.Count; j++)
                    {
                        if (i == j)
                            continue;

                        Li *= (x - x_list[j]) / (x_list[i] - x_list[j]);
                    }
                    L.Add(y_list[i] * Li);
                }

                return L.Sum();
            }
            public static string Lagrange_polynom(List<double> x_list, List<double> y_list)
            {
                if (x_list == null || y_list == null || x_list.Count == 0 || y_list.Count == 0 || x_list.Count != y_list.Count)
                    return null;

                string polynom = "L(x)=";

                List<double> L = new List<double>();
                for (int i = 0; i < x_list.Count; i++)
                {
                    polynom += string.Format("{0}*(", y_list[i]);

                    for (int j = 0; j < x_list.Count; j++)
                    {
                        if (i == j)
                            continue;

                        polynom += string.Format("((x-{0})/{1})", x_list[j], x_list[i] - x_list[j]);

                        if (j != x_list.Count - 1)
                        {
                            if (i == x_list.Count - 1 && j == i - 1)
                                continue;
                            else
                                polynom += "*";
                        }
                    }

                    polynom += ")";

                    if (i != x_list.Count - 1)
                        polynom += "+" + "\n";
                }

                return polynom;
            }

            public static double Newton_polynom(List<double> x_list, List<double> y_list, double x)
            {
                if (x_list == null || y_list == null || x_list.Count == 0 || y_list.Count == 0 || x_list.Count != y_list.Count || double.IsNaN(x) == true)
                    return double.NaN;

                double[][] f = new double[x_list.Count - 1][];
                for (int i = 0; i < f.Count(); i++)
                    f[i] = new double[f.Count() - i];

                for (int j = 0; j < f.Count(); j++)
                {
                    for (int i = 0; i < f.Count() - j; i++)
                    {
                        if (j == 0)
                            f[j][i] = (y_list[i + 1] - y_list[i]) / (x_list[i + 1] - x_list[i]);
                        else
                            f[j][i] = (f[j - 1][i + 1] - f[j - 1][i]) / (x_list[i + 1 + j] - x_list[i]);
                    }
                }

                List<double> L = new List<double>();
                L.Add(y_list[0]);

                for (int i = 0; i < f.Count(); i++)
                {
                    double Li = f[i][0];

                    for (int j = i; j >= 0; j--)
                        Li *= (x - x_list[j]);

                    L.Add(Li);
                }

                return L.Sum();
            }
            public static string Newton_polynom(List<double> x_list, List<double> y_list)
            {
                if (x_list == null || y_list == null || x_list.Count == 0 || y_list.Count == 0 || x_list.Count != y_list.Count)
                    return null;

                double[][] f = new double[x_list.Count - 1][];
                for (int i = 0; i < f.Count(); i++)
                    f[i] = new double[f.Count() - i];

                for (int j = 0; j < f.Count(); j++)
                {
                    for (int i = 0; i < f.Count() - j; i++)
                    {
                        if (j == 0)
                            f[j][i] = (y_list[i + 1] - y_list[i]) / (x_list[i + 1] - x_list[i]);
                        else
                            f[j][i] = (f[j - 1][i + 1] - f[j - 1][i]) / (x_list[i + 1 + j] - x_list[i]);
                    }
                }

                string polynom = "L(x)=";
                polynom += string.Format("{0}", y_list[0]);

                for (int i = 0; i < f.Count(); i++)
                {
                    polynom += "+" + "\n" + "(";

                    polynom += string.Format("{0}", f[i][0]);

                    for (int j = i; j >= 0; j--)
                        polynom += string.Format("*(x-{0})", x_list[j]);

                    polynom += ")";
                }

                return polynom;
            }
        }

        public abstract class Lab5
        {
            public static List<double[]> Picard(Func<double, double> i_function, double x_start, double x_end, double h)
            {
                if (i_function == null || double.IsNaN(x_start) == true || double.IsNaN(x_end) == true || double.IsNaN(h) == true)
                    return null;

                if (x_start >= x_end)
                    return null;

                if (h <= 0)
                    return null;

                List<double[]> points = new List<double[]>();

                for (double x = x_start; x < x_end; x += h)
                    points.Add(new double[] { x, i_function(x) });

                return points;
            }

            public static List<double[]> Eyler(Func<double, double, double> function, double x_start, double y_x_start, double x_end, double h)
            {
                if (function == null || double.IsNaN(x_start) == true || double.IsNaN(y_x_start) == true || double.IsNaN(x_end) == true || double.IsNaN(h) == true)
                    return null;

                if (x_start >= x_end)
                    return null;

                if (h <= 0)
                    return null;

                List<double[]> points = new List<double[]>();
                points.Add(new double[] { x_start, y_x_start });

                for (int i = 1; x_start + h * i <= x_end; i++)
                {
                    double xi = x_start + i * h;
                    double yi = points[i - 1][1] + h * function(points[i - 1][0], points[i - 1][1]);
                    points.Add(new double[] { xi, yi });
                }

                return points;
            }

            public static List<double[]> Modifie_Eyler(Func<double, double, double> function, double x_start, double y_x_start, double x_end, double h)
            {
                if (function == null || double.IsNaN(x_start) == true || double.IsNaN(y_x_start) == true || double.IsNaN(x_end) == true || double.IsNaN(h) == true)
                    return null;

                if (x_start >= x_end)
                    return null;

                if (h <= 0)
                    return null;

                List<double[]> points = new List<double[]>();
                points.Add(new double[] { x_start, y_x_start });

                List<double[]> mod_points = new List<double[]>();
                mod_points.Add(new double[] { x_start, y_x_start });

                for (int i = 1; x_start + h * i <= x_end; i++)
                {
                    double xi = x_start + i * h;
                    double yi = mod_points[i - 1][1] + h * function(mod_points[i - 1][0], mod_points[i - 1][1]);
                    points.Add(new double[] { xi, yi });

                    yi = points[i - 1][1] + h / 2 * (function(points[i - 1][0], points[i - 1][1]) + function(points[i][0], points[i][1]));
                    mod_points.Add(new double[] { xi, yi });
                }

                return mod_points;
            }
        }
    }
}