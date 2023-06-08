using MauiGui.Model;
using System.Runtime.Versioning;

namespace Guiwindows.ViewModel
{
    [SupportedOSPlatform("windows")]
    public class IPVM : MVVMKit.ViewModel
    {
        private readonly WinModel model;

        public string Address { get; private set; }

        public IPVM()
        {
            model = WinModel.Instance;
            Address = model.Settings.ToString();
            OnPropertyChanged(nameof(Address));

            model.Connection.ConnectionStateChanged += Connection_ConnectionStateChanged;
            try
            {
                model.Camera.Index = 0;
            }
            catch { }
        }

        private void Connection_ConnectionStateChanged(object sender, Connections.ConnectionEventEventArgs e)
        {
            if (e.SenderState == Connections.ConnectionState.Connected &&
                e.ReceiverState == Connections.ConnectionState.Connected)
                Shell.Current.GoToAsync("//MainPage");
        }
    }
}
