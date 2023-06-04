using System.Text;


namespace ConnectionsMaui.Tests
{
    public class MessageTests
    {
        [Fact]
        public void Message_StringConstructor_CreatesMessageCorrectly()
        {
            var msg = new Message("test message");
            Assert.Equal(Message.MessageType.String, msg.Type);
            Assert.Equal("test message", msg.GetContents());
        }

        [Fact]
        public void Message_MemoryStreamConstructor_CreatesMessageCorrectly()
        {
            var memStream = new MemoryStream(Encoding.UTF8.GetBytes("test stream"));
            var msg = new Message(memStream);
            Assert.Equal(Message.MessageType.MemoryStream, msg.Type);
            Assert.Equal(memStream.ToArray(), ((MemoryStream)msg.GetContents()).ToArray());
        }

    }
}