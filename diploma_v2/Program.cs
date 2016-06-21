using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        /// <summary>
        /// Частота дискретизации хранения
        /// </summary>
        public static int discr;            // число, определяющее размерность дискретного хранения фазовых и сопряженных переменных

        public static double ep = 1e-10;    //точность вычислений
        public static double eps = 1e-15;   // точность вычислений

        public static double step = 0.01;   // шаг

        public static Task task = new Task();
        public static string method = "Grad";

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
    }
}
