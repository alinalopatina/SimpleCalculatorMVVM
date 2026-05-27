using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Runtime.InteropServices;
using SimpleCalculatorMVVM.Core;
using SimpleCalculatorMVVM.Expressions;
using SimpleCalculatorMVVM.Memory;

namespace SimpleCalculatorMVVM.Models
{
    public class CalculatorEngine
    {
        // Событие для ошибок
        public event Action OnError;

        private string currentInput = "";
        private string previousInput = "";
        private string currentOperation = "";
        private bool isNewInput = true;
        private bool operationPerformed = false;

        // ====================== ИСПОЛЬЗОВАНИЕ БИБЛИОТЕК ======================

        // 1. Статическая библиотека Core
        private readonly IScientificCalculator _scientificCalculator;

        // 2. Динамическая библиотека Memory (динамический вызов)
        private IMemoryManager _memoryManager;
        private IntPtr _memoryDllHandle;

        // История
        private List<string> history = new List<string>();

        public string CurrentInput => currentInput;

        // ====================== КОНСТРУКТОР ======================

        public CalculatorEngine()
        {
            // Инициализация статической библиотеки Core
            _scientificCalculator = new ScientificCalculator();

            // Загрузка динамической библиотеки Memory
            LoadMemoryLibraryDynamically();
        }

        // ====================== ЗАГРУЗКА DLL ДИНАМИЧЕСКИ ======================

