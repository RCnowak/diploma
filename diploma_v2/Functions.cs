using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma_v2
{
    partial class Program
    {
        public static double func(ref Task t, double step, double h, ref Vector z, ref Vector add)
        {
            Vector y = new Vector(dimAllP);
            y = z + h * add;
            Project(ref y);
            return t.faim(ref t, ref y, step);
        }

        // Возвращает весь вектор f(t,x,u) вектор иксов с точкой (сама программа считает)
        public static Vector fvec(ref Task t, Vector xt, double s, ref Vector z, double tdiscr, double step)// function to be integrated
        {
            Vector tmp = new Vector(dimX);                      // x'[i]
            Vector gint = new Vector(dimX);
            gint = alerIntExpr(s, ref xt, ref z, ref t, tdiscr, step);
            for (int i = 0; i < dimX; i++)
            {
                tmp[i] = t.f(xt, s, ref z, i, ref gint, step);
            }
            return tmp;
        }

        /// <summary>
        /// ОБратная матрица.
        /// </summary>
        /// <param name="t">Задача.</param>
        /// <param name="xt">Вектор х.</param>
        /// <param name="pt">Векто р.</param>
        /// <param name="s">Время s.</param>
        /// <param name="z">Стурктура управления.</param>
        /// <param name="step">Шаг интегрирования.</param>
        /// <returns>Значение обратной матрицы</returns>
        public static double M_obr(ref Task t, Vector xt, ref Vector pt, double s, ref Vector z, double step)
        {
            return pt * fvec(ref t, xt, s, ref z, t0, step);
        }

        /// <summary>
        /// Норма вектора
        /// </summary>
        /// <param name="z">Вектор</param>
        /// <param name="dim">Размерност вектора</param>
        /// <returns>Норма вектора.</returns>
        public static double meg(Vector z, int dim)
        {
            double res = 0.0;
            for (int i = 0; i < dim; i++)
            {
                res += z[i] * z[i];
            }
            return Math.Sqrt(res);
        }

        // Функция положительной срезки
        double Cut(double value)
        {
            if (value > 0)
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Получить управление в точке t.
        /// </summary>
        /// <param name="t">Время.</param>
        /// <param name="z">Структура управления.</param>
        /// <param name="u">Управление, куда сохранится результат функции.</param>
        public static void control(double t, ref Vector z, ref double[] u)
        {
            for (int i = 0; i < dimU; i++)
            {
                u[i] = 0;
            }
            // время t из первого интервала
            if ((t >= t0) && (t < z[dimU * dimP]))
            {
                for (int j = 0; j < dimU; j++)
                {
                    for (int k = 0; k < dimP; k++)
                    {
                        u[j] += z[j * dimP + k] * Math.Pow(t, k);
                    }
                }
            }
            else if ((t >= z[dimU * dimP]) && (t < z[dimTk * (dimU * dimP + 1) - 1]))
            {
                for (int i = 1; i < dimTk; i++)
                {
                    // средние интервалы времени (некрайние)
                    if (t >= (z[i * (dimU * dimP + 1) - 1]) && (t < z[(i + 1) * (dimU * dimP + 1) - 1]))
                    {
                        for (int j = 0; j < dimU; j++)
                        {
                            for (int k = 0; k < dimP; k++)
                            {
                                u[j] += z[j * dimP + i * (dimU * dimP + 1) + k] * Math.Pow(t, k);
                            }
                        }
                    }
                }
            }
            // время t из последнего интервала
            else
            {
                for (int j = 0; j < dimU; j++)
                {
                    for (int k = 0; k < dimP; k++)
                    {
                        u[j] += z[j * dimP + dimTk * (dimU * dimP + 1) + k] * Math.Pow(t, k);
                    }
                }
            }
            // 
        }

        //программа собирает все интегралы, введенные пользователем, в вектор
        // возвращает весь вектор g(s,t,x,u)  
        // function to be integrated                                                
        public static Vector gvec(ref Task t, double s, double ts, Vector xs, ref Vector z)
        {
            Vector tmp = new Vector(dimX);  // подынтегральная функция справа от x'
            for (int i = 0; i < dimX; i++)
            {
                tmp[i] = t.g(s, ts, xs, ref z, i);
            }
            return tmp;
        }

        // возвращает вектор df по u_k  управление 
        //программа собирает производные функций фи по управлению в вектор
        public static Vector dfui(ref Task t, Vector xt, double s, ref Vector z, int k) // f' by uk
        {
            Vector tmp = new Vector(dimX);
            for (int i = 0; i < dimX; i++)
            {
                tmp[i] = t.dfu(xt, s, ref z, i, k);
            }
            return tmp;
        }

        // возвращает вектор dg по u_k  управление
        //программа собирает производные интеграла по управлению в вектор
        public static Vector dgui(ref Task t, double s, double ts, Vector xs, ref Vector z, int k) // g' by uk
        {
            Vector tmp = new Vector(dimX);
            for (int i = 0; i < dimX; i++)
            {
                tmp[i] = t.dgdu(s, ts, xs, ref z, i, k);
            }
            return tmp;
        }

        // матрица Якоби df/dx
        public static Matrix dfdx(Task t, Vector xt, double s, ref Vector z)
        {
            Matrix H = new Matrix(dimX, dimX);
            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimX; j++)
                {
                    H[i, j] = t.dfx(xt, s, ref z, i, j);
                }
            }
            return H;
        }

        // матрица Якоби dg/dx по распределенному запаздыванию
        public static Matrix dgdx(Task t, double s, double ts, Vector xs, ref Vector z)
        {
            Matrix H = new Matrix(dimX, dimX);
            for (int i = 0; i < dimX; i++)
                for (int j = 0; j < dimX; j++)
                    H[i, j] = t.dgdx(s, ts, xs, ref z, i, j);
            return H;
        }

        //функция Н
        public static double M(ref Task t, Vector xt, ref Vector pt, double s, ref Vector z, double step)
        {
            double sum = 0;
            Vector tmp = new Vector(dimX);
            Vector gint = new Vector(dimX);
            int i;

            for (i = 0; i < dimX; i++)
            {
                gint[i] = 0;
            }
            for (i = 0; i < dimX; i++)
            {
                tmp[i] = t.f(xt, s, ref z, i, ref gint, step);
            }

            sum = pt * tmp;
            if (s < T)
            {
                sum += SubIntM(ref t, xt, s, ref z, i, step);
            }
            return sum;
        }

        public static double dHu(ref Task t, Vector xt, ref Vector pt, double s, ref Vector z, int i, double step)
        {  // i-я компонента вектора
            double sum = 0;

            sum = pt * dfui(ref t, xt, s, ref z, i);
            if (s < T)
            {
                sum += SubIntDHu(ref t, xt, s, ref z, i, step);
            }
            return sum;
        }


    }
}
