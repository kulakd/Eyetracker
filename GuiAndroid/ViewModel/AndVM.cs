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

            Application.Current.MainPage.Appearing += Instance_Appearing;
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

        #region MainPage Buttons
        private ICommand green;
        private ICommand red;
        private ICommand gray;

        public ICommand GreenBtnClick
        {
            get
            {
                if (green == null)
                    green = new RelayCommand(o =>
                    {

                    });
                return green;
            }
        }
        public ICommand RedBtnClick
        {
            get
            {
                if (red == null)
                    red = new RelayCommand(o =>
                    {
                        Task<bool> answer = App.AlertServices.ChoiceAsync("Potwierdzenie", "Na pewno chcesz odmówić pomocy?", "Owszem", "Nie");
                        answer.ContinueWith(AnswerCont);
                    });
                return red;
            }
        }
        private async Task AnswerCont(Task<bool> answerTask)
        {
            bool answer = answerTask.Result;
            if (answer)
            {
                await App.AlertServices.AlertAsync("Potwierdzenie", "To bierz się do roboty! :)", "Aye Aye Captain");
                return;
            }
            else
            {
                await App.AlertServices.AlertAsync("Potwierdzenie", "Odmówiłeś Pomocy Osobie Potrzebującej :)", "Cieszę się :)");
                Application.Current.CloseWindow(Application.Current.MainPage.Window);
            }
        }

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
        #endregion

        #region Settings
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
        #endregion
    }
}
