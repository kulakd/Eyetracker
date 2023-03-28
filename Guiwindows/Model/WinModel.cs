using Connections;
using EyeTracker;
using QRCoder;
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

        private readonly P2PTCPVideoConnection connection = new P2PTCPVideoConnection();
        private readonly ConnectionSettings settings;

        public WinModel()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress add = host.AddressList[0];
            settings = new ConnectionSettings(add, 223711, 223712);

            cam.NewFrameEvent += (s, f) =>
            {
                if (connection.SenderConnectionState == ConnectionState.Connected) // czy trzeba sprawdzać?
                    connection.Video = f;
            };
            cam.Index = 0;
            Task.Run(LoopWait);
        }

        public async void LoopWait()
        {
            bool res = await Task.Run(AwaitConnection);

            if (res)
                return;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(LoopWait);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public async Task<bool> AwaitConnection() =>
            await Task.FromResult(connection.WaitForConnection(settings));

        public void OnClose()
        {
            cam.Stop();
            connection.Disconnect();
        }

        #region QR
        private readonly QRCodeGenerator generator = new QRCodeGenerator();

        private void CreateQR()
        {
        }

        #endregion
    }
}
