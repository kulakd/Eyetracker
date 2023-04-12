using Connections;
using GuiAndroid.Model;
using MVVMKit;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace GuiAndroid.ViewModel
{
    public class AndVM : MVVMKit.ViewModel
    {
        private readonly AndModel model = new AndModel();

        private readonly Parameters ustawienia = AndUstawienia.Wczytaj();

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
                if (tmp != null) tmp.Dispose();
                OnPropertyChanged(nameof(Video));
            }
        }

        public ImageSource Video => ImageSource.FromStream(() => video);

        public AndVM()
        {
            model.Connection.ImageReceived += Connection_ImageReceived; ;

            Application.Current.MainPage.Appearing += Instance_Appearing;
        }

        private void Connection_ImageReceived(object sender, MemoryStream e)
        {
            video = e;
        }

        private void Instance_Appearing(object sender, EventArgs e)
        {
            ConnectPlease();
        }

        private ICommand click;
        public ICommand Click
        {
            get
            {
                if (click == null)
                    click = new RelayCommand((o) => ConnectPlease());
                return click;
            }
        }

        public async void Dialog()
        {
        }

        public async void ConnectPlease()
        {
            bool flaga = false;
            while (!flaga)
            {
                string IPaddress = await App.AlertServices.InputBoxAsync("Adres IP", "Podaj adres AjPI:", "Podane", "");
                try
                {
                    ConnectionSettings CS = ConnectionSettings.Parse(IPaddress);
                    flaga = model.Connection.Connect(CS);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        public double B
        {
            get
            {
                return ustawienia.B;
            }
            set
            {
                ustawienia.B = value;
                OnPropertyChanged(nameof(B));
            }
        }
        public int C
        {
            get
            {
                return ustawienia.C;
            }
            set
            {
                ustawienia.C = value;
                OnPropertyChanged(nameof(C));
            }
        }
        public string T
        {
            get
            {
                return ustawienia.T;
            }
            set
            {
                ustawienia.T = value;
                OnPropertyChanged(nameof(T));
            }
        }
        public void _Zapisz()
        {
            AndUstawienia.Zapisz(ustawienia.B, ustawienia.C, ustawienia.T);
        }

        public ICommand Zapisz
        {
            get
            {
                return new RelayCommand((object p) => { _Zapisz(); });
            }
        }
    }
}
