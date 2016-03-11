using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    [Serializable]
    public class XML_settings
    {
        public double[][] lab1_matrix { get; set; }
        public string lab1_matrix_size { get; set; }

        public string lab1_gauss_seidel_accuracy { get; set; }

        public string lab2_function { get; set; }

        public string lab2_chords_start_interval { get; set; }
        public string lab2_chords_end_interval { get; set; }
        public string lab2_chords_accuracy { get; set; }

        public string lab2_newton_initial_approximation { get; set; }
        public string lab2_newton_accuracy { get; set; }

        public XML_settings()
        {

        }
        public XML_settings(UI ui)
        {
            this.lab1_matrix = ui.lab1_matrix.ToArray();
            this.lab1_matrix_size = ui.textBox_lab1_matrix_size.Text;

            this.lab1_gauss_seidel_accuracy = ui.textBox_lab1_gauss_seidel_accuracy.Text;

            this.lab2_function = ui.comboBox_lab2_function.SelectedItem.ToString();

            this.lab2_chords_start_interval = ui.textBox_lab2_chords_start_interval.Text;
            this.lab2_chords_end_interval = ui.textBox_lab2_chords_end_interval.Text;
            this.lab2_chords_accuracy = ui.textBox_lab2_chords_accuracy.Text;

            this.lab2_newton_initial_approximation = ui.textBox_lab2_newton_initial_approximation.Text;
            this.lab2_newton_accuracy = ui.textBox_lab2_newton_accuracy.Text;
        }
    }
}