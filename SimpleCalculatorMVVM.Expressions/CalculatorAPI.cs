using System;
using System.Runtime.InteropServices;

namespace SimpleCalculatorMVVM.Expressions
{
    /// <summary>
    /// API для экспорта функций из DLL (для использования с динамической загрузкой)
    /// </summary>
    public static class CalculatorAPI
    {
        /// <summary>
        /// Вычисление выражения (экспортируемая функция)
        /// </summary>
        [DllExport("EvaluateExpression", CallingConvention = CallingConvention.Cdecl)]
        public static double EvaluateExpression([MarshalAs(UnmanagedType.LPStr)] string expression)
        {
            try
            {
                return ExpressionParser.Evaluate(expression);
            }
            catch
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Получение версии (экспортируемая функция)
        /// </summary>
        [DllExport("GetLibraryVersion", CallingConvention = CallingConvention.Cdecl)]
        public static IntPtr GetLibraryVersion()
        {
            var version = ExpressionParser.GetVersion();
            return Marshal.StringToHGlobalAnsi(version);
        }

        /// <summary>
        /// Освобождение памяти, выделенной под строку
        /// </summary>
        [DllExport("FreeString", CallingConvention = CallingConvention.Cdecl)]
        public static void FreeString(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
                Marshal.FreeHGlobal(ptr);
        }
    }

    /// <summary>
    /// Атрибут для экспорта функций из DLL
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DllExportAttribute : Attribute
    {
        public string Name { get; set; }
        public CallingConvention CallingConvention { get; set; }

        public DllExportAttribute(string name)
        {
            Name = name;
            CallingConvention = CallingConvention.StdCall;
        }
    }
}