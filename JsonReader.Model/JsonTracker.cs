using JsonReader.Model.Interfaces;

namespace JsonReader.Model
{
    public class JsonTracker : IJsonTracker
    {
        private string _filePath;
        private DateTime _lastModificationTime = DateTime.MinValue;
        PeriodicTimer _timer;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public JsonTracker(string jsonFileName) 
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), jsonFileName);
            if (File.Exists(_filePath))
                throw new ArgumentException($"The file '{_filePath}' doesn't exist");

            _filePath = path;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
        }



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