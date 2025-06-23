using Avalonia.Controls;
using AvaloniaPrivateClinic.ViewModels;

namespace AvaloniaPrivateClinic.Views;

public partial class AppointmentViewWindow : Window
{
    public AppointmentViewWindow(int appointmentNumber)
    {
        InitializeComponent();
        AppointmentViewViewModel viewModel = new(appointmentNumber);
        DataContext = viewModel;

        viewModel.SuccessAction = Close;
    }
}