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
        public ContinueCalculation()
        {
            InitializeComponent();

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
            saveFileDialog.ShowDialog();

            // сохраняем текст в файл
            System.IO.File.WriteAllText(saveFileDialog.FileName, textBox1.Text);
            MessageBox.Show("Файл сохранен");
        }

        private void Solve_Click(object sender, EventArgs e)
        {
            // присваиваем ответ в начальное приближение
            for (int i = 0; i < Program.task.dimAllP; i++)
            {
                Program.task.z0[i] = Program.task.z[i].ToString();
            }

            if (Newton.Checked) {
                Program.method = "Newton";
            } else if (Gradient.Checked) {
                Program.method = "Spusk";
            } else {
                Program.method = "DFP";
            }

            Program.Run();
            this.Close();
        }
    }
}
