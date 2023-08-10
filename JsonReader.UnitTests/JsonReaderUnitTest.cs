using JsonReader.Common;
using JsonReader.Model;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

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
            var textChanged = false;
            var path = JsonReaderConstants.JsonFilePath;
            var tracker = new JsonTracker(path);
            tracker.StartAsync();
            tracker.TextChanged += (s, e) => textChanged = true;
            Assert.False(textChanged);

            // Act
            File.AppendAllText(path, " ");

            // Assert
            // Add a little bit bigger period (+1s)
            var delay = JsonTracker.TrackingPeriod.Add(TimeSpan.FromSeconds(1));
            await Task.Delay(delay);
            Assert.True(textChanged, "Text changes have not been changed");
        }
    }
}