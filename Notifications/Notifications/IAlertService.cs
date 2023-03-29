namespace Notifications
{
    public interface IAlertService
    {
        Task AlertAsync(string title, string message, string confirmation);
        Task<bool> ChoiceAsync(string title, string message, string confirm, string reject);
        Task<string> InputBoxAsync(string title, string message, string confirmation);
    }
}
