using Notifications;

namespace GuiAndroid;

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
}
