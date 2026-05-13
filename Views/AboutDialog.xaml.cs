using System;
using System.Resources;
using System.Reflection;
using System.Windows;

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

            BtnOK.Click += (s, e) => Close();
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

                var version = Assembly.GetExecutingAssembly().GetName().Version;
                VersionText.Text = version != null
                    ? $"Версия {version}"
                    : "Версия 2.0.0";
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