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
        public MainWindow()
        {
            InitializeComponent();
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
            MenuItem_gauss_seidel.IsChecked = false;
        }
        private void MenuItem_gauss_seidel_Click(object sender, RoutedEventArgs e)
        {
            MenuItem_calculate.IsEnabled = true;
            MenuItem_gauss_with_main_element.IsChecked = false;
        }

        private void MenuItem_calculate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_help_Click(object sender, RoutedEventArgs e)
        {

        }        
    }
}
