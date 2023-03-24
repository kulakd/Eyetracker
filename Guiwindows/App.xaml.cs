namespace Guiwindows;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        const int newWidth = 1920;
        const int newHeight = 1200;
        window.X = 0;
        window.Y = 0;
        window.Width = newWidth;
        window.Height = newHeight;
        window.MinimumHeight = newHeight;
        window.MinimumWidth = newWidth;
        window.MaximumHeight = newHeight;
        window.MaximumWidth = newWidth;
        return window;
    }
}
