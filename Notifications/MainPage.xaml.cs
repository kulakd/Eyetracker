using Android.App;
using Android.Media;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using static Android.Webkit.ConsoleMessage;

namespace Notifications;

public partial class MainPage : ContentPage
{
    private readonly IAudioManager audioManager;
    private bool alarmActive;
    public MainPage(IAudioManager audioManager)
    {
        InitializeComponent();
        this.audioManager = audioManager;

        LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;

        // Wyłączenie przycisku "Wyłącz"
        stopButton.IsEnabled = false;
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
            NotificationId = 1338,
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
            NotificationId = 1339,
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
        int secondsToVibrate = Random.Shared.Next(1, 7);
        TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

        Vibration.Default.Vibrate(vibrationLength);
    }


    private void startButton_Clicked(object sender, EventArgs e)
    {

        // Włączenie aktywności budzika
        alarmActive = true;

        // Zablokowanie przycisku "Uruchom" i odblokowanie przycisku "Wyłącz"
        startButton.IsEnabled = false;
        stopButton.IsEnabled = true;


    }

    private void stopButton_Clicked(object sender, EventArgs e)
    {
        // Wyłączenie aktywności budzika
        alarmActive = false;

        // Odblokowanie przycisku "Uruchom" i zablokowanie przycisku "Wyłącz"
        startButton.IsEnabled = true;
        stopButton.IsEnabled = false;

        // Aktualizacja etykiety z komunikatem o stanie 
        messageLabel.Text = "Alarm wyłączony";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();





        // Pętla główna aplikacji
        while (true)
        {
            if (alarmActive)
            {
                AlarmSound3();

                // Wyświetlenie aktualnego czasu

            }

            // Uspienie wątku na 1 sekundę
            await System.Threading.Tasks.Task.Delay(1000);
        }
    }




}
