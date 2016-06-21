using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma_v2
{
    /// <summary>
    /// Класс вектор
    /// </summary>
    class Vector
    {
        /// <summary>
        /// Значения вектора
        /// </summary>
        public double[] vec;

        // Конструктор вектора размерности dim
        /// <summary>
        /// Конструктор класса <see cref="Vector"/>.
        /// </summary>
        /// <param name="dim">Размерность вектора.</param>
        public Vector(int dim)
        {
            vec = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                vec[i] = 0;
            }
        }
        // Умножение матрицы записанной построчно в вектор на другой вектор
        /// <summary>
        /// Операция умножения матрицы запсианой построчно на вектор.
        /// </summary>
        /// <param name="v1">Матрица.</param>
        /// <param name="v2">вектор.</param>
        /// <returns>Результат умножения.</returns>
        public Vector Mult(Vector v1, Vector v2)
        {
            int dim = (int)(v1.vec.Length / v2.vec.Length);
            Vector tmp = new Vector(dim);
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < v2.vec.Length; j++)
                {
                    tmp[i] += v1[i, j] * v2[j];
                }
            }
            return tmp;
        }

        // Перегрузки операторов
        // Перегружаем индексатор
        /// <summary>
        ///Перегрузка индексаторов <see cref="System.Double"/> для вектора.
        /// </summary>
        /// <param name="index">Индекс.</param>
        /// <returns>Значение вектора.</returns>
        public double this[int index]
        {
            get
            {
                return vec[index];
            }

            set
            {
                vec[index] = value;
            }
        }
        /// <summary>
        /// Перегрузка индексаторов <see cref="System.Double"/> для матрицы.
        /// </summary>
        /// <param name="i"> Строка.</param>
        /// <param name="j"> Столбец.</param>
        /// <returns>Значение матрицы в точке [i;j]</returns>
        public double this[int i, int j]
        {
            get
            {
                return vec[i * (int)(Math.Sqrt(vec.Length)) + j];
            }

            set
            {
                vec[i * (int)(Math.Sqrt(vec.Length)) + j] = value;
            }
        }
        // Сложение вектров
        /// <summary>
        /// Перегрузка оператора сложения.
        /// </summary>
        /// <param name="v1">Левый операнд.</param>
        /// <param name="v2">Правый операнд.</param>
        /// <returns>Реузльтат сложения.</returns>
        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector tmp = new Vector(v1.vec.Length);
            for (int i = 0; i < v1.vec.Length; i++)
            {
                tmp[i] = v1[i] + v2[i];
            }
            return tmp;
        }
        // Разность векторов
        /// <summary>
        /// Перегрузка оператора разности.
        /// </summary>
        /// <param name="v1">Левый операнд.</param>
        /// <param name="v2">Правый операнд.</param>
        /// <returns>Результат разности.</returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            int dim = v1.vec.Length;
            Vector tmp = new Vector(dim);
            for (int i = 0; i < dim; i++)
            {
                tmp[i] = v1[i] - v2[i];
            }
            return tmp;
        }
        // Произведение векторов
        /// <summary>
        /// Перегрузка оператора произведения для векторов.
        /// </summary>
        /// <param name="v1">Левый операнд.</param>
        /// <param name="v2">Правый операнд.</param>
        /// <returns>Результат произведения.</returns>
        public static double operator *(Vector v1, Vector v2)
        {
            double tmp = 0.0;
            for (int i = 0; i < v1.vec.Length; i++)
            {
                tmp += v1[i] * v2[i];
            }
            return tmp;
        }
        // Умножение вектора на число
        /// <summary>
        /// Перегрузка оператора проивзедения для скаляра и вектора.
        /// </summary>
        /// <param name="a">Скаляр.</param>
        /// <param name="v">Вектор.</param>
        /// <returns>Результат умножения.</returns>
        public static Vector operator *(double a, Vector v)
        {
            Vector tmp = new Vector(v.vec.Length);
            for (int i = 0; i < v.vec.Length; i++)
            {
                tmp[i] = a * v[i];
            }
            return tmp;
        }
        // Правое умножение вектора на матрицу
        /// <summary>
        /// Перегрузка оператора произведения для вектора и матрицы (Правое умножение).
        /// </summary>
        /// <param name="m">Матрица.</param>
        /// <param name="v">Вектор.</param>
        /// <returns>Результат умножения.</returns>
        public static Vector operator *(Matrix m, Vector v)
        {
            Vector tmp = new Vector(v.vec.Length);
            for (int i = 0; i < v.vec.Length; i++)
            {
                tmp[i] = 0;
                for (int j = 0; j < v.vec.Length; j++)
                {
                    tmp[i] += m[i, j] * v[j];
                }
            }
            return tmp;
        }
        // Левое умножение вектора на матрицу
        /// <summary>
        /// Перегрузка оператора произведения для вектора и матрицы (Левое умножение).
        /// </summary>
        /// <param name="v">Вектор.</param>
        /// <param name="m">Матрица.</param>
        /// <returns>Результат умножения.</returns>
        public static Vector operator *(Vector v, Matrix m)
        {
            Vector tmp = new Vector(v.vec.Length);
            for (int i = 0; i < v.vec.Length; i++)
            {
                tmp[i] = 0;
                for (int j = 0; j < v.vec.Length; j++)
                {
                    tmp[i] += m[j, i] * v[j];
                }
            }
            return tmp;
        }
    }


    /// <summary>
    /// Класс матрица
    /// </summary>
    class Matrix
    {
        public double[,] d;

        // Конструктор матрицы размерности dim1*dim2
        /// <summary>
        /// Конструтор класса <see cref="Matrix"/>.
        /// </summary>
        /// <param name="dim1">Размерность строк.</param>
        /// <param name="dim2">Размерность столбцов.</param>
        public Matrix(int dim1, int dim2)
        {
            d = new double[dim1, dim2];
            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    d[i, j] = 0;
                }
            }
        }

        // Транспонирование матрицы
        /// <summary>
        /// Транспонирование матрицы
        /// </summary>
        /// <param name="m">Матрица.</param>
        /// <returns>Транспонированная матрица.</returns>
        public Matrix Tran(Matrix m)
        {
            int l = m.d.Length;
            int r = m.d.Rank;
            Matrix tmp = new Matrix(l, r);
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    tmp[i, j] = m[j, i];
                }
            }
            return tmp;
        }

        // Перегрузка операторов
        // Перегрузка индексаторов
        /// <summary>
        /// Перегрузка индексаторов <see cref="System.Double"/> 
        /// </summary>
        /// <param name="i">Строка.</param>
        /// <param name="j">Столбец.</param>
        /// <returns>Элемент [i,j].</returns>
        public double this[int i, int j]
        {
            get
            {
                return d[i, j];
            }

            set
            {
                d[i, j] = value;
            }
        }
        // Сложение матриц
        /// <summary>
        /// Перегрузка оператора сложения для матриц
        /// </summary>
        /// <param name="m1">Операнд 1.</param>
        /// <param name="m2">Опернад 2.</param>
        /// <returns>Результат сложения.</returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            int l = m1.d.Length;
            int r = m1.d.Rank;
            Matrix tmp = new Matrix(l, r);
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    tmp[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return tmp;
        }
        // Разность матриц
        /// <summary>
        /// Перегрузка оператора разности для матриц
        /// </summary>
        /// <param name="m1">Операнд 1</param>
        /// <param name="m2">Опернад 2</param>
        /// <returns>TРезультат разности.</returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            int l = m1.d.Length;
            int r = m1.d.Rank;
            Matrix tmp = new Matrix(l, r);
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    tmp[i, j] = m1[i, j] - m2[i, j];
                }
            }
            return tmp;
        }
        // Умножение матрицы на скаляр
        /// <summary>
        /// Перегрузка оператора умножения для матрица и скаляра
        /// </summary>
        /// <param name="a">Скаляр.</param>
        /// <param name="m">Матрица.</param>
        /// <returns>Результат умножения.</returns>
        public static Matrix operator *(double a, Matrix m)
        {
            int l = m.d.Length;
            int r = m.d.Rank;
            Matrix tmp = new Matrix(l, r);
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    tmp[i, j] = a * m[i, j];
                }
            }
            return tmp;
        }
        // Умножение матриц
        /// <summary>
        /// Перегрузка оператора умножения для матриц
        /// </summary>
        /// <param name="m1">Операнд 1.</param>
        /// <param name="m2">Опернад 2.</param>
        /// <returns>The result of the operator.</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int l = m1.d.Length;
            int r = m1.d.Rank;
            Matrix tmp = new Matrix(l, r);
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    tmp[i, j] = 0;
                    for (int k = 0; k < l; k++)
                    {
                        tmp[i, j] += m1[i, k] - m2[k, j];
                    }
                }
            }
            return tmp;
        }
    }
}
