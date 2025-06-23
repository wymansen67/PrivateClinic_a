using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using AvaloniaPrivateClinic.Encryption;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace AvaloniaPrivateClinic.ViewModels;

public partial class PreAppointmentViewModel : ViewModelBase
{
    // ──────── FIELDS ────────
    private readonly ClinicDbContext _context = new();
    private PreAppointmentWindow Window { get; }
    private Specialist? Specialist { get; }
    internal User User { get; set; }
    internal Dictionary<Equipment, int> _equipmentPositions = new();

    // ──────── OBSERVABLE PROPERTIES ────────
    [ObservableProperty] private ObservableCollection<Equipment> _availableEquipments = new();
    [ObservableProperty] private ObservableCollection<Equipment> _claimedEquipments = new();
    [ObservableProperty] private ObservableCollection<Patient> _patients = new();
    [ObservableProperty] private ObservableCollection<Patient> _filteredPatients = new();
    [ObservableProperty] private ObservableCollection<Office> _offices = new();
    [ObservableProperty] private ObservableCollection<Purpose> _purposes = new();
    [ObservableProperty] private ObservableCollection<Specialist> _specialists = new();
    [ObservableProperty] private ObservableCollection<TimeOnly> _availableTimes = new();

    [ObservableProperty] private bool _patientOperations;
    [ObservableProperty] private bool _isReadyToReserve;

    [ObservableProperty] private string _searchString = string.Empty;

    [ObservableProperty] private Office _selectedOffice = new();
    [ObservableProperty] private Patient _selectedPatient = new();
    [ObservableProperty] private Purpose? _selectedPurpose;
    [ObservableProperty] private Specialist? _selectedSpecialist;
    [ObservableProperty] private DateOnly? _selectedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
    [ObservableProperty] private TimeOnly? _selectedTime;

    [ObservableProperty] private int _selectedIndexPatient = 0;
    [ObservableProperty] private int _selectedIndexTime = 0;

    public DateTime? SelectedDateTime
    {
        get => SelectedDate.HasValue ? SelectedDate.Value.ToDateTime(TimeOnly.MinValue) : null;
        set => SelectedDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }
    
    public DateTime MinSelectableDate => DateTime.Today.AddDays(1);
    public DateTime MaxSelectableDate => DateTime.Today.AddMonths(1);
    
    [ObservableProperty] private ObservableCollection<Appointment> _plannedAppointments = new();

    [ObservableProperty] private Appointment? _selectedPlannedAppointment;

    [ObservableProperty] private bool _isUsingPlannedAppointment;


    // ──────── COMMANDS ────────
    public ICommand BackCommand { get; }

    [RelayCommand] public async Task AddPatient() => await OpenPatientWindowAsync(new UserControlWindow());
    [RelayCommand] public async Task EditPatient() => await OpenPatientWindowAsync(new UserControlWindow(SelectedPatient));
    [RelayCommand] public void SearchPatient() => ApplySearchFilter();
    [RelayCommand] public async Task ReserveAppointment() => await TryReserveAppointmentAsync();

    // ──────── CONSTRUCTOR ────────
    public PreAppointmentViewModel(Specialist? specialist, User user, PreAppointmentWindow window)
    {
        Specialist = specialist;
        User = user;
        Window = window;

        if (user.IsSuperuser)
            PatientOperations = true;

        ClaimedEquipments = new ObservableCollection<Equipment>();
        BackCommand = new RelayCommand(Back);

        _ = UpdateAvailableTimesAsync();
    }

    // ──────── ONCHANGED PARTIALS ────────
    partial void OnSearchStringChanged(string value) => SearchPatient();
    
    partial void OnSelectedDateChanged(DateOnly? value)
    {
        _ = UpdateAvailableTimesAsync();
        ValidateAppointmentReady();
    }

    partial void OnSelectedSpecialistChanged(Specialist? value)
    {
        _ = UpdateAvailableTimesAsync();
    }

    // ──────── PUBLIC METHODS ────────
    public void Start() => StartAppointment();
    public void Back() => OpenPreviousWindow();

    // ──────── PRIVATE METHODS ────────
    private async Task OpenPatientWindowAsync(UserControlWindow window)
    {
        var result = await window.ShowDialog<bool?>(Window);
        if (result == true) LoadPatients();
    }

