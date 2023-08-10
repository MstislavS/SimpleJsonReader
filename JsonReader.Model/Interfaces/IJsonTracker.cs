namespace JsonReader.Model.Interfaces
{
    public interface IJsonTracker
    {
        Task StartAsync();

        string JsonFilePath { get; }

        event EventHandler<string> TextChanged;
        void Stop();

        string ReadText();
    }
}
