using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace Notifications
{
    public class Notification
    {
        private readonly int _id;
        private readonly string _title;
        private readonly string _message;
        private readonly int _badgeNumber;
        private string _sound;
        private readonly IAudioManager audioManager;
        private bool alarmActive = false;

        public Notification(int id, string title, string message, int BadgeNumber, string sound, IAudioManager audio)
        {
            audioManager = audio;
            _id = id;
            _title = title;
            _message = message;
            _badgeNumber = BadgeNumber;
            _sound = sound;
        }

        public void ShowNotification()
        {
            NotificationRequest request = new NotificationRequest
            {
                NotificationId = _id,
                Title = _title,
                Description = _message,
                BadgeNumber = _badgeNumber,
            };

            LocalNotificationCenter.Current.Show(request);
        }

        public async void AlarmSound()
        {
            IAudioPlayer player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(_sound));
            player.Play();

        }

        public void AlarmVibration(int secound)
        {
            int secondsToVibrate = Random.Shared.Next(1, secound);
            TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);
            Vibration.Default.Vibrate(vibrationLength);
        }

        public void Zatrzymaj()
        {
            alarmActive = false;
        }

        public void Nieskonczony()
        {
            alarmActive = true;
            OnAppearing();
            ShowOptionsButton_Clicked();
        }

        private async void ShowOptionsButton_Clicked()
        {
            string action = await Application.Current.MainPage.DisplayActionSheet("wybierz", "", null, "Akcept", "nie akcept");

            switch (action)
            {
                case "Akcept":
                    Zatrzymaj();
                    break;

                case "nie akcept":
                    Zatrzymaj();
                    break;
                default:
                    break;
            }
        }

        protected async void OnAppearing()
        {
            while (true)
            {
                if (alarmActive) AlarmVibration(1);

                await Task.Delay(2000);
            }
        }
    }
}
