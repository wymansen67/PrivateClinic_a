using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using AvaloniaPrivateClinic.Models;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaPrivateClinic.Views;

public partial class ChooseAppointmentDialog : Window
{
    public ChooseAppointmentDialog(List<Appointment> appointments)
    {
        InitializeComponent();
        Appointments = new ObservableCollection<Appointment>(appointments);
        DataContext = this;
    }

    public ObservableCollection<Appointment>? Appointments { get; set; }
    public Appointment? SelectedAppointment { get; set; }

    [RelayCommand]
    private void Confirm()
    {
        if (SelectedAppointment != null)
        {
            var result = SelectedAppointment.AppointmentNumber;
            Close(result);
        }
    }
}