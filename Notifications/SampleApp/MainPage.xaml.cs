using Notifications;
using Plugin.Maui.Audio;

namespace SampleApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IAudioManager audioManager;

        public MainPage(IAudioManager audioManager)
        {
            this.audioManager = audioManager;
            InitializeComponent();
        }

        private void startButton_Clicked(object sender, EventArgs e)
        {
            Notification notification = new Notification(1338, "Tytuł powiadomienia", "Treść powiadomienia", 48, "alarm2.mp3", audioManager);
            notification.ShowNotification();
            notification.AlarmSound();
            notification.AlarmVibration(2);
            notification.Nieskonczony();
        }
    }
}