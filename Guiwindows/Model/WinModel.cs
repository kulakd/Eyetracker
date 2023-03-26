using Connections;
using EyeTracker;
using System.Net;
using System.Runtime.Versioning;

namespace MauiGui.Model
{
    [SupportedOSPlatform("windows")]
    class WinModel
    {
        private static WinModel instance;
        public static WinModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new WinModel();
                return instance;
            }
        }

        private CamTracker cam = new CamTracker();
        public CamTracker Camera => cam;

        private P2PTCPVideoConnection connection = new P2PTCPVideoConnection();
        private ConnectionSettings settings = new ConnectionSettings(IPAddress.Loopback, 223711, 223712);
        //TODO: connection init via bt

        public WinModel()
        {
            cam.NewFrameEvent += (s, f) =>
            {
                if (connection.SenderConnectionState == ConnectionState.Connected) // czy trzeba sprawdzać?
                    connection.Video = f;
            };
            cam.Index = 0;
            Task.Run(AwaitConnection); //TODO: what if fails?
        }

        public async Task<bool> AwaitConnection() =>
            await Task.FromResult(connection.WaitForConnection(settings));

        public void OnClose()
        {
            cam.Stop();
            connection.Disconnect();
        }
    }
}
