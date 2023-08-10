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
            DataContext = new MainViewModel(new JsonTracker(JsonReaderConstants.JsonFilePath));
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            ((MainViewModel)DataContext).Dispose();
        }
    }
}
