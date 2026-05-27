using System;

namespace SimpleCalculatorMVVM.Memory
{
    /// <summary>
    /// Интерфейс менеджера памяти
    /// </summary>
    public interface IMemoryManager
    {
        /// <summary>
        /// Текущее значение памяти
        /// </summary>
        double MemoryValue { get; }

        /// <summary>
        /// Очистить память
        /// </summary>
        void Clear();

        /// <summary>
        /// Восстановить значение из памяти
        /// </summary>
        double Recall();

        /// <summary>
        /// Добавить значение в память
        /// </summary>
        void Add(double value);

        /// <summary>
        /// Вычесть значение из памяти
        /// </summary>
        void Subtract(double value);

        /// <summary>
        /// Сохранить значение в память (замена)
        /// </summary>
        void Store(double value);

        /// <summary>
        /// Получить строковое представление состояния памяти
        /// </summary>
        string GetMemoryStatus();

        /// <summary>
        /// Проверить, есть ли значение в памяти
        /// </summary>
        bool HasValue();
    }
}