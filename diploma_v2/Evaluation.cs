using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace diploma_v2
{
    partial class Task
    {
        /// <summary>
        /// Проверка, что токен является оператором.
        /// </summary>
        /// <param name="op">Потенциальный оператор.</param>
        /// <returns><c>true</c> если "ор" оператор; иначе, <c>false</c>.</returns>
        public static bool isOperator(string op)
        {
            if (op[0] == '+' || op[0] == '-' || op[0] == '*' || op[0] == '/' || op[0] == 'c' || op[0] == 's' || op[0] == 't' || op[0] == 'l' || op[0] == '^' || op[0] == 'n')
            {
                return true;
            }
            else return false;
        }

        public static bool isFirstclass(string op1)
        {
            switch (op1[0])
            {
                case '+':
                    return true;
                case '-':
                    return true;
                default:
                    return false;
            }
        }
        public static bool isSecondclass(string op1)
        {
            switch (op1[0])
            {
                case '*':
                    return true;
                case '/':
                    return true;
                default:
                    return false;
            }

        }
        public static bool isThirdclass(string op1)
        {
            if (op1 == "sin" || op1 == "cos" || op1 == "tg" || op1 == "ctg" || op1 == "ln" || op1 == "neg" || op1 == "cut")
                return true;
            else return false;
        }
        public static bool isFourthclass(string op1)
        {
            if (op1 == "^")
                return true;
            else return false;
        }

        /// <summary>
        /// Определение приоритета оператора "ор1" над "ор2".
        /// </summary>
        /// <param name="op1"> Оператор 1.</param>
        /// <param name="op2">Оператор 2.</param>
        /// <returns>Приоритет</returns>
        public static int precedence(string op1, string op2)
        {
            if (isFirstclass(op1) && isFirstclass(op2)) return 0;
            else
            {
                if (isFirstclass(op1) && isSecondclass(op2)) return -1;
                else
                {
                    if (isFirstclass(op1) && isThirdclass(op2)) return -1;
                    else
                    {
                        if (isFirstclass(op1) && isFourthclass(op2)) return -1;
                        else
                        {
                            if (isSecondclass(op1) && isFirstclass(op2)) return 1;
                            else
                            {
                                if (isSecondclass(op1) && isSecondclass(op2)) return 0;
                                else
                                {
                                    if (isSecondclass(op1) && isThirdclass(op2)) return -1;
                                    else
                                    {
                                        if (isSecondclass(op1) && isFourthclass(op2)) return -1;
                                        else
                                        {
                                            if (isThirdclass(op1) && isFirstclass(op2)) return 1;
                                            else
                                            {
                                                if (isThirdclass(op1) && isSecondclass(op2)) return 1;
                                                else
                                                {
                                                    if (isThirdclass(op1) && isThirdclass(op2)) return 0;
                                                    else
                                                    {
                                                        if (isThirdclass(op1) && isFourthclass(op2)) return -1;
                                                        else
                                                        {
                                                            if (isFourthclass(op1) && isFirstclass(op2)) return 1;
                                                            else
                                                            {
                                                                if (isFourthclass(op1) && isSecondclass(op2)) return 1;
                                                                else
                                                                {
                                                                    if (isFourthclass(op1) && isThirdclass(op2)) return 1;
                                                                    else
                                                                    {
                                                                        if (isFourthclass(op1) && isFourthclass(op2)) return 0; else return 5;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        // Проверка наличия скобок
        public static bool notParanth(string p)
        {
            if (p != "(")
            {
                return true;
            }
            else return false;
        }

        public static int getindex(string v)
        {
            string index = "";
            for (int i = 0; i < v.Length - 1; i++)
            {
                if (Char.IsDigit(v[i + 1]))
                {
                    index += v[i + 1];
                }
                else
                {
                    Console.WriteLine("Error! Wrong index of x");
                    Process.Start(Assembly.GetExecutingAssembly().Location);            // Ошибка парсинга, перезагрузка приложения
                    Environment.Exit(0);
                }
            }
            return Convert.ToInt32(index, Program.nfi);

        }

        public static double getnumber(string v)
        {
            
            double number = 0;
            try
            {
                number = Convert.ToDouble(v, Program.nfi);
            }
            catch
            {
                MessageBox.Show("Error! Wrong syntax!");
                Process.Start(Assembly.GetExecutingAssembly().Location);            // Ошибка парсинга, перезагрузка приложения
                Environment.Exit(0);
            }
            return number;
        }

        /// <summary>
        /// Вычисление унарной операции.
        /// </summary>
        /// <param name="x">Операнд.</param>
        /// <param name="v">Оператор.</param>
        /// <returns>Значение операции.</returns>
        public static double unary_func(double x, string v)
        {
            switch (v[0])
            {
                case 's':
                    return Math.Sin(x);
                case 'c':
                    if (v[1] == 'o')
                        return Math.Cos(x);
                    else if (v[1] == 'u')
                        return Program.Cut(x);
                    else
                        return 1 / Math.Tan(x);
                case 't':
                    return Math.Tan(x);
                case 'l':
                    return Math.Log(x);
                case 'n':
                    return -x;
                default: return 0;
            }
        }

        /// <summary>
        /// Проверка, что операция бинарная
        /// </summary>
        /// <param name="v">Оператор.</param>
        /// <returns><c>true</c> если операция бинарная; иначе, <c>false</c>.</returns>
        public static bool isBinary(string v)
        {
            switch (v[0])
            {
                case '+':
                    return true;
                case '-':
                    return true;
                case '*':
                    return true;
                case '/':
                    return true;
                case '^':
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Вычисление бинарной операции.
        /// </summary>
        /// <param name="x">Левый операнд.</param>
        /// <param name="y">Правый операнд.</param>
        /// <param name="v">Оператор.</param>
        /// <returns>Значение бинарной операции.</returns>
        public static double binary_func(double x, double y, string v)
        {
            switch (v[0])
            {
                case '+':
                    return x + y;
                case '-':
                    return x - y;
                case '*':
                    return x * y;
                case '/':
                    return x / y;
                case '^':
                    return Math.Pow(x, y);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Разделение фрмулы на токены, которые представляют собой атомарные единицы формулы
        /// </summary>
        /// <param name="formula">Строка с формулой.</param>
        /// <param name="tokens">Массив токенов.</param>
        public static void divide(string formula, ref string[] tokens)
        {
            char a = ' ';
            int v = 0;
            string bufer = "";
            for (int i = 0; i < formula.Length; i++)
            {
                a = formula[i];
                switch (a)
                {
                    case 'y':
                        {
                            bufer = "y";
                            break;
                        }
                    case '=':
                        {
                            bufer = "=";
                            break;
                        }
                    case '+':
                        {
                            bufer = "+";
                            break;
                        }
                    case '-':
                        {
                            if (formula[i - 1] == '=' || formula[i - 1] == '(' || i - 1 == -1)
                                bufer = "neg";
                            else
                                bufer = "-";
                            break;
                        }
                    case '*':
                        {
                            bufer = "*";
                            break;
                        }
                    case '/':
                        {
                            bufer = "/";
                            break;
                        }
                    case '(':
                        {
                            bufer = "(";
                            break;
                        }
                    case ')':
                        {
                            bufer = ")";
                            break;
                        }
                    case 'l':
                        {
                            bufer = "ln";
                            i += 1;
                            break;
                        }
                    case 's':
                        {
                            if (i < formula.Length - 2 && formula[i + 1] == 'i')
                            {
                                bufer = "sin";
                                i += 2;
                            }
                            else
                            {
                                bufer = "s";
                            }
                            break;
                        }
                    case 'c':
                        {
                            if (i < formula.Length - 2 && formula[i + 1] == 'o')
                            {
                                bufer = "cos";
                            }
                            else if (i < formula.Length - 2 && formula[i + 1] == 't')
                            {
                                bufer = "ctg";
                            }
                            else
                            {
                                bufer = "cut";
                            }
                            i += 2;
                            break;
                        }
                    case 't':
                        {
                            if (i < formula.Length - 2 && formula[i + 1] == 'g')
                            {
                                bufer = "tg";
                                i += 1;
                            }
                            else bufer = "t";
                            break;
                        }
                    case 'e':
                        {
                            bufer = "e";
                            break;
                        }
                    case '^':
                        {
                            bufer = "^";
                            break;
                        }
                    case 'u':
                        {
                            bufer = "u";
                            break;
                        }
                    case 'x':
                        {
                            int s = 0;
                            int b = i;
                            while ((i < formula.Length - 1) && Char.IsDigit(formula[i + 1]))
                            {
                                s++;
                                i++;
                            }
                            bufer = formula.Substring(b, s + 1);
                            break;
                        }
                    default:
                        {
                            while ((i < formula.Length) && (Char.IsDigit(formula[i]) || formula[i] == '.'))
                            {
                                bufer = bufer + formula[i];
                                i++;
                            }
                            i--;
                            break;
                        }
                }
                tokens[v] = bufer;
                v++;
                bufer = "";
            }


        }

        /// <summary>
        /// Замена постфиксных токенов на конкретные значения, применение к ним  разичных операторов для вычисления
        /// </summary>
        /// <param name="postfixtokens">Постфиксная форма записи формулы</param>
        /// <param name="tokens">Изначальный массив токенов</param>
        public static void replace(ref string[] postfixtokens, ref string[] tokens)
        {

            Stack<string> tokenStack = new Stack<string>(10);
            string stringValue = "(";
            tokenStack.Push(stringValue);
            int i = 0;
            while ((i < tokens.Length) && (tokens[i] != "") && (tokens[i] != null))
            {
                i++;
            }
            if (i == tokens.Length - 2)
            {
                tokens[tokens.Length - 2] = ")";
                tokens[tokens.Length - 1] = "";
            }
            else
            {
                tokens[i] = ")";
                tokens[i + 1] = "";
            }
            i = 0;
            int v = 0;
            string curenttoken = "";
            while (tokens[i] != "")
            {
                stringValue = tokens[i];
                switch (stringValue[0])
                {
                    case 'x':
                        {
                            postfixtokens[v] = stringValue;
                            v++;
                            break;
                        };
                    case 'u':
                        {
                            postfixtokens[v] = stringValue;
                            v++;
                            break;
                        };
                    case 'e':
                        {
                            postfixtokens[v] = stringValue;
                            v++;
                            break;
                        };
                    case '(':
                        {
                            tokenStack.Push(stringValue);
                            break;
                        };
                    case ')':
                        {
                            bool tmp = true;
                            try
                            {
                                curenttoken = tokenStack.Pop();
                            }
                            catch
                            {
                                tmp = false;
                            }
                            if (tmp)
                            {
                                while (notParanth(curenttoken))
                                {

                                    postfixtokens[v] = curenttoken;
                                    v++;
                                    curenttoken = tokenStack.Pop();
                                }
                            }
                            break;
                        };
                    default:
                        {
                            if (stringValue[0] == 's')
                            {
                                if (stringValue.Length == 1)
                                {
                                    postfixtokens[v] = stringValue;
                                    v++;
                                    break;
                                }
                            }
                            if (stringValue[0] == 't')
                            {
                                if (stringValue.Length == 1)
                                {
                                    postfixtokens[v] = stringValue;
                                    v++;
                                    break;
                                }
                            };
                            if (stringValue != "")
                            {
                                curenttoken = "";
                                if (isOperator(stringValue))
                                {
                                    curenttoken = tokenStack.Pop();
                                    if (isOperator(curenttoken))
                                    {
                                        while (isOperator(curenttoken) && precedence(stringValue, curenttoken) <= 0)
                                        {
                                            postfixtokens[v] = curenttoken;
                                            v++;
                                            curenttoken = tokenStack.Pop();
                                        }
                                        tokenStack.Push(curenttoken);
                                    }
                                    else tokenStack.Push(curenttoken);
                                    tokenStack.Push(stringValue);
                                }
                                if (Char.IsDigit(stringValue[0]))
                                {
                                    postfixtokens[v] = stringValue;
                                    v++;
                                }

                            }
                            break;
                        }
                }
                i++;

            }
        }

        public static double eval(ref string[] postfixtokens, ref double[] numbers)
        {
            int i = 0;
            string stringValue;

            Stack<double> numberStack = new Stack<double>(10);
            i = 0;
            int j;
            while ((i < postfixtokens.Length) && (postfixtokens[i] != "") && (postfixtokens[i] != null))
            {
                i++;
            }
            postfixtokens[i] = "\0";
            double firstoperand;
            double secondoperand;
            i = 0;
            while (postfixtokens[i] != "\0")
            {
                stringValue = postfixtokens[i];
                switch (stringValue[0])
                {
                    case 'x':
                        j = getindex(stringValue);
                        numberStack.Push(numbers[j - 1]);
                        break;
                    case 'u':
                        numberStack.Push(numbers[numbers.Length - 3]);
                        break;
                    case 'e':
                        numberStack.Push(Math.E);
                        break;
                    case '-':
                        secondoperand = numberStack.Pop();
                        firstoperand = numberStack.Pop();
                        numberStack.Push(binary_func(firstoperand, secondoperand, stringValue));
                        break;

                    case '+':
                        secondoperand = numberStack.Pop();
                        firstoperand = numberStack.Pop();
                        numberStack.Push(binary_func(firstoperand, secondoperand, stringValue));
                        break;
                    case '*':
                        secondoperand = numberStack.Pop();
                        firstoperand = numberStack.Pop();
                        numberStack.Push(binary_func(firstoperand, secondoperand, stringValue));
                        break;
                    case '/':
                        secondoperand = numberStack.Pop();
                        firstoperand = numberStack.Pop();
                        numberStack.Push(binary_func(firstoperand, secondoperand, stringValue));
                        break;
                    case '^':
                        secondoperand = numberStack.Pop();
                        firstoperand = numberStack.Pop();
                        numberStack.Push(binary_func(firstoperand, secondoperand, stringValue));
                        break;

                    default:
                        if (stringValue[0] == 's')
                        {
                            if (stringValue.Length == 1)
                            {
                                numberStack.Push(numbers[numbers.Length - 2]);
                                break;
                            }
                        }
                        if (stringValue[0] == 't')
                        {
                            if (stringValue.Length == 1)
                            {
                                numberStack.Push(numbers[numbers.Length - 1]);
                                break;
                            }
                        };
                        if (Char.IsDigit(stringValue[0]))
                        {
                            firstoperand = getnumber(stringValue);
                            numberStack.Push(firstoperand);
                        }
                        else
                        {
                            firstoperand = numberStack.Pop();
                            numberStack.Push(unary_func(firstoperand, stringValue));
                        }
                        break;

                }
                i++;
            }
            double result;
            result = numberStack.Pop();
            return result;

        }

        /// <summary>
        /// Анализирует строку "formula" и подставляет туда значения x, u, t и s
        /// </summary>
        /// <param name="formula">Строка с формулой</param>
        /// <param name="x">Вектор x</param>
        /// <param name="u">Управление</param>
        /// <param name="s">Время s.</param>
        /// <param name="t">Время t.</param>
        /// <returns>Значение формулы после подстановки туда x, u, t и s</returns>
        public static double evaluate(ref string formula, ref Vector x, double u, double s, double t)
        {
            double value;
            string[] tokens = new string[100];
            string[] postfixtokens = new string[100];
            double[] y = new double[x.vec.Length + 3];

            for (int i = 0; i < x.vec.Length; i++)
            {
                y[i] = x[i];
            }
            y[x.vec.Length] = u;
            y[x.vec.Length + 1] = s;
            y[x.vec.Length + 2] = t;

            divide(formula, ref tokens);
            replace(ref postfixtokens, ref tokens);
            value = eval(ref postfixtokens, ref y);

            return value;
        }
    }
}
