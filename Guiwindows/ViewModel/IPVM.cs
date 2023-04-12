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
            Task.Run(async () =>
            {
                await Task.Delay(15000);
                await SwitchPage();
            });
            model.Start();
        }

        private async Task SwitchPage()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
