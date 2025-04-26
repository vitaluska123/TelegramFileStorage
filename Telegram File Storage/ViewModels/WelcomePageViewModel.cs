using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace TelegramFileStorage.ViewModels
{
    public class WelcomePageViewModel : ViewModelBase
    {
        public ObservableCollection<string> WelcomeSlides { get; } = new()
        {
            "Добро пожаловать в Telegram File Storage!",
            "Пользовательское соглашение (заглушка)",
            "Вход / Регистрация / Продолжить без синхронизации (заглушка)",
            "Ввод токена бота (заглушка)",
            "Ввод ID канала (заглушка)",
            "Как пригласить бота в канал (заглушка)",
            "Краткая инструкция (заглушка)"
        };
        private int _welcomeIndex = 0;
        public int WelcomeIndex
        {
            get => _welcomeIndex;
            set { _welcomeIndex = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentWelcomeSlide)); }
        }
        public string CurrentWelcomeSlide => WelcomeSlides.Count > 0 && WelcomeIndex >= 0 && WelcomeIndex < WelcomeSlides.Count ? WelcomeSlides[WelcomeIndex] : "";
        public ICommand NextWelcomeCommand => new RelayCommand(NextWelcome);
        public ICommand PrevWelcomeCommand => new RelayCommand(PrevWelcome);
        public ICommand SkipWelcomeCommand => new RelayCommand(SkipWelcome);
        public event Action? WelcomeFinished;
        private void NextWelcome()
        {
            if (WelcomeIndex < WelcomeSlides.Count - 1)
                WelcomeIndex++;
            else
                WelcomeFinished?.Invoke();
        }
        private void PrevWelcome()
        {
            if (WelcomeIndex > 0)
                WelcomeIndex--;
        }
        private void SkipWelcome()
        {
            WelcomeFinished?.Invoke();
        }
    }
}
