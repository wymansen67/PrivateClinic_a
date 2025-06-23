using System;
using Avalonia.Controls;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.ViewModels;

namespace AvaloniaPrivateClinic.Views;

public partial class UserControlWindow : Window
{
    public UserControlWindow()
    {
        InitializeComponent();
        UserControlViewModel viewModel = new();
        DataContext = viewModel;
    }

    public UserControlWindow(Patient patient)
    {
        InitializeComponent();
        UserControlViewModel viewModel = new(patient);
        DataContext = viewModel;
    }

    private void UserControlWindow_Closed(object sender, EventArgs e)
    {
        Close(true);
    }
}