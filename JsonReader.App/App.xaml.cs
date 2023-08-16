using JsonReader.Common;
using JsonReader.Model;
using JsonReader.Model.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        private static IServiceProvider? s_serviceProvider;

        public App()
        {
            SetupExceptionHandling();
        }

        public static IServiceProvider Container
        {
            get
            {
                if (s_serviceProvider == null)
                {
                    s_serviceProvider = new ServiceCollection()
                        .AddSingleton<IJsonTracker, JsonTracker>(c => new JsonTracker(JsonReaderConstants.JsonFilePath, JsonReaderConstants.TrackingPeriod))
                        .BuildServiceProvider();
                }
                return s_serviceProvider;
            }
        }
        private void SetupExceptionHandling()
        {
            // Simple exception handling

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine($"Exception in UnhandledException {e.ExceptionObject}");
            };

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