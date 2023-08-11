using JsonReader.Model;

namespace JsonReader.UnitTests
{
    public class JsonReaderUnitTest
    {
        /// <summary>
        /// Test validates that json tracker detects new changes.
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Successful tracking file changes")]
        public void TrackingFileChange_Success()
        {
            // Arrange
            AutoResetEvent textChanged = new(false);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "json-test.json");
            JsonTracker tracker = new(path, TimeSpan.FromMilliseconds(3));
            _ = tracker.StartAsync();
            tracker.TextChanged += (s, e) => textChanged.Set();
            bool wasRaised = textChanged.WaitOne(1);
            Assert.False(wasRaised);

            // Act
            File.AppendAllText(path, "+");

            // Assert
            wasRaised = textChanged.WaitOne(TimeSpan.FromSeconds(3));
            Assert.True(wasRaised, "Text changes have not been changed");
        }
    }
}