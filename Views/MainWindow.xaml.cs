using SimpleCalculatorMVVM.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Text.Json;

namespace SimpleCalculatorMVVM.Views
{
    public partial class MainWindow : Window
    {
        private ButtonFactory _buttonFactory;
        private bool _isScientificMode = false;
        private ViewModels.CalculatorViewModel _viewModel;
        private AppConfig _config;

        public MainWindow()
        {
            InitializeComponent();

            // 1. Загружаем конфигурацию
            LoadConfig();

            // 2. Создаём ViewModel
            _viewModel = DataContext as ViewModels.CalculatorViewModel;

            if (_viewModel != null)
            {
                _viewModel.OnRequestModeSwitch += OnModeSwitch;
                _viewModel.OnRequestSetTopmost += SetTopmost;
            }

            // 3. Создаём фабрику кнопок
            _buttonFactory = new ButtonFactory();

            // 4. Создаём кнопки
            CreateButtons();

            // 5. Подписываемся на Loaded (один раз!)
            this.Loaded += MainWindow_Loaded;

            // 6. Подписка на клавиатуру
            this.KeyDown += MainWindow_KeyDown;

            // 7. Подписка на курсор
            this.MouseMove += MainWindow_MouseMove;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем шрифт
            var digitalFont = this.FindResource("DigitalFont") as FontFamily;
            if (digitalFont != null && DisplayTextBox != null)
                DisplayTextBox.FontFamily = digitalFont;

            // Применяем конфигурацию ТОЛЬКО ЗДЕСЬ
            ApplyConfig();
        }

        private void LoadConfig()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                string json = File.ReadAllText(path);

                _config = JsonSerializer.Deserialize<AppConfig>(json);

                if (_config == null)
                    _config = new AppConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки конфигурации:\n" + ex.Message);
                _config = new AppConfig();
            }

        }

        private void ApplyConfig()
        {
            // Защита от нулевых значений
            if (_config.FontSize <= 0)
                _config.FontSize = 42;

            if (_config.WindowWidth <= 0)
                _config.WindowWidth = 520;

            if (_config.WindowHeight <= 0)
                _config.WindowHeight = 800;

            // Применяем размеры окна
            this.Width = _config.WindowWidth;
            this.Height = _config.WindowHeight;

            // Применяем размер шрифта
            if (DisplayTextBox != null)
                DisplayTextBox.FontSize = _config.FontSize;

            // Применяем фон
            try
            {
                var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(_config.BackgroundColor);
                this.Background = brush;
            }
            catch
            {
                this.Background = Brushes.Black;
            }

            // Применяем тему
            if (_config.Theme == "Dark")
                _viewModel?.SetDarkThemeCommand.Execute(null);
            else
                _viewModel?.SetLightThemeCommand.Execute(null);
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
            if (ButtonsGrid == null || _buttonFactory == null)
                return;

            ButtonsGrid.Children.Clear();
            ButtonsGrid.RowDefinitions.Clear();
            ButtonsGrid.ColumnDefinitions.Clear();

            var buttonDefs = _isScientificMode
                ? _buttonFactory.GetScientificButtons()
                : _buttonFactory.GetStandardButtons();

            if (buttonDefs == null || buttonDefs.Count == 0)
                return;

            int maxRow = 0;
            int maxCol = 0;

            foreach (var def in buttonDefs)
            {
                int lastRow = def.Row;
                int lastCol = def.Column + (def.ColumnSpan > 1 ? def.ColumnSpan - 1 : 0);

                if (lastRow > maxRow) maxRow = lastRow;
                if (lastCol > maxCol) maxCol = lastCol;
            }

            for (int r = 0; r <= maxRow; r++)
                ButtonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int c = 0; c <= maxCol; c++)
                ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            ButtonsGrid.Height = double.NaN;
            ButtonsGrid.Width = double.NaN;

            foreach (var buttonDef in buttonDefs)
            {
                var button = new Button();
                buttonDef.ApplyStyle(button);

                button.Command = _viewModel?.ButtonCommand;
                button.CommandParameter = buttonDef.Content;

                int row = Math.Min(buttonDef.Row, ButtonsGrid.RowDefinitions.Count - 1);
                int col = Math.Min(buttonDef.Column, ButtonsGrid.ColumnDefinitions.Count - 1);

                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);

                if (buttonDef.ColumnSpan > 1)
                {
                    int span = Math.Min(buttonDef.ColumnSpan, ButtonsGrid.ColumnDefinitions.Count - col);
                    Grid.SetColumnSpan(button, span);
                }

                ButtonsGrid.Children.Add(button);
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            var element = this.InputHitTest(position) as FrameworkElement;

            if (element is Button)
                this.Cursor = Cursors.Hand;
            else if (element is TextBox)
                this.Cursor = Cursors.IBeam;
            else
                this.Cursor = Cursors.Arrow;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
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
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = (e.Key - Key.D0).ToString();
                _viewModel?.ButtonCommand?.Execute(digit);
                e.Handled = true;
            }
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
    }
}
