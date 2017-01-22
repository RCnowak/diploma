using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diploma_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Program.logTextBox = logTextBox;
            Program.status = status;
        }

        // Создать новую задачу
        private void NewTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 testDialog = new Form2();
            testDialog.ShowDialog(this);
            testDialog.Dispose();
            if (Program.task.isInitialized)
            {
                string[] inputMask = new string[Program.task.dimX * (6 + 2 * Program.task.dimX) + Program.task.dimAllP + 3];
                StringBuilder sb = new StringBuilder();
                GetBlankTask(ref inputMask, ref sb);
                taskToolStripMenuItem.Enabled = true;
                toolStripDropDownButton1.Enabled = true;
                toolStripButton3.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }  
        }

        // Загрузить задачу
        private void loadTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog(this);
            try
            {
                string[] result = File.ReadAllLines(fileDialog.FileName);
                fileDialog.Dispose();
                Program.task = new Task();
                string[] inputMask;
                try
                {
                    Program.task.dimX = Convert.ToInt32(result[1], Program.nfi);
                    Program.task.dimP = Convert.ToInt32(result[3], Program.nfi);
                    Program.task.t0 = Convert.ToInt32(result[5], Program.nfi);
                    Program.task.T = Convert.ToInt32(result[7], Program.nfi);
                    Program.task.dimTk = Convert.ToInt32(result[9], Program.nfi);
                    Program.task.step = Convert.ToDouble(result[11], Program.nfi);
                    Program.task.m = Convert.ToBoolean(result[13], Program.nfi);
                    Program.task.Init();
                    Program.task.isInitialized = true;

                    taskToolStripMenuItem.Enabled = true;
                    toolStripDropDownButton1.Enabled = true;
                    toolStripButton3.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;

                    inputMask =
                        new string[Program.task.dimX * (6 + 2 * Program.task.dimX) + Program.task.dimAllP + 3];
                    StringBuilder sb = new StringBuilder();

                    try
                    {
                        string[] gottenData = new string[result.Length - 14];
                        for (int i = 0; i < result.Length - 14; i++)
                        {
                            gottenData[i] = result[i + 14];
                        }
                        GetLoadedTask(ref gottenData, ref sb);
                    }
                    catch
                    {
                        GetBlankTask(ref inputMask, ref sb);
                    }
                }
                catch
                {
                    MessageBox.Show("Данные введены в неверном формате!");
                }
            }
            catch
            {

            }        
        }

        // Создание заготовки задачи
        private void GetBlankTask(ref string[] inputMask, ref StringBuilder sb)
        {
            textBox1.Clear();

            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i] = sb.AppendFormat("Enter the initial value for x{0}", i + 1).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + Program.dimX] = sb.AppendFormat("Enter the last value for p{0}", i + 1).ToString();
            }
            sb.Clear();
            inputMask[2 * Program.dimX] = "Enter the initial value of u";
            for (int i = 0, k = 0; i < Program.dimAllP; i++)
            {
                for (int j = i; j - i < Program.dimP; j++)
                {
                    sb.Clear();
                    inputMask[j + 1 + 2 * Program.dimX] = sb.AppendFormat("Enter the v{0}{1}", j - i, k).ToString();
                }
                i += Program.dimP;
                if (i < Program.dimAllP - 1)
                {
                    k++;
                    sb.Clear();
                    inputMask[i + 1 + 2 * Program.dimX] = sb.AppendFormat("Enter the t{0}", k).ToString();
                }
            }

            sb.Clear();
            inputMask[1 + 2 * Program.dimX + Program.dimAllP] = "Functions:";
            sb.Clear();
            inputMask[2 + 2 * Program.dimX + Program.dimAllP] = "Enter the function to minimize";

            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 2 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the terminal part of dx{0}dt", (i + 1)).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 3 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the integral part of dx{0}dt", i + 1).ToString();
            }

            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 4 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the terminal part of dx{0}/dt by du", i + 1).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 5 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the integral part of dx{0}/dt by du", i + 1).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                for (int j = 0; j < Program.dimX; j++)
                {
                    sb.Clear();
                    inputMask[j + 3 + 6 * Program.dimX + Program.dimAllP + i * Program.dimX] = sb.AppendFormat("Enter the terminal part of dx{0}/dt by dx{1}", i + 1, j + 1).ToString();
                }
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                for (int j = 0; j < Program.dimX; j++)
                {
                    sb.Clear();
                    inputMask[j + 3 + Program.dimX * (6 + Program.dimX) + i * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the integral part of dx{0}/dt by dx{1}", i + 1, j + 1).ToString();
                }
            }
            textBox1.Text = string.Join("\r\n", inputMask);
        }

        // Вывод загруженной задачи
        private void GetLoadedTask(ref string[] inputMask, ref StringBuilder sb)
        {
            textBox1.Clear();

            textBox1.Text = string.Join("\r\n", inputMask);
            /*for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i] = sb.AppendFormat("Enter the initial value for x{0}", i + 1).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + Program.dimX] = sb.AppendFormat("Enter the last value for p{0}", i + 1).ToString();
            }
            sb.Clear();
            inputMask[2 * Program.dimX] = "Enter the initial value of u";
            for (int i = 0, k = 0; i < Program.dimAllP; i++)
            {
                for (int j = i; j - i < Program.dimP; j++)
                {
                    sb.Clear();
                    inputMask[j + 1 + 2 * Program.dimX] = sb.AppendFormat("Enter the v{0}{1}", j - i, k).ToString();
                }
                i += Program.dimP;
                if (i < Program.dimAllP - 1)
                {
                    k++;
                    sb.Clear();
                    inputMask[i + 1 + 2 * Program.dimX] = sb.AppendFormat("Enter the t{0}", k).ToString();
                }
            }

            sb.Clear();
            inputMask[1 + 2 * Program.dimX + Program.dimAllP] = "Functions:";
            sb.Clear();
            inputMask[2 + 2 * Program.dimX + Program.dimAllP] = "Enter the function to minimize";

            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 2 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the terminal part of dx{0}dt", (i + 1)).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 3 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the integral part of dx{0}dt", i + 1).ToString();
            }

            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 4 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the terminal part of dx{0}/dt by du", i + 1).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                sb.Clear();
                inputMask[i + 3 + 5 * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the integral part of dx{0}/dt by du", i + 1).ToString();
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                for (int j = 0; j < Program.dimX; j++)
                {
                    sb.Clear();
                    inputMask[j + 3 + 6 * Program.dimX + Program.dimAllP + i * Program.dimX] = sb.AppendFormat("Enter the terminal part of dx{0}/dt by dx{1}", i + 1, j + 1).ToString();
                }
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                for (int j = 0; j < Program.dimX; j++)
                {
                    sb.Clear();
                    inputMask[j + 3 + Program.dimX * (6 + Program.dimX) + i * Program.dimX + Program.dimAllP] = sb.AppendFormat("Enter the integral part of dx{0}/dt by dx{1}", i + 1, j + 1).ToString();
                }
            }
            textBox1.Text = string.Join("\r\n", inputMask);*/
        }

        // Запуск решения из основной панели
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string[] str = textBox1.Text.Split('\r');
            GetInfoFromTextBox(ref str);

            Program.Run();
        }
        // Запуск решения из меню Task
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] str = textBox1.Text.Split('\r');
            GetInfoFromTextBox(ref str);

            Program.Run();
        }

        /// <summary>
        /// Получить данные из главного текстового окна
        /// </summary>
        /// <param name="str">Массив строк взятых из TextBox</param>
        private static void GetInfoFromTextBox(ref string[] str)
        {
            string[] tmp1 = new string[Program.dimX * (6 + 2 * Program.dimX) + Program.dimAllP + 1];
            int index;

            for (int i = 0, k = 1, j = 1; i < Program.dimX * (6 + 2 * Program.dimX) + Program.dimAllP + 1; i++)
            {
                if (i >= 2 * Program.dimX + Program.dimAllP)
                {
                    index = 4 * Program.dimX + 2 * Program.dimAllP + 2 * j + 1;
                    tmp1[i] = str[index].Substring(1, str[index].Length - 1);
                    j++;
                    continue;
                }
                if (i >= 2 * Program.dimX)
                {
                    index = 4 * Program.dimX + 2 * k;
                    tmp1[i] = str[index].Substring(1, str[index].Length - 1);
                    k++;
                    continue;
                }
                index = 2 * i + 1;
                tmp1[i] = str[index].Substring(1, str[index].Length - 1);
            }

            for (int i = 0; i < Program.dimX; i++)
            {
                Program.task.x0[i] = tmp1[i];
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                Program.task.p0[i] = tmp1[i + Program.dimX];
            }
            for (int i = 0; i < Program.dimAllP; i++)
            {
                Program.task.z0[i] = tmp1[i + 2 * Program.dimX];
            }
            Program.task.functional = tmp1[2 * Program.dimX + Program.dimAllP];
            for (int i = 0; i < Program.dimX; i++)
            {
               Program.task.xdt[i] = tmp1[i + 2 * Program.dimX + Program.dimAllP + 1];
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                Program.task.ints[i] = tmp1[i + 3 * Program.dimX + Program.dimAllP + 1];
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                Program.task.xdtdu[i] = tmp1[i + 4 * Program.dimX + Program.dimAllP + 1];
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                Program.task.intsdu[i] = tmp1[i + 5 * Program.dimX + Program.dimAllP + 1];
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                for (int j = 0; j < Program.dimX; j++)
                {
                    Program.task.xdtdx[i, j] = tmp1[j + 1 + 6 * Program.dimX + Program.dimAllP + i * Program.dimX];
                }
            }
            for (int i = 0; i < Program.dimX; i++)
            {
                for (int j = 0; j < Program.dimX; j++)
                {
                    Program.task.intsdx[i, j] = tmp1[j + 1 + Program.dimX * (6 + Program.dimX) + i * Program.dimX + Program.dimAllP];
                }
            }

            Program.task.InitZ();
        }

        // Переключение между методами
        private void gradientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.method = Method.Spusk;
            toolStripDropDownButton1.Text = "Градиентный метод";
            Program.logTextBox.Text = String.Format("Выбран метод {0}", Program.method);
        }

        private void newtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.method = Method.Newton;
            toolStripDropDownButton1.Text = "Метод Ньютона";
            Program.logTextBox.Text = String.Format("Выбран метод {0}", Program.method);
        }

        private void dFPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.method = Method.DFP;
            toolStripDropDownButton1.Text = "Метод ДФП";
            Program.logTextBox.Text = String.Format("Выбран метод {0}", Program.method);
        }



        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options testDialog = new Options();
            testDialog.ShowDialog(this);
            testDialog.Dispose();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] config = new string[14];
            config[0] = "dimX";
            config[1] = Program.task.dimX.ToString();
            config[2] = "dimP";
            config[3] = Program.task.dimP.ToString();
            config[4] = "t0";
            config[5] = Program.task.t0.ToString();
            config[6] = "T";
            config[7] = Program.task.T.ToString();
            config[8] = "tk";
            config[9] = Program.task.dimTk.ToString();
            config[10] = "step";
            config[11] = Program.task.step.ToString();
            config[12] = "numerical deriviatives";
            config[13] = Program.task.m.ToString();

            string[] funcs = textBox1.Text.Split('\r');
            string[] str = new string[2];
            str[0] = String.Join("\r\n", config);
            str[1] = String.Join("\r", funcs);
            string strS = String.Join("\r\n", str);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Сохранить задачу";
            saveFileDialog1.ShowDialog();
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, strS);
            MessageBox.Show("Файл сохранен");
        }
    }
}
