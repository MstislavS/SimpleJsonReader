using JsonReader.Common;
using JsonReader.Model.Interfaces;
using System.Windows.Input;

namespace JsonReader.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IJsonTracker _jsonTracker;
        private string _text;

        public MainViewModel(IJsonTracker jsonTracker)
        {
            Argument.NotNull(jsonTracker, nameof(jsonTracker));
            _text = string.Empty;
            _jsonTracker = jsonTracker;

            LoadCommand = CreateAsyncCommand(OnLoaded);
            ReadFileCommand = CreateAsyncCommand(ReadFile);
            CancelFileCommand = CreateAsyncCommand(CancelTracking);

            _jsonTracker.TextChanged += JsonTracker_TextChanged;
        }

        public ICommand CancelFileCommand { get; }

        public string FilePath => _jsonTracker.JsonFilePath;

        public ICommand LoadCommand { get; }

        public ICommand ReadFileCommand { get; }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                SetProperty(ref _text, value);
            }
        }

        protected override void DisposeManagedResources()
        {
            _jsonTracker.Stop();
            _jsonTracker.TextChanged -= JsonTracker_TextChanged;

            base.DisposeManagedResources();
        }

        private void CancelTracking()
        {
            _jsonTracker.Stop();
        }

        private void JsonTracker_TextChanged(object? sender, string newText)
        {
            Text = newText;
        }

        private void OnLoaded()
        {
            _ = _jsonTracker.StartAsync();
        }

        private void ReadFile()
        {
            Text = _jsonTracker.ReadText();
        }
    }
}