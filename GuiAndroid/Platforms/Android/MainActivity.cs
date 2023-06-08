using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace GuiAndroid;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    //set an activity on main application to get the reference on the service
    public static MainActivity ActivityCurrent { get; set; }
    public MainActivity()
    {
        ActivityCurrent = this;
    }
}

[Service]
public class DemoServices : Service, IServiceTest //we implement our service (IServiceTest) and use Android Native Service Class
{
    public override IBinder OnBind(Intent intent)
    {
        throw new NotImplementedException();
    }

    [return: GeneratedEnum]//we catch the actions intents to know the state of the foreground service
    public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        if (intent.Action == "START_SERVICE")
        {
            RegisterNotification();//Proceed to notify
        }
        else if (intent.Action == "STOP_SERVICE")
        {
            StopForeground(true);//Stop the service
            StopSelfResult(startId);
        }

        return StartCommandResult.NotSticky;
    }

    //Start and Stop Intents, set the actions for the MainActivity to get the state of the foreground service
    //Setting one action to start and one action to stop the foreground service
    public void Start()
    {
        Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(DemoServices));
        startService.SetAction("START_SERVICE");
        MainActivity.ActivityCurrent.StartService(startService);
    }

    public void Stop()
    {
        Intent stopIntent = new Intent(MainActivity.ActivityCurrent, this.Class);
        stopIntent.SetAction("STOP_SERVICE");
        MainActivity.ActivityCurrent.StartService(stopIntent);
    }

    private void RegisterNotification()
    {
        NotificationChannel channel = new NotificationChannel("ServiceChannel", "ServiceDemo", NotificationImportance.Max);
        NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
        manager.CreateNotificationChannel(channel);
        Notification notification = new Notification.Builder(this, "ServiceChannel")
           .SetContentTitle("Service Working")
           .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
           .SetOngoing(true)
           .Build();

        StartForeground(100, notification);
    }
}
