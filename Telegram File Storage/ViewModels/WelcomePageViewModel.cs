using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace TelegramFileStorage.ViewModels
{
    public class InputField : INotifyPropertyChanged
    {
        public string Type { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;
        private string _value = string.Empty;
        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class Slide
    {
        public string Id { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Text { get; set; }
        public List<InputField> InputFields { get; set; } = new();
        public string? NextSlide { get; set; }
        public bool ShowAgreement { get; set; }
        public string? InputPlaceholder { get; set; }
        public bool ShowInput { get; set; }
        public bool ShowAuthButtons { get; set; }
        public bool ShowSkip { get; set; }
        public string? LoginSlideId { get; set; }
        public string? RegisterSlideId { get; set; }
        public string? NoSyncSlideId { get; set; }
        public List<string> Explanations { get; set; } = new();
        public bool IsExplanation { get; set; }
        public string? Type { get; set; }
        public string? Image { get; set; }
        public string? ImagePath => string.IsNullOrWhiteSpace(Image) ? null : $"avares://TelegramFileStorage/Assets/SlidesImages/{Image}";
        public bool ShowFinishButton { get; set; }
    }

    public class WelcomePageViewModel : ViewModelBase
    {
        public ObservableCollection<Slide> Slides { get; } = new();
        private Dictionary<string, Slide> _slideMap = new();
        private readonly Stack<string> _slideHistory = new();
        private int _welcomeIndex = 0;
        public int WelcomeIndex
        {
            get => _welcomeIndex;
            set {
                _welcomeIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentSlide));
                OnPropertyChanged(nameof(CanGoNext));
            }
        }
        public Slide? CurrentSlide => Slides.Count > 0 && WelcomeIndex >= 0 && WelcomeIndex < Slides.Count ? Slides[WelcomeIndex] : null;
        public bool CanGoNext =>
            (CurrentSlide?.Id != "agreement" || AgreementAccepted)
            && (!(CurrentSlide?.InputFields.Count > 0) || AllInputFieldsFilled())
            && !(CurrentSlide?.NextSlide == null)
            && !(CurrentSlide?.IsExplanation == true);
        private bool _agreementAccepted;
        public bool AgreementAccepted
        {
            get => _agreementAccepted;
            set {
                _agreementAccepted = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanGoNext));
            }
        }
        private string? _inputValue;
        public string? InputValue
        {
            get => _inputValue;
            set { _inputValue = value; OnPropertyChanged(); }
        }
        private string? _login;
        public string? Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(); }
        }
        private string? _password;
        public string? Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        // Значения всех InputField по ключу
        public Dictionary<string, string> InputValues { get; } = new();
        public ICommand NextWelcomeCommand => new AsyncRelayCommand(NextWelcomeAsync);
        public ICommand PrevWelcomeCommand => new RelayCommand(PrevWelcome);
        public ICommand SkipWelcomeCommand => new RelayCommand(SkipWelcome);
        public ICommand LoginCommand => new RelayCommand(() => OnAuth("login_start"));
        public ICommand RegisterCommand => new RelayCommand(() => OnAuth("register_start"));
        public ICommand NoSyncCommand => new RelayCommand(() => OnAuth("nosync"));
        public ICommand LoginFinishCommand => new RelayCommand(() => OnAuth("login_finish"));
        public ICommand RegisterFinishCommand => new RelayCommand(() => OnAuth("register_finish"));
        public ICommand ShowExplanationCommand => new RelayCommand(ShowExplanation);
        public new event PropertyChangedEventHandler? PropertyChanged;
        public event Action? WelcomeFinished;

        public WelcomePageViewModel()
        {
            LoadSlidesFromXml();
            Slides.CollectionChanged += (s, e) => SubscribeInputFields();
            SubscribeInputFields();
        }

        private void LoadSlidesFromXml()
        {
            var assembly = typeof(WelcomePageViewModel).Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith("Slides.xml"));
            if (resourceName == null) return;
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) return;
            var doc = XDocument.Load(stream);
            var slides = new List<Slide>();
            var root = doc.Root;
            if (root == null) return;
            foreach (var slideElem in root.Elements("Slide"))
            {
                var slide = new Slide
                {
                    Id = slideElem.Attribute("id")?.Value ?? string.Empty,
                    Title = slideElem.Element("Title")?.Value,
                    Text = slideElem.Element("Text")?.Value,
                    NextSlide = slideElem.Element("NextSlide")?.Value,
                    ShowAgreement = bool.TryParse(slideElem.Element("ShowAgreement")?.Value, out var showAgreement) && showAgreement,
                    ShowInput = bool.TryParse(slideElem.Element("ShowInput")?.Value, out var showInput) && showInput,
                    ShowAuthButtons = bool.TryParse(slideElem.Element("ShowAuthButtons")?.Value, out var showAuthButtons) && showAuthButtons,
                    ShowSkip = bool.TryParse(slideElem.Element("ShowSkip")?.Value, out var showSkip) && showSkip,
                    InputPlaceholder = slideElem.Element("InputPlaceholder")?.Value,
                    LoginSlideId = slideElem.Element("LoginSlideId")?.Value,
                    RegisterSlideId = slideElem.Element("RegisterSlideId")?.Value,
                    NoSyncSlideId = slideElem.Element("NoSyncSlideId")?.Value,
                    IsExplanation = bool.TryParse(slideElem.Element("IsExplanation")?.Value, out var isExpl) && isExpl,
                    Type = slideElem.Attribute("type")?.Value,
                    Image = slideElem.Element("Image")?.Value,
                    ShowFinishButton = bool.TryParse(slideElem.Element("ShowFinishButton")?.Value, out var showFinish) && showFinish,
                };
                var inputFieldsElem = slideElem.Element("InputFields");
                if (inputFieldsElem != null)
                {
                    foreach (var inputElem in inputFieldsElem.Elements("InputField"))
                    {
                        slide.InputFields.Add(new InputField
                        {
                            Type = inputElem.Attribute("type")?.Value ?? "text",
                            Key = inputElem.Attribute("key")?.Value ?? string.Empty,
                            Placeholder = inputElem.Attribute("placeholder")?.Value ?? string.Empty
                        });
                    }
                }
                var explanationsElem = slideElem.Element("Explanations");
                if (explanationsElem != null)
                {
                    foreach (var explIdElem in explanationsElem.Elements("ExplanationSlideId"))
                    {
                        if (!string.IsNullOrWhiteSpace(explIdElem.Value))
                            slide.Explanations.Add(explIdElem.Value.Trim());
                    }
                }
                slides.Add(slide);
                _slideMap[slide.Id] = slide;
            }
            Slides.Clear();
            foreach (var s in slides)
                Slides.Add(s);
        }

        private void SubscribeInputFields()
        {
            foreach (var slide in Slides)
            {
                foreach (var field in slide.InputFields)
                {
                    field.PropertyChanged -= InputField_PropertyChanged;
                    field.PropertyChanged += InputField_PropertyChanged;
                }
            }
        }

        private void InputField_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InputField.Value))
            {
                OnPropertyChanged(nameof(CanGoNext));
            }
        }

        private void SyncInputFieldValuesToDictionary()
        {
            if (CurrentSlide?.InputFields != null)
            {
                foreach (var field in CurrentSlide.InputFields)
                {
                    InputValues[field.Key] = field.Value;
                }
            }
        }

        private async System.Threading.Tasks.Task NextWelcomeAsync()
        {
            ErrorMessage = null;
            SyncInputFieldValuesToDictionary();
            if (CurrentSlide != null)
                _slideHistory.Push(CurrentSlide.Id);

            // Авторизация
            if (CurrentSlide?.Id == "login")
            {
                var login = InputValues.TryGetValue("Login", out var l) ? l : null;
                var password = InputValues.TryGetValue("Password", out var p) ? p : null;
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    ErrorMessage = "Введите логин и пароль";
                    ShowNotification(new NotificationModel { Type = NotificationType.Error, Message = "Введите логин и пароль" });
                    return;
                }
                bool success = await AuthorizeAsync(login, password);
                if (!success)
                {
                    ErrorMessage = "Неверный логин или пароль";
                    ShowNotification(new NotificationModel { Type = NotificationType.Error, Message = "Неверный логин или пароль" });
                    return;
                }
                else
                {
                    ShowNotification(new NotificationModel { Type = NotificationType.Success, Message = "Успешная авторизация!" });
                }
            }
            // Сохранение токена бота
            if (CurrentSlide?.Id == "bot_token")
            {
                var token = InputValues.TryGetValue("BotToken", out var t) ? t : null;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    SettingsController.Current.TelegramToken = token;
                    SettingsController.Save();
                }
            }
            // Сохранение ChannelId
            if (CurrentSlide?.Id == "channel_id")
            {
                var channelId = InputValues.TryGetValue("ChannelId", out var c) ? c : null;
                if (!string.IsNullOrWhiteSpace(channelId))
                {
                    SettingsController.Current.ChannelId = channelId;
                    SettingsController.Save();
                }
            }
            var nextId = CurrentSlide?.NextSlide;
            if (!string.IsNullOrEmpty(nextId) && _slideMap.TryGetValue(nextId, out var nextSlide))
            {
                WelcomeIndex = Slides.IndexOf(nextSlide);
            }
            else if (WelcomeIndex < Slides.Count - 1)
            {
                WelcomeIndex++;
            }
            else
            {
                WelcomeFinished?.Invoke();
            }
        }

        private async System.Threading.Tasks.Task<bool> AuthorizeAsync(string login, string password)
        {
            return await ApiController.AuthorizeAsync(login, password);
        }

        private void PrevWelcome()
        {
            if (_slideHistory.Count > 0)
            {
                var prevId = _slideHistory.Pop();
                if (_slideMap.TryGetValue(prevId, out var prevSlide))
                    WelcomeIndex = Slides.IndexOf(prevSlide);
            }
        }
        private void SkipWelcome()
        {
            WelcomeFinished?.Invoke();
        }
        private void OnAuth(string type)
        {
            if (CurrentSlide != null)
                _slideHistory.Push(CurrentSlide.Id);
            string? nextId = null;
            if (type == "login_start")
                nextId = CurrentSlide?.GetType().GetProperty("LoginSlideId")?.GetValue(CurrentSlide) as string ?? (CurrentSlide as dynamic)?.LoginSlideId;
            else if (type == "register_start")
                nextId = CurrentSlide?.GetType().GetProperty("RegisterSlideId")?.GetValue(CurrentSlide) as string ?? (CurrentSlide as dynamic)?.RegisterSlideId;
            else if (type == "nosync")
                nextId = CurrentSlide?.GetType().GetProperty("NoSyncSlideId")?.GetValue(CurrentSlide) as string ?? (CurrentSlide as dynamic)?.NoSyncSlideId;
            else
                nextId = CurrentSlide?.NextSlide;
            if (!string.IsNullOrEmpty(nextId) && _slideMap.TryGetValue(nextId, out var nextSlide))
            {
                WelcomeIndex = Slides.IndexOf(nextSlide);
            }
        }
        private void ShowExplanation()
        {
            var explElem = CurrentSlide?.Id != null
                ? CurrentSlide?.GetType().GetProperty("Explanations")?.GetValue(CurrentSlide)
                : null;
            string? explId = null;
            if (CurrentSlide != null)
            {
                // Поиск ExplanationSlideId в XML (через Explanations)
                var slideElem = Slides.FirstOrDefault(s => s.Id == CurrentSlide.Id);
                if (slideElem != null)
                {
                    // В Slides.xml Explanations/ExplanationSlideId
                    var doc = Slides;
                    var expl = CurrentSlide?.Explanations?.FirstOrDefault();
                    if (expl != null)
                        explId = expl;
                }
            }
            // Альтернатива: искать ExplanationSlideId в Explanations property
            if (explId == null && CurrentSlide != null)
            {
                var explProp = CurrentSlide.GetType().GetProperty("ExplanationSlideId");
                if (explProp != null)
                    explId = explProp.GetValue(CurrentSlide) as string;
            }
            // Если нашли id — переходим
            if (!string.IsNullOrEmpty(explId) && _slideMap.TryGetValue(explId, out var explSlide))
            {
                if (CurrentSlide != null)
                _slideHistory.Push(CurrentSlide.Id);
                WelcomeIndex = Slides.IndexOf(explSlide);
            }
        }
        private bool AllInputFieldsFilled()
        {
            if (CurrentSlide?.InputFields == null || CurrentSlide.InputFields.Count == 0)
                return true;
            foreach (var field in CurrentSlide.InputFields)
            {
                if (string.IsNullOrWhiteSpace(field.Value))
                    return false;
            }
            return true;
        }

        // Показывает уведомление через MainWindowViewModel, если он есть в DataContext главного окна
        private void ShowNotification(NotificationModel model)
        {
            var app = Avalonia.Application.Current;
            if (app?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow?.DataContext is MainWindowViewModel mainVM)
                {
                    mainVM.ShowNotification(model);
                }
            }
        }
    }
}
