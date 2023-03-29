using MauiGui.Model;
using MVVMKit;
using System.Runtime.Versioning;
using System.Windows.Input;
using Connections;

namespace Guiwindows.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class WinVM : MVVMKit.ViewModel
    {
        private readonly WinModel model;

        public bool ShowIP { get; private set; }
        public bool ShowBtns { get; private set; }
        public string Address => model.Settings.ToString();

        public WinVM()
        {
            model = new WinModel();
            model.Alert += (s, m) => App.AlertServices.AlertAsync("Connection", m, "ok");
            model.Connection.ConnectionStateChanged += ConnectionStateChanged;
            model.Start();
            OnPropertyChanged(nameof(Address));
        }

        private void ConnectionStateChanged(object sender, ConnectionEventEventArgs e)
        {
            ShowIP =
                e.SenderState == ConnectionState.NotConnected ||
                e.ReceiverState == ConnectionState.NotConnected;
            ShowBtns =
                e.SenderState == ConnectionState.Connected &&
                e.ReceiverState == ConnectionState.Connected;
            OnPropertyChanged(nameof(ShowIP), nameof(ShowBtns));
        }

        private ICommand stop;
        private ICommand come;
        private ICommand help;

        public ICommand Stop
        {
            get
            {
                if (stop == null)
                    stop = new RelayCommand((o) =>
                    {
                        App.AlertServices.AlertAsync("ok", "ok", "ok");
                    });
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
    }
}
