using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma_v2
{
    partial class Program
    {
        //         Функции для решения задач квадратичного программирования,
        //            проектирование на множество вида  t0<=t1<=...=tN=T



        //
        /// <summary>
        /// Метод Проекций
        /// </summary>
        /// <param name="y">Вектор y.</param>
        public static void Project(ref Vector y)
        {
            Vector p = new Vector(dimTk);
            // проекция моментов переключения управления
            for (int i = 0; i < dimTk; i++)
            {
                p.vec[i] = y[(i + 1) * (dimU * dimP + 1) - 1];
            }
            p = Proj(p);
            for (int i = 0; i < dimTk; i++)
            {
                y[(i + 1) * (dimU * dimP + 1) - 1] = p[i];
            }
        }

        public static Vector Proj(Vector z)
        {
            Vector x = new Vector(dimTk);
            Vector c = new Vector(dimTk);
            Vector h = new Vector(dimTk);
            double lambda;
            double[] u = new double[dimTk + 1];
            double[] v = new double[dimTk + 1];
            double[] e = new double[dimTk + 1];
            int index = 0;
            bool flag = true;
            int[] I = new int[dimTk + 1];                                // множество активных ограничений


            for (int i = 0; i < dimTk + 1; i++)
            {
                I[i] = 0;
            }
            // всем координатам присвается нижняя граница допустимого множества
            for (int i = 0; i < dimTk; i++)
            {
                x[i] = t0;           // получаем допустимую точку
            }
            x[dimTk - 1] = T;

            c = x - z;
            u[dimTk] = 0;
            for (int i = dimTk; i > 0; i--)
            {
                u[i - 1] = u[i] - c[i - 1];
            }

            defact(ref I, ref x);                                    // определяе множество активных ограничений

            while (flag)
            {
                defc(ref I, ref c, ref z);  // определяем вектор с
                DecSL(ref c, ref h, ref I);
                for (int i = 0; i < dimTk + 1; i++)
                {
                    v[i] = 0;
                }
                for (int i = 0; i < I[0]; i++)
                {
                    v[I[i + 1] - 1] = h[i];
                }
                defe(ref e, ref u, ref v);
                // определение максимально возможной длины шага
                lambda = 1;
                lambda = deflamb(ref index, ref e, ref x, ref I);
                if (lambda < 1)
                // увеличения множества активных ограничений
                {
                    for (int i = 0; i < dimTk + 1; i++)
                    {
                        u[i] += lambda * (v[i] - u[i]);
                    }
                    Inp(ref I, index);
                }
                if (lambda >= 1)
                {
                    for (int i = 0; i < dimTk + 1; i++)
                    {
                        // уменьшение множества активных ограничений
                        if ((v[i] > 0) && (In(ref I, i)))
                        {
                            flag = false;
                            Outp(ref I, i);
                            break;
                        }
                    }
                    for (int i = 0; i < dimTk + 1; i++)
                    {
                        u[i] = v[i];
                    }
                    flag = !flag;
                }
                for (int i = 0; i < dimTk; i++)
                {
                    x[i] = z[i] - u[i] + u[i + 1];
                }
            }
            return x;
        }

        // определяется множество активных ограничений
        public static void defact(ref int[] I, ref Vector z)
        {
            int i = 0;
            I[0] = 0; // мощность множества активных ограничений
                      //  активное ли первое ограничение t0<=t1
            if (Math.Abs(z[0] - t0) < eps)
            {
                I[0]++;
                I[1] = 1;
            }
            for (i = 1; i < dimTk; i++)
            {
                if (Math.Abs(z[i - 1] - z[i]) < eps) //  активное ли i-тое ограничение ti<=t_(i+1)
                {
                    I[0]++;
                    I[I[0]] = i + 1;
                }

                if (Math.Abs(z[dimTk - 1] - T) < eps) //  активное ли последнее ограничение tN<=T
                {
                    I[0]++;
                    I[I[0]] = dimTk + 1;
                }
            }
        }
        // Определяется вектор с
        public static void defc(ref int[] I, ref Vector c, ref Vector z)
        {
            int i;
            for (i = 0; i < I[0]; i++)
            {
                if (I[i + 1] == 1)
                {
                    c[i] = -t0 + z[0];
                }
                else if (I[i + 1] == dimTk + 1)
                {
                    c[i] = T - z[dimTk - 1];
                }
                else
                {
                    c[i] = z[I[i + 1] - 1] - z[I[i + 1] - 2];
                }
            }
        }
        // определение вектора невязок
        public static void defe(ref double[] e, ref double[] u, ref double[] v)
        {
            int i;
            e[0] = v[0] - u[0] - v[1] + u[1];
            e[dimTk] = v[dimTk] - u[dimTk] - v[dimTk - 1] + u[dimTk - 1];
            for (i = 0; i < dimTk - 1; i++)
            {
                e[i + 1] = u[i] - v[i] + 2 * (v[i + 1] - u[i + 1]) + u[i + 2] - v[i + 2];
            }
        }
        // проверка на принадлежность множеству активных ограничений
        public static bool In(ref int[] I, int ind)
        {
            int i;
            for (i = 0; i < I[0]; i++)
            {
                if (ind == I[i + 1] - 1)
                {
                    return true;
                }
            }
            return false;
        }
        // определение максимально возможного шага по данному направлению
        public static double deflamb(ref int index, ref double[] e, ref Vector x, ref int[] I)
        {
            double lambda = 1, c1;
            bool flag1, flag2;
            flag1 = true;

            for (int i = 0; i < dimTk + 1; i++)
                if (e[i] > 0)
                {
                    flag2 = !(In(ref I, i));
                    if (flag1 && flag2)
                    {
                        if (i == 0)
                        {
                            lambda = (x[0] - t0) / e[i];
                        }
                        else if (i == dimTk)
                        {
                            lambda = (T - x[dimTk - 1]) / e[i];
                        }
                        else
                        {
                            lambda = (x[i] - x[i - 1]) / e[i];
                        }
                        index = i;
                        flag1 = false;
                    }
                    else if (flag2)
                    {
                        if (i == 0)
                        {
                            c1 = (x[0] - t0) / e[i];
                        }
                        else if (i == dimTk)
                        {
                            c1 = (T - x[dimTk - 1]) / e[i];
                        }
                        else c1 = (x[i] - x[i - 1]) / e[i];
                        if (c1 < lambda)
                        {
                            lambda = c1;
                            index = i;
                        }
                    }
                }
            return lambda;
        }
        // фнкция увеличения мноества активных ограничений
        public static void Inp(ref int[] I, int index)
        {
            int i, j;
            for (i = 0; i < I[0]; i++)
            {
                if (I[i + 1] - 1 > index)
                {
                    break;
                }
            }
            for (j = I[0]; j > i; j--)
            {
                I[j + 1] = I[j];
            }
            I[i + 1] = index + 1;
            I[0]++;
        }
        // функция уменьшения множества активных ограничений
        public static void Outp(ref int[] I, int index)
        {
            for (int i = 0; i < I[0]; i++)
            {
                if (I[i + 1] - 1 >= index)
                {
                    I[i + 1] = I[i + 2];
                }
            }
            I[0]--;
        }
        //решение блока системы линейного уравнений в случае активности первого
        // ограничения СЛУ
        public static void Per1(ref Vector c, ref Vector h, int end)
        {
            for (int i = end; i >= 0; i--)
            {
                h[i] = 0;
                for (int j = 0; j <= i; j++)
                {
                    h[i] += c[j];
                }
                if (i < end)
                {
                    h[i] += h[i + 1];
                }
            }
        }
        // решение блока системы линейного уравнений в случае неактивности первого
        // и последнего СЛУ
        public static void Per2(ref Vector c, ref Vector h, int end, int dim)
        {
            for (int i = end; i > end - dim; i--)
            {
                h[i] = 0;
                for (int j = end - dim + 1; j <= i; j++)
                {
                    h[i] += Convert.ToDouble(j + dim - end) / (i - end + dim) * c[j];
                }
                if (i < end)
                {
                    h[i] = Convert.ToDouble(i - end + dim) / (i + 1 - end + dim) * (h[i] + h[i + 1]);
                }
                else
                {
                    h[end] *= Convert.ToDouble(dim) / (1 + dim);
                }
            }
        }
        // решение блока системы линейного уравнений в случае активности первого
        // и последнего СЛУ
        public static void Per3(ref Vector c, ref Vector h, int end, int dim)
        {
            for (int i = end; i > end - dim; i--)
            {
                h[i] = 0;
                for (int j = end - dim + 1; j <= i; j++)
                {
                    h[i] += Convert.ToDouble(j + dim - end) / (i - end + dim) * c[j];
                }
                if (i < end)
                {
                    h[i] = Convert.ToDouble(i - end + dim) / (i + 1 - end + dim) * (h[i] + h[i + 1]);
                }
                else
                {
                    h[end] *= Convert.ToDouble(dim);
                }
            }
        }

        public static void DecSL(ref Vector c, ref Vector h, ref int[] I)
        {
            int i, k = 1;
            for (i = 1; i < I[0]; i++)
            {
                if (I[i] + 1 < I[i + 1])
                {
                    if (I[i - I[i] + I[k]] == 1)
                    {
                        Per1(ref c, ref h, i - 1);
                    }
                    else
                    {
                        Per2(ref c, ref h, i - 1, I[i] - I[k] + 1);
                        k = i + 1;
                    }
                }
            }
            if (I[i - I[i] + I[k]] == 1)
            {
                Per1(ref c, ref h, i - 1);
            }
            else if (I[i] == dimTk + 1)
            {
                Per3(ref c, ref h, i - 1, I[i] - I[k] + 1);
            }
            else
            {
                Per2(ref c, ref h, i - 1, I[i] - I[k] + 1);
            }
        }
    }
}
