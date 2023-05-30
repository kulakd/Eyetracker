using Connections;
using EyeTracker;
using System.Net;
using System.Runtime.Versioning;
using TrackerConnector;

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
        public readonly PipeReceiver ETReceiver = new PipeReceiver();

        private WinModel()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress add = host.AddressList[1];
            Settings = new ConnectionSettings(add, 4000, 4001);

            ETReceiver.TrackerEvent += HandleTrackerEvent;
            ETReceiver.ConnectPipe();

            cam.NewFrameEvent += (s, f) => Connection.Video = f;
            Connection.ConnectionStateChanged += Connection_ConnectionStateChanged;
            Start();
        }

        private void HandleTrackerEvent(object sender, TrackerEventType e)
        {
            if (Connection.SenderConnectionState == ConnectionState.Connected)
            {
                switch (e)
                {
                    case TrackerEventType.WAKE_UP:
                        Connection.Send("W");
                        break;
                    case TrackerEventType.ALARM:
                        Connection.Send("A");
                        break;
                }
            }
        }

        private void Connection_ConnectionStateChanged(object sender, ConnectionEventEventArgs e)
        {
            if (e.SenderState == ConnectionState.NotConnected &&
                e.ReceiverState == ConnectionState.NotConnected)
                Start();
        }

        public void Start()
        {
            while (!Connection.WaitForConnection(Settings)) ;
        }

        public void OnClose()
        {
            cam.Stop();
            Connection.Disconnect();
        }

    }
}
