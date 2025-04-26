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
        // Удаляем подписку на OpenSettingsRequested, так как теперь настройки открываются только как страница
    }
}