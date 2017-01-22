using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma_v2
{
    partial class Task
    {
        public double ep = 1e-10;           // точность вычисления нормы градиента
        public double eps = 1e-15;          // точность вычисления интеграла

        public int dimX;                    // dimension of X
        public int dimU;                    // dimension of U
        public int dimP;                    // размерность параметризации одной координаты управления
        public int dimTk;                   // число моментов переключения
        public int dimAllP;                 // размерность пространства параметров
        public int discr;                   // число, определяющее размерность дискретного хранения фазовых и сопряженных переменных

        public double t0;                   // ­начальное время
        public double T;                    // конечное время
        public double step;                 // шаг интегрирвоания
        public double pk;
        public int n;                       // количество значений функций для численного дифференцирования
        public bool m = true;               // способ назождения градиента. Если true - численно, если false - аналитически
        public int it = 2;                  // количество итераций

        /// <summary>
        /// Матрица вторых производных, записанная построчно в вектор
        /// </summary>
        public Vector Q;
        /// <summary>
        /// Дискретное запоминание Х. Является матрицей, но записана построчно
        /// </summary>
        public Vector[] X;
        /// <summary>
        /// Дискретное запоминание Р. Является матрицей, но записана построчно
        /// </summary>
        public Vector[] P;                 
        /// <summary>
        /// Структура управления
        /// </summary>
        public Vector z;          

        public string functional;           //Функционал задачи, введенный пользователем
        public string[] xdt;                //Массив терминальной части dx/dt, введенный пользователем
        public string[] ints;               //Массив интегралов в dx/dt, введенный пользователем
        public string[] x0;                 //Массив начальных условиев для фазовых переменных, введенный пользователем
        public string[] p0;                 //Массив конечных условий для сопряженных пермеенных, введенный пользователем
        public string[] xdtdu;              //Массив производных терминальных частей dx/dt по управлению, введенный пользователем
        public string[] intsdu;             //Массив производных интегральных частей dx/dt по управлению, введенный пользователем
        public string[,] xdtdx;             //Массив производных термнальных частей dx/dt по другому х, введенный пользователем
        public string[,] intsdx;            //Массив производных интегральной части dx/dt по другому х, введенный пользователем
        public string[] z0;                 // Начальное приближение для параметров упаравления и точки переключения

        public bool isInitialized = false;


        /// <summary>
        /// инициализация задачи
        /// </summary>
        public void Init()
        {
            dimU = 1;
            dimAllP = dimTk * (dimU * dimP + 1) + dimU * dimP;
            discr = 51;

            pk = 1;
            n = 2;

            Q = new Vector(dimAllP * dimAllP);
            X = new Vector[discr];
            P = new Vector[discr];
            z = new Vector(dimAllP);

            xdt = new string[dimX];
            x0 = new string[dimX];
            p0 = new string[dimX];
            ints = new string[dimX];
            xdtdu = new string[dimX];
            intsdu = new string[dimX];
            xdtdx = new string[dimX, dimX];
            intsdx = new string[dimX, dimX];
            z0 = new string[dimAllP];

            Program.t0 = t0;
            Program.T = T;
            Program.dimX = dimX;
            Program.dimU = dimU;
            Program.dimP = dimP;
            Program.dimTk = dimTk;
            Program.dimAllP = dimAllP;
            Program.discr = discr;
        }

        /// <summary>
        /// целевая функция
        /// </summary>
        /// <param name="t"> Задача</param>
        /// <param name="z">Структура управления и моментов переключения</param>
        /// <param name="h"></param>
        /// <returns></returns>
        public double faim(ref Task t, ref Vector z, double h)
        {
            Vector x = new Vector(t.dimX);

            x = Program.XintD(ref t, ref z, T, h);

            return evaluate(ref functional, ref x, 0, 0, 0);
        }

        //вводим начальные условия для иксов
        public Vector Initx0(ref Vector z, double t)
        {
            Vector x = new Vector(dimX);
            double[] u = new double[dimU]; // x = x(t)
            Program.control(t, ref z, ref u);
            for (int i = 0; i < dimX; i++)
            {
                x[i] = evaluate(ref this.x0[i], ref x, u[0], 0, t); //int.Parse(this.x0[i]);
            }
            return x;
        }

        //вводим конечный условия для сопряженных переменных
        public Vector Initp0(ref Vector x, ref Vector z, double t)
        {
            Vector p = new Vector(dimX);
            double[] u = new double[dimU]; // x = x(t)
            Program.control(t, ref z, ref u);

            for (int i = 0; i < dimX; i++)
            {
                p[i] = evaluate(ref p0[i], ref x, u[0], 0, t);
            }                                                       // конечные условия для сопряженных переменных
            return p;
        }

        //
        public void InitZ()
        {
            Vector x = new Vector(dimX);
            for (int i = 0; i < dimAllP; i++)
            {
                this.z[i] = evaluate(ref this.z0[i], ref x, 0, 0, 0);
            }
        }

        // function to be integrated x'[i]   
        //вектор х с точкой (вводит пользователь)
        public double f(Vector xt, double s, ref Vector z, int i, ref Vector gint, double step)
        {
            double[] u = new double[dimU]; // x = x(t)
            Program.control(s, ref z, ref u);

            return evaluate(ref xdt[i], ref xt, u[0], 0, s) + gint[i];
        }
        //интегралы(вводит пользователь)
        // UndIntegrFunction in right side of differential equation                            
        public double g(double s, double ts, Vector xs, ref Vector z, int i)
        {
            double[] u = new double[dimU]; // x = x(t)
            Program.control(s, ref z, ref u);

            return evaluate(ref ints[i], ref xs, u[0], s, ts);
        }
        //производная функции фи по управлению (вводит пользователь)
        public double dfu(Vector xt, double t, ref Vector z, int i, int j) // f' by u                      
        {                                  // df[i]  by du[j]
            double[] u = new double[dimU];
            Program.control(t, ref z, ref u);

            return evaluate(ref xdtdu[i], ref xt, u[0], 0, t);
        }
        //производная интеграла по управлению (вводит пользователь)
        public double dgdu(double s, double t, Vector xs, ref Vector z, int i, int j) // g' by u            
        {        // dg[i]  by du[j]
            double[] u = new double[dimU];
            Program.control(t, ref z, ref u);

            return evaluate(ref intsdu[i], ref xs, u[0], s, t);
        }
        // f' by x  (df[i] by dx[j]) 
        //пользователь вводит производную каждой функции фи по каждому х
        public double dfx(Vector xt, double t, ref Vector z, int i, int j)
        {
            double[] ut = new double[dimU];
            Program.control(t, ref z, ref ut);

            return evaluate(ref xdtdx[i, j], ref xt, ut[0], 0, t);
        }

        // f' by x  (dg[i] by dx[j]) subintegral expression  
        //пользователь вводит производную каждого интеграла по каждому х
        public double dgdx(double s, double t, Vector xs, ref Vector z, int i, int j)
        {
            double[] u = new double[dimU];
            Program.control(s, ref z, ref u);

            return evaluate(ref intsdx[i, j], ref xs, u[0], s, t);
        }
        //производная кадлого управления по v
        // l - координата u, k - параметр в этой координате
        public double dudv(ref Vector z, double t, int l, int k)
        {

            double tmp = 1;
            for (int i = 1; i <= k; i++)
            {
                tmp *= t;
            }
            // время t из первого интервала
            if ((t >= t0) && (t < z[dimU * dimP]))
            {
                switch (k)
                {
                    case 0: return 1;
                    default: return tmp;
                };
            }
            else
                for (int i = 1; i < dimTk; i++)
                    // средние интервалы времени (некрайние)
                    if (t >= (z[i * (dimU * dimP + 1) - 1]) && (t < z[(i + 1) * (dimU * dimP + 1) - 1]))
                        switch (k)
                        {
                            case 0: return 1;
                            default: return tmp;
                        };
            // время t из последнего интервала
            if ((t >= z[dimTk * (dimU * dimP + 1) - 1]) && (t <= T))
                switch (k)
                {
                    case 0: return 1;
                    default: return tmp;
                };
            return 0;
        }
    }
}
