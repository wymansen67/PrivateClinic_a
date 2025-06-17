using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaPrivateClinic.Converters;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.ViewModels;

namespace AvaloniaPrivateClinic.Views;

public partial class MainWindow : Window
{
    public MainWindow(Specialist? specialist, User user)
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel(specialist, user, this);
        DataContext = ViewModel;

        Opened += OnWindowOpened;
    }

    private MainWindowViewModel ViewModel { get; }

    private void OnWindowOpened(object? sender, EventArgs e)
    {
        Task.Run(ViewModel.LoadDataAsync);
    }
}