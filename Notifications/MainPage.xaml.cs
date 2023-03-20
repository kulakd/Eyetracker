using Android.Media;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace Notifications;

public partial class MainPage : ContentPage
{
    private readonly IAudioManager audioManager;

    public MainPage(IAudioManager audioManager)
    {
        InitializeComponent();
        this.audioManager = audioManager;
        LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
    }

    private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
    {
        if (e.IsDismissed)
        {
            
        }
        else if (e.IsTapped)
        {

        }
    }

    private void Alarm1(object sender, EventArgs e)
    {
        AlarmSound1();

        var request = new NotificationRequest
        {
            NotificationId = 1337,
            Title = "Alarm.",
            Description = "Użytkownik otworzył oczy.",
            BadgeNumber = 42,
        };

        LocalNotificationCenter.Current.Show(request);
    }

    private async void AlarmSound1()
    {
        var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alarm-clock-short.wav"));
        player.Play();
    }
    private void Alarm2(object sender, EventArgs e)
    {
        AlarmSound2();

        var request = new NotificationRequest
        {
            NotificationId = 1337,
            Title = "Alarm.",
            Description = "Użytkownik rusza oczami.",
            BadgeNumber = 42,
        };

        LocalNotificationCenter.Current.Show(request);
    }

    private async void AlarmSound2()
    {
        var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alarm2.mp3"));
        player.Play();
    }
    private void Alarm3(object sender, EventArgs e)
    {
        AlarmSound3();

        var request = new NotificationRequest
        {
            NotificationId = 1337,
            Title = "Alarm!!!",
            Description = "Użytkownik krztusi się!!!",
            BadgeNumber = 42,
        };

        LocalNotificationCenter.Current.Show(request);
    }

    private async void AlarmSound3()
    {
        var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("loud_alarm.wav"));
        player.Play();
    }
}
