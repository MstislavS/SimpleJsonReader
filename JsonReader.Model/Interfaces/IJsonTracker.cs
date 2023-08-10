namespace JsonReader.Model.Interfaces
{
    public interface IJsonTracker
    {
        Task StartAsync();

        event EventHandler<string> TextChanged;
        void Stop();

        string ReadText();
    }
}
