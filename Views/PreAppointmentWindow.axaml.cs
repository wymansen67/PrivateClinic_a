using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaPrivateClinic.Views;

public partial class PreAppointmentWindow : Window
{
    public PreAppointmentWindow(Specialist? specialist, User user)
    {
        InitializeComponent();
        ViewModel = new PreAppointmentViewModel(specialist, user, this);
        DataContext = ViewModel;

        PrepareInterface();
        Task.Run(LoadAllDataAsync);
    }

    private PreAppointmentViewModel ViewModel { get; }

    private void PrepareInterface()
    {
        if (ViewModel.User.IsSuperuser)
        {
            CbReserve.IsChecked = true; 
            BAddPatient.IsVisible = true;
            CbSpecialists.IsVisible = true;
            SpTime.IsVisible = true;
        }

        ViewModel.ClaimedEquipments.CollectionChanged += EquipmentCollectionChanged;

        BInsertEquipment.AddHandler(Gestures.TappedEvent, EquipmentInsert);
        BRemoveEquipment.AddHandler(Gestures.TappedEvent, EquipmentRemove);
    }

    private async Task LoadAllDataAsync()
    {
        Task[] tasks;

        if (ViewModel.User.IsSuperuser)
            tasks =
            [
                ViewModel.UpdatePatientsAsync(),
                ViewModel.UpdateOfficesAsync(),
                ViewModel.UpdatePurposesAsync(),
                ViewModel.UpdateEquipmentsAsync(),
                ViewModel.UpdateSpecialistsAsync()
            ];
        else
            tasks =
            [
                ViewModel.UpdatePatientsAsync(),
                ViewModel.UpdateOfficesAsync(),
                ViewModel.UpdatePurposesAsync(),
                ViewModel.UpdateEquipmentsAsync()
            ];

        try
        {
            await Task.WhenAll(tasks);
            
            ViewModel.LoadPlannedAppointmentsAsync().GetAwaiter().GetResult();
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                CbPatients.SelectedIndex = 0;
                CbOffices.SelectedIndex = 0;
                CbPurposes.SelectedIndex = 0;
                CbSpecialists.SelectedIndex = 0;
                CbEquipment.SelectedIndex = 0;
            });
        }
        catch
        {
        }
    }

    private void CbPatients_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var patient = (Patient?)CbPatients.SelectedItem;

        if (patient != null)
        {
            var gender = patient.Gender == 'm' ? "Мужской" : "Женский";
            var hasInsurance = patient.InsuranceId != 0;
            var insurance = hasInsurance && patient.InsuranceId.ToString().Length >= 8
                ? patient.InsuranceId.ToString()
                : "Страховка отсутствует";

            var patientInfo = $"Имя: {patient.FirstName}\n" +
                              $"Фамилия: {patient.LastName}\n" +
                              $"Отчество: {patient.MiddleName}\n" +
                              $"Дата рождения: {patient.Birthday.ToShortDateString()}\n" +
                              $"Пол: {gender}\n" +
                              $"Номер страховки: {insurance}\n" +
                              $"Номер телефона: {patient.Phone}\n" +
                              $"Адрес: {patient.Address}";

            PatientInfo.Text = patientInfo;
        }
        else
        {
            PatientInfo.Text = "No patient selected.";
        }
    }

    private void EquipmentCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        GetOfficeForEquipment();
    }

    private void GetOfficeForEquipment()
    {
        if (EquipmentListBox.Items.Count > 0)
        {
            var selectedEquipmentIds = ViewModel.ClaimedEquipments.Select(e => e.EquipmentId).ToList();

            if (!selectedEquipmentIds.Any())
            {
                CbOffices.ItemsSource = null;
                return;
            }

            using (var dbContext = new ClinicDbContext())
            {
                var offices = dbContext.Offices
                    .Include(o => o.TypeNavigation)
                    .Include(o => o.Equipment)
                    .ToList()
                    .Where(o => selectedEquipmentIds.All(eqId => o.Equipment.Any(e => e.EquipmentId == eqId)))
                    .ToList();

                CbOffices.ItemsSource = offices;
                CbOffices.SelectedIndex = offices.Any() ? 0 : -1;
            }
        }
    }

    private void EquipmentInsert(object? sender, TappedEventArgs e)
    {
        if (CbEquipment.SelectedItem is Equipment selectedEquipment)
        {
            ViewModel.ClaimedEquipments.Add(selectedEquipment);

            if (!ViewModel._equipmentPositions.ContainsKey(selectedEquipment))
                ViewModel._equipmentPositions[selectedEquipment] =
                    ViewModel.AvailableEquipments.IndexOf(selectedEquipment);

            ViewModel.AvailableEquipments.Remove(selectedEquipment);
            CbEquipment.SelectedIndex = 0;

            if (ViewModel.ClaimedEquipments.Count > 0) BRemoveEquipment.IsVisible = true;
        }
    }

    private void EquipmentRemove(object? sender, TappedEventArgs e)
    {
        if (EquipmentListBox.SelectedItem is Equipment selectedEquipment)
        {
            ViewModel.ClaimedEquipments.Remove(selectedEquipment);

            if (ViewModel._equipmentPositions.TryGetValue(selectedEquipment, out var index))
            {
                index = Math.Min(index, ViewModel.AvailableEquipments.Count);
                ViewModel.AvailableEquipments.Insert(index, selectedEquipment);
            }
            else
            {
                ViewModel.AvailableEquipments.Add(selectedEquipment);
            }

            CbEquipment.SelectedIndex = ViewModel.AvailableEquipments.Count > 0 ? 0 : -1;

            if (ViewModel.ClaimedEquipments.Count == 0)
            {
                BRemoveEquipment.IsVisible = false;
                Task.Run(ViewModel.UpdateOfficesAsync);
            }
        }
    }

    private void CbDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        /*if (cbDates.SelectedItem is DateTime selectedDate)
        {
            List<string> hours = Enumerable.Range(9, 20).Select(i => i.ToString("D2")).ToList();
            List<string> minutes = Enumerable.Range(0, 60).Select(i => i.ToString("D2")).ToList();

            spTime.IsVisible = Visibility.Visible;
            cbHours.ItemsSource = hours;
            cbHours.SelectedIndex = 0;
            cbMinutes.ItemsSource = minutes;
            cbMinutes.SelectedIndex = 0;
        }*/
    }

    private void CbReserve_OnIsCheckedChangedChanged(object? sender, RoutedEventArgs e)
    {
        if ((bool)CbReserve.IsChecked!)
        {
            SpPlanning.IsVisible = true;
            if (!ViewModel.User.IsSuperuser) CbSpecialists.IsVisible = false;
            BStartAppointment.Content = "Запланировать приём";
        }
        else
        {
            SpPlanning.IsVisible = false;
            BStartAppointment.Content = "Начать приём";
        }
    }
}