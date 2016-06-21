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
        /// ВЫчисление определенного интеграла по формуле Симпсона
        /// </summary>
        /// <param name="t">Задача.</param>
        /// <param name="ts">Время ts.</param>
        /// <param name="Th">Время th.</param>
        /// <param name="z">Структура управления.</param>
        /// <param name="i">The i.</param>
        /// <param name="k">The k.</param>
        /// <param name="step1">Шаг интегрирования.</param>
        /// <returns>System.Double.</returns>
        public static double SimpsonD(ref Task t, double ts, double Th, ref Vector z, int i, int k, double step1)
        {
            double sum = 0; // sum - сумма для dH/du
            double y0, y1, y2, s, step;
            Vector x = new Vector(dimX);
            Vector p = new Vector(dimX);
            step = step1;


            s = ts;
            x = GetX(ref t, s, ref z);
            p = GetP(ref t, s);
            y2 = dHu(ref t, x, ref p, s, ref z, i, step1) * t.dudv(ref z, s, i, k); // в точке s

            while (s + step < Th)
            {
                y0 = y2;
                x = GetX(ref t, s + step / 2, ref z);
                p = GetP(ref t, s + step / 2);
                // в точке (s+h/2);
                y1 = dHu(ref t, x, ref p, s + step / 2, ref z, i, step1) * t.dudv(ref z, s + step / 2, i, k);
                x = GetX(ref t, s + step, ref z);
                p = GetP(ref t, s + step);
                // в точке (s+h);
                y2 = dHu(ref t, x, ref p, s + step, ref z, i, step1) * t.dudv(ref z, s + step, i, k);

                sum += step / 6 * (y0 + 4 * y1 + y2);
                s += step;
            }
            step = Th - s;
            y0 = y2;
            x = GetX(ref t, s + step / 2, ref z);
            p = GetP(ref t, s + step / 2);
            // в точке (s+h/2);
            y1 = dHu(ref t, x, ref p, s + step / 2, ref z, i, step1) * t.dudv(ref z, s + step / 2, i, k);
            x = GetX(ref t, s + step, ref z);
            p = GetP(ref t, s + step);
            // в точке (s+h);
            y2 = dHu(ref t, x, ref p, s + step, ref z, i, step1) * t.dudv(ref z, s + step, i, k);

            sum += step / 6 * (y0 + 4 * y1 + y2);

            return sum;
        }

        /// <summary>
        /// Интегрирвоание фазовых переменных
        /// </summary>
        /// <param name="ts">Время ts.</param>
        /// <param name="tf">Время tf.</param>
        /// <param name="z">Структура управления.</param>
        /// <param name="t">Задача.</param>
        /// <param name="x">Вектор x.</param>
        /// <param name="tdiscr">Частота дсикретизации запоминания.</param>
        /// <param name="step">Шан интегрирования.</param>
        public static void alerx(double ts, double tf, ref Vector z, ref Task t, ref Vector x, double tdiscr, double step)
        {
            Vector tmp = new Vector(dimX);
            // variables for method
            Vector k1 = new Vector(dimX);
            Vector k2 = new Vector(dimX);
            double step1;

            while (ts < tf - step) // Метод Рунге-Кутта 2-го порядка для фазовых переменных
            {
                tmp = x;
                k1 = fvec(ref t, tmp, ts, ref z, tdiscr, step);
                tmp = x + step * k1;
                k2 = fvec(ref t, tmp, ts + step, ref z, tdiscr, step);
                x += (step / 2) * (k1 + k2);
                ts += step;
            }
            if (tf - ts > eps)
            {
                step1 = tf - ts;
                tmp = x;
                k1 = fvec(ref t, tmp, ts, ref z, tdiscr, step);
                tmp = x + step1 * k1;
                k2 = fvec(ref t, tmp, ts + step1 - eps, ref z, tdiscr, step);
                x += (step1 / 2) * (k1 + k2);
            }
        }

        /// <summary>
        /// Интегрирвоание фазовых переменных
        /// </summary>
        /// <param name="ts">Время ts.</param>
        /// <param name="tf">Время tf.</param>
        /// <param name="t">Задача.</param>
        /// <param name="p">The p.</param>
        /// <param name="z">Структура управления.</param>
        /// <param name="tdiscr">Частота дсикретизации запоминания.</param>
        /// <param name="step">Шаг интегрирования.</param>
        public static void alerp(double ts, double tf, ref Task t, ref Vector p, ref Vector z, double tdiscr, double step)
        {
            Vector tmp = new Vector(dimX);
            // variables for method
            Vector k1 = new Vector(dimX);
            Vector k2 = new Vector(dimX);
            double step1;

            while (tf - step > ts) // метод Рунге-Кутта 2-го порядка для двойственных переменных
            {
                tmp = p;
                k1 = dp(ref t, ref tmp, tf, ref z, tdiscr, step);
                tmp = p - step * k1;
                k2 = dp(ref t, ref tmp, tf - step, ref z, tdiscr, step);
                p += -(step / 2) * (k1 + k2);
                tf -= step;
            }
            if (tf - ts > eps)
            {
                step1 = tf - ts;
                tmp = p;
                k1 = dp(ref t, ref tmp, tf, ref z, tdiscr, step);
                tmp = p - step1 * k1;
                k2 = dp(ref t, ref tmp, tf - step1, ref z, tdiscr, step);
                p += -(step1 / 2) * (k1 + k2);
            }
        }

        // Интеграл в функции х с точкой
        public static Vector alerIntExpr(double s, ref Vector xt, ref Vector z, ref Task t, double tdiscr, double step)
        {
            Vector xs = new Vector(dimX);
            Vector x = new Vector(dimX);
            // variables for method
            Vector k1 = new Vector(dimX);
            Vector k2 = new Vector(dimX);
            double ts = t0;
            for (int i = 0; i < dimX; i++)
            {
                x[i] = 0;
            }
            while (ts < tdiscr - step) // Метод Рунге-Кутта 2-го порядка для фазовых переменных
            {
                xs = GetX(ref t, ts, ref z);
                k1 = gvec(ref t, ts, s, xs, ref z);
                xs = GetX(ref t, ts + step, ref z);
                k2 = gvec(ref t, ts + step, s, xs, ref z);
                x += (step / 2) * (k1 + k2);
                ts += step;
            }
            if (s - ts > eps)
            {
                step = s - ts;
                xs = GetX(ref t, ts, ref z);
                k1 = gvec(ref t, ts, s, xs, ref z);
                k2 = gvec(ref t, s, s, xt, ref z);
                x += (step / 2) * (k1 + k2);
            }
            return x;
        }
        //р с  точкой // p'      x=x(t)  p=p(t)
        public static Vector dp(ref Task t, ref Vector pt, double s, ref Vector z, double tdiscr, double step)
        {
            Vector dpdt = new Vector(dimX);
            Vector x = new Vector(dimX);

            x = GetX(ref t, s, ref z);
            dpdt = (-1) * pt * dfdx(t, x, s, ref z);

            if (s < T)
            {
                dpdt += (-1) * SubIntP(s, ref x, pt, ref z, ref t, tdiscr, step);
            }
            return dpdt;
        }
        //интеграл в функции р
        public static Vector SubIntP(double ts, ref Vector xt, Vector pt, ref Vector z, ref Task t, double tdiscr, double step)
        {
            Vector tmp = new Vector(dimX);
            Vector p = new Vector(dimX);
            // variables for method
            Vector k1 = new Vector(dimX);
            Vector k2 = new Vector(dimX);
            double tf = T;

            for (int i = 0; i < dimX; i++)
            {
                p[i] = 0;
            }

            // метод Рунге-Кутта 2-го порядка для двойственных переменных

            while (tf - tdiscr > step)
            {
                tmp = GetP(ref t, tf);
                k1 = tmp * dgdx(t, ts, tf, xt, ref z);
                tmp = GetP(ref t, tf - step);
                k2 = tmp * dgdx(t, ts, tf - step, xt, ref z);
                p += -(step / 2) * (k1 + k2);
                tf -= step;
            }
            if (tf - ts > eps)
            {
                step = tf - ts;
                tmp = GetP(ref t, tf);
                k1 = tmp * dgdx(t, ts, tf, xt, ref z);
                k2 = pt * dgdx(t, ts, ts, xt, ref z);
                p += -(step / 2) * (k1 + k2);
            }
            return p;
        }
        //интегреал в функции Н
        public static double SubIntM(ref Task t, Vector xt, double s, ref Vector z, int i, double step)
        {
            double sum = 0; // sum - сумма для dH/du
            double y1, y2, y0;
            Vector x = new Vector(dimX);
            Vector p = new Vector(dimX);

            p = GetP(ref t, s);
            y2 = p * gvec(ref t, s, s, xt, ref z); // в точке s

            while (s + step < T)
            {
                y0 = y2;
                p = GetP(ref t, s + step / 2);
                // в точке (s+h/2);
                y1 = p * gvec(ref t, s, s + step / 2, xt, ref z);
                p = GetP(ref t, s + step);
                // в точке (s+h);
                y2 = p * gvec(ref t, s, s + step, xt, ref z);

                sum += step / 6 * (y0 + 4 * y1 + y2);
                s += step;
            }
            step = T - s;
            y0 = y2;
            p = GetP(ref t, s + step / 2);
            // в точке (s+h/2);
            y1 = p * gvec(ref t, s, s + step / 2, xt, ref z);
            p = GetP(ref t, s + step);
            // в точке (s+h);
            y2 = p * gvec(ref t, s, s + step, xt, ref z);

            sum += step / 6 * (y0 + 4 * y1 + y2);

            return sum;

        }
        //расчсет интеграла в функции dНdu
        public static double SubIntDHu(ref Task t, Vector xt, double s, ref Vector z, int i, double step)
        {
            double sum = 0; // sum - сумма для dH/du
            double y1, y2, y0;
            Vector x = new Vector(dimX);
            Vector p = new Vector(dimX);

            p = GetP(ref t, s);
            y2 = p * dgui(ref t, s, s, xt, ref z, i); // в точке s 

            while (s + step < T)
            {
                y0 = y2;
                p = GetP(ref t, s + step / 2);
                // в точке (s+h/2);
                y1 = p * dgui(ref t, s, s + step / 2, xt, ref z, i);
                p = GetP(ref t, s + step);
                // в точке (t+h);
                y2 = p * dgui(ref t, s, s + step, xt, ref z, i);

                sum += step / 6 * (y0 + 4 * y1 + y2);
                s += step;
            }
            step = T - s;
            y0 = y2;
            p = GetP(ref t, s + step / 2);
            // в точке (s+h/2);
            y1 = p * dgui(ref t, s, s + step / 2, xt, ref z, i);
            p = GetP(ref t, s + step);
            // в точке (s+h);
            y2 = p * dgui(ref t, s, s + step, xt, ref z, i);

            sum += step / 6 * (y0 + 4 * y1 + y2);

            return sum;

        }
    }
}
