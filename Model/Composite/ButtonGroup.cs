using System.Collections.Generic;
using System.Windows.Controls;

namespace SimpleCalculatorMVVM.Models.Composite
{
    /// <summary>
    /// Компонент (интерфейс) для кнопок
    /// </summary>
    public interface IButtonComponent
    {
        void Render(Grid grid, object command, int baseRow, int baseCol);
        string GetContent();
    }

    /// <summary>
    /// Лист (отдельная кнопка)
    /// </summary>
    public class LeafButton : IButtonComponent
    {
        private readonly CalculatorButton _button;

        public LeafButton(CalculatorButton button)
        {
            _button = button;
        }

        public void Render(Grid grid, object command, int baseRow, int baseCol)
        {
            var btn = new Button();
            _button.ApplyStyle(btn);
            btn.Command = command as System.Windows.Input.ICommand;
            btn.CommandParameter = _button.Content;

            Grid.SetRow(btn, baseRow + _button.Row);
            Grid.SetColumn(btn, baseCol + _button.Column);

            if (_button.ColumnSpan > 1)
                Grid.SetColumnSpan(btn, _button.ColumnSpan);

            grid.Children.Add(btn);
        }

        public string GetContent() => _button.Content;
    }

    /// <summary>
    /// Композит (группа кнопок)
    /// </summary>
    public class ButtonGroup : IButtonComponent
    {
        private readonly List<IButtonComponent> _buttons = new List<IButtonComponent>();
        private readonly string _groupName;

        public ButtonGroup(string groupName)
        {
            _groupName = groupName;
        }

        public void AddButton(IButtonComponent button)
        {
            _buttons.Add(button);
        }

        public void RemoveButton(IButtonComponent button)
        {
            _buttons.Remove(button);
        }

        public void Render(Grid grid, object command, int baseRow, int baseCol)
        {
            System.Diagnostics.Debug.WriteLine($"[COMPOSITE] Рендеринг группы: {_groupName} с {_buttons.Count} кнопками");

            foreach (var button in _buttons)
            {
                button.Render(grid, command, baseRow, baseCol);
            }
        }

        public string GetContent() => $"[Группа: {_groupName}]";
    }
}