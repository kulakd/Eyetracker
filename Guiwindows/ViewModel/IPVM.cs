using MauiGui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

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
            model.Start();

            model.Connection.ConnectionStateChanged += Connection_ConnectionStateChanged;
            model.Camera.Index = 0;
        }

        private void Connection_ConnectionStateChanged(object sender, Connections.ConnectionEventEventArgs e)
        {
            if (e.SenderState == Connections.ConnectionState.Connected &&
                e.ReceiverState == Connections.ConnectionState.Connected)
                Shell.Current.GoToAsync("//MainPage");
        }
    }
}