    private void ValidateAppointmentReady()
    {
        IsReadyToReserve = SelectedDate is not null && SelectedTime is not null;
    }

    private void ApplySearchFilter()
    {
        if (string.IsNullOrWhiteSpace(SearchString))
        {
            FilteredPatients = new ObservableCollection<Patient>(Patients);
            return;
        }

        var query = SearchString.Trim().ToLowerInvariant();
        var filtered = Patients
            .Where(p => $"{p.LastName} {p.FirstName} {p.MiddleName}".ToLowerInvariant().Contains(query))
            .ToList();

        FilteredPatients = new ObservableCollection<Patient>(filtered);
        SelectedIndexPatient = 0;
    }

    private void LoadPatients()
    {
        Patients = new ObservableCollection<Patient>(
            _context.Patients.ToList().Select(p =>
            {
                p.FirstName = Cryptography.Decrypt(p.FirstName)!;
                p.MiddleName = Cryptography.Decrypt(p.MiddleName)!;
                p.LastName = Cryptography.Decrypt(p.LastName)!;
                p.Address = Cryptography.Decrypt(p.Address)!;
                return p;
            }).OrderBy(p => p.LastName).ToList()
        );

        FilteredPatients = new ObservableCollection<Patient>(Patients);
    }

    private List<TimeOnly> GenerateAllTimeSlots()
    {
        var result = new List<TimeOnly>();
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(18, 0);

        for (var time = start; time < end; time = time.AddMinutes(15))
        {
            result.Add(time);
            Debug.WriteLine($">>> Generated slot: {time}");
        }

        return result;
    }

    private async Task<List<TimeOnly>> GetBusyTimesAsync(DateOnly date)
    {
        var appointments = await _context.Appointments
            .Where(a => a.Date == date && a.Specialist == SelectedSpecialist.Id && a.Time != null)
            .ToListAsync();

        return appointments
            .Where(a => a.Time.HasValue)
            .Select(a => a.Time.Value)
            .Distinct()
            .ToList();
    }

    private async Task UpdateAvailableTimesAsync()
    {
        Debug.WriteLine(">>> UpdateAvailableTimesAsync called");

        if (SelectedDate is null || (Specialist?.Id ?? SelectedSpecialist?.Id) is null)
        {
            Debug.WriteLine(">>> Missing date or specialist ID");
            return;
        }

        var busyTimes = await GetBusyTimesAsync(SelectedDate.Value);
        var allTimes = GenerateAllTimeSlots();

        var freeTimes = Enumerable
            .Except(allTimes, busyTimes, EqualityComparer<TimeOnly>.Default)
            .ToList();

        Debug.WriteLine($">>> Found {freeTimes.Count} free time slots");

        AvailableTimes = new ObservableCollection<TimeOnly>(freeTimes);
    }
    
    public async Task LoadPlannedAppointmentsAsync()
    {
        var appointments = _context.Appointments
            .Include(a => a.PatientNavigation)
            .Include(a => a.PurposeNavigation)
            .Include(a => a.SpecialistNavigation)
            .Where(a => a.Specialist == Specialist!.Id && a.IsPlanned == true && a.Date == DateOnly.FromDateTime(DateTime.Today))
            .ToList()
            .Select(a =>
            {
                var firstName = Cryptography.Decrypt(a.PatientNavigation.FirstName);
                var middleName = Cryptography.Decrypt(a.PatientNavigation.MiddleName);
                var lastName = Cryptography.Decrypt(a.PatientNavigation.LastName);
                var address = Cryptography.Decrypt(a.PatientNavigation.Address);

                a.PatientNavigation.FirstName = firstName != null ? firstName : a.PatientNavigation.FirstName;
                a.PatientNavigation.MiddleName =
                    middleName != null ? middleName : a.PatientNavigation.MiddleName;
                a.PatientNavigation.LastName = lastName != null ? lastName : a.PatientNavigation.LastName;
                a.PatientNavigation.Address = address != null ? address : a.PatientNavigation.Address;

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
            .ToList();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            PlannedAppointments = new ObservableCollection<Appointment>(appointments);
        });
    }

    private void OpenPreviousWindow()
    {
        var mainWindow = new MainWindow(Specialist, User);
        mainWindow.Show();
        Window.Close();
    }

