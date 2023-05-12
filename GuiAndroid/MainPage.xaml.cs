using GuiAndroid.Model;
using GuiAndroid.ViewModel;
namespace GuiAndroid;


public partial class MainPage : ContentPage
{
    public static GraphicsView Canvas { get; private set; }
    public MainPage()
    {
        InitializeComponent();
        Canvas = GraphicsV;
    }
}


