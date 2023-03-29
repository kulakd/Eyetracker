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
            model = new WinModel();
            model.Alert += (s, m) => App.AlertServices.AlertAsync("Connection", m, "ok");
            model.Start();
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
