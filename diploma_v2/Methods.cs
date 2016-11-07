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
        /// Метод Ньютона.
        /// </summary>
        /// <param name="t">Задача.</param>
        public static void Newton(ref Task t)
        {

            Vector z2 = new Vector(dimAllP);
            Vector g = new Vector(dimAllP);
            Vector h = new Vector(dimAllP);
            Vector add = new Vector(dimAllP);
            Vector y = new Vector(dimX);
            Vector H = new Vector(dimAllP * dimAllP);
            double L;
            double check;
            double funval; //значение целевой функции
            int count = 1;

           // Console.Clear();

            MatrHes(ref t, ref t.z, t.m, step);
            if (t.m)
            {
                GradD(ref t, ref t.z, ref g, step);
            }
            else
            {
                Grad(ref t, ref t.z, ref g, step);
            }

            Console.WriteLine("Initial gradient");
            for (int i = 0; i < dimAllP; i++)
            {
                Console.Write("g[{0}] = {1}          ", i, g[i]);
            }

            funval = t.faim(ref t, ref t.z, step);  //начальное значение функции
            Console.WriteLine(" f = {0}", funval);

            Console.WriteLine(" Result");

            // Проверка нормы градиента, возможно уже нет смысла решать задачу
            check = meg(g, dimAllP);
            bool flag = true;
            if (check < ep)
            {
                flag = false;
            }

            while (flag)
            {
                add = (-1) * g;
                DecSLU(ref t, ref add, ref h);
                z2 = t.z + h;
                Project(ref z2);

                L = 1;
                while (funval <= t.faim(ref t, ref z2, step))
                {
                    L /= 3;
                    z2 = t.z + L * h;
                    Project(ref z2);
                    if (L < 1e-20)
                    {
                        break;
                    }
                }

                MatrHes(ref t, ref z2, t.m, step);
                if (t.m)
                {
                    GradD(ref t, ref z2, ref g, step);
                }
                else
                {
                    Grad(ref t, ref z2, ref g, step);
                }

                Console.WriteLine(" Iteration  -  {0}", count);
                for (int i = 0; i < dimAllP; i++)
                {
                    Console.WriteLine(" z [{0}] = {1}, g[{2}] = {3}", i + 1, z2[i], i + 1, g[i]);
                }

                funval = t.faim(ref t, ref z2, step);
                Console.WriteLine(" f = {0}", funval);

                if (meg(t.z - z2, dimAllP) < ep)
                {
                    flag = false;
                }

                t.z = z2;
                check = meg(g, dimAllP);
                if (check < ep)
                {
                    break;
                }
                count++;
                if (count == 15)
                {
                    flag = false;
                }
                Console.WriteLine("  The solution is  ");
                for (int i = 0; i < dimAllP; i++)
                {
                    Console.WriteLine(" z [{0}] = {1}  \n", i + 1, z2[i]);
                }
            }
        }

        /// <summary>
        /// Метод наискорейшего спуска.
        /// </summary>
        /// <param name="t">Задача.</param>
        public static void Spusk(ref Task t)
        {
            Vector z2 = new Vector(dimAllP);
            Vector g = new Vector(dimAllP);
            Vector h = new Vector(dimAllP);
            Vector add = new Vector(dimAllP);
            double L, check, funval;
            int count = 0;
            Vector y = new Vector(dimX);
            bool flag = true;

            //Console.Clear();

            if (t.m)
            {
                GradD(ref t, ref t.z, ref g, step);
            }
            else
            {
                Grad(ref t, ref t.z, ref g, step);
            }


            Console.WriteLine("Initial gradient");
            for (int i = 0; i < dimAllP; i++)
            {
                Console.Write("g[{0}] = {1}          ", i, g[i]);
            }


            funval = t.faim(ref t, ref t.z, step);
            Console.WriteLine(" f = {0}", funval);

            Console.WriteLine(" Result ");

            // В качестве проверки следуте использовать норму вектора градиента, так как в точке максимума градиент должен быть равен нулю
            check = meg(g, dimAllP);
            //Количество итераций
            count++;
            // Если норма градиента достаточна мала, процесс прекращается
            if (check < ep)
                flag = false;

            while (flag)
            {
                add = (-1) * g;                            // Антиградиент
                L = onemin(ref t.z, ref add, step, ref t); // Одномерна минимизация функции в точке (z+l*add) методом золотого сечения
                z2 = t.z + L * add;                        // Вектор управления для новой итерации
                Project(ref z2);                           //Метод проекций для контроля правильного порядка моментов переключения
                if (funval < t.faim(ref t, ref z2, step))
                    for (int i = 0; i < dimAllP; i++)
                    {
                        for (int j = 0; j < dimAllP; j++)
                        {
                            add[j] = 0;
                        }
                        add[i] = -g[i];
                        L = onemin(ref z2, ref add, step, ref t); // minim function value of f(l) at point (z+l*add)
                        z2 += L * add;
                        Project(ref z2);
                    }
                if (funval < t.faim(ref t, ref z2, step))
                    break;

                if (t.m)
                {
                    GradD(ref t, ref z2, ref g, step);
                }
                else
                {
                    Grad(ref t, ref z2, ref g, step);
                }

                Console.WriteLine(" Iteration  -  {0}", count);

                Console.WriteLine("");
                for (int i = 0; i < dimAllP; i++)
                {
                    Console.WriteLine(" z [{0}] = {1}, g[{2}] = {3}", i + 1, z2[i], i + 1, g[i]);
                }

                funval = t.faim(ref t, ref z2, step);
                Console.WriteLine(" f = {0}", funval);

                // 
                if (meg(t.z - z2, dimAllP) < ep)
                    flag = false;
                t.z = z2;
                // Ежеитерационная проверка нормы вектора
                check = meg(g, dimAllP);
                if (check < ep)
                    break;
                // При достижении болшого количества итераций процесс прекращается
                count++;
                if (count == 15)
                    flag = false;
            }

            Console.WriteLine("   The solution is");
            for (int i = 0; i < dimAllP; i++)
            {
                Console.WriteLine(" z [{0}] = {1} ", i + 1, t.z[i]);
            }
        }

        /// <summary>
        /// Метод DFP.
        /// </summary>
        /// <param name="t">Задача.</param>
        public static void DFP(ref Task t)
        {
            Vector z2 = new Vector(dimAllP);
            Vector g = new Vector(dimAllP);
            Vector g2 = new Vector(dimAllP);
            Vector h = new Vector(dimAllP);
            Vector dg = new Vector(dimAllP);
            Vector dz = new Vector(dimAllP);
            Vector y = new Vector(dimX);
            double L, L2, check, funval;
            int j, count = 0;
            bool flag = true;

           // Console.Clear();

            if (t.m)
            {
                GradD(ref t, ref t.z, ref g, step);
            }
            else
            {
                Grad(ref t, ref t.z, ref g, step);
            }
            for (int i = 0; i < dimAllP; i++)
                for (j = 0; j < dimAllP; j++)
                    if (i == j)
                        t.Q[i, j] = 1;
                    else
                        t.Q[i, j] = 0;

            Console.WriteLine("Initial gradient");
            for (int i = 0; i < dimAllP; i++)
            {
                Console.Write("g[{0}] = {1}          ", i, g[i]);
            }

            funval = t.faim(ref t, ref t.z, step);
            Console.WriteLine(" f = {0}", funval);

            Console.WriteLine(" Result");

            check = meg(g, dimAllP);

            count++;
            if (check < ep)
                flag = false;
            while (flag)
            {
                h.Mult(t.Q, g); //h = t.Q * g;
                h = (-1) * h;
                L = onemin(ref t.z, ref h, step, ref t); // minim function value of f(l) at point (z+L*add)
                z2 = t.z + L * h;
                if (t.m)
                {
                    GradD(ref t, ref z2, ref g2, step);
                }
                else
                {
                    Grad(ref t, ref z2, ref g2, step);
                }
                dg = g2 - g;
                dz = z2 - t.z;
                h.Mult(t.Q, dg);  //h = t.Q * dg;
                L = dg * h;
                L2 = dz * dz;
                for (int i = 0; i < dimAllP; i++)
                    for (j = 0; j < dimAllP; j++)
                        t.Q[i, j] += -h[i] * h[j] / L + dz[i] * dz[j] / L2;

                Console.WriteLine(" Iteration  -  {0}", count);
                for (int i = 0; i < dimAllP; i++)
                {
                    Console.WriteLine(" z [{0}] = {1}, g[{2}] = {3}", i + 1, z2[i], i + 1, g[i]);
                }

                funval = t.faim(ref t, ref z2, step);
                Console.WriteLine(" f = {0}", funval);
                if (meg(t.z - z2, dimAllP) < ep)
                    flag = false;
                t.z = z2;
                g = g2;
                check = meg(g, dimAllP);
                if (check < ep)
                    break;
                count++;
                if (count == 15)
                    flag = false;
            }

            Console.WriteLine("  The solution is   ");
            for (int i = 0; i < dimAllP; i++)
            {
                Console.WriteLine(" z [{0}] = {1}  \n", i + 1, z2[i]);
            }
            t.z = z2;
        }

        /// <summary>
        /// Метод золотго сечения.
        /// </summary>
        /// <param name="z"> Структура управления.</param>
        /// <param name="add"> Приращение.</param>
        /// <param name="step"> Шаг интугрирования.</param>
        /// <param name="t">Задача.</param>
        /// <returns>Множитель приращения чтобы оеспечить максимально эффективный шаг.</returns>
        public static double onemin(ref Vector z, ref Vector add, double step, ref Task t)
        {
            double tau = (Math.Sqrt(5) - 1) / 2;
            double theta = (3 - Math.Sqrt(5)) / 2;
            double ak, bk, sk, rk, fs, fr;
            double a = 0.0, b = 1, e = ep;
            double fa, fb;
            ak = a;
            bk = b;
            sk = ak + tau * (bk - ak);
            rk = ak + theta * (bk - ak);
            fa = func(ref t, step, a, ref z, ref add);
            fs = func(ref t, step, sk, ref z, ref add);
            fr = func(ref t, step, rk, ref z, ref add);

            while ((bk - ak) > e)
            {
                if (fr <= fs)
                {
                    bk = sk;
                    sk = rk;
                    fs = fr;
                    rk = ak + theta * (bk - ak);
                    fr = func(ref t, step, rk, ref z, ref add);
                }
                else
                {
                    ak = rk;
                    rk = sk;
                    fr = fs;
                    sk = ak + tau * (bk - ak);
                    fs = func(ref t, step, sk, ref z, ref add);
                }
            }
            b = (ak + bk) / 2;
            fb = func(ref t, step, b, ref z, ref add);
            if (fa > fb)
                return b;
            while ((fb > fa) && (b > e))
            {
                b /= 3;
                fb = func(ref t, step, b, ref z, ref add);
            }
            return b;
        }
    }
}
