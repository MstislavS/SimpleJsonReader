using JsonReader.Model.Interfaces;

namespace JsonReader.Model
{
    public class JsonTracker : IJsonTracker
    {
        private string _filePath;
        private DateTime _lastModificationTime = DateTime.MinValue;
        PeriodicTimer _timer;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public static readonly TimeSpan TrackingPeriod = TimeSpan.FromSeconds(2);

        public JsonTracker(string path) 
        {
            if (!File.Exists(path))
                throw new ArgumentException($"The file '{path}' doesn't exist");

            _filePath = path;
            _timer = new PeriodicTimer(TrackingPeriod);
        }

        public string JsonFilePath => _filePath;

        public event EventHandler<string> TextChanged = delegate { };

        public string ReadText()
        {
            return File.Exists(_filePath) ? File.ReadAllText(_filePath) : string.Empty;
        }

        public async Task StartAsync()
        {
            CheckAndReadJsonFile();
            
            while(await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
            {
                CheckAndReadJsonFile();
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private void CheckAndReadJsonFile()
        {
            if(File.Exists(_filePath))
            {
                var time = File.GetLastWriteTime(_filePath);
                if (time > _lastModificationTime)
                {
                    _lastModificationTime= time;
                    var handler = TextChanged;
                    handler(this, File.ReadAllText(_filePath));
                }
            }
        }
    }
}