using System;
using System.Resources;
using System.Reflection;
using System.Windows;
using SimpleCalculatorMVVM.About;  // ← ДОБАВЛЕНО

namespace SimpleCalculatorMVVM.Views
{
    public partial class AboutDialog : Window
    {
        private ResourceManager _resourceManager;

        public AboutDialog()
        {
            InitializeComponent();

            // Динамическая загрузка ресурсов
            LoadResourcesDynamically();

            // Использование статической библиотеки About
            LoadDeveloperInfo();

            BtnOK.Click += (s, e) => Close();
        }

        private void LoadDeveloperInfo()
        {
            try
            {
                // Информация из библиотеки About
                TitleText.Text = DeveloperInfo.ProductName;

                var version = Assembly.GetExecutingAssembly().GetName().Version;
                if (version != null)
                {
                    VersionText.Text = $"Версия {DeveloperInfo.Version} (build {version.Revision})";
                }
                else
                {
                    VersionText.Text = $"Версия {DeveloperInfo.Version}";
                }

                // Можно добавить отображение разработчиков
                // var developers = DeveloperInfo.GetDevelopers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки информации о разработчиках: {ex.Message}");
                // Запасные значения
                TitleText.Text = "Smart Calculator MVVM";
                VersionText.Text = "Версия 2.0.0";
            }
        }

        private void LoadResourcesDynamically()
        {
            try
            {
                _resourceManager = new ResourceManager(
                    "SimpleCalculatorMVVM.Resources.AppResources",
                    Assembly.GetExecutingAssembly());

                string appTitle = _resourceManager.GetString("AppTitle");
                if (!string.IsNullOrEmpty(appTitle))
                    TitleText.Text = appTitle;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки ресурсов: {ex.Message}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _resourceManager?.ReleaseAllResources();
        }
    }
}