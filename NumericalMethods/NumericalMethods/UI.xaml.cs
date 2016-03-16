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

        public UI()
        {
            InitializeComponent();

            open_file = new OpenFileDialog();
            save_file = new SaveFileDialog();
            lab1_matrix = new List<double[]>();
            lab2_functions = new Dictionary<string, Func<double, double>>();
            lab2_d_functions = new Dictionary<string, Func<double, double>>();

            textBox_lab1_gauss_seidel_accuracy.Text = ((double)0.001).ToString();
            textBox_lab2_chords_accuracy.Text = ((double)0.001).ToString();
            textBox_lab2_newton_accuracy.Text = ((double)0.001).ToString();

            lab2_functions.Add(" 4: x^3 - x^2 + 3 = 0", delegate (double x) { return Math.Pow(x, 3) - Math.Pow(x, 2) + 3; });
            lab2_d_functions.Add(" 4: x^3 - x^2 + 3 = 0", delegate (double x) { return 3 * Math.Pow(x, 2) - 2 * x; });

            lab2_functions.Add("19: x^3 + x^2 + 2 = 0", delegate (double x) { return Math.Pow(x, 3) + Math.Pow(x, 2) + 2; });
            lab2_d_functions.Add("19: x^3 + x^2 + 2 = 0", delegate (double x) { return 3 * Math.Pow(x, 2) + 2 * x; });

            comboBox_lab2_function.ItemsSource = lab2_functions.Keys.ToArray();
            comboBox_lab2_function.SelectedIndex = 0;
        }

        private void MenuItem_open_file_Click(object sender, RoutedEventArgs e)
        {
            open_file.FileOk += open_file_FileOk;
            open_file.InitialDirectory = Directory.GetCurrentDirectory();
            open_file.Multiselect = false;
            open_file.Filter = "XML Files | *.xml";
            open_file.FileName = "settings";
            open_file.ShowDialog();
        }
        private void open_file_FileOk(object sender, CancelEventArgs e)
        {
            XML_settings settings;
            try
            {
                using (FileStream fs = new FileStream(open_file.FileName, FileMode.Open))
                {
                    XmlSerializer settings_xml = new XmlSerializer(typeof(XML_settings));
                    settings = (XML_settings)settings_xml.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно прочитать файл!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dataGrid_lab1_matrix_generate(settings.lab1_matrix.Length);
            lab1_matrix = settings.lab1_matrix.ToList();
            dataGrid_lab1_matrix.ItemsSource = lab1_matrix;
            while (dataGrid_lab1_matrix.Columns.Count != lab1_matrix.Count + 1)
                dataGrid_lab1_matrix.Columns.RemoveAt(lab1_matrix.Count + 1);
            dataGrid_lab1_matrix.Items.Refresh();

            textBox_lab1_matrix_size.Text = settings.lab1_matrix_size;

            textBox_lab1_gauss_seidel_accuracy.Text = settings.lab1_gauss_seidel_accuracy;

            if (lab2_functions.ContainsKey(settings.lab2_function))
                comboBox_lab2_function.SelectedItem = settings.lab2_function.ToString();

            textBox_lab2_chords_start_interval.Text = settings.lab2_chords_start_interval;
            textBox_lab2_chords_end_interval.Text = settings.lab2_chords_end_interval;
            textBox_lab2_chords_accuracy.Text = settings.lab2_chords_accuracy;

            textBox_lab2_newton_initial_approximation.Text = settings.lab2_newton_initial_approximation;
            textBox_lab2_newton_accuracy.Text = settings.lab2_newton_accuracy;
        }

        private void MenuItem_save_file_Click(object sender, RoutedEventArgs e)
        {
            save_file.FileOk += save_file_FileOk;
            save_file.InitialDirectory = Directory.GetCurrentDirectory();
            save_file.Filter = "XML File | *.xml";
            save_file.FileName = "settings";
            save_file.ShowDialog();
        }
        private void save_file_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(save_file.FileName, FileMode.Create))
                {
                    XmlSerializer settings_xml = new XmlSerializer(typeof(XML_settings));
                    XML_settings settings = new XML_settings(this);

                    settings_xml.Serialize(fs, settings);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно записать в файл!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void show_roots(double root)
        {
            string message = "Вычисленные корни:" + "\n";
            message += string.Format("x = {0}", root);

            MessageBox.Show(message, "Корни", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void show_roots(double[] roots)
        {
            string message = "Вычисленные корни:" + "\n";
            for (int i = 0; i < roots.Length; i++)
                message += string.Format("x{0} = {1}", i + 1, roots[i]) + (i != roots.Length - 1 ? "\n" : "");

            MessageBox.Show(message, "Корни", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void button_lab1_calculate_Click(object sender, RoutedEventArgs e)
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
            {
                MessageBox.Show("Матрица не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double[][] x = dataGrid_lab1_matrix_get_x();
            double[] b = dataGrid_lab1_matrix_get_b();

            double[] roots = null;

            if (TabItem_lab1_gauss_main.IsSelected == true)
                roots = Methods.Lab1.Gauss_main(x, b);
            else
            {
                double accuracy;
                try
                {
                    accuracy = double.Parse(textBox_lab1_gauss_seidel_accuracy.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Некорректно задана точность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                roots = Methods.Lab1.Gauss_seidel(x, b, accuracy);
            }

            if (roots == null || roots.Length == 0)
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
            catch (Exception ex)
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
                for (int j = 0; j < lab1_matrix[i].Length; j++)
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
            {
                dataGrid_lab1_matrix.Columns.RemoveAt(dataGrid_lab1_matrix.Columns.Count - 1);
            }
        }
        private double[][] dataGrid_lab1_matrix_get_x()
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
                return null;

            double[][] x = new double[lab1_matrix.Count][];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new double[x.Length];
                for (int j = 0; j < x.Length; j++)
                    x[i][j] = lab1_matrix[i][j];
            }

            return x;
        }
        private double[] dataGrid_lab1_matrix_get_b()
        {
            if (lab1_matrix == null || lab1_matrix.Count == 0)
                return null;

            double[] b = new double[lab1_matrix.Count];
            for (int i = 0; i < b.Length; i++)
                b[i] = lab1_matrix[i][b.Length];

            return b;
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
                catch (Exception ex)
                {
                    MessageBox.Show("Некорректно задано начало отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    end_interval = double.Parse(textBox_lab2_chords_end_interval.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Некорректно задан конец отрезка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    accuracy = double.Parse(textBox_lab2_newton_accuracy.Text);
                }
                catch (Exception ex)
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
                catch (Exception ex)
                {
                    MessageBox.Show("Некорректно задано начальное приближение!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    accuracy = double.Parse(textBox_lab2_newton_accuracy.Text);
                }
                catch (Exception ex)
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
    }
}
