using System.Collections.Generic;
using SimpleCalculatorMVVM.Models.Commands;

namespace SimpleCalculatorMVVM.Models.Decorators
{
    /// <summary>
    /// Декоратор для кэширования результатов операций
    /// </summary>
    public class CacheDecorator : CalculatorCommandDecorator
    {
        private static readonly Dictionary<string, double> _cache = new Dictionary<string, double>();
        private readonly string _operationKey;
        private bool _wasCached;

        public CacheDecorator(ICalculatorCommand command, string operationKey) : base(command)
        {
            _operationKey = operationKey;
        }

        public override void Execute()
        {
            if (_cache.ContainsKey(_operationKey))
            {
                _wasCached = true;
                // В реальном коде здесь было бы возвращение кэшированного результата
                System.Diagnostics.Debug.WriteLine($"[CACHE] Результат для {_operationKey} взят из кэша");
            }
            else
            {
                _wasCached = false;
                base.Execute();
                System.Diagnostics.Debug.WriteLine($"[CACHE] Результат для {_operationKey} вычислен и сохранён");
            }
        }

        public static void ClearCache()
        {
            _cache.Clear();
        }
    }
}