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
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Program.task.eps = Convert.ToDouble(textBox1.Text, Program.nfi);
                Program.task.ep = Convert.ToDouble(textBox2.Text, Program.nfi);
                Program.eps = Convert.ToDouble(textBox1.Text, Program.nfi);
                Program.ep = Convert.ToDouble(textBox2.Text, Program.nfi);
                Program.task.discr = Convert.ToInt32(textBox3.Text, Program.nfi);
                Program.task.it = Convert.ToInt32(textBox4.Text, Program.nfi);
                Program.task.n = Convert.ToInt32(textBox5.Text, Program.nfi);

                this.Close();
            }
            catch
            {
                MessageBox.Show("Wrong type of data!");
                this.Close();
            }
        }
    }
}
