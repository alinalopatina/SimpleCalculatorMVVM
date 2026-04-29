using SimpleCalculatorMVVM.Models.Commands;

namespace SimpleCalculatorMVVM.Models.Decorators
{
    /// <summary>
    /// Базовый декоратор для команд калькулятора
    /// </summary>
    public abstract class CalculatorCommandDecorator : ICalculatorCommand
    {
        protected ICalculatorCommand _wrappedCommand;

        protected CalculatorCommandDecorator(ICalculatorCommand command)
        {
            _wrappedCommand = command;
        }

        public virtual void Execute()
        {
            _wrappedCommand.Execute();
        }

        public virtual void Undo()
        {
            _wrappedCommand.Undo();
        }
    }
}