using Avalonia.Controls;
using TelegramFileStorage.ViewModels;
using ViewModels;

namespace TelegramFileStorage.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext ??= new ViewModels.MainWindowViewModel();
        if (DataContext is ViewModels.MainWindowViewModel vm)
        {
            vm.OpenSettingsRequested += OpenSettingsWindow;
        }
    }

    public async void OpenSettingsWindow()
    {
        var settingsWindow = new SettingsWindow
        {
            DataContext = new SettingsViewModel()
        };
        await settingsWindow.ShowDialog(this);
    }
}