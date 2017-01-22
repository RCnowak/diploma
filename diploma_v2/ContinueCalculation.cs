using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diploma_v2
{
    public partial class ContinueCalculation : Form
    {
        public Boolean continueSolving { get; set; }

        public ContinueCalculation()
        {
            InitializeComponent();

            continueSolving = false;

            textBox1.AppendText(String.Format("Используемый метод: {0}" + Environment.NewLine, Program.method));
            textBox1.AppendText(String.Format("f = {0}" + Environment.NewLine, Program.lastFunval));
            for (int i = 0, k = 0, t = 1; i < Program.task.dimAllP; i++)
            {
                for (int j = i; j - i < Program.dimP; j++)
                {
                    textBox1.AppendText(String.Format("v{0}{1} = " + Program.task.z[i].ToString() + Environment.NewLine, j - i, k));
                }
                i += Program.dimP;
                if (i < Program.dimAllP - 1)
                {
                    k++;
                    textBox1.AppendText(String.Format("t{0} = " + Program.task.z[i].ToString() + Environment.NewLine, k));
                }
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mustBeSaved.Checked)
            {
                save();
            }
            this.Close();
        }

        private void save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Сохранить задачу";
            var result = saveFileDialog.ShowDialog();

            // сохраняем текст в файл
            if (result == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, textBox1.Text);
                MessageBox.Show("Файл сохранен");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Newton.Enabled = checkBox1.Enabled;
            Gradient.Enabled = checkBox1.Enabled;
            DFP.Enabled = checkBox1.Enabled;
            continueSolving = checkBox1.Enabled;
        }

        private void Newton_CheckedChanged(object sender, EventArgs e)
        {
            Program.method = Method.Newton;
        }

        private void Gradient_CheckedChanged(object sender, EventArgs e)
        {
            Program.method = Method.Spusk;
        }

        private void DFP_CheckedChanged(object sender, EventArgs e)
        {
            Program.method = Method.DFP;
        }
    }
}
