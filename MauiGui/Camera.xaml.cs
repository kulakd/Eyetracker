namespace MauiGui;

public partial class Camera : ContentPage
{
	public Camera()
	{
		InitializeComponent();
		//StanChanger();
    }

	public void StanChanger(MainPage MP)
	{
		int status = MP.stan;
		if(status == 1 )
		{
			StanLbl.Text = "I NEED HELP, PLEASE COME TO ME";
			StanLbl.TextColor = Color.FromArgb("ffff00");
        }
		if(status == 2 ) 
		{
            StanLbl.Text = "I'M IN DIRE NEED OF HELP, PLEASE HELP ME";
            StanLbl.TextColor = Color.FromArgb("ff0000");
        }
		else
		{
			StanLbl.Text = "EVERYTHINGS OKEY:)";
			StanLbl.TextColor = Color.FromArgb("02ff00");
		}
	}
}