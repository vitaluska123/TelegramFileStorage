namespace TelegramFileStorage.ViewModels
{
    public enum NotificationType
    {
        Error,
        Success,
        Alert,
        Loading,
        Accept
    }

    public class NotificationModel
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public double Progress { get; set; } // Для loading
        public bool Show { get; set; } = true;
        public string AcceptText { get; set; } = "Принять";
        public string CancelText { get; set; } = "Отменить";
    }
}
