using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleCalculatorMVVM.Models
{
    /// <summary>
    /// Базовый класс для всех кнопок калькулятора
    /// </summary>
    public abstract class CalculatorButton
    {
        public string Name { get; protected set; }
        public string Content { get; protected set; }
        public int Row { get; protected set; }
        public int Column { get; protected set; }
        public int RowSpan { get; protected set; } = 1;
        public int ColumnSpan { get; protected set; } = 1;
        public bool IsScientific { get; protected set; } = false;

        public abstract void ApplyStyle(Button button);
    }

    /// <summary>
    /// Кнопка с цифрой
    /// </summary>
    public class NumberButton : CalculatorButton
    {
        public NumberButton(string name, string content, int row, int col, bool isScientific = false)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.Style = (Style)Application.Current.Resources["NumberButtonStyle"];
            if (RowSpan > 1) Grid.SetRowSpan(button, RowSpan);
            if (ColumnSpan > 1) Grid.SetColumnSpan(button, ColumnSpan);
        }
    }

    /// <summary>
    /// Кнопка операции (+, -, ×, ÷)
    /// </summary>
    public class OperationButton : CalculatorButton
    {
        public OperationButton(string name, string content, int row, int col, bool isScientific = false)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.Style = (Style)Application.Current.Resources["OperatorButtonStyle"];
            if (RowSpan > 1) Grid.SetRowSpan(button, RowSpan);
            if (ColumnSpan > 1) Grid.SetColumnSpan(button, ColumnSpan);
        }
    }

    /// <summary>
    /// Кнопка действия (C, ±, %, =)
    /// </summary>
    public class ActionButton : CalculatorButton
    {
        public ActionButton(string name, string content, int row, int col, int colSpan = 1, bool isScientific = false)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            ColumnSpan = colSpan;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            if (Content == "=")
                button.Style = (Style)Application.Current.Resources["EqualsButtonStyle"];
            else
                button.Style = (Style)Application.Current.Resources["ActionButtonStyle"];
            if (RowSpan > 1) Grid.SetRowSpan(button, RowSpan);
            if (ColumnSpan > 1) Grid.SetColumnSpan(button, ColumnSpan);
        }
    }

    /// <summary>
    /// Научная кнопка (sin, cos, tan, и т.д.)
    /// </summary>
    public class ScientificButton : CalculatorButton
    {
        public ScientificButton(string name, string content, int row, int col, bool isScientific = true)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.Style = (Style)Application.Current.Resources["ScientificButtonStyle"];
            if (RowSpan > 1) Grid.SetRowSpan(button, RowSpan);
            if (ColumnSpan > 1) Grid.SetColumnSpan(button, ColumnSpan);
        }
    }

    /// <summary>
    /// Кнопка памяти
    /// </summary>
    public class MemoryButton : CalculatorButton
    {
        public MemoryButton(string name, string content, int row, int col, bool isScientific = true)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.Style = (Style)Application.Current.Resources["MemoryButtonStyle"];
            if (RowSpan > 1) Grid.SetRowSpan(button, RowSpan);
            if (ColumnSpan > 1) Grid.SetColumnSpan(button, ColumnSpan);
        }
    }
}