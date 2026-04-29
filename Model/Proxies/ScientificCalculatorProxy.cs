using System;
using SimpleCalculatorMVVM.Models.Adapters;

namespace SimpleCalculatorMVVM.Models.Proxies
{
    /// <summary>
    /// Proxy для научного калькулятора с ленивой загрузкой
    /// </summary>
    public class ScientificCalculatorProxy : IScientificCalculator
    {
        private IScientificCalculator _realCalculator;
        private readonly object _lock = new object();

        private IScientificCalculator RealCalculator
        {
            get
            {
                if (_realCalculator == null)
                {
                    lock (_lock)
                    {
                        if (_realCalculator == null)
                        {
                            System.Diagnostics.Debug.WriteLine("[PROXY] Ленивая инициализация научного калькулятора");
                            _realCalculator = new ScientificAdapter();
                        }
                    }
                }
                return _realCalculator;
            }
        }

        public double ComputeSin(double degrees)
        {
            // Дополнительная проверка доступа
            if (double.IsNaN(degrees) || double.IsInfinity(degrees))
                throw new ArgumentException("Некорректное значение для sin");

            return RealCalculator.ComputeSin(degrees);
        }

        public double ComputeCos(double degrees)
        {
            return RealCalculator.ComputeCos(degrees);
        }

        public double ComputeTan(double degrees)
        {
            return RealCalculator.ComputeTan(degrees);
        }

        public double ComputeLog(double value)
        {
            return RealCalculator.ComputeLog(value);
        }

        public double ComputeLn(double value)
        {
            return RealCalculator.ComputeLn(value);
        }

        public double ComputeSqrt(double value)
        {
            return RealCalculator.ComputeSqrt(value);
        }

        public double ComputePower2(double value)
        {
            return RealCalculator.ComputePower2(value);
        }

        public double ComputeExp(double value)
        {
            return RealCalculator.ComputeExp(value);
        }

        // Метод для проверки состояния
        public bool IsInitialized => _realCalculator != null;
    }
}