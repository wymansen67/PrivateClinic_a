using Avalonia.Controls;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.ViewModels;

namespace AvaloniaPrivateClinic.Views;

public partial class AppointmentWindow : Window
{
    public AppointmentWindow(Specialist specialist, Patient? patient, Office? office, User user,
        Appointment? appointment)
    {
        InitializeComponent();
        ViewModel = appointment == null
            ? new AppointmentViewModel(specialist, patient!, office!, user, this)
            : new AppointmentViewModel(specialist, appointment!, user, this);
        ViewModel.WindowHeight = Height;
        DataContext = ViewModel;
    }

    private AppointmentViewModel ViewModel { get; }
}