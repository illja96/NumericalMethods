using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace NumericalMethods
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<double[]> matrix_values;

        public MainWindow()
        {
            InitializeComponent();

            matrix_values = new List<double[]>();
            open_file = new OpenFileDialog();
            save_file = new SaveFileDialog();
        }

        OpenFileDialog open_file;
        private void MenuItem_open_file_Click(object sender, RoutedEventArgs e)
        {
            open_file.FileOk += Open_file_FileOk;
            open_file.InitialDirectory = Directory.GetCurrentDirectory();
            open_file.Multiselect = false;
            open_file.Filter = "Text Files | *.txt";
            open_file.FileName = "matrix";
            open_file.ShowDialog();

        }
        private void Open_file_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                StreamReader file_reader = new StreamReader(File.Open(open_file.FileName, FileMode.Open));
                int size = int.Parse(file_reader.ReadLine());
                double[][] matrix = new double[size][];
                for (int i = 0; i < size; i++)
                {
                    string row = file_reader.ReadLine();
                    List<string> values = row.Split(new char[] { ' ' }).ToList();
                    if (values.Last() == "")
                        values.Remove(values.Last());

                    if (values.Count != size + 1)
                        throw new ArgumentNullException();

                    matrix[i] = new double[size + 1];

                    for (int j = 0; j < size + 1; j++)
                        matrix[i][j] = double.Parse(values[j]);
                }

                matrix_values.Clear();
                Generate_dataGrid(size);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size + 1; j++)
                        matrix_values[i][j] = matrix[i][j];
                }
                dataGrid_matrix.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть файл!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        SaveFileDialog save_file;
        private void MenuItem_save_to_file_Click(object sender, RoutedEventArgs e)
        {
            if (matrix_values.Count == 0)
            {
                MessageBox.Show("Матрица значений не заполнена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            save_file.FileOk += Save_file_FileOk;
            save_file.InitialDirectory = Directory.GetCurrentDirectory();
            save_file.Filter = "Text Files | *.txt";
            save_file.FileName = "matrix";
            save_file.ShowDialog();
        }
        private void Save_file_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                StreamWriter file_writer = new StreamWriter(File.Open(save_file.FileName, FileMode.Create));
                double[][] matrix = Get_matrix_from_dataGrid();
                file_writer.WriteLine(matrix.Length);

                foreach (var row in matrix_values)
                {
                    foreach (var value in row)
                    {
                        file_writer.Write(value);
                        file_writer.Write(" ");
                    }

                    file_writer.Write(file_writer.NewLine);
                }
                file_writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно сохранить файл!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MenuItem_gauss_with_main_element_Click(object sender, RoutedEventArgs e)
        {
            MenuItem_gauss_with_main_element.IsChecked = true;
            MenuItem_gauss_seidel.IsChecked = false;
        }
        private void MenuItem_gauss_seidel_Click(object sender, RoutedEventArgs e)
        {
            MenuItem_gauss_with_main_element.IsChecked = false;
            MenuItem_gauss_seidel.IsChecked = true;
        }

        private void MenuItem_calculate_Click(object sender, RoutedEventArgs e)
        {
            if (matrix_values.Count == 0)
            {
                MessageBox.Show("Матрица значений не заполнена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MenuItem_gauss_seidel.IsChecked == false && MenuItem_gauss_with_main_element.IsChecked == false)
            {
                MessageBox.Show("Не выбран алгоритм решения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double[] roots = null;
            double[][] matrix = Get_matrix_from_dataGrid().ToArray();

            if (Methods.DiagonallyDominant(matrix) == true)
            {
                MessageBox.Show("Матрица имеет диагональное преобладание.");
            }
            else
            {
                MessageBox.Show("Матрица не имеет диагонального преобладания.");
            }

            if (MenuItem_gauss_with_main_element.IsChecked == true)
            {
                roots = Methods.Gauss_with_main_element(matrix);
            }

            if (MenuItem_gauss_seidel.IsChecked == true)
            {
                double accuracy = double.NaN;
                if (double.TryParse(textBox_accuracy.Text, out accuracy))
                {
                    MessageBox.Show("Некорректно задана точность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                

                roots = Methods.Gauss_seidel(Get_matrix_from_dataGrid(), accuracy);
            }

            Show_roots(roots);

        }

        private void MenuItem_help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_size_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_matrix.Columns.Clear();
            matrix_values.Clear();

            int size = -1;
            if(int.TryParse(textBox_size.Text, out size) == false)
            {
                MessageBox.Show("Некорректно задан размер генерируемой матрицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
            Generate_dataGrid(size);
        }
        private void Generate_dataGrid(int size)
        {
            DataGridTextColumn column;
            double[] row;
            for (int i = 1; i <= size; i++)
            {
                column = new DataGridTextColumn();
                column.Header = "X" + i;
                column.Binding = new Binding("[" + (i - 1) + "]");
                dataGrid_matrix.Columns.Add(column);

                row = new double[size + 1];
                for (int j = 0; j < size; j++)
                    row[j] = 0;

                matrix_values.Add(row);
            }

            column = new DataGridTextColumn();
            column.Header = "B";
            column.Binding = new Binding("[" + (size) + "]");
            dataGrid_matrix.Columns.Add(column);

            dataGrid_matrix.ItemsSource = matrix_values;
            dataGrid_matrix.Items.Refresh();


            while (size + 1 != dataGrid_matrix.Columns.Count)
            {
                dataGrid_matrix.Columns.RemoveAt(dataGrid_matrix.Columns.Count - 1);
            }
        }

        private void button_random_matrix_Click(object sender, RoutedEventArgs e)
        {
            if (matrix_values.Count == 0)
            {
                MessageBox.Show("Матрица значений не сгенерирована!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double accuracy = double.NaN;
            if(double.TryParse(textBox_accuracy.Text, out accuracy))
            {
                MessageBox.Show("Некорректно задана точность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int accuracy_count = 0;
            for (; accuracy != Math.Round(accuracy); accuracy_count++)
                accuracy *= 10;

            Random rand = new Random();

            for (int i = 0; i < matrix_values.Count; i++)
            {
                for (int j = 0; j < matrix_values[i].Length; j++)
                    matrix_values[i][j] = Math.Round(rand.NextDouble() * rand.Next(-10, 10), accuracy_count);
            }

            dataGrid_matrix.Items.Refresh();
        }

        private double[][] Get_matrix_from_dataGrid()
        {
            if (matrix_values.Count == 0)
                return null;

            return matrix_values.ToArray();
        }

        private void Show_roots(double[] roots)
        {
            if (roots == null || roots.Length == 0)
            {
                MessageBox.Show("Невозможно получить корни!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string message = "";

            for (int i = 0; i < roots.Length; i++)
                message += "X" + (i + 1) + " = " + roots[i] + (i == roots.Length - 1 ? "" : "\n");

            MessageBox.Show(message, "Корни", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
