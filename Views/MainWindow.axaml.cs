using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
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
    }

    private MainWindowViewModel ViewModel { get; }

    private async void InputElement_OnTapped(object? sender, TappedEventArgs e)
    {
        var button = sender as Button;
        if (button?.DataContext is KeyValuePair<DateOnly, List<Appointment>> kvp)
        {
            var appointment = kvp;
            var appointments = appointment.Value;
            if (appointments.Count == 1)
            {
                AppointmentViewWindow aView = new(appointments[0].AppointmentNumber);
                await aView.ShowDialog(this);
            }
            else
            {
                // В вызывающем окне:
                var aChoose = new ChooseAppointmentDialog(appointments);
                var result = await aChoose.ShowDialog<int?>(this); // если допускаешь отмену или null

                if (result != null)
                {
                    var aView = new AppointmentViewWindow(result.Value);
                    await aView.ShowDialog(this);
                }
            }
        }
    }
}