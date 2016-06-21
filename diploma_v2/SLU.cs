using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma_v2
{
    partial class Program
    {
        //        РЕШЕНИЕ  СИСТЕМЫ  ЛИНЕЙНЫХ  УРАВНЕНИЙ
        // выбор перового ненулевого элемента в i-того  столбце и перестановка
        // выборс строки с i-той
        public static void ReStand(ref Task t, ref Vector x, int i)

        {
            bool flag = true;
            double tmp;
            int k;

            for (k = i; k < dimAllP; k++)
            {
                if (Math.Abs(t.Q[k, i]) > 1e-15)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine("Degeneration Algebric System");
                //exit(1);
            }
            for (int j = i; j < dimAllP; j++)
            {
                tmp = t.Q[i, j];
                t.Q[i, j] = t.Q[k, j];
                t.Q[k, j] = tmp;
            }
            tmp = x[i];
            x[i] = x[k];
            x[k] = tmp;
        }

        public static void StrWay(ref Task t, ref Vector x)
        {
            double tmp;
            // Прямой ход метода Гаусса 
            for (int i = 0; i < dimAllP; i++)
            {
                if (Math.Abs(t.Q[i, i]) < 1e-15)
                {
                    ReStand(ref t, ref x, i);
                }
                for (int j = i + 1; j < t.dimAllP; j++)
                {
                    tmp = t.Q[j, i] / t.Q[i, i];
                    for (int k = i; k < dimAllP; k++)
                    {
                        t.Q[i, j] -= t.Q[i, k] * tmp;
                        if (Math.Abs(t.Q[j, k]) < 1e-15)
                        {
                            t.Q[i, j] = 0;
                        }
                    }
                    x[j] -= x[i] * tmp;
                    if (Math.Abs(x[j]) < 1e-15)
                    {
                        x[j] = 0;
                    }
                }
            }
        }

        public static void BackWay(ref Task t, ref Vector x, ref Vector h)
        { // обратный ход метода Гаусса 
            for (int i = dimAllP - 1; i >= 0; i--)
            {
                h[i] = x[i];
                for (int j = dimAllP - 1; j > i; j--)
                {
                    h[i] -= t.Q[i, j] * h[j];
                }
                h[i] /= t.Q[i, i];
            }
        }

        /// <summary>
        /// Решение СЛУ методом Гаусса.
        /// </summary>
        /// <param name="t">Задача.</param>
        /// <param name="y">Вектор y.</param>
        /// <param name="h">Вектор h.</param>
        public static void DecSLU(ref Task t, ref Vector y, ref Vector h)
        {
            Vector x = new Vector(dimX);
            x = y;

            StrWay(ref t, ref x);
            BackWay(ref t, ref x, ref h);
        }
        // конец решения СЛУ
    }
}
