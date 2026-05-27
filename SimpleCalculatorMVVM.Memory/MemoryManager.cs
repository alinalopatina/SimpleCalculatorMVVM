using System;
using System.Globalization;

namespace SimpleCalculatorMVVM.Memory
{
    /// <summary>
    /// Реализация менеджера памяти калькулятора
    /// </summary>
    public class MemoryManager : IMemoryManager
    {
        private double _memory = 0;
        private readonly object _lock = new object();

        public double MemoryValue
        {
            get
            {
                lock (_lock)
                    return _memory;
            }
        }

        /// <summary>
        /// Очистка памяти (MC)
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _memory = 0;
                OnMemoryChanged?.Invoke(_memory);
            }
        }

        /// <summary>
        /// Восстановление из памяти (MR)
        /// </summary>
        public double Recall()
        {
            lock (_lock)
                return _memory;
        }

        /// <summary>
        /// Добавление в память (M+)
        /// </summary>
        public void Add(double value)
        {
            lock (_lock)
            {
                _memory += value;
                OnMemoryChanged?.Invoke(_memory);
            }
        }

        /// <summary>
        /// Вычитание из памяти (M-)
        /// </summary>
        public void Subtract(double value)
        {
            lock (_lock)
            {
                _memory -= value;
                OnMemoryChanged?.Invoke(_memory);
            }
        }

        /// <summary>
        /// Сохранение в память (замена)
        /// </summary>
        public void Store(double value)
        {
            lock (_lock)
            {
                _memory = value;
                OnMemoryChanged?.Invoke(_memory);
            }
        }

        /// <summary>
        /// Строковое состояние памяти
        /// </summary>
        public string GetMemoryStatus()
        {
            lock (_lock)
            {
                return _memory == 0 ? "M: пусто" : $"M: {_memory.ToString(CultureInfo.CurrentCulture)}";
            }
        }

        /// <summary>
        /// Проверка наличия значения в памяти
        /// </summary>
        public bool HasValue()
        {
            lock (_lock)
                return Math.Abs(_memory) > double.Epsilon;
        }

        /// <summary>
        /// Событие изменения памяти
        /// </summary>
        public event Action<double> OnMemoryChanged;
    }
}