using System.Net;

namespace Connections
{
    public class ConnectionSettings
    {
        public IPAddress Address { get; set; }
        public int SendPort { get; set; }
        public int ReceivePort { get; set; }

        public ConnectionSettings(IPAddress address, int sendPort, int receivePort)
        {
            Address = address;
            SendPort = sendPort;
            ReceivePort = receivePort;
        }
        public ConnectionSettings(string hostname, int sendPort, int receivePort)
            : this(IPAddress.Parse(hostname), sendPort, receivePort) { }

        public static ConnectionSettings Parse(string s)
        {
            string[] ss = s.Split(":");
            if (ss.Length != 3)
                throw new FormatException("Missing ports");
            return new ConnectionSettings(ss[0], int.Parse(ss[1]), int.Parse(ss[2]));
        }

        public override string ToString() =>
           $"IP: {Address}; Send:Receive ports {SendPort}:{ReceivePort}";
    }
}
