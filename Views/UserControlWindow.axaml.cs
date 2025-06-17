using System;
using Avalonia.Controls;

namespace AvaloniaPrivateClinic.Views;

public partial class UserControlWindow : Window
{
    public UserControlWindow()
    {
        InitializeComponent();
    }
    
    private void UserControlWindow_Closed(object sender, EventArgs e)
    {
        Close(true);
    }
}