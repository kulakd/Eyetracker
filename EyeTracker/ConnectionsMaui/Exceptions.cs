namespace Connections
{
    public class InvalidMessageStreamException : Exception
    {
        public int Desired { get; private set; }
        public int Read { get; private set; }
        public InvalidMessageStreamException(string? message, int desired, int read)
            : base(message) { Desired = desired; Read = read; }
    }

    public class InvalidMessageException : Exception
    {
        public InvalidMessageException(string? message) : base(message) { }
    }

    public class UnexpectedConnectionException : Exception
    {
        public string Expected { get; private set; }
        public string Received { get; private set; }

        public UnexpectedConnectionException(string? message, string expected, string received)
            : base(message) { Expected = expected; Received = received; }
    }
}