    private void StartAppointment()
    {
        AppointmentWindow appointmentWindow;
        if (IsUsingPlannedAppointment && SelectedPlannedAppointment != null)
        {
            appointmentWindow = new AppointmentWindow(
                Specialist,
                SelectedPlannedAppointment.PatientNavigation,
                SelectedOffice,
                User,
                SelectedPlannedAppointment);

            appointmentWindow.Show();
            Window.Close();
            return;
        }

        
        appointmentWindow = new AppointmentWindow(Specialist, SelectedPatient, SelectedOffice, User, null);
        appointmentWindow.Show();
        Window.Close();
    }

    private async Task TryReserveAppointmentAsync()
    {
        var exists = await _context.Appointments.AnyAsync(a =>
            a.Date == SelectedDate &&
            a.Time == SelectedTime &&
            a.Specialist == (Specialist != null ? Specialist.Id : SelectedSpecialist!.Id));

        if (exists)
        {
            Debug.WriteLine("Приём на это время уже запланирован!");
            return;
        }

        if (SelectedPatient == null || SelectedOffice == null || SelectedDate == null || SelectedTime == null)
        {
            await MessageBoxManager.GetMessageBoxStandard("Info", "Не удалось сформировать запись. Проверьте выбранную дату и время").ShowAsync();
            return;
        }
           

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var discount = (short)(SelectedPatient.InsuranceId > 0 ? 10 : 0);

            var appointment = new Appointment
            {
                Date = SelectedDate.Value,
                Time = SelectedTime.Value,
                Patient = SelectedPatient.PatientId,
                Office = SelectedOffice.OfficeId,
                Specialist = Specialist?.Id ?? SelectedSpecialist?.Id ?? throw new InvalidOperationException("No specialist selected"),
                Discount = discount,
                Purpose = SelectedPurpose?.PurposeId ?? throw new InvalidOperationException("No purpose selected"),
                Commentaries = string.Empty,
                IsPlanned = true,
                TotalPrice = 0
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            await MessageBoxManager.GetMessageBoxStandard("Info", "Запись успешно сформирована").ShowAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Debug.WriteLine($"Ошибка при планировании приёма: {ex}");
            throw;
        }
    }

    // ──────── INTERNAL METHODS ────────
    internal async Task UpdatePatientsAsync()
    {
        var patientsList = await new ClinicDbContext().Patients.ToListAsync();

        patientsList = patientsList.Select(p =>
        {
            p.FirstName = Cryptography.Decrypt(p.FirstName)!;
            p.MiddleName = Cryptography.Decrypt(p.MiddleName)!;
            p.LastName = Cryptography.Decrypt(p.LastName)!;
            p.Address = Cryptography.Decrypt(p.Address)!;
            return p;
        }).ToList();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            Patients = new ObservableCollection<Patient>(patientsList);
            FilteredPatients = new ObservableCollection<Patient>(Patients);
        });
    }

    internal async Task UpdateOfficesAsync()
    {
        var officesList = await new ClinicDbContext().Offices
            .Include(o => o.TypeNavigation)
            .ToListAsync();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            Offices = new ObservableCollection<Office>(officesList);
        });
    }

    internal async Task UpdateSpecialistsAsync()
    {
        var specialistsList = await new ClinicDbContext().Specialists
            .Include(s => s.SpecializationNavigation)
            .ToListAsync();

        specialistsList = specialistsList.Select(s =>
        {
            s.FirstName = Cryptography.Decrypt(s.FirstName)!;
            s.MiddleName = Cryptography.Decrypt(s.MiddleName)!;
            s.LastName = Cryptography.Decrypt(s.LastName)!;
            return s;
        }).ToList();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            Specialists = new ObservableCollection<Specialist>(specialistsList);
        });
    }

    internal async Task UpdatePurposesAsync()
    {
        var purposesList = await new ClinicDbContext().Purposes.ToListAsync();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            Purposes = new ObservableCollection<Purpose>(purposesList);
        });
    }

    internal async Task UpdateEquipmentsAsync()
    {
        var equipmentsList = await new ClinicDbContext().Equipments.ToListAsync();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            AvailableEquipments = new ObservableCollection<Equipment>(equipmentsList);

            _equipmentPositions.Clear();
            for (var i = 0; i < AvailableEquipments.Count; i++)
                _equipmentPositions[AvailableEquipments[i]] = i;
        });
    }
}
