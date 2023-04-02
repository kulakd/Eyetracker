using GuiAndroid.Model;

namespace GuiAndroid;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
        sliderB.Value = AndUstawienia.Przycisk();
        sliderC.Value = AndUstawienia.Czcionka();
	}

    private void sliders_ValueChanged(object sender, EventArgs e)
    {
        int b = (int)sliderB.Value;
        int c = (int)sliderC.Value;
        B.Text = b.ToString();
        C.Text = c.ToString();
    }

    private void Zapis(object sender, EventArgs e)
    {
        test.Text = "Zapisano";
        int b = (int)sliderB.Value;
        int c = (int)sliderC.Value;
        AndUstawienia.Zapisz(b, c);
    }
}