using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Notifications;

namespace GuiAndroid;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddSingleton<IAlertService, AlertService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Add dependecy injection to main page
        builder.Services.AddSingleton<MainPage>();

#if ANDROID
        builder.Services.AddTransient<IServiceTest, DemoServices>();
#endif

        return builder.Build();
	}

}

public interface IServiceTest
{
    void Start();
    void Stop();
}