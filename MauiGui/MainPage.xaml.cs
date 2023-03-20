namespace MauiGui;

public partial class MainPage : ContentPage
{
    public bool flaga = true;
    public int stan;
    public MainPage()
    {
        InitializeComponent();
#if WINDOWS
        Runnin(flaga);
#endif
    }
    async void Runnin(bool flaga)
    {
        await Loading.ProgressTo(1, 60000, Easing.Linear);
        // if (Loading.Progress == 1)
        //     Application.Current.CloseWindow(Application.Current.MainPage.Window);
    }

    public void ButtonPressed(object sender, EventArgs e)
    {
        if (STOPbtn.IsPressed == true)
        {
            Application.Current.CloseWindow(Application.Current.MainPage.Window);
            stan = -1;
        }
        if (ComeBtn.IsPressed == true)
        {
            Labelka.Background = new SolidColorBrush(Colors.Yellow);
            Labelka.Text = "YOUR KEEPERS HAVE BEEN INFORMED";
            stan = 1;
        }
        if (HelpBtn.IsPressed == true)
        {
            Labelka.Background = new SolidColorBrush(Colors.Red);
            Labelka.Text = "HELP IS ON THE WAY";
            stan = 2;
        }
    }
}

