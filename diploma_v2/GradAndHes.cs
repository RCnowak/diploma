using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma_v2
{
    partial class Program
    {
        /// <summary>
        /// Вычисление матрицы Гессе
        /// </summary>
        /// <param name="t">Объект класса Task("Задача")</param>
        /// <param name="z">Структура управления</param>
        /// <param name="m">Численное вычисление производных <c>true</c> [m].</param>
        /// <param name="step">Шаг интегрирования.</param>
        public static void MatrHes(ref Task t, ref Vector z, bool m, double step)
        {
            double h;
            if (m)
                h = 0.000005;
            else
                h = 0.000001;
            double[] dy = new double[t.n + 1];
            double s = t.n / 2;
            double res;
            Vector z2 = new Vector(dimAllP);
            Matrix G = new Matrix(dimAllP, t.n + 1);
            Vector g = new Vector(dimAllP);

            for (int i = 0; i < dimAllP; i++)
            {
                z2 = z;
                z2[i] = z[i] - h * t.n / 2;

                Console.WriteLine(" derivative of the {0} variable ", i);
                for (int k = 0; k <= t.n; k++)
                {
                    if (m)
                    {
                        GradD(ref t, ref z2, ref g, step);
                    }
                    else
                    {
                        Grad(ref t, ref z2, ref g, step);
                    }

                    for (int j = 0; j < dimAllP; j++)
                    {
                        G.d[j, k] = g[j];
                    }
                    z2[i] += h;
                }
                for (int j = i; j < dimAllP; j++)
                {
                    for (int k = 0; k <= t.n; k++)
                    {
                        dy[k] = G.d[j, k];
                    }
                    res = derive(h, dy, t, s);
                    t.Q[i, j] = res;
                }
            }
        }

        /// <summary>
        /// Численное высиление градиента
        /// </summary>
        /// <param name="t">Объект класса Task("Задача")</param>
        /// <param name="z">Струтура управления</param>
        /// <param name="g">Вектор градиента в который будут сохраняться данные</param>
        /// <param name="step">Шан интегрирования</param>
        public static void GradD(ref Task t, ref Vector z, ref Vector g, double step)
        {
            double h = 0.000001;
            double[] dy = new double[t.n + 1];
            double s = t.n / 2;
            Vector z2 = new Vector(dimAllP);


            for (int i = 0; i < dimAllP; i++)
            {
                z2 = z;
                z2[i] = z[i] - h * t.n / 2;

                for (int k = 0; k <= t.n; k++)
                {
                    dy[k] = t.faim(ref t, ref z2, step);
                    z2[i] += h;

                }
                g[i] = derive(h, dy, t, s);

            }
        }

        /// <summary>
        /// Численное выичсление производных
        /// </summary>
        /// <param name="step">шан интегрирования</param>
        /// <param name="y">Вектор значений целевой функции с прирощением по каждому аргументу</param>
        /// <param name="t">Объект класса Task("Задача")</param>
        /// <param name="s">Время</param>
        /// <returns>Значение производной в точке s</returns>
        public static double derive(double step, double[] y, Task t, double s)
        {
            double A = 1;
            double B = 0;
            double sum = 0;

            for (int i = 0; i < t.n; i++)
            {
                for (int j = 0; j < t.n - i; j++)
                {
                    y[j] *= -1;
                    y[j] += y[j + 1];
                }
                B = Bk(A, B, s, i + 1);
                A = Ak(A, s, i + 1);
                sum += y[0] * B;
            }
            return sum / step;


        }

        public static double Ak(double Ak1, double s, int k)
        {
            if (k == 0)
            {
                return 1;
            }
            else
            {
                return Ak1 * (s - k + 1) / k;
            }
        }

        public static double Bk(double Ak1, double Bk1, double s, int k)
        {
            if (k == 0)
            {
                return 0;
            }
            else
            {
                return Bk1 * (s - k + 1) / k + Ak1 / k;
            }
        }



        /// <summary>
        /// Аналитический градиент
        /// </summary>
        /// <param name="t">Объект класса Task("Задача")</param>
        /// <param name="z">Структура управления</param>
        /// <param name="g">Вектор граиента, куда будет сохранять знаячения функция</param>
        /// <param name="step">The step.</param>
        public static void Grad(ref Task t, ref Vector z, ref Vector g, double step)
        {
            Vector grad = new Vector(dimAllP);
            Vector x = new Vector(dimX);
            Vector l = new Vector(dimX);
            Vector p = new Vector(dimX);
            Vector x1 = new Vector(dimX);
            Vector l1 = new Vector(dimX);
            Vector p1 = new Vector(dimX);

            x = XintD(ref t, ref z, T, step);
            p = PintD(ref t, ref z, t0, step);

            // производные по значениям управления
            for (int j = 0; j < dimU; j++)
                for (int k = 0; k < dimP; k++)
                    grad[j * dimP + k] = SimpsonD(ref t, t0, z[dimU * dimP], ref z, j, k, 2 * step);// на начальном отрезке
                                                                                                    // производные по значениям управления (средние интервалы)
            for (int i = 1; i < dimTk; i++)
                for (int j = 0; j < dimU; j++)
                    for (int k = 0; k < dimP; k++)
                        grad[i * (dimU * dimP + 1) + j * dimP + k] =
                         SimpsonD(ref t, z[i * (dimU * dimP + 1) - 1], z[(i + 1) * (dimU * dimP + 1) - 1], ref z, j, k, 2 * step);

            // производные по значениям управления на конечном отрезке
            //  ( две из которых нестандартны и параметризуют управление a/(t+b) )
            for (int j = 0; j < dimU; j++)
                for (int k = 0; k < dimP; k++)
                    grad[j * dimP + k + dimTk * (dimU * dimP + 1)] = SimpsonD(ref t, z[dimTk * (dimU * dimP + 1) - 1], T, ref z, j, k, 2 * step);

            for (int i = 1; i < dimTk + 1; i++)    // производные по моментам переключения
            {
                if (z[i * (dimU * dimP + 1) - 1] <= T)
                {
                    x = GetX(ref t, z[i * (dimU * dimP + 1) - 1], ref z);
                    p = GetP(ref t, z[i * (dimU * dimP + 1) - 1]);
                    grad[i * (dimU * dimP + 1) - 1] = -M_obr(ref t, x, ref p, z[i * (dimU * dimP + 1) - 1] + eps, ref z, step) +
                        M_obr(ref t, x, ref p, z[i * (dimU * dimP + 1) - 1] - eps, ref z, step);
                }
            }
            g = grad;

        }











    }
}
