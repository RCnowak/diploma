using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace diploma_v2
{
    partial class Program
    {

        /// <summary>
        /// Начльное время
        /// </summary>
        public static double t0;            // ­ начальное время
        /// <summary>
        /// Конечное время
        /// </summary>
        public static double T;             // конечное время
        /// <summary>
        /// Размерность Х
        /// </summary>
        public static int dimX;             // dimension of X
        public static int dimU;             // dimension of U
        /// <summary>
        /// Размерность параметризации одной координаты управления
        /// </summary>
        public static int dimP;             // размерность параметризации одной координаты управления
        /// <summary>
        /// Число моментов переключения
        /// </summary>
        public static int dimTk;            // число моментов переключения
        /// <summary>
        /// Размерность пространства параметров
        /// </summary>
        public static int dimAllP;          // размерность пространства параметров

        public static double lastFunval;    // последнее значение целевой функции
        /// <summary>
        /// Частота дискретизации хранения
        /// </summary>
        public static int discr;            // число, определяющее размерность дискретного хранения фазовых и сопряженных переменных

        public static double ep = 1e-10;    //точность вычислений
        public static double eps = 1e-15;   // точность вычислений

        public static double step = 0.01;   // шаг

        public static TextBox logTextBox;  // Окно вывода логов вычисления
        public static Label status;        // Статус работы программы

        public static Task task = new Task();

        public static Method method = Method.Spusk; // Дефолтный метод

        public static NumberFormatInfo nfi = CultureInfo.CreateSpecificCulture("en-US").NumberFormat; // Правила парсинга чисел

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void Run()
        {
            Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                Program.status.Text = String.Format("Запущен метод {0}", Program.method);
            }), DispatcherPriority.ContextIdle);
            for (int i = 0; i < 1; i++)
            {
                if (Program.method == Method.Spusk)
                {
                    Program.Newton(ref Program.task);
                }
                else if (Program.method == Method.DFP)
                {
                    Program.DFP(ref Program.task);
                }
                else
                {
                    Program.Spusk(ref Program.task);
                }
            }

            Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                Program.status.Text = "Вычисления закончены";
            }), DispatcherPriority.ContextIdle);
            ContinueCalculation dialog = new ContinueCalculation();
            dialog.ShowDialog();
            try
            {
                if (dialog.continueSolving)
                {
                    // присваиваем ответ в начальное приближение
                    for (int i = 0; i < Program.task.dimAllP; i++)
                    {
                        Program.task.z0[i] = Program.task.z[i].ToString();
                    }
                    Program.Run();
                }
            }
            catch
            {

            }
        }
    }
}
