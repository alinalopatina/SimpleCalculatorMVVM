using System;

namespace SimpleCalculatorMVVM.Memory
{
    /// <summary>
    /// Фабрика для создания экземпляров MemoryManager
    /// </summary>
    public static class MemoryFactory
    {
        private static IMemoryManager _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Создание нового экземпляра менеджера памяти
        /// </summary>
        public static IMemoryManager Create()
        {
            return new MemoryManager();
        }

        /// <summary>
        /// Получение синглтон-экземпляра (для всего приложения)
        /// </summary>
        public static IMemoryManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)  // ← Заменяем ??= на явную проверку
                    {
                        _instance = new MemoryManager();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Сброс синглтон-экземпляра
        /// </summary>
        public static void ResetInstance()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }

    /// <summary>
    /// Точка входа для динамической загрузки DLL
    /// </summary>
    public static class DllEntryPoint
    {
        /// <summary>
        /// Экспортируемая функция для создания менеджера памяти
        /// </summary>
        public static IMemoryManager CreateMemoryManager()
        {
            return MemoryFactory.Create();
        }

        /// <summary>
        /// Экспортируемая функция для получения синглтона
        /// </summary>
        public static IMemoryManager GetMemoryManagerInstance()
        {
            return MemoryFactory.GetInstance();
        }

        /// <summary>
        /// Экспортируемая функция для получения версии
        /// </summary>
        public static string GetVersion()
        {
            return "1.0.0";
        }
    }
}