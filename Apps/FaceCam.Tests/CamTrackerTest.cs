
using System.Drawing.Imaging;


namespace FaceCam.Tests
{
    public class CamTrackerTest
    {

        [Fact]
        public void CheckPaddingOutOfRange_ExpectException()
        {
            // Arrange
            var camTracker = new CamTracker();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => camTracker.Padding = -1);
            Assert.Throws<ArgumentOutOfRangeException>(() => camTracker.Padding = 1);
        }

        [Fact]
        public void Stop_WhenNotStreaming_ShouldNotThrowException()
        {
            // Arrange
            var camTracker = new CamTracker();

            // Act
            var exception = Record.Exception(() => camTracker.Stop());

            // Assert
            Assert.Null(exception);
        }

       
    }
}