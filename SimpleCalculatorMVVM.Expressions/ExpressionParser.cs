using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCalculatorMVVM.Expressions
{
    /// <summary>
    /// Парсер и вычислитель математических выражений
    /// Поддерживает: +, -, *, /, (, )
    /// </summary>
    public class ExpressionParser
    {
        /// <summary>
        /// Вычисление математического выражения
        /// </summary>
        /// <param name="expression">Строка с выражением (например "2+3*4")</param>
        /// <returns>Результат вычисления</returns>
        public static double Evaluate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return 0;

            try
            {
                // 1. Токенизация
                var tokens = Tokenize(expression);

                // 2. Преобразование в обратную польскую нотацию (ОПН)
                var rpn = ConvertToRPN(tokens);

                // 3. Вычисление ОПН
                return EvaluateRPN(rpn);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления выражения '{expression}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Разбиение строки на токены (числа и операторы)
        /// </summary>
        private static List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var currentNumber = "";

            foreach (char c in expression)
            {
                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    // Собираем число (заменяем запятую на точку)
                    currentNumber += (c == ',' ? '.' : c);
                }
                else if ("+-*/()".Contains(c))
                {
                    // Завершаем текущее число, если есть
                    if (!string.IsNullOrEmpty(currentNumber))
                    {
                        tokens.Add(currentNumber);
                        currentNumber = "";
                    }
                    // Добавляем оператор
                    tokens.Add(c.ToString());
                }
                else if (!char.IsWhiteSpace(c))
                {
                    // Неизвестный символ
                    throw new ArgumentException($"Недопустимый символ: '{c}'");
                }
            }

            // Добавляем последнее число, если есть
            if (!string.IsNullOrEmpty(currentNumber))
                tokens.Add(currentNumber);

            return tokens;
        }

        /// <summary>
        /// Преобразование инфиксной записи в обратную польскую нотацию (алгоритм сортировочной станции)
        /// </summary>
        private static List<string> ConvertToRPN(List<string> tokens)
        {
            var output = new List<string>();
            var operators = new Stack<string>();

            // Приоритеты операций
            var precedence = new Dictionary<string, int>
            {
                { "+", 1 },
                { "-", 1 },
                { "*", 2 },
                { "/", 2 }
            };

            foreach (var token in tokens)
            {
                // Если токен - число, добавляем в выход
                if (double.TryParse(token, out _))
                {
                    output.Add(token);
                }
                // Если токен - оператор
                else if (precedence.ContainsKey(token))
                {
                    // Пока есть операторы с большим или равным приоритетом в стеке
                    while (operators.Count > 0 &&
                           precedence.ContainsKey(operators.Peek()) &&
                           precedence[operators.Peek()] >= precedence[token])
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token);
                }
                // Открывающая скобка
                else if (token == "(")
                {
                    operators.Push(token);
                }
                // Закрывающая скобка
                else if (token == ")")
                {
                    // Выталкиваем до открывающей скобки
                    while (operators.Count > 0 && operators.Peek() != "(")
                        output.Add(operators.Pop());

                    // Удаляем открывающую скобку
                    if (operators.Count > 0 && operators.Peek() == "(")
                        operators.Pop();
                    else
                        throw new ArgumentException("Непарная скобка ')'");
                }
            }

            // Выталкиваем оставшиеся операторы
            while (operators.Count > 0)
            {
                var op = operators.Pop();
                if (op == "(" || op == ")")
                    throw new ArgumentException("Непарная скобка");
                output.Add(op);
            }

            return output;
        }

        /// <summary>
        /// Вычисление выражения в обратной польской нотации
        /// </summary>
        private static double EvaluateRPN(List<string> rpn)
        {
            var stack = new Stack<double>();

            foreach (var token in rpn)
            {
                // Если число - в стек
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                // Если оператор - вычисляем
                else
                {
                    if (stack.Count < 2)
                        throw new ArgumentException($"Недостаточно операндов для операции '{token}'");

                    var b = stack.Pop();
                    var a = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(a + b);
                            break;
                        case "-":
                            stack.Push(a - b);
                            break;
                        case "*":
                            stack.Push(a * b);
                            break;
                        case "/":
                            if (Math.Abs(b) < double.Epsilon)
                                throw new DivideByZeroException("Деление на ноль");
                            stack.Push(a / b);
                            break;
                        default:
                            throw new ArgumentException($"Неизвестный оператор: '{token}'");
                    }
                }
            }

            if (stack.Count != 1)
                throw new ArgumentException("Некорректное выражение");

            return stack.Pop();
        }

        /// <summary>
        /// Проверка корректности выражения
        /// </summary>
        public static bool IsValidExpression(string expression)
        {
            try
            {
                Evaluate(expression);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Получение версии библиотеки
        /// </summary>
        public static string GetVersion() => "1.0.0";
    }
}