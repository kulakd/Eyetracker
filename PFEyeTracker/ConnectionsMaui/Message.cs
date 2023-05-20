using System.Text;

namespace Connections
{
    public class Message
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public enum MessageType : byte { EndConnection, String, MemoryStream, SendNextFrame }

        #region Object
        public MessageType Type { get; private set; }
        private byte[] data;
        private bool HasContents =>
           Type == MessageType.MemoryStream || Type == MessageType.String;


        private Message(MessageType type)
        {
            Type = type;
            data = null;
        }

        public static Message EndMessage => new Message(MessageType.EndConnection);
        public static Message NextFrameMessage => new Message(MessageType.SendNextFrame);

        public Message(string message)
        {
            Type = MessageType.String;
            data = encoding.GetBytes(message);
        }

        public Message(MemoryStream stream)
        {
            Type = MessageType.MemoryStream;
            stream.Position = 0;
            data = stream.ToArray();
        }

        public object GetContents()
        {
            switch (Type)
            {
                case MessageType.String:
                    if (data == null)
                        throw new InvalidMessageException("Message Object invalid!");
                    return encoding.GetString(data);

                case MessageType.MemoryStream:
                    if (data == null)
                        throw new InvalidMessageException("Message Object invalid!");
                    return new MemoryStream(data);
            }
            return null;
        }
        #endregion

        public static async void Send(Message message, Stream stream, CancellationToken cancellationToken)
        {
            stream.WriteByte((byte)message.Type);
            if (message.HasContents)
            {
                byte[] len = BitConverter.GetBytes(message.data.Length);
                await stream.WriteAsync(len, 0, len.Length, cancellationToken);
                await stream.WriteAsync(message.data, 0, message.data.Length, cancellationToken);
            }
        }

        public static async Task<Message> Receive(Stream stream, CancellationToken cancellationToken)
        {
            Message message = new Message((MessageType)stream.ReadByte());

            if (message.HasContents)
            {
                byte[] lenBytes = await ReadBytes(4, stream, cancellationToken);
                int dataLength = BitConverter.ToInt32(lenBytes);

                if (dataLength == 0)
                    throw new Exception("Message Invalid");

                message.data = await ReadBytes(dataLength, stream, cancellationToken);
            }

            return message;
        }
        private static async Task<byte[]> ReadBytes(int count, Stream stream, CancellationToken cancellationToken)
        {
            byte[] data = new byte[count];
            int read = 0;
            while (read != count)
                read += await stream.ReadAsync(data, 0, count - read, cancellationToken);
            return data;
        }
    }
}
