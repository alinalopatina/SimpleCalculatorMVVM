using System;
using System.Diagnostics;
using SimpleCalculatorMVVM.Models.Commands;

namespace SimpleCalculatorMVVM.Models.Decorators
{
    /// <summary>
    /// Декоратор для логирования выполнения команд
    /// </summary>
    public class LoggingDecorator : CalculatorCommandDecorator
    {
        private readonly string _commandName;

        public LoggingDecorator(ICalculatorCommand command, string commandName) : base(command)
        {
            _commandName = commandName;
        }

        public override void Execute()
        {
            Debug.WriteLine($"[LOG] Выполнение команды: {_commandName} в {DateTime.Now:HH:mm:ss.fff}");

            var stopwatch = Stopwatch.StartNew();
            base.Execute();
            stopwatch.Stop();

            Debug.WriteLine($"[LOG] Команда {_commandName} выполнена за {stopwatch.ElapsedMilliseconds} мс");
        }

        public override void Undo()
        {
            Debug.WriteLine($"[LOG] Отмена команды: {_commandName}");
            base.Undo();
            Debug.WriteLine($"[LOG] Команда {_commandName} отменена");
        }
    }
}