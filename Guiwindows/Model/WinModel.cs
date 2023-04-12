using Connections;
using EyeTracker;
using Notifications;
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

        public readonly P2PTCPVideoConnection Connection = new P2PTCPVideoConnection();
        public readonly ConnectionSettings Settings;

        public event EventHandler<string> Alert;
        private void FireAlert(string message)
        {
            if (Alert != null) Alert(this, message);
        }

        public event EventHandler ConnectionAttempt;
        private void FireCA()
        {
            if (ConnectionAttempt != null) ConnectionAttempt(this, EventArgs.Empty);
        }

        private WinModel()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress add = host.AddressList[1];
            Settings = new ConnectionSettings(add, 4000, 4001);

            cam.NewFrameEvent += (s, f) =>
            {
                if (Connection.SenderConnectionState == ConnectionState.Connected) // czy trzeba sprawdzać?
                    Connection.Video = f;
            };
        }

        public void Start()
        {
            FireAlert($"Connect on {Settings}");
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
            await Task.FromResult(Connection.WaitForConnection(Settings));

        public void OnClose()
        {
            cam.Stop();
            Connection.Disconnect();
        }

        #region QR 
        // na przyszłość
        private readonly QRCodeGenerator generator = new QRCodeGenerator();

        private void CreateQR()
        {
        }

        #endregion

    }
}
