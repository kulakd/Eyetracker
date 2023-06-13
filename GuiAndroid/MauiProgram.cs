using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Notifications;
using Plugin.Maui.Audio;

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
		builder.Services.AddSingleton<IAudioManager,  AudioManager>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
	}

}

