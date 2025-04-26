using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Threading;

namespace TelegramFileStorage.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        private NotificationModel? _notification;
        private CancellationTokenSource? _autoHideCts;
        public NotificationModel? Notification
        {
            get => _notification;
            set { _notification = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsVisible)); }
        }

        public bool IsVisible => Notification != null && Notification.Show;

        public ICommand? AcceptCommand { get; set; }
        public ICommand? CancelCommand { get; set; }

        public void Show(NotificationModel model, Action? onAccept = null, Action? onCancel = null)
        {
            Notification = model;
            if (model.Type == NotificationType.Accept)
            {
                AcceptCommand = new RelayCommand(() => { onAccept?.Invoke(); Hide(); });
                CancelCommand = new RelayCommand(() => { onCancel?.Invoke(); Hide(); });
                OnPropertyChanged(nameof(AcceptCommand));
                OnPropertyChanged(nameof(CancelCommand));
            }
            // Автоскрытие для всех, кроме Loading и Accept
            if (model.Type != NotificationType.Loading && model.Type != NotificationType.Accept)
            {
                _autoHideCts?.Cancel();
                _autoHideCts = new CancellationTokenSource();
                var token = _autoHideCts.Token;
                Task.Run(async () => {
                    try {
                        await Task.Delay(3000, token);
                        if (!token.IsCancellationRequested)
                            Hide();
                    } catch (TaskCanceledException) { }
                });
            }
        }
        public void Hide()
        {
            Notification = null;
            _autoHideCts?.Cancel();
        }
    }
}
