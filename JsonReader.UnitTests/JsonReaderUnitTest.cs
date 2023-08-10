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
            var path = Path.Combine(Directory.GetCurrentDirectory(), "json-test.json");
            var tracker = new JsonTracker(path, TimeSpan.FromMilliseconds(3));
            tracker.StartAsync();
            tracker.TextChanged += (s, e) => textChanged.Set();
            var wasRaised = textChanged.WaitOne(1);
            Assert.False(wasRaised);

            // Act
            File.AppendAllText(path, "+");

            // Assert
            wasRaised = textChanged.WaitOne(TimeSpan.FromSeconds(3));
            Assert.True(wasRaised, "Text changes have not been changed");

        }
    }
}