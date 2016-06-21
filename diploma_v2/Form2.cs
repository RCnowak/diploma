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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Program.task.dimX = Convert.ToInt32(textBox1.Text);
                Program.task.dimP = Convert.ToInt32(textBox2.Text);
                Program.task.t0 = Convert.ToInt32(textBox3.Text);
                Program.task.T = Convert.ToInt32(textBox4.Text);
                Program.task.dimTk = Convert.ToInt32(textBox5.Text);
                Program.task.step = Convert.ToDouble(textBox6.Text);
                Program.task.m = radioButton2.Checked;
                Program.task.Init();
                Program.task.isInitialized = true;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Wrong type of data!");
                Program.task.isInitialized = false;
                this.Close();
            }
        }
    }
}
