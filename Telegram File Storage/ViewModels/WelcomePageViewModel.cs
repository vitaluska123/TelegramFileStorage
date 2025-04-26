using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace TelegramFileStorage.ViewModels
{
    public class WelcomeSlide
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? ImagePath { get; set; }
        public bool ShowSkip { get; set; }
        public bool ShowAgreement { get; set; }
        // Можно добавить другие свойства для кнопок, ссылок и т.д.
    }

    public class WelcomePageViewModel : ViewModelBase
    {
        public ObservableCollection<WelcomeSlide> Slides { get; } = new()
        {
            new WelcomeSlide { Title = "Добро пожаловать в Telegram File Storage!", Text = "Программа для хранения файлов в Telegram через бота и канал.", ShowSkip = true },
            new WelcomeSlide { Title = "Пользовательское соглашение", Text = "Пожалуйста, ознакомьтесь с условиями использования.", ShowAgreement = true },
            new WelcomeSlide { Title = "Вход / Регистрация", Text = "Войдите, зарегистрируйтесь или продолжите без синхронизации.", ShowSkip = true },
            new WelcomeSlide { Title = "Ввод токена бота", Text = "Введите токен вашего Telegram-бота.", ShowSkip = true },
            new WelcomeSlide { Title = "Ввод ID канала", Text = "Введите ID вашего Telegram-канала (например, -100XXXXXXXXXX).", ShowSkip = true },
            new WelcomeSlide { Title = "Как пригласить бота в канал", Text = "Добавьте бота в канал и дайте ему права администратора.", ShowSkip = true },
            new WelcomeSlide { Title = "Краткая инструкция", Text = "Теперь вы готовы пользоваться приложением!", ShowSkip = true }
        };
        private int _welcomeIndex = 0;
        public int WelcomeIndex
        {
            get => _welcomeIndex;
            set { _welcomeIndex = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentSlide)); }
        }
        public WelcomeSlide? CurrentSlide => Slides.Count > 0 && WelcomeIndex >= 0 && WelcomeIndex < Slides.Count ? Slides[WelcomeIndex] : null;
        public ICommand NextWelcomeCommand => new RelayCommand(NextWelcome);
        public ICommand PrevWelcomeCommand => new RelayCommand(PrevWelcome);
        public ICommand SkipWelcomeCommand => new RelayCommand(SkipWelcome);
        public event Action? WelcomeFinished;
        private void NextWelcome()
        {
            if (WelcomeIndex < Slides.Count - 1)
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
