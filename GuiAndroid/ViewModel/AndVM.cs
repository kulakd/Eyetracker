using Connections;
using GuiAndroid.Model;
using MVVMKit;
using System.Diagnostics;
using System.Windows.Input;


namespace GuiAndroid.ViewModel
{
    public class AndVM : MVVMKit.ViewModel
    {
        private readonly AndModel model = new AndModel();
        private readonly Parameters ustawienia = AndUstawienia.Wczytaj();

        public AndVM()
        {
            model.Connection.ImageReceived += Connection_ImageReceived;
            model.Connection.StringReceived += Connection_StringReceived;
            model.Connection.ConnectionStateChanged += Connection_ConnectionStateChanged;
            Application.Current.MainPage.Appearing += Instance_Appearing;
        }

        private void Connection_ConnectionStateChanged(object sender, ConnectionEventEventArgs e)
        {
            if (e.SenderState == ConnectionState.NotConnected && e.ReceiverState == ConnectionState.NotConnected)
                App.AlertServices.AlertVibrateAsync("UWAGA", "POŁĄCZENIE Z PACJENTEM ZOSTAŁO ZERWANE, PROSZE POŁĄCZYĆ SIĘ PONOWNIE", "Zamknij", 500);
        }

        private void Connection_StringReceived(object sender, string e)
        {
            switch (e[0])
            {
                case 'N':
                    App.AlertServices.AlertVibrateAsync("WSZYSTKO WPORZĄDKU", "Pacjent nie potrzebuje uwagi", "Zamknij", 500);
                    break;
                case 'W':
                    App.AlertServices.AlertVibrateAsync("INFORMACJA", "Pacjent przebudził się", "Zamknij", 500);
                    break;
                case 'C':
                    App.AlertServices.AlertVibrateAsync("UWAGA", "Pacjent może wymagać pilnej uwagi", "Zamknij", 1500);
                    break;
                case 'H':
                    App.AlertServices.AlertVibrateAsync("NIEBEZPIECZEŃSTWO", "Pacjent pilnie potrzebuje pomocy", "Zamknij", 8000);
                    break;
                case 'A':
                    App.AlertServices.AlertVibrateAsync("NIEBEZPIECZEŃSTWO", "Pacjent może potrzebować pomocy", "Zamknij", 10000);
                    break;
            }
        }

        private Stopwatch _watch = Stopwatch.StartNew();
        private void Connection_ImageReceived(object sender, MemoryStream e)
        {
            DrawableCanvas.Stream = e;
            if (_watch.ElapsedMilliseconds < 100)
                return;
            _watch.Restart();
            MainPage.Canvas.Invalidate();
        }

        private void Instance_Appearing(object sender, EventArgs e)
        {
            ConnectPlease();
        }

        public async void ConnectPlease()
        {
            bool flaga = model.Connection.SenderConnectionState != ConnectionState.NotConnected;
            while (!flaga)
            {
                string IPaddress = await App.AlertServices.InputBoxAsync("Adres IP", "Wpisz adres IP urządzenia:", "Potwierdź", "Anuluj");
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
        private ICommand gray;

        public ICommand GrayBtnClick
        {
            get
            {
                if (gray == null)
                    gray = new RelayCommand(o =>
                    {
                        ConnectPlease();
                    });
                return gray;
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
            AndUstawienia.Zapisz(ustawienia.Font, ustawienia.Background);
        }

        public async void Info()
        {
            await App.Current.MainPage.DisplayAlert("UWAGA", "Zmiany zostaną wprowadzone po ponownym uruchomieniu aplikacji.", "Potwierdź");
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
