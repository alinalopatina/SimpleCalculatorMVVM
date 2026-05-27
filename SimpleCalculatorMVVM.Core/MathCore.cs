using System;

namespace SimpleCalculatorMVVM.Core
{
    /// <summary>
    /// Статическая библиотека для математических вычислений
    /// Содержит базовые математические функции
    /// </summary>
    public static class MathCore
    {
        // === Базовые математические функции ===

        /// <summary>
        /// Синус угла (в радианах)
        /// </summary>
        public static double Sin(double radians) => Math.Sin(radians);

        /// <summary>
        /// Косинус угла (в радианах)
        /// </summary>
        public static double Cos(double radians) => Math.Cos(radians);

        /// <summary>
        /// Тангенс угла (в радианах)
        /// </summary>
        public static double Tan(double radians) => Math.Tan(radians);

        /// <summary>
        /// Натуральный логарифм
        /// </summary>
        public static double Ln(double value) => Math.Log(value);

        /// <summary>
        /// Десятичный логарифм
        /// </summary>
        public static double Log10(double value) => Math.Log10(value);

        /// <summary>
        /// Квадратный корень
        /// </summary>
        public static double Sqrt(double value) => Math.Sqrt(value);

        /// <summary>
        /// Возведение в степень
        /// </summary>
        public static double Pow(double baseValue, double exponent) => Math.Pow(baseValue, exponent);

        /// <summary>
        /// Экспонента (e^x)
        /// </summary>
        public static double Exp(double value) => Math.Exp(value);

        // === Научные функции с градусами ===

        /// <summary>
        /// Синус угла в градусах
        /// </summary>
        public static double SinDegrees(double degrees) => Sin(degrees * Math.PI / 180.0);

        /// <summary>
        /// Косинус угла в градусах
        /// </summary>
        public static double CosDegrees(double degrees) => Cos(degrees * Math.PI / 180.0);

        /// <summary>
        /// Тангенс угла в градусах
        /// </summary>
        public static double TanDegrees(double degrees) => Tan(degrees * Math.PI / 180.0);

        // === Вспомогательные функции ===

        /// <summary>
        /// Проверка на допустимость для логарифма
        /// </summary>
        public static bool IsValidForLog(double value) => value > 0;

        /// <summary>
        /// Проверка на допустимость для квадратного корня
        /// </summary>
        public static bool IsValidForSqrt(double value) => value >= 0;

        /// <summary>
        /// Преобразование радиан в градусы
        /// </summary>
        public static double RadiansToDegrees(double radians) => radians * 180.0 / Math.PI;

        /// <summary>
        /// Преобразование градусов в радианы
        /// </summary>
        public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
    }
}