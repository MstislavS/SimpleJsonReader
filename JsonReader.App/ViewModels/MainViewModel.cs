using JsonReader.Common;
using JsonReader.Model.Interfaces;
using System;
using System.Windows.Input;

namespace JsonReader.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IJsonTracker _jsonTracker;

        private string _text;
        public ICommand LoadCommand { get; }
        public ICommand ReadFileCommand { get; }
        public ICommand CancelFileCommand { get; }

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

        private void CancelTracking()
        {
            _jsonTracker.Stop();
        }

        private void ReadFile()
        {
            Text = _jsonTracker.ReadText();
        }

        private void JsonTracker_TextChanged(object? sender, string newText)
        {
            Text = newText;
        }

        private void OnLoaded()
        {
            _jsonTracker.StartAsync();
        }


        /// <summary>
        /// Gets the JSON text.
        /// </summary>
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        protected override void DisposeManagedResources()
        {
            _jsonTracker.Stop();
            _jsonTracker.TextChanged -= JsonTracker_TextChanged;

            base.DisposeManagedResources();
        }
    }
}