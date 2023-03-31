using Connections;
using MauiGui.Model;
using MVVMKit;
using Notifications;
using System.Runtime.Versioning;
using System.Windows.Input;

namespace Guiwindows.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class WinVM : MVVMKit.ViewModel
    {
        private readonly WinModel model;

        public bool NotConnected { get; private set; }
        public bool Connected { get; private set; }

        public WinVM()
        {
            model = WinModel.Instance;
            model.Alert += (s, m) => App.AlertServices.AlertAsync("Connection", m, "ok");
            model.ConnectionAttempt += (s, m) => DisplayIP();

            model.Connection.ConnectionStateChanged += ConnectionStateChanged;

            model.Start();
            Application.Current.MainPage.Appearing += (s, a) => Task.Run(DisplayIP);
        }

        private void ConnectionStateChanged(object sender, ConnectionEventEventArgs e)
        {
            NotConnected =
                e.SenderState == ConnectionState.NotConnected ||
                e.ReceiverState == ConnectionState.NotConnected ||
                e.SenderState == ConnectionState.Waiting ||
                e.ReceiverState == ConnectionState.Waiting;
            Connected =
                e.SenderState == ConnectionState.Connected &&
                e.ReceiverState == ConnectionState.Connected;
            OnPropertyChanged(nameof(NotConnected), nameof(Connected));
            if (Connected)
                model.Camera.Index = 0;
        }

        private async void DisplayIP()
        {
            await App.AlertServices.AlertAsync("IP PLEASE", model.Settings.ToString(), "Maybe");
        }

        private ICommand stop;
        private ICommand come;
        private ICommand help;
        private ICommand polacz;

        public ICommand Stop
        {
            get
            {
                if (stop == null)
                    stop = new RelayCommand((o) => DisplayIP());
                return stop;
            }
        }
        public ICommand Come
        {
            get
            {
                if (come == null)
                    come = new RelayCommand((o) => { return; });
                return come;
            }
        }
        public ICommand Help
        {
            get
            {
                if (help == null)
                    help = new RelayCommand((o) => { return; });
                return help;
            }
        }

        public void OnClose() => // wywołać to, jak się zamyka aplikacja
            model.OnClose();

        //public ICommand ConnectoReparum
        //{
        //    get
        //    {
        //         if (polacz == null)
        //            stop = new RelayCommand((o) => DisplayIP());
        //         return stop;
        //    }
        //}

    }
}
