﻿namespace Notifications
{
    public class AlertService : IAlertService
    {
        public Task AlertAsync(string title, string message, string confirmation) =>
            Application.Current.MainPage.DisplayAlert(title, message, confirmation);

        public Task<bool> ChoiceAsync(string title, string message, string confirm, string reject) =>
            Application.Current.MainPage.DisplayAlert(title, message, confirm, reject);

        public Task<string> InputBoxAsync(string title, string message, string confirmation) =>
            Application.Current.MainPage.DisplayPromptAsync(title, message, confirmation);
    }
}
