using System;

namespace SimpleCalculatorMVVM.Core
{
    /// <summary>
    /// Интерфейс научного калькулятора
    /// </summary>
    public interface IScientificCalculator
    {
        double ComputeSin(double degrees);
        double ComputeCos(double degrees);
        double ComputeTan(double degrees);
        double ComputeLog(double value);
        double ComputeLn(double value);
        double ComputeSqrt(double value);
        double ComputePower2(double value);
        double ComputeExp(double value);
    }

    /// <summary>
    /// Реализация научного калькулятора с использованием MathCore
    /// </summary>
    public class ScientificCalculator : IScientificCalculator
    {
        /// <summary>
        /// Вычисление синуса угла в градусах
        /// </summary>
        public double ComputeSin(double degrees)
        {
            try
            {
                return MathCore.SinDegrees(degrees);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления sin({degrees}°): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Вычисление косинуса угла в градусах
        /// </summary>
        public double ComputeCos(double degrees)
        {
            try
            {
                return MathCore.CosDegrees(degrees);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления cos({degrees}°): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Вычисление тангенса угла в градусах
        /// </summary>
        public double ComputeTan(double degrees)
        {
            try
            {
                return MathCore.TanDegrees(degrees);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления tan({degrees}°): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Вычисление десятичного логарифма
        /// </summary>
        public double ComputeLog(double value)
        {
            if (!MathCore.IsValidForLog(value))
                throw new ArgumentException($"log({value}) требует x > 0");

            try
            {
                return MathCore.Log10(value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления log({value}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Вычисление натурального логарифма
        /// </summary>
        public double ComputeLn(double value)
        {
            if (!MathCore.IsValidForLog(value))
                throw new ArgumentException($"ln({value}) требует x > 0");

            try
            {
                return MathCore.Ln(value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления ln({value}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Вычисление квадратного корня
        /// </summary>
        public double ComputeSqrt(double value)
        {
            if (!MathCore.IsValidForSqrt(value))
                throw new ArgumentException($"sqrt({value}) требует x ≥ 0");

            try
            {
                return MathCore.Sqrt(value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления sqrt({value}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Возведение в квадрат
        /// </summary>
        public double ComputePower2(double value)
        {
            try
            {
                return MathCore.Pow(value, 2);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления {value}²: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Вычисление экспоненты
        /// </summary>
        public double ComputeExp(double value)
        {
            try
            {
                return MathCore.Exp(value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка вычисления e^{value}: {ex.Message}", ex);
            }
        }
    }
}