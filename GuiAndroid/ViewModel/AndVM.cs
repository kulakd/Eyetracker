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

        //private AndUstawienia ustawienia = new AndUstawienia();

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

        public (int, int) rozmiary;

        public AndVM()
        {
            model.Connection.ImageReceived += (s, im) => video = im;

            //rozmiary = AndUstawienia.Czytaj();

            Application.Current.MainPage.Appearing += Instance_Appearing;            
        }

        private void Instance_Appearing(object sender, EventArgs e)
        {
            ConnectPlease();
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

        public async void ConnectPlease()
        {
            bool flaga = false;
            while (!flaga)
            {
                string IPaddress = await App.AlertServices.InputBoxAsync("Adres IP", "Podaj adres AjPI:", "Podane","");
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

        //public int B
        //{
        //    get
        //    {
        //        return ustawienia.B;
        //    }
        //    set
        //    {
        //        ustawienia.B = value;
        //        OnPropertyChanged(nameof(B));
        //    }
        //}
        //public int C
        //{
        //    get
        //    {
        //        return ustawienia.C;
        //    }
        //    set
        //    {
        //        ustawienia.C = value;
        //        OnPropertyChanged(nameof(C));
        //    }
        //}
        //public void _Zapisz()
        //{
        //    AndUstawienia.Zapisz(ustawienia.B, ustawienia.C);
        //}

        //public ICommand Zapisz
        //{
        //    get
        //    {
        //        return new RelayCommand((object p) => { _Zapisz(); });
        //    }
        //}
    }
}
