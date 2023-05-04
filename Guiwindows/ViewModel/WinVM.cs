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

        public WinVM()
        {
            model = WinModel.Instance;
            model.Connection.ConnectionStateChanged += ConnectionStateChanged;
        }

        private void ConnectionStateChanged(object sender, ConnectionEventEventArgs e)
        {
            bool NotConnected =
                   e.SenderState == ConnectionState.NotConnected ||
                   e.ReceiverState == ConnectionState.NotConnected ||
                   e.SenderState == ConnectionState.Waiting ||
                   e.ReceiverState == ConnectionState.Waiting;
            bool Connected =
                  e.SenderState == ConnectionState.Connected &&
                  e.ReceiverState == ConnectionState.Connected;
            OnPropertyChanged(nameof(NotConnected), nameof(Connected));
            //if (Connected)
            //    model.Camera.Index = 0;
            //else if (NotConnected)
            //{
            //    model.Camera.Stop();
            //    Shell.Current.GoToAsync("//IPPage");
            //}
        }

        private ICommand stop;
        private ICommand come;
        private ICommand help;

        public ICommand Stop
        {
            get
            {
                if (stop == null)
                    stop = new RelayCommand((o) => { return; });
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
