using Notifications;

<<<<<<< HEAD
namespace Guiwindows;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }
    public static IAlertService AlertServices { get; private set; }

    public App(IServiceProvider provider)
    {
        InitializeComponent();

        Services = provider;
        AlertServices = Services.GetService<IAlertService>();

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
=======
namespace Guiwindows;

public partial class App : Application
{
    public static IServiceProvider Services;
    public static IAlertService AlertService;

	public App(IServiceProvider provider)
	{
		InitializeComponent();

        Services = provider;
        AlertService = Services.GetService<AlertService>();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        const int newWidth = 1440;
        const int newHeight = 1080;
        window.Width = newWidth;
        window.Height = newHeight;
        window.MinimumHeight = newHeight;
        window.MinimumWidth = newWidth;
        window.MaximumHeight = newHeight;
        window.MaximumWidth = newWidth;
        return window;
    }
}
>>>>>>> b73c20e23832b0e2e73d177854e741a7aa474e3b
