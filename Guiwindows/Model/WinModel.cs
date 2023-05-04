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

        public readonly P2PTCPVideoConnection Connection = new P2PTCPVideoConnection();
        public readonly ConnectionSettings Settings;

        private WinModel()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress add = host.AddressList[1];
            Settings = new ConnectionSettings(add, 4000, 4001);

            cam.NewFrameEvent += (s, f) => Connection.Video = f;
        }

        public void Start()
        {
            Task.Run(LoopWait);
        }

        public async void LoopWait()
        {
            bool res = await Task.FromResult(Connection.WaitForConnection(Settings));

            if (res)
                return;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(LoopWait);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public void OnClose()
        {
            cam.Stop();
            Connection.Disconnect();
        }

    }
}
