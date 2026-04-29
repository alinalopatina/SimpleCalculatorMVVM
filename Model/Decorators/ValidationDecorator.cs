using System;
using System.Windows;
using SimpleCalculatorMVVM.Models.Commands;

namespace SimpleCalculatorMVVM.Models.Decorators
{
    /// <summary>
    /// Декоратор для валидации входных данных
    /// </summary>
    public class ValidationDecorator : CalculatorCommandDecorator
    {
        private readonly string _input;

        public ValidationDecorator(ICalculatorCommand command, string input) : base(command)
        {
            _input = input;
        }

        public override void Execute()
        {
            if (!Validate())
            {
                MessageBox.Show($"Некорректный ввод: {_input}", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            base.Execute();
        }

        private bool Validate()
        {
            // Проверка на слишком длинное число
            if (_input.Length > 15 && _input != ".")
            {
                return false;
            }

            // Проверка на несколько точек
            if (_input == "." && (_wrappedCommand is DigitCommand))
            {
                // Точку можно добавить только один раз — эту проверку делает сам Engine
                return true;
            }

            return true;
        }
    }
}