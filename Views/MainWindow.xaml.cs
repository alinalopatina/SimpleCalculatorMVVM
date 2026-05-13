using SimpleCalculatorMVVM.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimpleCalculatorMVVM.Views
{
    public partial class MainWindow : Window
    {
        private ButtonFactory _buttonFactory;
        private bool _isScientificMode = false;
        private ViewModels.CalculatorViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _buttonFactory = new ButtonFactory();
            _viewModel = DataContext as ViewModels.CalculatorViewModel;

            if (_viewModel != null)
            {
                _viewModel.OnRequestModeSwitch += OnModeSwitch;
                _viewModel.OnRequestSetTopmost += SetTopmost;
            }

            CreateButtons();

            // Установка курсора
            this.MouseMove += MainWindow_MouseMove;
            this.Loaded += MainWindow_Loaded;

            // Подписка на события клавиатуры
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Статическая загрузка шрифта из ресурсов
            var digitalFont = this.FindResource("DigitalFont") as FontFamily;
            if (digitalFont != null && DisplayTextBox != null)
            {
                DisplayTextBox.FontFamily = digitalFont;
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            var element = this.InputHitTest(position) as FrameworkElement;

            // Динамическое изменение курсора
            if (element is Button)
            {
                this.Cursor = Cursors.Hand;
            }
            else if (element is TextBox)
            {
                this.Cursor = Cursors.IBeam;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка горячих клавиш
            if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _viewModel?.UndoCommand?.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.Y && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _viewModel?.RedoCommand?.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                _viewModel?.ClearCommand?.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                _viewModel?.EqualsCommand?.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                _viewModel?.ProcessBackspace();
                e.Handled = true;
            }
            // Цифры
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = (e.Key - Key.D0).ToString();
                _viewModel?.ButtonCommand?.Execute(digit);
                e.Handled = true;
            }
            // Операции
            else if (e.Key == Key.Add || e.Key == Key.OemPlus)
            {
                _viewModel?.ButtonCommand?.Execute("+");
                e.Handled = true;
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                _viewModel?.ButtonCommand?.Execute("−");
                e.Handled = true;
            }
            else if (e.Key == Key.Multiply)
            {
                _viewModel?.ButtonCommand?.Execute("×");
                e.Handled = true;
            }
            else if (e.Key == Key.Divide)
            {
                _viewModel?.ButtonCommand?.Execute("÷");
                e.Handled = true;
            }
        }

        private void OnModeSwitch(string mode)
        {
            _isScientificMode = mode == "Scientific";
            CreateButtons();
        }

        private void SetTopmost(bool topmost)
        {
            this.Topmost = topmost;
        }

        private void CreateButtons()
        {
            ButtonsGrid.Children.Clear();

            var buttonDefs = _isScientificMode ?
                _buttonFactory.GetScientificButtons() :
                _buttonFactory.GetStandardButtons();

            foreach (var buttonDef in buttonDefs)
            {
                Button button = new Button();
                buttonDef.ApplyStyle(button);

                button.Command = _viewModel?.ButtonCommand;
                button.CommandParameter = buttonDef.Content;

                Grid.SetRow(button, buttonDef.Row);
                Grid.SetColumn(button, buttonDef.Column);

                if (buttonDef.ColumnSpan > 1)
                    Grid.SetColumnSpan(button, buttonDef.ColumnSpan);

                ButtonsGrid.Children.Add(button);
            }
        }
    }
}