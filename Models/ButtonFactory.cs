using System.Collections.Generic;

namespace SimpleCalculatorMVVM.Models
{
    public class ButtonFactory
    {
        public List<CalculatorButton> GetStandardButtons()
        {
            var buttons = new List<CalculatorButton>();

            // Ряд 0: C, ±, %, ÷
            buttons.Add(new ActionButton("BtnC", "C", 0, 0));
            buttons.Add(new ActionButton("BtnPlusMinus", "±", 0, 1));
            buttons.Add(new ActionButton("BtnPercent", "%", 0, 2));
            buttons.Add(new OperationButton("BtnDivide", "÷", 0, 3));

            // Ряд 1: 7, 8, 9, ×
            buttons.Add(new NumberButton("Btn7", "7", 1, 0));
            buttons.Add(new NumberButton("Btn8", "8", 1, 1));
            buttons.Add(new NumberButton("Btn9", "9", 1, 2));
            buttons.Add(new OperationButton("BtnMultiply", "×", 1, 3));

            // Ряд 2: 4, 5, 6, −
            buttons.Add(new NumberButton("Btn4", "4", 2, 0));
            buttons.Add(new NumberButton("Btn5", "5", 2, 1));
            buttons.Add(new NumberButton("Btn6", "6", 2, 2));
            buttons.Add(new OperationButton("BtnSubtract", "−", 2, 3));

            // Ряд 3: 1, 2, 3, +
            buttons.Add(new NumberButton("Btn1", "1", 3, 0));
            buttons.Add(new NumberButton("Btn2", "2", 3, 1));
            buttons.Add(new NumberButton("Btn3", "3", 3, 2));
            buttons.Add(new OperationButton("BtnAdd", "+", 3, 3));

            // Ряд 4: 0 (широкая), ., =, Undo, Redo
            buttons.Add(new ActionButton("Btn0", "0", 4, 0, 2));
            buttons.Add(new NumberButton("BtnPoint", ".", 4, 2));
            buttons.Add(new ActionButton("BtnEquals", "=", 4, 3));
            buttons.Add(new ActionButton("BtnUndo", "↩", 5, 0));
            buttons.Add(new ActionButton("BtnRedo", "↪", 5, 1));

            return buttons;
        }

        public List<CalculatorButton> GetScientificButtons()
        {
            var buttons = new List<CalculatorButton>();

            // Ряд 0: Научные функции (верхний ряд)
            buttons.Add(new ScientificButton("BtnSin", "sin", 0, 0));
            buttons.Add(new ScientificButton("BtnCos", "cos", 0, 1));
            buttons.Add(new ScientificButton("BtnTan", "tan", 0, 2));
            buttons.Add(new ScientificButton("BtnLn", "ln", 0, 3));

            // Ряд 1: Научные функции
            buttons.Add(new ScientificButton("BtnSqrt", "√", 1, 0));
            buttons.Add(new ScientificButton("BtnPower", "x²", 1, 1));
            buttons.Add(new ScientificButton("BtnExp", "eˣ", 1, 2));
            buttons.Add(new ScientificButton("BtnLog", "log", 1, 3));

            // Ряд 2: Кнопки памяти
            buttons.Add(new MemoryButton("BtnMc", "MC", 2, 0));
            buttons.Add(new MemoryButton("BtnMr", "MR", 2, 1));
            buttons.Add(new MemoryButton("BtnMPlus", "M+", 2, 2));
            buttons.Add(new MemoryButton("BtnMMinus", "M-", 2, 3));

            // Ряд 3: C, ±, %, ÷
            buttons.Add(new ActionButton("BtnC", "C", 3, 0));
            buttons.Add(new ActionButton("BtnPlusMinus", "±", 3, 1));
            buttons.Add(new ActionButton("BtnPercent", "%", 3, 2));
            buttons.Add(new OperationButton("BtnDivide", "÷", 3, 3));

            // Ряд 4: 7, 8, 9, ×
            buttons.Add(new NumberButton("Btn7", "7", 4, 0));
            buttons.Add(new NumberButton("Btn8", "8", 4, 1));
            buttons.Add(new NumberButton("Btn9", "9", 4, 2));
            buttons.Add(new OperationButton("BtnMultiply", "×", 4, 3));

            // Ряд 5: 4, 5, 6, −
            buttons.Add(new NumberButton("Btn4", "4", 5, 0));
            buttons.Add(new NumberButton("Btn5", "5", 5, 1));
            buttons.Add(new NumberButton("Btn6", "6", 5, 2));
            buttons.Add(new OperationButton("BtnSubtract", "−", 5, 3));

            // Ряд 6: 1, 2, 3, +
            buttons.Add(new NumberButton("Btn1", "1", 6, 0));
            buttons.Add(new NumberButton("Btn2", "2", 6, 1));
            buttons.Add(new NumberButton("Btn3", "3", 6, 2));
            buttons.Add(new OperationButton("BtnAdd", "+", 6, 3));

            // Ряд 7: 0, ., =, Undo, Redo
            buttons.Add(new ActionButton("Btn0", "0", 7, 0, 2));
            buttons.Add(new NumberButton("BtnPoint", ".", 7, 2));
            buttons.Add(new ActionButton("BtnEquals", "=", 7, 3));
            buttons.Add(new ActionButton("BtnUndo", "↩", 8, 0));
            buttons.Add(new ActionButton("BtnRedo", "↪", 8, 1));

            return buttons;
        }
    }
}