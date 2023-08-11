using JsonReader.Model.Interfaces;

namespace JsonReader.Model
{
    public class JsonTracker : IJsonTracker
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly PeriodicTimer _timer;

        private DateTime _lastModificationTime = DateTime.MinValue;
        public JsonTracker(string path, TimeSpan trackingPeriod)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"The file '{path}' doesn't exist");
            }
            
            if (trackingPeriod.TotalMilliseconds < 1)
            {
                throw new ArgumentException($"The tracking period '{trackingPeriod}' sould be greater then 1 milisecond");
            }

            JsonFilePath = path;
            _timer = new PeriodicTimer(trackingPeriod);
        }

        public event EventHandler<string> TextChanged = delegate { };

        public string JsonFilePath { get; }

        public string ReadText()
        {
            return File.Exists(JsonFilePath) ? File.ReadAllText(JsonFilePath) : string.Empty;
        }

        public async Task StartAsync()
        {
            CheckAndReadJsonFile();

            while (await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
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
            if (File.Exists(JsonFilePath))
            {
                DateTime time = File.GetLastWriteTime(JsonFilePath);
                if (time > _lastModificationTime)
                {
                    _lastModificationTime = time;
                    var handler = TextChanged;
                    handler(this, File.ReadAllText(JsonFilePath));
                }
            }
        }
    }
}