using Connections;
using GuiAndroid.Model;
using MVVMKit;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;

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
            bool flaga = model.Connection.SenderConnectionState != ConnectionState.NotConnected;
            while (!flaga)
            {

                string IPaddress = await App.AlertServices.InputBoxAsync("Adres IP", "Podaj adres AjPI:", "Zatwierdź","Anuluj");
                try 
                {
                    ConnectionSettings CS = ConnectionSettings.Parse(IPaddress);
                    flaga = model.Connection.Connect(CS);
                }
                catch (Exception ex)
                {
                    flaga = true;
                }
            }
        }

        public double Buttons
        {
            get
            {
                return ustawienia.Buttons;
            }
            set
            {
                ustawienia.Buttons = value;
                OnPropertyChanged(nameof(Buttons));
            }
        }
        public int Font
        {
            get
            {
                return ustawienia.Font;
            }
            set
            {
                ustawienia.Font = value;
                OnPropertyChanged(nameof(Font));
            }
        }
        public string Background
        {
            get
            {
                return ustawienia.Background;
            }
            set
            {
                ustawienia.Background = value;
                OnPropertyChanged(nameof(Background));
            }
        }
        public void _Zapisz()
        {
            AndUstawienia.Zapisz(ustawienia.Buttons, ustawienia.Font, ustawienia.Background);
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