        private void LoadMemoryLibraryDynamically()
        {
            try
            {
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string dllPath = System.IO.Path.Combine(exeDirectory, "SimpleCalculatorMVVM.Memory.dll");

                if (!System.IO.File.Exists(dllPath))
                {
                    dllPath = System.IO.Path.Combine(Environment.CurrentDirectory, "SimpleCalculatorMVVM.Memory.dll");
                }

                if (!System.IO.File.Exists(dllPath))
                {
                    dllPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        @"..\..\..\SimpleCalculatorMVVM.Memory\bin\Debug\SimpleCalculatorMVVM.Memory.dll");
                }

                if (System.IO.File.Exists(dllPath))
                {
                    _memoryDllHandle = LoadLibrary(dllPath);

                    if (_memoryDllHandle != IntPtr.Zero)
                    {
                        IntPtr createFuncPtr = GetProcAddress(_memoryDllHandle, "CreateMemoryManager");

                        if (createFuncPtr != IntPtr.Zero)
                        {
                            var createMemoryManager = (CreateMemoryManagerDelegate)Marshal.GetDelegateForFunctionPointer(
                                createFuncPtr, typeof(CreateMemoryManagerDelegate));

                            _memoryManager = createMemoryManager();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки DLL памяти: {ex.Message}");
            }

            if (_memoryManager == null)
            {
                _memoryManager = new FallbackMemoryManager();
            }
        }

        private delegate IMemoryManager CreateMemoryManagerDelegate();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private class FallbackMemoryManager : IMemoryManager
        {
            private double _memory = 0;
            public double MemoryValue => _memory;
            public void Clear() => _memory = 0;
            public double Recall() => _memory;
            public void Add(double value) => _memory += value;
            public void Subtract(double value) => _memory -= value;
            public void Store(double value) => _memory = value;
            public string GetMemoryStatus() => _memory == 0 ? "M: пусто" : $"M: {_memory}";
            public bool HasValue() => Math.Abs(_memory) > double.Epsilon;
        }

        // ====================== ОСНОВНЫЕ МЕТОДЫ ======================

        public void ProcessDigitOrPoint(string content)
        {
            string decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

            if (content == ".")
                content = decimalSeparator;

            if (isNewInput)
            {
                if (content == "0")
                    currentInput = "0";
                else if (content == decimalSeparator)
                    currentInput = "0" + decimalSeparator;
                else
                    currentInput = content;

                isNewInput = false;
            }
            else
            {
                if (currentInput == "0" && content == "0")
                    return;
                else if (currentInput == "0" && content != decimalSeparator && content != "0")
                    currentInput = content;
                else if (content == decimalSeparator && !currentInput.Contains(decimalSeparator))
                    currentInput += content;
                else if (content != decimalSeparator && currentInput.Length < 15)
                    currentInput += content;
            }
        }

        public void ProcessOperation(string content)
        {
            if (content == "=")
            {
                CalculateResult();
                currentOperation = "";
                isNewInput = true;
                operationPerformed = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(currentInput) && !operationPerformed)
                {
                    if (!string.IsNullOrEmpty(previousInput))
                        CalculateResult();

                    previousInput = currentInput;
                    currentOperation = ConvertOperation(content);
                    isNewInput = true;
                    operationPerformed = true;
                }
                else if (operationPerformed)
                {
                    currentOperation = ConvertOperation(content);
                }
            }
        }

        public void ProcessSpecialFunction(string content)
        {
            switch (content)
            {
                case "C": ClearAll(); break;
                case "±": ToggleSign(); break;
                case "%": CalculatePercent(); break;
            }
        }

        public void ProcessBackspace()
        {
            if (!string.IsNullOrEmpty(currentInput) && currentInput != "0")
            {
                if (currentInput.Length > 1)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                else
                {
                    currentInput = "0";
                    isNewInput = true;
                }
            }
        }

        // ====================== НАУЧНЫЕ ФУНКЦИИ (через библиотеку Core) ======================

        public void ExecuteScientificFunction(string function)
        {
            if (string.IsNullOrEmpty(currentInput) || currentInput == "0")
                return;

            try
            {
                double num = double.Parse(currentInput, CultureInfo.InvariantCulture);
                double result = 0;
                string operation = "";

                switch (function)
                {
                    case "sin":
                        result = _scientificCalculator.ComputeSin(num);
                        operation = $"sin({num}°)";
                        break;
                    case "cos":
                        result = _scientificCalculator.ComputeCos(num);
                        operation = $"cos({num}°)";
                        break;
                    case "tan":
                        result = _scientificCalculator.ComputeTan(num);
                        operation = $"tan({num}°)";
                        break;
                    case "ln":
                        result = _scientificCalculator.ComputeLn(num);
                        operation = $"ln({num})";
                        break;
                    case "log":
                        result = _scientificCalculator.ComputeLog(num);
                        operation = $"log({num})";
                        break;
                    case "√":
                        result = _scientificCalculator.ComputeSqrt(num);
                        operation = $"√({num})";
                        break;
                    case "x²":
                        result = _scientificCalculator.ComputePower2(num);
                        operation = $"{num}²";
                        break;
                    case "eˣ":
                        result = _scientificCalculator.ComputeExp(num);
                        operation = $"e^{num}";
                        break;
                }

                currentInput = result.ToString(CultureInfo.CurrentCulture);
                AddToHistory($"{operation} = {currentInput}");
                isNewInput = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                OnError?.Invoke();
            }
        }

        // ====================== МЕТОДЫ ПАМЯТИ (через динамическую DLL) ======================

        public void HandleMemoryOperation(string operation)
        {
            try
            {
                double current = string.IsNullOrEmpty(currentInput) ? 0 :
                    double.Parse(currentInput, CultureInfo.CurrentCulture);

                switch (operation)
                {
                    case "MC":
                        _memoryManager.Clear();
                        MessageBox.Show("Память очищена", "MC",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "MR":
                        double memoryValue = _memoryManager.Recall();
                        currentInput = memoryValue.ToString(CultureInfo.CurrentCulture);
                        isNewInput = true;
                        break;
                    case "M+":
                        _memoryManager.Add(current);
                        MessageBox.Show($"Добавлено в память: {_memoryManager.MemoryValue}", "M+",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "M-":
                        _memoryManager.Subtract(current);
                        MessageBox.Show($"Вычтено из памяти: {_memoryManager.MemoryValue}", "M-",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка памяти: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                OnError?.Invoke();
            }
        }

        // ====================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ======================

        private void AddToHistory(string entry)
        {
            history.Add($"{DateTime.Now:HH:mm:ss} - {entry}");
            if (history.Count > 20)
                history.RemoveAt(0);
        }

        private string ConvertOperation(string operation)
        {
            switch (operation)
            {
                case "÷": return "/";
                case "×": return "*";
                case "−": return "-";
                default: return operation;
            }
        }

        private void CalculateResult()
        {
            if (string.IsNullOrEmpty(previousInput) || string.IsNullOrEmpty(currentInput) || string.IsNullOrEmpty(currentOperation))
                return;

            try
            {
                double num1 = double.Parse(previousInput, CultureInfo.CurrentCulture);
                double num2 = double.Parse(currentInput, CultureInfo.CurrentCulture);
                double result = 0;

                switch (currentOperation)
                {
                    case "+": result = num1 + num2; break;
                    case "-": result = num1 - num2; break;
                    case "*": result = num1 * num2; break;
                    case "/":
                        if (Math.Abs(num2) < double.Epsilon)
                        {
                            MessageBox.Show("Деление на ноль невозможно!", "Ошибка",
                                          MessageBoxButton.OK, MessageBoxImage.Warning);
                            ClearAll();
                            OnError?.Invoke();
                            return;
                        }
                        result = num1 / num2;
                        break;
                }

                currentInput = result.ToString(CultureInfo.CurrentCulture);
                previousInput = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAll();
                OnError?.Invoke();
            }
        }

        private void ToggleSign()
        {
            if (!string.IsNullOrEmpty(currentInput) && currentInput != "0")
            {
                currentInput = currentInput.StartsWith("-") ?
                              currentInput.Substring(1) : "-" + currentInput;
            }
        }

        private void CalculatePercent()
        {
            if (!string.IsNullOrEmpty(currentInput))
            {
                try
                {
                    if (!string.IsNullOrEmpty(previousInput))
                    {
                        double num1 = double.Parse(previousInput, CultureInfo.CurrentCulture);
                        double num2 = double.Parse(currentInput, CultureInfo.CurrentCulture);
                        currentInput = ((num1 * num2) / 100).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        double num = double.Parse(currentInput, CultureInfo.CurrentCulture) / 100;
                        currentInput = num.ToString(CultureInfo.CurrentCulture);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка вычисления процента!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearAll()
        {
            currentInput = "";
            previousInput = "";
            currentOperation = "";
            isNewInput = true;
            operationPerformed = false;
        }

        public string GetFormattedDisplay()
        {
            string text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;

            if (text.Length > 15)
            {
                try
                {
                    double num = double.Parse(text, CultureInfo.CurrentCulture);
                    text = num.ToString("E5", CultureInfo.CurrentCulture);
                }
                catch
                {
                    if (text.Length > 20)
                        text = text.Substring(0, 20);
                }
            }

            return text;
        }

        // ====================== МЕТОДЫ ДЛЯ COMMAND (Undo/Redo) ======================

        public string GetCurrentInput() => currentInput;
        public string GetPreviousInput() => previousInput;
        public string GetCurrentOperation() => currentOperation;
        public bool IsOperationPerformed() => operationPerformed;
        public bool IsNewInput() => isNewInput;

        public void SetPreviousInput(string value) => previousInput = value;
        public void SetCurrentOperation(string value) => currentOperation = value;
        public void SetOperationPerformed(bool value) => operationPerformed = value;
        public void SetNewInputFlag(bool value) => isNewInput = value;

        public void RestoreCurrentInput(string input)
        {
            currentInput = input;
            isNewInput = true;
        }
    }
}