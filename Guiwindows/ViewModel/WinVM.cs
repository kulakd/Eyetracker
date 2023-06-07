using Connections;
using MauiGui.Model;
using MVVMKit;
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
            if (e.SenderState == ConnectionState.NotConnected &&
                e.ReceiverState == ConnectionState.NotConnected)
            {
                model.Camera.Stop();
                Shell.Current.GoToAsync("//IPPage");
            }
            else
                model.Camera.Index = 0;
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
                        model.Connection.Send("N");
                    });
                return stop;
            }
        }
        public ICommand Come
        {
            get
            {
                if (come == null)
                    come = new RelayCommand((o) =>
                    {
                        model.Connection.Send("C");
                    });
                return come;
            }
        }
        public ICommand Help
        {
            get
            {
                if (help == null)
                    help = new RelayCommand((o) =>
                    {
                        model.Connection.Send("H");
                    });
                return help;
            }
        }

        public void OnClose() => // wywołać to, jak się zamyka aplikacja
            model.OnClose();
    }
}
