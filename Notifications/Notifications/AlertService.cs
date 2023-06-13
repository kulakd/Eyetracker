using Plugin.Maui.Audio;

namespace Notifications
{
    public class AlertService : IAlertService
    {
        private readonly IAudioManager _audioManager;
        public string Sound { get; set; }
        public AlertService(IAudioManager audioManager) { _audioManager = audioManager; }

        public Task AlertAsync(string title, string message, string confirmation) =>
            Application.Current.MainPage.DisplayAlert(title, message, confirmation);

        public Task AlertVibrateAsync(string title, string message, string confirmation, long millis)
        {
            Vibration.Vibrate(millis);
            if (_audioManager != null)
            {
                IAudioPlayer player = _audioManager.CreatePlayer(FileSystem.OpenAppPackageFileAsync(Sound).Result);
                player.Play(); 
            }
            return Application.Current.MainPage.DisplayAlert(title, message, confirmation);
        }

        public Task<bool> ChoiceAsync(string title, string message, string confirm, string reject) =>
            Application.Current.MainPage.DisplayAlert(title, message, confirm, reject);

        public Task<string> InputBoxAsync(string title, string message, string confirm, string reject) =>
            Application.Current.MainPage.DisplayPromptAsync(title, message, confirm, reject);
    }
}
