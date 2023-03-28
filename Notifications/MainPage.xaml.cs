using Android.App;
using Android.Media;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using static Android.Webkit.ConsoleMessage;
using AudioManager = Plugin.Maui.Audio.AudioManager;

namespace Notifications;

public partial class MainPage : ContentPage
{
    //private readonly IAudioManager audioManager;
    private bool alarmActive;
    public MainPage()
    {
        InitializeComponent();
        //this.audioManager = audioManager;
        // Wyłączenie przycisku "Wyłącz"
    }

    //private void Alarm1(object sender, EventArgs e)
    //{
    //    AlarmSound1();

    //    NotificationRequest request = new NotificationRequest
    //    {
    //        NotificationId = 1337,
    //        Title = "Alarm.",
    //        Description = "Użytkownik otworzył oczy.",
    //        BadgeNumber = 42,
    //    };

    //    LocalNotificationCenter.Current.Show(request);
    //}

    //private async void AlarmSound1()
    //{
    //    IAudioPlayer player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alarm-clock-short.wav"));
    //    player.Play();
    //}

    //private void Alarm2(object sender, EventArgs e)
    //{
    //    AlarmSound2();

    //    NotificationRequest request = new NotificationRequest
    //    {
    //        NotificationId = 1338,
    //        Title = "Alarm.",
    //        Description = "Użytkownik rusza oczami.",
    //        BadgeNumber = 42,
    //    };

    //    LocalNotificationCenter.Current.Show(request);
    //}

    //private async void AlarmSound2()
    //{
    //    IAudioPlayer player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alarm2.mp3"));
    //    player.Play();
    //}
    //private void Alarm3(object sender, EventArgs e)
    //{
    //    AlarmSound3();

    //    NotificationRequest request = new NotificationRequest
    //    {
    //        NotificationId = 1339,
    //        Title = "Alarm!!!",
    //        Description = "Użytkownik krztusi się!!!",
    //        BadgeNumber = 42,
    //    };

    //    LocalNotificationCenter.Current.Show(request);
    //}

    private async void AlarmSound3()
    {
        IAudioPlayer player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("loud_alarm.wav"));
        player.Play();
        int secondsToVibrate = Random.Shared.Next(1, 2);
        TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

        Vibration.Default.Vibrate(vibrationLength);
    }


    private void startButton_Clicked(object sender, EventArgs e)
    {

        // Włączenie aktywności budzika
        alarmActive = true;
        ShowOptionsButton_Clicked();
        // Zablokowanie przycisku "Uruchom" i odblokowanie przycisku "Wyłącz"
        startButton.IsEnabled = false;


    }

    private void stopButton_Clicked()
    {
        // Wyłączenie aktywności budzika
        alarmActive = false;

        // Odblokowanie przycisku "Uruchom" i zablokowanie przycisku "Wyłącz"
        startButton.IsEnabled = true;

        // Aktualizacja etykiety z komunikatem o stanie 
       
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
            await System.Threading.Tasks.Task.Delay(2000);
        }
    }


    private async void ShowOptionsButton_Clicked()
    {
        string action = await DisplayActionSheet("Select an action", "", null, "Akcept", "nie akcept");

        switch (action)
        {
            case "Akcept":
                stopButton_Clicked();
                break;

            case "nie akcept":
                stopButton_Clicked();
                break;

 

            default:
                break;
        }
    }

}
