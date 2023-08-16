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
        public async Task TrackingFileChange_Success()
        {
            // Arrange
            var textChanged = new AutoResetEvent(false);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "json-test.json");
            var tracker = new JsonTracker(path, TimeSpan.FromMilliseconds(2));
            _ = tracker.StartAsync();
            tracker.TextChanged += (s, e) => textChanged.Set();
            bool wasRaised = textChanged.WaitOne(1);
            Assert.False(wasRaised);

            // Act
            File.AppendAllText(path, "+");
            
            // Assert
            // TODO: Sometimes 6 ms are not enought to get the result
            wasRaised = textChanged.WaitOne(TimeSpan.FromMilliseconds(6));
            Assert.True(wasRaised, "Text changes have not been changed");
            tracker.Stop();
        }
    }
}