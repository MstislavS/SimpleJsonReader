namespace JsonReader.Model.Interfaces
{
    public interface IJsonTracker
    {
        event EventHandler<string> TextChanged;

        string JsonFilePath { get; }

        string ReadText();

        Task StartAsync();

        void Stop();
    }
}