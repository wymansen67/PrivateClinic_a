using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaPrivateClinic.ViewModels;

namespace AvaloniaPrivateClinic.Views;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();

        DataContext = new LoginViewModel();

        var uri = new Uri("avares://AvaloniaPrivateClinic/Assets/logo.png");
        var bitmap = new Bitmap(AssetLoader.Open(uri));
        ILogo.Source = bitmap;
    }
}