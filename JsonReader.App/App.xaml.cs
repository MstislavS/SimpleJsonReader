using System;
using System.Threading.Tasks;
using System.Windows;

namespace JsonReader.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                Console.WriteLine($"Exception in UnhandledException {e.ExceptionObject}");

            DispatcherUnhandledException += (s, e) =>
            {
                Console.WriteLine($"Exception in Application.Current.DispatcherUnhandledException {e.Exception}");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Console.WriteLine($"TaskScheduler.UnobservedTaskException {e.Exception}");
                e.SetObserved();
            };
        }
    }
}
