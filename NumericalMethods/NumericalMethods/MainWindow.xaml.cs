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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NumericalMethods
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<object> matrix_values; 

        public MainWindow()
        {
            InitializeComponent();

            matrix_values = new List<object>();         

    }

    private void MenuItem_open_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open_file = new OpenFileDialog();
            open_file.DefaultExt = Directory.GetCurrentDirectory();
            open_file.ShowDialog();
        }
        private void MenuItem_save_to_file_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save_file = new SaveFileDialog();
            save_file.DefaultExt = Directory.GetCurrentDirectory();
            save_file.ShowDialog();
        }

        private void MenuItem_gauss_with_main_element_Click(object sender, RoutedEventArgs e)
        {
            MenuItem_calculate.IsEnabled = true;
            MenuItem_gauss_with_main_element.IsChecked = true;
            MenuItem_gauss_seidel.IsChecked = false;

            textBox_result.Clear();
        }
        private void MenuItem_gauss_seidel_Click(object sender, RoutedEventArgs e)
        {
            MenuItem_calculate.IsEnabled = true;
            MenuItem_gauss_with_main_element.IsChecked = false;
            MenuItem_gauss_seidel.IsChecked = true;

            textBox_result.Clear();
        }

        private void MenuItem_calculate_Click(object sender, RoutedEventArgs e)
        {
            textBox_result.Clear();

            if (MenuItem_gauss_with_main_element.IsChecked == true)
            {
                double[,] myArr = new double[3, 4];

                Random ran = new Random();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        myArr[i, j] = ran.Next(0, 9);
                    }
                }

                double[] x = new double[3];          

                if (Methods.Gauss_with_main_element(myArr, 3)==null)
                {
                    textBox_result.Text = "Данная система не имеет решений!";
                }
                else
                {
                    x = Methods.Gauss_with_main_element(myArr, 3);

                    foreach (double d in x)
                    {
                        textBox_result.Text += d.ToString() + ";";
                    }
                }
                 

                



            }

            if (MenuItem_gauss_seidel.IsChecked == true)
            {
                Methods.Gauss_seidel();
            }
        }

        private void MenuItem_help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_size_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_matrix.Columns.Clear();
            matrix_values.Clear();

            int size = int.Parse(textBox_size.Text);

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

            while(size +1 != dataGrid_matrix.Columns.Count)
            {
                dataGrid_matrix.Columns.RemoveAt(dataGrid_matrix.Columns.Count - 1);
            }
        }
    }
}
