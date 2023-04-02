using GuiAndroid.Model;

namespace GuiAndroid;

public partial class Credits : ContentPage
{
	public Credits()
	{
		InitializeComponent();
		rozmiarCzcionki();
    }

	private void rozmiarCzcionki()
	{
        int r = (int)AndUstawienia.Czcionka();
		l1.FontSize = r;
        l2.FontSize = r;
        l3.FontSize = r;
        l4.FontSize = r;
        l5.FontSize = r;
        l6.FontSize = r;
        l7.FontSize = r;
        l8.FontSize = r;
        l9.FontSize = r;
        l10.FontSize = r;
    }

}