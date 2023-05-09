using Connections;
using GuiAndroid.Model;
using MVVMKit;
using System.Windows.Input;

namespace GuiAndroid.ViewModel
{
    public class AndVM : MVVMKit.ViewModel
    {
        private readonly AndModel model = new AndModel();

        private readonly Parameters ustawienia = AndUstawienia.Wczytaj();

        //private MemoryStream[] _video = new MemoryStream[2];
        //private int _videoIndex = 0;
        //private bool firstVisible = true;
        //private MemoryStream video
        //{
        //    set
        //    {
        //        if (value == null || value.Length == 0 || !value.CanRead)
        //            throw new ArgumentNullException("value");
        //        MemoryStream tmp = _video[_videoIndex];
        //        _video[_videoIndex] = value;
        //        if (tmp != null) tmp.Dispose();
        //        switch (_videoIndex)
        //        {
        //            case 0:
        //                firstVisible = false;
        //                OnPropertyChanged(nameof(Video1), nameof(Visible1), nameof(Visible2));
        //                break;
        //            case 1:
        //                firstVisible = true;
        //                OnPropertyChanged(nameof(Video2), nameof(Visible1), nameof(Visible2));
        //                break;
        //        }
        //        //_videoIndex = ++_videoIndex % _video.Length;
        //        _videoIndex = 1;
        //        model.Connection.ReceiveVideo = false;
        //    }
        //}

        //public ImageSource[] Video => _video.Select(v => ImageSource.FromStream(() => v)).ToArray();
        //public ImageSource Video1 => ImageSource.FromStream(() => { _video[0].Position = 0; return _video[0]; });
        //public ImageSource Video2 => ImageSource.FromStream(() => { _video[1].Position = 0; return _video[1]; });
        //public bool Visible1 => !firstVisible;
        //public bool Visible2 => firstVisible;

        public AndVM()
        {
            model.Connection.ImageReceived += Connection_ImageReceived;

            Application.Current.MainPage.Appearing += Instance_Appearing;
        }

        private void Connection_ImageReceived(object sender, MemoryStream e)
        {
            DrawableCanvas.Stream = e;
            MainPage.Canvas.Invalidate();
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

                string IPaddress = await App.AlertServices.InputBoxAsync("Adres IP", "Podaj adres AjPI:", "Zatwierdź", "Anuluj");
                try
                {
                    ConnectionSettings CS = ConnectionSettings.Parse(IPaddress);
                    flaga = model.Connection.Connect(CS);
                }
                catch
                {
                    flaga = true;
                }
            }
            model.Connection.ReceiveVideo = true;
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

        public async void Info()
        {
            await App.Current.MainPage.DisplayAlert("Uwaga", "Zmiany zostaną wprowadzone po ponownym uruchomieniu aplikacji.", "OK");
        }

        public ICommand Zapisz
        {
            get
            {
                return new RelayCommand((object p) => { Info(); _Zapisz(); });
            }
        }
    }
}
