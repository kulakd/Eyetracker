using GuiAndroid.Model;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace GuiAndroid.ViewModel
{
    public class AndVM : MVVMKit.ViewModel
    {
        private readonly AndModel model = new AndModel();

        private MemoryStream _video;
        private MemoryStream video
        {
            get
            {
                _video.Position = 0;
                return _video;
            }
            set
            {
                MemoryStream tmp = _video;
                _video = value;
                tmp.Dispose();
                OnPropertyChanged(nameof(Video));
            }
        }

        public ImageSource Video => ImageSource.FromStream(() => video);

        public AndVM()
        {
            model.Connection.ImageReceived += (s, im) => video = im;
            //public string IpAddress = IPAddress
        }

        private ICommand click;
        public ICommand Click
        {
            get {
                //if (click == null)
                   //click = new RelayCommand();
                return click; }
        }

        public async void Dialog()
        {
        }


    }
}
