using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioManager = Plugin.Maui.Audio.AudioManager;

namespace Notifications.Model
{
    public class NotifModel
    {
        public async void AlarmSound1()
        {
            IAudioPlayer audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alarm-clock-short.wav"));
            audioPlayer.Play();
        }

        public void Alarm1()
        {
            AlarmSound1();
            NotificationRequest request = new NotificationRequest
            {
                NotificationId = 1,
                Title = "Alarm.",
                Description = "Użytkownik otworzył oczy.",
                BadgeNumber = 42,
            };

            LocalNotificationCenter.Current.Show(request);
        }

        private async void AlarmSound2()
        {
            IAudioPlayer player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("alarm2.mp3"));
            player.Play();
        }

        public void Alarm2()
        {
            AlarmSound2();
            NotificationRequest request = new NotificationRequest
            {
                NotificationId = 2,
                Title = "Alarm.",
                Description = "Użytkownik rusza oczami.",
                BadgeNumber = 42,
            };

            LocalNotificationCenter.Current.Show(request);
        }
        private async void AlarmSound3()
        {
            IAudioPlayer player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("loud_alarm.wav"));
            player.Play();
            int secondsToVibrate = Random.Shared.Next(1, 2);
            TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

            Vibration.Default.Vibrate(vibrationLength);
        }

        public void Alarm3()
        {
            AlarmSound3();
            NotificationRequest request = new NotificationRequest
            {
                NotificationId = 3,
                Title = "Alarm!!!",
                Description = "Użytkownik krztusi się!!!",
                BadgeNumber = 42,
            };

            LocalNotificationCenter.Current.Show(request);
        }

    }
}
