using JsonReader.App.ViewModels;
using JsonReader.Model.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace JsonReader.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(App.Container.GetService<IJsonTracker>());
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                // Clean the view model and stop monitor
                viewModel.Dispose();
            }
        }
    }
}