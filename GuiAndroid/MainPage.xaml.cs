using GuiAndroid.Model;
using GuiAndroid.ViewModel;

namespace GuiAndroid;


public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        double Y = (double)AndUstawienia.Przycisk();
        string t = AndUstawienia.Tlo();
        green.ScaleY = Y / 100;
        red.ScaleY = Y / 100;
        BackgroundImageSource = t + ".png";
    }

    private void GreenButtonClicked(object sender, EventArgs e)
    {

    }

    private async void RedButtonClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Potwierdzenie", "Na pewno chcesz odmówić pomocy?", "Nie", "Owszem");
        if (answer)
        {
            await DisplayAlert("Potwierdzenie", "To bierz się do roboty! :)", "Aye Aye Captain");
            return;
        }
        else
        {
            await DisplayAlert("Potwierdzenie", "Odmówiłeś Pomocy Osobie Potrzebującej :)", "Cieszę się :)");
            Application.Current.CloseWindow(Application.Current.MainPage.Window);
        }
    }
}


