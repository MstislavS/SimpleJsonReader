namespace JsonReader.Common
{
    public static class JsonReaderConstants
    {
        public static readonly string JsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "json.json");
        public static readonly TimeSpan TrackingPeriod = TimeSpan.FromSeconds(2);

    }
}
