using System;
using System.ComponentModel;
using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Media;
using SimpleCalculatorMVVM.Models;
using SimpleCalculatorMVVM.Models.Commands;
using SimpleCalculatorMVVM.Views;

namespace SimpleCalculatorMVVM.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private CalculatorEngine _engine;
        private CommandInvoker _invoker;
        private string _display = "0";
        private string _statusText = "Готов";
        private string _memoryStatus = "0";
        private string _currentMode = "Стандартный";
        private string _currentTheme = "Тёмная";
        private bool _alwaysOnTop = false;
        private ResourceManager _resourceManager;

        // Звук
        private bool _soundEnabled = true;
        private string _soundIcon = "🔊";

        // Команды
        public ICommand ButtonCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand SwitchToStandardCommand { get; }
        public ICommand SwitchToScientificCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand SetDarkThemeCommand { get; }
        public ICommand SetLightThemeCommand { get; }
        public ICommand ToggleSoundCommand { get; }

        // Свойства
        public string Display
        {
            get => _display;
            set { _display = value; OnPropertyChanged(); }
        }

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        public string MemoryStatus
        {
            get => _memoryStatus;
            set { _memoryStatus = value; OnPropertyChanged(); }
        }

        public string CurrentMode
        {
            get => _currentMode;
            set { _currentMode = value; OnPropertyChanged(); }
        }

        public string CurrentTheme
        {
            get => _currentTheme;
            set { _currentTheme = value; OnPropertyChanged(); }
        }

        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set
            {
                _alwaysOnTop = value;
                OnPropertyChanged();
                OnRequestSetTopmost?.Invoke(value);
            }
        }

        public bool SoundEnabled
        {
            get => _soundEnabled;
            set { _soundEnabled = value; OnPropertyChanged(); }
        }

        public string SoundIcon
        {
            get => _soundIcon;
            set { _soundIcon = value; OnPropertyChanged(); }
        }

        // События для View
        public event Action<string> OnRequestModeSwitch;
        public event Action<bool> OnRequestSetTopmost;

        public CalculatorViewModel()
        {
            _engine = new CalculatorEngine();
            _invoker = new CommandInvoker();

            // Динамическая загрузка ресурсов
            LoadResourcesDynamically();

            // Инициализация команд
            ButtonCommand = new RelayCommand(OnButtonClick);
            UndoCommand = new RelayCommand(_ => { _invoker.Undo(); UpdateDisplay(); });
            RedoCommand = new RelayCommand(_ => { _invoker.Redo(); UpdateDisplay(); });
            ClearCommand = new RelayCommand(_ =>
            {
                var cmd = new ActionCommand(_engine, "C");
                cmd.Execute();
                UpdateDisplay();
            });
            EqualsCommand = new RelayCommand(_ =>
            {
                var cmd = new OperationCommand(_engine, "=");
                cmd.Execute();
                UpdateDisplay();
            });
            SwitchToStandardCommand = new RelayCommand(_ => SwitchMode("Standard"));
            SwitchToScientificCommand = new RelayCommand(_ => SwitchMode("Scientific"));
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
            ShowAboutCommand = new RelayCommand(_ => ShowAboutDialog());
            SetDarkThemeCommand = new RelayCommand(_ => SwitchToDarkTheme());
            SetLightThemeCommand = new RelayCommand(_ => SwitchToLightTheme());
            ToggleSoundCommand = new RelayCommand(_ => ToggleSound());
        }

        private void LoadResourcesDynamically()
        {
            try
            {
                _resourceManager = new ResourceManager(
                    "SimpleCalculatorMVVM.Resources.AppResources",
                    Assembly.GetExecutingAssembly());

                string status = _resourceManager.GetString("StatusReady");
                if (!string.IsNullOrEmpty(status))
                    StatusText = status;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки ресурсов: {ex.Message}");
            }
        }

        private void SwitchMode(string mode)
        {
            CurrentMode = mode == "Standard" ? "Стандартный" : "Научный";
            StatusText = $"Переключен в режим: {CurrentMode}";
            OnRequestModeSwitch?.Invoke(mode);
        }

        private void SwitchToDarkTheme()
        {
            ApplyTheme("Themes/DarkTheme.xaml");
            CurrentTheme = "Тёмная";
            StatusText = "Тёмная тема активирована";
        }

        private void SwitchToLightTheme()
        {
            ApplyTheme("Themes/LightTheme.xaml");
            CurrentTheme = "Светлая";
            StatusText = "Светлая тема активирована";
        }

        private void ApplyTheme(string themePath)
        {
            try
            {
                var newTheme = new ResourceDictionary();
                newTheme.Source = new Uri(themePath, UriKind.Relative);

                var appResources = Application.Current.Resources;
                var mergedDictionaries = appResources.MergedDictionaries;

                for (int i = 0; i < mergedDictionaries.Count; i++)
                {
                    if (mergedDictionaries[i].Source != null &&
                        (mergedDictionaries[i].Source.OriginalString.Contains("LightTheme") ||
                         mergedDictionaries[i].Source.OriginalString.Contains("DarkTheme")))
                    {
                        mergedDictionaries.RemoveAt(i);
                        i--;
                    }
                }

                mergedDictionaries.Add(newTheme);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка смены темы: {ex.Message}");
            }
        }

        private void ShowAboutDialog()
        {
            var dialog = new AboutDialog();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        // ВОСПРОИЗВЕДЕНИЕ ЗВУКА (системный, без скачивания файлов)
      
        private void PlayClickSound()
        {
            if (!SoundEnabled) return;

            try
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.silent_short_click);
                player.Play();
            }
            catch { /* Игнорируем ошибки */ }
        }

        private void ToggleSound()
        {
            SoundEnabled = !SoundEnabled;
            SoundIcon = SoundEnabled ? "🔊" : "🔇";
            StatusText = SoundEnabled ? "Звук включён" : "Звук выключен";
        }

        private void OnButtonClick(object parameter)
        {
            if (parameter is not string content) return;

            // ВОСПРОИЗВЕДЕНИЕ ЗВУКА ПРИ НАЖАТИИ НА КНОПКУ
            PlayClickSound();

            ICalculatorCommand command = CreateCommand(content);
            if (command != null)
            {
                _invoker.ExecuteCommand(command);
                UpdateDisplay();
            }
        }

        private ICalculatorCommand CreateCommand(string content)
        {
            if (content == "↩")
            {
                _invoker.Undo();
                UpdateDisplay();
                return null;
            }
            if (content == "↪")
            {
                _invoker.Redo();
                UpdateDisplay();
                return null;
            }

            if (content.Length == 1 && (char.IsDigit(content[0]) || content == "."))
                return new DigitCommand(_engine, content);

            if (content == "+" || content == "−" || content == "×" || content == "÷")
                return new OperationCommand(_engine, content);
            if (content == "=")
                return new OperationCommand(_engine, "=");

            if (Array.Exists(new[] { "sin", "cos", "tan", "ln", "log", "√", "x²", "eˣ" }, f => f == content))
                return new ScientificCommand(_engine, content);

            if (Array.Exists(new[] { "MC", "MR", "M+", "M-" }, m => m == content))
                return new MemoryCommand(_engine, content);

            if (content == "C" || content == "±" || content == "%")
                return new ActionCommand(_engine, content);

            return null;
        }

        private void UpdateDisplay()
        {
            Display = _engine.GetFormattedDisplay();
            MemoryStatus = "0";
        }

        public void ProcessBackspace()
        {
            var command = new BackspaceCommand(_engine);
            _invoker.ExecuteCommand(command);
            UpdateDisplay();
        }

        public CalculatorEngine GetEngine() => _engine;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Освобождение ресурсов
        public void Dispose()
        {
            _resourceManager?.ReleaseAllResources();
            _invoker?.Clear();
        }
    }
}