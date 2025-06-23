using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AvaloniaPrivateClinic.Encryption;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaPrivateClinic.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ClinicDbContext _context = new();

    [ObservableProperty] private ObservableCollection<AppointmentRow> _appointmentsTable = new();

    [ObservableProperty] private string _loggedInAs = string.Empty;

    public MainWindowViewModel(Specialist? specialist, User user, MainWindow window)
    {
        Specialist = specialist;
        User = user;
        Window = window;
        if (specialist != null)
            LoggedInAs =
                $"Авторизован как:\n{Specialist.LastName} {Specialist.FirstName.Substring(0, 1)}. {Specialist.MiddleName.Substring(0, 1)}.\n{Specialist.SpecializationNavigation.Specialization1}";

        ResetCommand = new RelayCommand(Reset);
        AddCommand = new RelayCommand(Add);

        Initialize();
    }

    private Specialist? Specialist { get; }
    private User User { get; }
    private MainWindow Window { get; }
    public ObservableCollection<Appointment>? Appointments { get; set; }
    public Dictionary<DateOnly, List<Appointment>>? AppointmentDates { get; set; }

    public ObservableCollection<DateOnly>? Dates { get; set; }

    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    private void Reset()
    {
        SearchString = string.Empty;
        SelectedDate = null;
    }


    private void Add()
    {
        CreateAppointment();
    }

    private void Initialize()
    {
        LoadDataAsync();
    }

    public bool LoadDataAsync()
    {
        try
        {
            Appointments = GetAppointments();
            AppointmentDates = GetAppointmentsByDate(Appointments);
            Dates = GetDatesFromAppointments(AppointmentDates);
            AppointmentsTable.Clear();

            foreach (var group in Appointments!
                         .GroupBy(a =>
                             $"{a.PatientNavigation.LastName} {a.PatientNavigation.FirstName} {a.PatientNavigation.MiddleName}"))
            {
                var row = new AppointmentRow
                {
                    Patient = group.Key,
                    AppointmentsByDate = Dates!
                        .Select(d => new { Date = d, List = group.Where(a => a.Date == d).ToList() })
                        .Where(x => x.List.Any()) // ← вот это исключает пустые
                        .ToDictionary(x => x.Date, x => x.List)
                };
                AppointmentsTable.Add(row);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    private void ApplyFilters()
    {
        if (Appointments is null || Dates is null)
            return;

        var filteredAppointments = Appointments.AsEnumerable();

        // Фильтр по ФИО
        if (!string.IsNullOrWhiteSpace(SearchString))
            filteredAppointments = filteredAppointments.Where(a =>
            {
                var fullName =
                    $"{a.PatientNavigation.LastName} {a.PatientNavigation.FirstName} {a.PatientNavigation.MiddleName}";
                return fullName.Contains(SearchString!, StringComparison.OrdinalIgnoreCase);
            });

        // Фильтр по дате
        if (SelectedDate.HasValue)
            filteredAppointments = filteredAppointments
                .Where(a => a.Date == SelectedDate.Value);

        var grouped = filteredAppointments
            .GroupBy(a =>
                $"{a.PatientNavigation.LastName} {a.PatientNavigation.FirstName} {a.PatientNavigation.MiddleName}");

        AppointmentsTable.Clear();

        foreach (var group in grouped)
        {
            var row = new AppointmentRow
            {
                Patient = group.Key,
                AppointmentsByDate = Dates!
                    .Select(d => new { Date = d, List = group.Where(a => a.Date == d).ToList() })
                    .Where(x => x.List.Any())
                    .ToDictionary(x => x.Date, x => x.List)
            };

            AppointmentsTable.Add(row);
        }
    }

    private ObservableCollection<Appointment> GetAppointments()
    {
        if (!User!.IsSuperuser)
        {
            ObservableCollection<Appointment> data = new
            (
                _context.Appointments
                    .Include(a => a.PatientNavigation)
                    .Include(a => a.SpecialistNavigation)
                    .Include(a => a.PurposeNavigation)
                    .Where(a => a.Specialist == Specialist!.Id)
                    .ToList()
                    .Select(a =>
                    {
                        var firstName = Cryptography.Decrypt(a.PatientNavigation.FirstName);
                        var middleName = Cryptography.Decrypt(a.PatientNavigation.MiddleName);
                        var lastName = Cryptography.Decrypt(a.PatientNavigation.LastName);

                        a.PatientNavigation.FirstName = firstName != null ? firstName : a.PatientNavigation.FirstName;
                        a.PatientNavigation.MiddleName =
                            middleName != null ? middleName : a.PatientNavigation.MiddleName;
                        a.PatientNavigation.LastName = lastName != null ? lastName : a.PatientNavigation.LastName;

                        firstName = Cryptography.Decrypt(a.SpecialistNavigation.FirstName);
                        middleName = Cryptography.Decrypt(a.SpecialistNavigation.MiddleName);
                        lastName = Cryptography.Decrypt(a.SpecialistNavigation.LastName);

                        a.SpecialistNavigation.FirstName =
                            firstName != null ? firstName : a.SpecialistNavigation.FirstName;
                        a.SpecialistNavigation.MiddleName =
                            middleName != null ? middleName : a.SpecialistNavigation.MiddleName;
                        a.SpecialistNavigation.LastName = lastName != null ? lastName : a.SpecialistNavigation.LastName;

                        a.Commentaries = Cryptography.Decrypt(a.Commentaries)!;
                        return a;
                    })
                    .ToList()
            );
            return data;
        }
        else
        {
            ObservableCollection<Appointment> data = new
            (
                _context.Appointments
                    .Include(a => a.PatientNavigation)
                    .Include(a => a.SpecialistNavigation)
                    .Include(a => a.PurposeNavigation)
                    .OrderBy(a => a.Date)
                    .ToList()
                    .Select(a =>
                    {
                        var firstName = Cryptography.Decrypt(a.PatientNavigation.FirstName);
                        var middleName = Cryptography.Decrypt(a.PatientNavigation.MiddleName);
                        var lastName = Cryptography.Decrypt(a.PatientNavigation.LastName);

                        a.PatientNavigation.FirstName = firstName != null ? firstName : a.PatientNavigation.FirstName;
                        a.PatientNavigation.MiddleName =
                            middleName != null ? middleName : a.PatientNavigation.MiddleName;
                        a.PatientNavigation.LastName = lastName != null ? lastName : a.PatientNavigation.LastName;

                        firstName = Cryptography.Decrypt(a.SpecialistNavigation.FirstName);
                        middleName = Cryptography.Decrypt(a.SpecialistNavigation.MiddleName);
                        lastName = Cryptography.Decrypt(a.SpecialistNavigation.LastName);

                        a.SpecialistNavigation.FirstName =
                            firstName != null ? firstName : a.SpecialistNavigation.FirstName;
                        a.SpecialistNavigation.MiddleName =
                            middleName != null ? middleName : a.SpecialistNavigation.MiddleName;
                        a.SpecialistNavigation.LastName = lastName != null ? lastName : a.SpecialistNavigation.LastName;

                        a.Commentaries = Cryptography.Decrypt(a.Commentaries)!;
                        return a;
                    }).ToList()
            );

            return data;
        }
    }

    private async Task<ObservableCollection<Appointment>> GetAppointmentsAsync()
    {
        if (!User!.IsSuperuser)
        {
            ObservableCollection<Appointment> data = new
            (
                await _context.Appointments
                    .Include(a => a.PatientNavigation)
                    .Include(a => a.SpecialistNavigation)
                    .Include(a => a.PurposeNavigation)
                    .OrderBy(a => a.Date)
                    .Where(a => a.Specialist == Specialist!.Id)
                    .ToListAsync()
            );

            return data;
        }
        else
        {
            ObservableCollection<Appointment> data = new
            (
                await _context.Appointments
                    .Include(a => a.PatientNavigation)
                    .Include(a => a.SpecialistNavigation)
                    .Include(a => a.PurposeNavigation)
                    .OrderBy(a => a.Date)
                    .ToListAsync()
            );

            return data;
        }
    }

    private Dictionary<DateOnly, List<Appointment>> GetAppointmentsByDate(
        ObservableCollection<Appointment> appointments)
    {
        var data = appointments.GroupBy(a => a.Date)
            .ToDictionary(g => g.Key, g => g.ToList());
        return data;
    }

    private ObservableCollection<DateOnly> GetDatesFromAppointments(
        Dictionary<DateOnly, List<Appointment>> appointmentDates)
    {
        ObservableCollection<DateOnly> data = new
        (
            appointmentDates.Keys
                .OrderBy(d => d)
                .Distinct()
                .ToList()
        );

        return data;
    }

    private void CreateAppointment()
    {
        var preAppointmentWindow = new PreAppointmentWindow(Specialist, User);
        preAppointmentWindow.Show();
        Window.Close();
    }

    public class AppointmentRow
    {
        public string Patient { get; set; } = "";
        public Dictionary<DateOnly, List<Appointment>> AppointmentsByDate { get; set; } = new();
    }

    #region SearchNFiltering

    [ObservableProperty] private string? _searchString;

    partial void OnSearchStringChanged(string? value)
    {
        ApplyFilters();
    }

    [ObservableProperty] private DateOnly? _selectedDate;

    partial void OnSelectedDateChanged(DateOnly? value)
    {
        ApplyFilters();
    }

    #endregion
}