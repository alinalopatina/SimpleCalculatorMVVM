using System.Windows;

namespace SimpleCalculatorMVVM
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Статическая загрузка ресурсов уже выполняется через App.xaml
            // Динамическая загрузка будет в ViewModel
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Освобождение ресурсов
            base.OnExit(e);
        }
    }
}