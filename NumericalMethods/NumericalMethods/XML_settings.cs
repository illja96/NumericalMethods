using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    [Serializable]
    public class XML_settings_lab1
    {
        public double[][] lab1_matrix { get; set; }
        public string lab1_matrix_size { get; set; }

        public string lab1_gauss_seidel_accuracy { get; set; }

        public XML_settings_lab1()
        {

        }
        public XML_settings_lab1(UI ui)
        {
            this.lab1_matrix = ui.lab1_matrix.ToArray();
            this.lab1_matrix_size = ui.textBox_lab1_matrix_size.Text;

            this.lab1_gauss_seidel_accuracy = ui.textBox_lab1_gauss_seidel_accuracy.Text;
        }
    }

    [Serializable]
    public class XML_settings_lab2
    {
        public string lab2_function { get; set; }

        public string lab2_chords_start_interval { get; set; }
        public string lab2_chords_end_interval { get; set; }
        public string lab2_chords_accuracy { get; set; }

        public string lab2_newton_initial_approximation { get; set; }
        public string lab2_newton_accuracy { get; set; }

        public XML_settings_lab2()
        {

        }
        public XML_settings_lab2(UI ui)
        {
            this.lab2_function = ui.comboBox_lab2_function.SelectedItem.ToString();

            this.lab2_chords_start_interval = ui.textBox_lab2_chords_start_interval.Text;
            this.lab2_chords_end_interval = ui.textBox_lab2_chords_end_interval.Text;
            this.lab2_chords_accuracy = ui.textBox_lab2_chords_accuracy.Text;

            this.lab2_newton_initial_approximation = ui.textBox_lab2_newton_initial_approximation.Text;
            this.lab2_newton_accuracy = ui.textBox_lab2_newton_accuracy.Text;
        }
    }

    [Serializable]
    public class XML_settings_lab3
    {
        public XML_settings_lab3()
        {

        }

        public XML_settings_lab3(UI ui)
        {

        }
    }

    [Serializable]
    public class XML_settings_lab4
    {
        public double[][] lab4_points { get; set; }
        public string lab4_points_count { get; set; }

        public string lab4_calculate_point { get; set; }

        public XML_settings_lab4()
        {

        }

        public XML_settings_lab4(UI ui)
        {
            this.lab4_points = ui.lab4_points.ToArray();
            this.lab4_points_count = ui.textBox_lab4_points_count.Text;

            this.lab4_calculate_point = ui.textBox_lab4_calculate_point.Text;
        }
    }

    [Serializable]
    public class XML_settings_lab5
    {
        public XML_settings_lab5()
        {

        }

        public XML_settings_lab5(UI ui)
        {

        }
    }
}