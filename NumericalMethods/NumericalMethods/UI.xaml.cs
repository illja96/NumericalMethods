using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NumericalMethods
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : Window
    {
        public OpenFileDialog open_file;
        public SaveFileDialog save_file;

        public List<double[]> lab1_matrix;

        public Dictionary<string, Func<double, double>> lab2_functions;
        public Dictionary<string, Func<double, double>> lab2_d_functions;

        public List<double[]> lab3_matrix;

        public List<double[]> lab4_points;

        public Dictionary<string, Func<double, double, double>> lab5_functions;

        public UI()
        {
            InitializeComponent();

            open_file = new OpenFileDialog();
            save_file = new SaveFileDialog();

            lab1_matrix = new List<double[]>();

            textBox_lab1_gauss_seidel_accuracy.Text = ((double)0.001).ToString();

            lab2_functions = new Dictionary<string, Func<double, double>>();
            lab2_d_functions = new Dictionary<string, Func<double, double>>();

            lab2_functions.Add(" 4: x^3 - x^2 + 3 = 0", delegate (double x) { return Math.Pow(x, 3) - Math.Pow(x, 2) + 3; });
            lab2_d_functions.Add(" 4: x^3 - x^2 + 3 = 0", delegate (double x) { return 3 * Math.Pow(x, 2) - 2 * x; });

            lab2_functions.Add("19: x^3 + x^2 + 2 = 0", delegate (double x) { return Math.Pow(x, 3) + Math.Pow(x, 2) + 2; });
            lab2_d_functions.Add("19: x^3 + x^2 + 2 = 0", delegate (double x) { return 3 * Math.Pow(x, 2) + 2 * x; });


            textBox_lab2_chords_accuracy.Text = ((double)0.001).ToString();
            textBox_lab2_newton_accuracy.Text = ((double)0.001).ToString();

            comboBox_lab2_function.ItemsSource = lab2_functions.Keys.ToArray();
            comboBox_lab2_function.SelectedIndex = 0;

            lab3_matrix = new List<double[]>();

            textBox_lab3_chords_accuracy.Text = ((double)0.001).ToString();

            lab4_points = new List<double[]>();

            lab5_functions = new Dictionary<string, Func<double, double, double>>();

            lab5_functions.Add("y' = 2y - 2x^2 - 3", delegate (double x, double y) { return 2 * y - 2 * Math.Pow(x, 2) - 3; });

            comboBox_lab5_function.ItemsSource = lab5_functions.Keys.ToArray();
            comboBox_lab5_function.SelectedIndex = 0;

            textBox_lab5_eyler_step.Text = ((double)0.1).ToString();
            textBox_lab5_mod_eyler_step.Text = ((double)0.1).ToString();
        }

        private void MenuItem_open_file_Click(object sender, RoutedEventArgs e)
        {
            open_file.FileOk += open_file_FileOk;
            open_file.InitialDirectory = Directory.GetCurrentDirectory();
            open_file.Multiselect = false;
            open_file.Filter = "XML Files | *.xml";
            open_file.FileName = "lab" + (tabControl_lab.SelectedIndex + 1);
            open_file.ShowDialog();
        }
        private void open_file_FileOk(object sender, CancelEventArgs e)
        {
            object settings = null;
            XmlSerializer settings_xml = null;

            try
            {
                using (FileStream fs = new FileStream(open_file.FileName, FileMode.Open))
                {
                    switch (tabControl_lab.SelectedIndex + 1)
                    {
                        case 1:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab1));
                            settings = (XML_settings_lab1)settings_xml.Deserialize(fs);
                            break;
                        case 2:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab2));
                            settings = (XML_settings_lab2)settings_xml.Deserialize(fs);
                            break;
                        case 3:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab3));
                            settings = (XML_settings_lab3)settings_xml.Deserialize(fs);
                            break;
                        case 4:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab4));
                            settings = (XML_settings_lab4)settings_xml.Deserialize(fs);
                            break;
                        case 5:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab5));
                            settings = (XML_settings_lab5)settings_xml.Deserialize(fs);
                            break;
                        default:
                            MessageBox.Show("Выберите вкладку для применения сохраненных настроек!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                    }

                    fs.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно прочитать файл!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                switch (tabControl_lab.SelectedIndex + 1)
                {
                    case 1:
                        dataGrid_lab1_matrix_generate((settings as XML_settings_lab1).lab1_matrix.Count());
                        lab1_matrix = (settings as XML_settings_lab1).lab1_matrix.ToList();
                        dataGrid_lab1_matrix.ItemsSource = lab1_matrix;
                        while (dataGrid_lab1_matrix.Columns.Count != lab1_matrix.Count + 1)
                            dataGrid_lab1_matrix.Columns.RemoveAt(lab1_matrix.Count + 1);
                        dataGrid_lab1_matrix.Items.Refresh();

                        textBox_lab1_matrix_size.Text = (settings as XML_settings_lab1).lab1_matrix_size;

                        textBox_lab1_gauss_seidel_accuracy.Text = (settings as XML_settings_lab1).lab1_gauss_seidel_accuracy;
                        break;

                    case 2:
                        if (lab2_functions.ContainsKey((settings as XML_settings_lab2).lab2_function))
                            comboBox_lab2_function.SelectedItem = (settings as XML_settings_lab2).lab2_function.ToString();

                        textBox_lab2_chords_start_interval.Text = (settings as XML_settings_lab2).lab2_chords_start_interval;
                        textBox_lab2_chords_end_interval.Text = (settings as XML_settings_lab2).lab2_chords_end_interval;
                        textBox_lab2_chords_accuracy.Text = (settings as XML_settings_lab2).lab2_chords_accuracy;

                        textBox_lab2_newton_initial_approximation.Text = (settings as XML_settings_lab2).lab2_newton_initial_approximation;
                        textBox_lab2_newton_accuracy.Text = (settings as XML_settings_lab2).lab2_newton_accuracy;
                        break;

                    case 3:
                        dataGrid_lab3_matrix_generate((settings as XML_settings_lab3).lab3_matrix.Count());
                        lab3_matrix = (settings as XML_settings_lab3).lab3_matrix.ToList();
                        dataGrid_lab3_matrix.ItemsSource = lab3_matrix;
                        while (dataGrid_lab3_matrix.Columns.Count != lab3_matrix.Count)
                            dataGrid_lab3_matrix.Columns.RemoveAt(lab3_matrix.Count);
                        dataGrid_lab3_matrix.Items.Refresh();

                        textBox_lab3_matrix_size.Text = (settings as XML_settings_lab3).lab3_matrix_size;
                        break;

                    case 4:
                        dataGrid_lab4_points_generate((settings as XML_settings_lab4).lab4_points.Count());
                        lab4_points = (settings as XML_settings_lab4).lab4_points.ToList();
                        dataGrid_lab4_points.ItemsSource = lab4_points;
                        while (dataGrid_lab4_points.Columns.Count != 2)
                            dataGrid_lab4_points.Columns.RemoveAt(2);
                        dataGrid_lab4_points.Items.Refresh();

                        textBox_lab4_points_count.Text = (settings as XML_settings_lab4).lab4_points_count;
                        textBox_lab4_calculate_point.Text = (settings as XML_settings_lab4).lab4_calculate_point;
                        break;

                    case 5:
                        break;

                    default:
                        MessageBox.Show("Ошибка при применении настроек!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при применении настроек!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MenuItem_save_file_Click(object sender, RoutedEventArgs e)
        {
            save_file.FileOk += save_file_FileOk;
            save_file.InitialDirectory = Directory.GetCurrentDirectory();
            save_file.Filter = "XML File | *.xml";
            save_file.FileName = "lab" + (tabControl_lab.SelectedIndex + 1);
            save_file.ShowDialog();
        }
        private void save_file_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(save_file.FileName, FileMode.Create))
                {
                    XmlSerializer settings_xml = null;
                    object settings = null;

                    switch (tabControl_lab.SelectedIndex + 1)
                    {
                        case 1:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab1));
                            settings = new XML_settings_lab1(this);
                            break;

                        case 2:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab2));
                            settings = new XML_settings_lab2(this);
                            break;

                        case 3:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab3));
                            settings = new XML_settings_lab3(this);
                            break;

                        case 4:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab4));
                            settings = new XML_settings_lab4(this);
                            break;

                        case 5:
                            settings_xml = new XmlSerializer(typeof(XML_settings_lab5));
                            settings = new XML_settings_lab5(this);
                            break;

                        default:
                            MessageBox.Show("Выберите вкладку для сохранения настроек!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                    }

                    settings_xml.Serialize(fs, settings);
                    fs.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно записать в файл!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void show_roots(double root)
        {
            string message = "";

            if (double.IsNaN(root) == true)
                message = "Корень отсутствует";
            else
            {
                message = "Вычисленные корни:" + "\n";
                message += string.Format("x = {0}", root);
            }

            MessageBox.Show(message, "Корни", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void show_roots(double[] roots)
        {
            string message = "";

            if (roots == null || roots.Count() == 0)
                message = "Корни отсутствуют";
            else
            {
                message = "Вычисленные корни:" + "\n";
                for (int i = 0; i < roots.Count(); i++)
                    message += string.Format("x{0} = {1}", i + 1, roots[i]) + (i != roots.Count() - 1 ? "\n" : "");

                message += "\n\n" + "Корни в уравнении:" + "\n";

                double[][] matrix = dataGrid_lab1_matrix_get_all();
                int d = max_accuracy_by_matrix(lab1_matrix.ToArray());

                for (int i = 0; i < roots.Count(); i++)
                {
                    double row_sum = 0;

                    for (int j = 0; j < roots.Count(); j++)
                    {
                        message += string.Format("{0} * {1}", matrix[i][j], roots[j]);
                        row_sum += matrix[i][j] * roots[j];

                        if (j == roots.Count() - 1)
                        {
                            message += string.Format(" = {0} ~~ {1}", Math.Round(row_sum, d), matrix[i][j + 1]);
                            message += "\n";
                        }
                        else
                            message += " + ";
                    }
                }
            }

            MessageBox.Show(message, "Корни", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private int max_accuracy_by_matrix(double[][] matrix)
        {
            if (matrix == null || matrix.Count() == 0 || matrix.LongCount() == 0)
                return -1;

            int max = -1;

            for (int i = 0; i < matrix.Count(); i++)
            {
                for (int j = 0; j < matrix[i].Count(); j++)
                {
                    int count = -1;

                    if (matrix[i][j].ToString().Contains('.') == true)
                    {
                        int index = matrix[i][j].ToString().IndexOf('.');
                        count = matrix[i][j].ToString().Skip(index + 1).Count();
                    }
                    else
                        count = 1;

                    if (max < count)
                        max = count;
                }
            }

            return max;
        }

        private void button_lab1_calculate_Click(object sender, RoutedEventArgs e)
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double[] roots = null;

            if (TabItem_lab1_gauss_main.IsSelected == true)
            {
                double[][] matrix = dataGrid_lab1_matrix_get_all();

                roots = Methods.Lab1.Gauss_main(matrix);
            }
            else
            {
                double[][] x = dataGrid_lab1_matrix_get_x();
                double[] b = dataGrid_lab1_matrix_get_b();
                double accuracy;

                try
                {
                    accuracy = double.Parse(textBox_lab1_gauss_seidel_accuracy.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задана точность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                roots = Methods.Lab1.Gauss_seidel(x, b, accuracy);
            }

            if (roots == null || roots.Count() == 0 || roots.Contains(double.PositiveInfinity) || roots.Contains(double.NegativeInfinity) || roots.Contains(double.NaN))
            {
                MessageBox.Show("Невозможно получить корни!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            show_roots(roots);
        }
        private void button_lab1_matrix_size_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid_lab1_matrix.Columns.Clear();
                lab1_matrix.Clear();

                int size = int.Parse(textBox_lab1_matrix_size.Text);
                dataGrid_lab1_matrix_generate(size);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно задан размер генерируемой матрицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void button_lab1_random_matrix_Click(object sender, RoutedEventArgs e)
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Random rand = new Random();
            for (int i = 0; i < lab1_matrix.Count; i++)
            {
                for (int j = 0; j < lab1_matrix[i].Count(); j++)
                    lab1_matrix[i][j] = rand.Next(-10, 10);
            }

            dataGrid_lab1_matrix.Items.Refresh();
        }
        private void dataGrid_lab1_matrix_generate(int size)
        {
            DataGridTextColumn column;
            double[] row;
            for (int i = 1; i <= size; i++)
            {
                column = new DataGridTextColumn();
                column.Header = "X" + i;
                column.Binding = new Binding("[" + (i - 1) + "]");
                dataGrid_lab1_matrix.Columns.Add(column);

                row = new double[size + 1];
                for (int j = 0; j < size; j++)
                    row[j] = 0;

                lab1_matrix.Add(row);
            }

            column = new DataGridTextColumn();
            column.Header = "B";
            column.Binding = new Binding("[" + (size) + "]");
            dataGrid_lab1_matrix.Columns.Add(column);

            dataGrid_lab1_matrix.ItemsSource = lab1_matrix;
            dataGrid_lab1_matrix.Items.Refresh();


            while (size + 1 != dataGrid_lab1_matrix.Columns.Count)
                dataGrid_lab1_matrix.Columns.RemoveAt(dataGrid_lab1_matrix.Columns.Count - 1);
        }
        private double[][] dataGrid_lab1_matrix_get_x()
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
                return null;

            double[][] x = new double[lab1_matrix.Count][];
            for (int i = 0; i < x.Count(); i++)
            {
                x[i] = new double[x.Count()];
                for (int j = 0; j < x.Count(); j++)
                    x[i][j] = lab1_matrix[i][j];
            }

            return x;
        }
        private double[] dataGrid_lab1_matrix_get_b()
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
                return null;

            double[] b = new double[lab1_matrix.Count];
            for (int i = 0; i < b.Count(); i++)
                b[i] = lab1_matrix[i][b.Count()];

            return b;
        }
        private double[][] dataGrid_lab1_matrix_get_all()
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
                return null;

            double[][] x = new double[lab1_matrix.Count][];
            for (int i = 0; i < x.Count(); i++)
            {
                x[i] = new double[x.Count() + 1];
                for (int j = 0; j < x.Count() + 1; j++)
                    x[i][j] = lab1_matrix[i][j];
            }

            return x;
        }
        private void dataGrid_lab1_matrix_set_all(double[][] x, double[] b)
        {
            dataGrid_lab1_matrix_generate(x.Count());

            double[][] matrix = new double[x.Count()][];
            for (int i = 0; i < matrix.Count(); i++)
            {
                matrix[i] = new double[matrix.Count() + 1];

                for (int j = 0; j < matrix[i].Count(); j++)
                {
                    if (j == matrix[i].Count() - 1)
                        matrix[i][j] = b[i];
                    else
                        matrix[i][j] = x[i][j];
                }
            }

            lab1_matrix = matrix.ToList();
            dataGrid_lab1_matrix.ItemsSource = lab1_matrix;
            while (dataGrid_lab1_matrix.Columns.Count != lab1_matrix.Count + 1)
                dataGrid_lab1_matrix.Columns.RemoveAt(lab1_matrix.Count + 1);
            dataGrid_lab1_matrix.Items.Refresh();
        }

        private void button_lab2_calculate_Click(object sender, RoutedEventArgs e)
        {
            double root = double.NaN;

            if (TabItem_lab2_chords.IsSelected == true)
            {
                double start_interval, end_interval, accuracy;
                try
                {
                    start_interval = double.Parse(textBox_lab2_chords_start_interval.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задано начало отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    end_interval = double.Parse(textBox_lab2_chords_end_interval.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задан конец отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    accuracy = double.Parse(textBox_lab2_newton_accuracy.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задана точность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Func<double, double> function = lab2_functions[comboBox_lab2_function.SelectedValue.ToString()];
                root = Methods.Lab2.Chords(function, start_interval, end_interval, accuracy);
            }
            else
            {
                double initial_approximation, accuracy;

                try
                {
                    initial_approximation = double.Parse(textBox_lab2_newton_initial_approximation.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задано начальное приближение!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    accuracy = double.Parse(textBox_lab2_newton_accuracy.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задана точность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Func<double, double> function = lab2_functions[comboBox_lab2_function.SelectedValue.ToString()];
                Func<double, double> d_function = lab2_d_functions[comboBox_lab2_function.SelectedValue.ToString()];
                root = Methods.Lab2.Newton(function, d_function, initial_approximation, accuracy);
            }

            if (root == double.NaN)
            {
                MessageBox.Show("Невозможно получить корни!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            show_roots(root);
        }

        private void show_self_values(double[] roots)
        {
            string message = "";

            if (roots == null || roots.Count() == 0)
            {
                message = "Собственные значения отсутствуют";
            }
            else
            {
                message = "Вычисленные cобственные значения:" + "\n";
                for (int i = 0; i < roots.Count(); i++)
                    message += string.Format("λ{0} = {1}", i + 1, roots[i]) + (i != roots.Count() - 1 ? "\n" : "");
            }

            MessageBox.Show(message, "Собственные значения", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void show_self_vectors(double[][] vectors)
        {
            string message = "";

            if (vectors == null || vectors.Count() == 0)
                message = "Собственные вектора отсутствуют";
            else
            {
                message = "Вычисленные cобственные вектора:" + "\n";

                for (int i = 0; i < vectors.Count(); i++)
                {
                    message += string.Format("x{0} = (", i + 1);

                    for (int j = 0; j < vectors[i].Count(); j++)
                    {
                        message += string.Format("{0}", vectors[i][j]);

                        if (j == vectors[i].Count() - 1)
                            message += ")" + "\n";
                        else
                            message += ", ";
                    }
                }
            }

            MessageBox.Show(message, "Собственные ветора", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void button_lab3_matrix_size_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid_lab3_matrix.Columns.Clear();
                lab3_matrix.Clear();

                int size = int.Parse(textBox_lab3_matrix_size.Text);
                dataGrid_lab3_matrix_generate(size);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно задан размер генерируемой матрицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void dataGrid_lab3_matrix_generate(int size)
        {
            DataGridTextColumn column;
            double[] row;
            for (int i = 1; i <= size; i++)
            {
                column = new DataGridTextColumn();
                column.Header = "";
                column.Binding = new Binding("[" + (i - 1) + "]");
                dataGrid_lab3_matrix.Columns.Add(column);

                row = new double[size];
                for (int j = 0; j < size; j++)
                    row[j] = 0;

                lab3_matrix.Add(row);
            }

            dataGrid_lab3_matrix.ItemsSource = lab3_matrix;
            dataGrid_lab3_matrix.Items.Refresh();


            while (size != dataGrid_lab3_matrix.Columns.Count)
                dataGrid_lab3_matrix.Columns.RemoveAt(dataGrid_lab3_matrix.Columns.Count - 1);
        }
        private void button_lab3_random_matrix_Click(object sender, RoutedEventArgs e)
        {
            if (lab3_matrix == null || lab3_matrix.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Random rand = new Random();
            for (int i = 0; i < lab3_matrix.Count; i++)
            {
                for (int j = 0; j < lab3_matrix[i].Count(); j++)
                    lab3_matrix[i][j] = rand.Next(-10, 10);
            }

            dataGrid_lab3_matrix.Items.Refresh();
        }
        private void button_lab3_calculate_Click(object sender, RoutedEventArgs e)
        {
            if (lab3_matrix == null || lab3_matrix.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double[][] A = dataGrid_lab3_matrix_get_all();

            double start_interval, end_interval, accuracy;
            try
            {
                start_interval = double.Parse(textBox_lab2_chords_start_interval.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно задано начало отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                end_interval = double.Parse(textBox_lab2_chords_end_interval.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно задан конец отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                accuracy = double.Parse(textBox_lab2_newton_accuracy.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно задана точность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double[] self_values = null;
            double[][] self_vectors = null;

            if (TabItem_lab3_krylov.IsSelected == true)
                Methods.Lab3.Krylov(A, start_interval, end_interval, accuracy, out self_values, out self_vectors);
            else
            {
                if (sender.Equals(button_lab3_calculate_self_values) == true)
                    Methods.Lab3.Verrier(A, start_interval, end_interval, accuracy, out self_values, out self_vectors);
                else
                    Methods.Lab3.Krylov(A, start_interval, end_interval, accuracy, out self_values, out self_vectors);
            }

            if (sender.Equals(button_lab3_calculate_self_values) == true)
                show_self_values(self_values);
            else
                show_self_vectors(self_vectors);
        }
        private double[][] dataGrid_lab3_matrix_get_all()
        {
            if (lab3_matrix == null || lab3_matrix.Count == 0)
                return null;

            double[][] a = new double[lab3_matrix.Count][];
            for (int i = 0; i < a.Count(); i++)
            {
                a[i] = new double[a.Count()];
                for (int j = 0; j < a.Count(); j++)
                    a[i][j] = lab3_matrix[i][j];
            }

            return a;
        }

        private void button_lab4_points_count_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid_lab4_points.Columns.Clear();
                lab4_points.Clear();

                int size = int.Parse(textBox_lab4_points_count.Text);
                dataGrid_lab4_points_generate(size);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно задан размер генерируемой матрицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void button_lab4_random_points_Click(object sender, RoutedEventArgs e)
        {
            if (lab4_points == null || lab4_points.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Random rand = new Random();
            for (int i = 0; i < lab4_points.Count; i++)
            {
                for (int j = 0; j < lab4_points[i].Count(); j++)
                    lab4_points[i][j] = rand.Next(-10, 10);
            }

            dataGrid_lab4_points.Items.Refresh();
        }
        private void dataGrid_lab4_points_generate(int size)
        {
            DataGridTextColumn column;
            double[] row;

            column = new DataGridTextColumn();
            column.Header = "X";
            column.Binding = new Binding("[0]");
            dataGrid_lab4_points.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Y";
            column.Binding = new Binding("[1]");
            dataGrid_lab4_points.Columns.Add(column);

            for (int i = 0; i < size; i++)
            {
                row = new double[2];

                for (int j = 0; j < 2; j++)
                    row[j] = 0;

                lab4_points.Add(row);
            }

            dataGrid_lab4_points.ItemsSource = lab4_points;
            dataGrid_lab4_points.Items.Refresh();

            while (dataGrid_lab4_points.Columns.Count != 2)
                dataGrid_lab4_points.Columns.RemoveAt(2);
        }
        private double[] dataGrid_lab4_points_get_x()
        {
            if (lab4_points == null || lab4_points.Count == 0)
                return null;

            double[] x = new double[lab4_points.Count];
            for (int i = 0; i < lab4_points.Count; i++)
                x[i] = lab4_points[i][0];

            return x;
        }
        private double[] dataGrid_lab4_points_get_y()
        {
            if (lab4_points == null || lab4_points.Count == 0)
                return null;

            double[] y = new double[lab4_points.Count];
            for (int i = 0; i < lab4_points.Count; i++)
                y[i] = lab4_points[i][1];

            return y;
        }

        private void button_lab4_show_polynom_Click(object sender, RoutedEventArgs e)
        {
            if (lab4_points == null || lab4_points.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<double> x_list = dataGrid_lab4_points_get_x().ToList();
            List<double> y_list = dataGrid_lab4_points_get_y().ToList();

            string polynom = "";

            if (TabItem_lab4_newton.IsSelected == true)
                polynom = Methods.Lab4.Newton_polynom(x_list, y_list);
            else
                polynom = Methods.Lab4.Lagrange_polynom(x_list, y_list);

            switch (MessageBox.Show("Показать полином как график?" + "\n" + "Требуется подключение к сети интернет", "Полином", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:
                    polynom = polynom.Replace("L(x)=", "");
                    polynom = polynom.Replace("+", "%2B");
                    System.Diagnostics.Process.Start(string.Format("{0}{1}", @"https://www.google.com.ua/search?q=", polynom));
                    break;

                case MessageBoxResult.No:
                    MessageBox.Show(polynom, "Полином", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }
        private void button_lab4_calculate_point_Click(object sender, RoutedEventArgs e)
        {
            if (lab4_points == null || lab4_points.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<double> x_list = dataGrid_lab4_points_get_x().ToList();
            List<double> y_list = dataGrid_lab4_points_get_y().ToList();
            double x = double.NaN;

            try
            {
                x = double.Parse(textBox_lab4_calculate_point.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректно задан X!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            double y = double.NaN;

            if (TabItem_lab4_newton.IsSelected == true)
                y = Methods.Lab4.Newton_polynom(x_list, y_list, x);
            else
                y = Methods.Lab4.Lagrange_polynom(x_list, y_list, x);

            MessageBox.Show(string.Format("[{0}:{1}]", x, y), "Вычисленая точка при помощи полинома", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void show_points(List<double[]> points)
        {
            string message = "";

            if (points == null || points.Count() == 0)
            {
                message = "Приближенные точки отсутствуют";
            }
            else
            {
                message = "Приближенные точки:" + "\n";
                for (int i = 0; i < points.Count(); i++)
                    message += string.Format("[{0} ; {1}]", points[i][0], points[i][1]) + (i != points.Count() - 1 ? "\n" : "");
            }

            MessageBox.Show(message, "Собственные значения", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void button_lab5_calculate_points_Click(object sender, RoutedEventArgs e)
        {
            if (TabItem_lab5_picard.IsSelected == true)
            {

            }
            else
            {
                double x_start, y_x_start, x_end, step;
                try
                {
                    if (TabItem_lab5_euler.IsSelected == true)
                        x_start = double.Parse(textBox_lab5_eyler_x_start.Text);
                    else
                        x_start = double.Parse(textBox_lab5_mod_eyler_x_start.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задано начало отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    if (TabItem_lab5_euler.IsSelected == true)
                        y_x_start = double.Parse(textBox_lab5_eyler_y_x_start.Text);
                    else
                        y_x_start = double.Parse(textBox_lab5_mod_eyler_y_x_start.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задано значение в начале отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    if (TabItem_lab5_euler.IsSelected == true)
                        x_end = double.Parse(textBox_lab5_eyler_x_end.Text);
                    else
                        x_end = double.Parse(textBox_lab5_mod_eyler_x_end.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задан конерц отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    if (TabItem_lab5_euler.IsSelected == true)
                        step = double.Parse(textBox_lab5_eyler_step.Text);
                    else
                        step = double.Parse(textBox_lab5_mod_eyler_step.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректно задано шаг!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Func<double, double, double> function = lab5_functions[comboBox_lab5_function.SelectedValue.ToString()];

                List<double[]> points;
                if (TabItem_lab5_euler.IsSelected == true)
                    points = Methods.Lab5.Eyler(function, x_start, y_x_start, x_end, step);
                else
                    points = Methods.Lab5.Modifie_Eyler(function, x_start, y_x_start, x_end, step);

                show_points(points);
            }
        }
    }
}