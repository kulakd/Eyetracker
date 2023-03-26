using MauiGui.Model;
using MVVMKit;
using System.Windows.Input;

namespace Guiwindows.ViewModel
{
    public class WinVM : MVVMKit.ViewModel
    {
        private readonly WinModel model = new WinModel();

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
