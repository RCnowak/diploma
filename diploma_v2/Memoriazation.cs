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
        /// Запоминание х на сетке
        /// </summary>
        /// <param name="t">Задача.</param>
        /// <param name="z">Структура управления.</param>
        /// <param name="s">Время s.</param>
        /// <param name="step">Шаг интегрирования.</param>
        /// <returns>Вектор х в точке s.</returns>
        public static Vector XintD(ref Task t, ref Vector z, double s, double step)
        {
            if (s <= t0)
            {
                return t.Initx0(ref z, s);
            }
            // при фиксированных управлении u, времени t0 и t выдает x(t)
            // step -  step of integration
            // t -  time of integration
            int i, k;
            Vector x = new Vector(dimX);
            Vector l = new Vector(dimX);
            double[] zh = new double[dimTk + 2];
            double ts, dt, h;


            zh[0] = t0;
            for (i = 0; i < dimTk; i++)
            {
                zh[i + 1] = z[(i + 1) * (dimU * dimP + 1) - 1];
            }
            zh[dimTk + 1] = T;

            k = 0;
            t.X[0] = t.Initx0(ref z, t0);
            x = t.X[0];
            i = 0;

            dt = (T - t0) / (discr - 1);
            ts = t0;
            h = dt;

            // Перебираем каждый интервал моментов переключения. t0 - t1, t1 - t2, t2 - t3, ... tn-1 - tn = T
            while (((s - eps) > zh[i + 1]) && (i < dimTk))
            {
                // Идем по конкретному интервалу
                while (ts + h < zh[i + 1])
                {
                    // Интегрирование х от ts до ts + h 
                    alerx(ts + eps, ts + h - eps, ref z, ref t, ref x, dt * k, step);
                    k++;
                    t.X[k] = x;
                    h = dt;
                    ts += h;
                }
                // Интегрирование х от ts до zh[i + 1]
                alerx(ts + eps, zh[i + 1] - eps, ref z, ref t, ref x, dt * k, step);
                h = dt - zh[i + 1] + ts;
                ts += dt - h;
                i++;
            }
            // Запоминание на интервале ti-s, где s между ti и ti+1 моментами переключения
            while (ts + h < s)
            {
                // Интегрирование х от ts до ts + h 
                alerx(ts + eps, ts + h - eps, ref z, ref t, ref x, dt * k, step);
                k++;
                t.X[k] = x;
                ts += h;
                h = dt;
            }
            // Последнее запоминание на сетке
            if (s + eps > T)
            {
                t.X[discr - 1] = x;

            }

            // Интегрирование х от ts до s 
            alerx(ts + eps, s - eps, ref z, ref t, ref x, dt * k, step);

            return x;
        }

        /// <summary>
        /// Запоминание p на сетк
        /// </summary>
        /// <param name="t">Задача.</param>
        /// <param name="z">Структура управления.</param>
        /// <param name="s">Время s.</param>
        /// <param name="step">Шаг интегрирования.</param>
        /// <returns>Вектор p в точке s.</returns>
        public static Vector PintD(ref Task t, ref Vector z, double s, double step)
        {
            double h;
            double dt = (T - t0) / (discr - 1);
            Vector x = new Vector(dimX);
            Vector p = new Vector(dimX);
            double[] zh = new double[dimTk + 2];
            double tf = T;

            // при фиксированных управлении u, времени t0 и t выдает p
            // h -  step of integration
            // t -  time of integration

            zh[0] = t0;
            int i, k;
            for (i = 0; i < dimTk; i++)
            {
                zh[i + 1] = z[(i + 1) * (dimU * dimP + 1) - 1];
            }
            zh[dimTk + 1] = T;

            // Значение фазовой переменной в конечный момент времени
            x = t.X[discr - 1];
            // Значение сопряженой перменной в конечный момент времени
            t.P[discr - 1] = t.Initp0(ref x, ref z, s);
            p = t.P[discr - 1];

            k = 1;
            i = 0;

            h = dt;

            // Перебираем каждый интервал моментов переключения в обратном порядке. t0 - t1, t1 - t2, t2 - t3, ... tn-1 - tn = T
            while ((s + eps < zh[dimTk - i]) && (i < dimTk))
            {
                // Идем по конкретному интервалу
                while (tf - h > zh[dimTk - i])
                {
                    // Интегрирование p от (tf - h) до tf
                    alerp(tf - h, tf, ref t, ref p, ref z, T - dt * (k - 1), step);
                    tf = T - k * h;
                    k++;
                    t.P[discr - k] = p;
                    h = dt;
                }
                if (tf > zh[dimTk - i])
                {
                    // Интегрирование p от (zh[dimTk - i] - h) до tf
                    alerp(zh[dimTk - i] + eps, tf, ref t, ref p, ref z, T - dt * (k - 1), step);
                    h = dt + zh[dimTk - i] - tf; tf -= dt - h;
                }
                i++;
            }
            // Запоминание на интервале s-ti+1, где s между ti и ti+1 моментами переключения
            while (tf - h > s)
            {
                // Интегрирование p от (tf - h) до tf
                alerp(tf - h, tf, ref t, ref p, ref z, T - dt * (k - 1), step);
                h = dt;
                tf = T - k * h;
                k++;
                t.P[discr - k] = p;
            }
            // Интегрирование p от s до tf
            alerp(s, tf, ref t, ref p, ref z, T - dt * (k - 1), step);

            // Последнее запоминание на сетке
            if (s - eps < t0)
            {
                t.P[0] = p;
            }

            return p;
        }

        /// <summary>
        /// Получить X из массива
        /// </summary>
        /// <param name="t">Задача.</param>
        /// <param name="s">Время s.</param>
        /// <param name="z">Структура управления.</param>
        /// <returns>Вектор х в точке s.</returns>
        public static Vector GetX(ref Task t, double s, ref Vector z)
        {
            int k;
            double dt = (T - t0) / (discr - 1);
            if (s >= T)
            {
                return (t.X[discr - 1] + (s - T) / dt * (t.X[discr - 1] - t.X[discr - 2]));
            }
            if (s <= t0)
            {
                return t.Initx0(ref z, s);
            }
            else
            {
                k = (int)((s - t0 - eps) / dt);
                return (t.X[k] + ((s - t0) / dt - k) * (t.X[k + 1] - t.X[k]));
            }
        }

        /// <summary>
        /// Получить Р из массива
        /// </summary>
        /// <param name="t">Задача.</param>
        /// <param name="s">Время s.</param>
        /// <returns>Вектор p в точке s.</returns>
        public static Vector GetP(ref Task t, double s)
        {
            double dt = (T - t0) / (discr - 1);
            int k = (int)((T - s - eps) / dt);

            if (s >= T)
            {
                return t.Initp0(ref t.X[discr - 1 - k], ref t.z, s);
            }
            if (s < t0)
            {
                return (t.P[0] + (s - t0) / dt * (t.P[1] - t.P[0]));
            }

            return (t.P[discr - k - 1] + ((T - s) / dt - k) * (t.P[discr - k - 2] - t.P[discr - 1 - k]));
        }
    }
}
