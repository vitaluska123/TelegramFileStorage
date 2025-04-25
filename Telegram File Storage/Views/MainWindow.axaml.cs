using System;
using Avalonia.Controls;
using TelegramFileStorage.ViewModels;

namespace TelegramFileStorage.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += MainWindow_DataContextChanged;
    }

    private void MainWindow_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is TelegramFileStorage.ViewModels.MainWindowViewModel vm)
        {
            vm.OpenSettingsRequested -= OpenSettingsWindow;
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