using System.Text;

namespace Connections
{
    public class Message
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public enum MessageType : byte { EndConnection, String, MemoryStream, SendNextFrame }

        public MessageType Type { get; private set; }
        private byte[] data;


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

        public static async void Send(Message message, Stream stream, CancellationToken cancellationToken)
        {
            stream.WriteByte((byte)message.Type);
            if (message.data != null && message.Type != MessageType.EndConnection)
            {
                byte[] len = BitConverter.GetBytes(message.data.Length);
                await stream.WriteAsync(len, 0, len.Length, cancellationToken);
                await stream.WriteAsync(message.data, 0, message.data.Length, cancellationToken);
            }
            else if (message.data == null)
            {
                byte[] len = BitConverter.GetBytes(0);
                await stream.WriteAsync(len, 0, len.Length, cancellationToken);
            }
        }
        private const int delayMilis = 10;
        public static async Task<Message> Receive(Stream stream, CancellationToken cancellationToken)
        {
            Message message = new Message((MessageType)stream.ReadByte());

            if (message.Type == MessageType.EndConnection || message.Type == MessageType.SendNextFrame)
                return message;

            byte[] len = new byte[4];
            int read = await stream.ReadAsync(len, 0, len.Length, cancellationToken);
            while (read != len.Length)
            {
                await Task.Delay(delayMilis);
                read += await stream.ReadAsync(len, 0, len.Length - read, cancellationToken);
            }
            int leni = BitConverter.ToInt32(len);

            if (leni == 0)
            {
                message.data = null;
            }
            else
            {
                byte[] data = new byte[leni];
                read = await stream.ReadAsync(data, 0, leni, cancellationToken);
                while (read != leni)
                {
                    await Task.Delay(delayMilis);
                    read += await stream.ReadAsync(data, 0, leni - read, cancellationToken);
                }
                message.data = data;
            }

            return message;
        }
    }
}
