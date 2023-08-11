using JsonReader.App.ViewModels;
using JsonReader.Common;
using JsonReader.Model;
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
            // TODO: Better to use container on getting JsonTracker model class. But I don't see such requirement in task
            DataContext = new MainViewModel(new JsonTracker(JsonReaderConstants.JsonFilePath, JsonReaderConstants.TrackingPeriod));
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if(DataContext is MainViewModel viewModel)
            {
                // Clean the view model and stop monitor
                viewModel.Dispose();
            }
        }
    }
}
