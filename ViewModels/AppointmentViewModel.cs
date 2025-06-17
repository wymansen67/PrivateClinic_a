using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaPrivateClinic.Encryption;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaPrivateClinic.ViewModels;

public partial class AppointmentViewModel : ViewModelBase
{
    private readonly AppointmentWindow _appointmentWindow;
    private readonly ClinicDbContext _context = new();
    private readonly Dictionary<Diagnosis, int> _diagnosisPositions = new();
    private readonly Dictionary<Equipment, int> _equipmentPositions = new();

    private readonly Appointment? _existingAppointment;
    private readonly bool _isEditMode;
    private readonly Dictionary<Service, int> _servicePositions = new();
    private readonly Specialist _specialist;
    private readonly User _user;

    [ObservableProperty] private ObservableCollection<Diagnosis> _availableDiagnoses = new();

    [ObservableProperty] private ObservableCollection<Equipment> _availableEquipments = new();

    [ObservableProperty] private ObservableCollection<Purpose> _availablePurposes = new();

    [ObservableProperty] private ObservableCollection<Service> _availableServices = new();

    [ObservableProperty] private bool _canEdit;

    [ObservableProperty] private bool _canRemoveDiagnosis;

    [ObservableProperty] private bool _canRemoveEquipment;

    [ObservableProperty] private bool _canRemoveService;

    [ObservableProperty] private string _commentaries = string.Empty;

    [ObservableProperty] private string _dateLabel = DateTime.Now.ToString("d");

    [ObservableProperty] private bool _isReadOnly;
    private Office? _office;

    [ObservableProperty] private string? _officeLabel;

    private Patient? _patient;

    [ObservableProperty] private string? _patientInfoText;

    [ObservableProperty] private Diagnosis? _selectedAvailableDiagnosis;

    [ObservableProperty] private Equipment? _selectedAvailableEquipment;

    [ObservableProperty] private Service? _selectedAvailableService;

    [ObservableProperty] private ObservableCollection<Diagnosis> _selectedDiagnoses = new();

    [ObservableProperty] private Diagnosis? _selectedDiagnosisToRemove;

    [ObservableProperty] private ObservableCollection<Equipment> _selectedEquipments = new();

    [ObservableProperty] private Equipment? _selectedEquipmentToRemove;

    [ObservableProperty] private Purpose? _selectedPurpose;

    [ObservableProperty] private ObservableCollection<Service> _selectedServices = new();

    [ObservableProperty] private Service? _selectedServiceToRemove;

    [ObservableProperty] private string _submitButtonText = "Зарегистрировать приём";

    [ObservableProperty] private decimal _totalPrice;

    [ObservableProperty] private string _windowTitle = "Новый приём";

    public AppointmentViewModel(Specialist specialist, Patient patient, Office office, User user,
        AppointmentWindow aWindow)
    {
        _specialist = specialist ?? throw new ArgumentNullException(nameof(specialist));
        _patient = patient ?? throw new ArgumentNullException(nameof(patient));
        _office = office ?? throw new ArgumentNullException(nameof(office));
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _appointmentWindow = aWindow ?? throw new ArgumentNullException(nameof(aWindow));

        _isEditMode = false;
        WindowTitle = $"Новый приём - Кабинет {_office.Number}";
        IsReadOnly = false;
        CanEdit = true;
        SubmitButtonText = "Зарегистрировать приём";

        InitializeEmptyCollections();
        _ = InitializeForNewAsync();
    }

    public AppointmentViewModel(Specialist specialist, Appointment appointment, User user, AppointmentWindow aWindow)
    {
        _specialist = specialist ?? throw new ArgumentNullException(nameof(specialist));
        _existingAppointment = appointment ?? throw new ArgumentNullException(nameof(appointment));
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _appointmentWindow = aWindow ?? throw new ArgumentNullException(nameof(aWindow));

        _isEditMode = true;
        WindowTitle = $"Просмотр/Редактирование приёма №{appointment.AppointmentNumber}";
        IsReadOnly = appointment.IsPlanned != true || _user.IsSuperuser;
        CanEdit = !IsReadOnly;
        SubmitButtonText = CanEdit ? "Внести изменения" : "Просмотр";

        InitializeEmptyCollections();
        _ = InitializeForExistingAsync();
    }


    private void InitializeEmptyCollections()
    {
        AvailablePurposes = new ObservableCollection<Purpose>();
        AvailableDiagnoses = new ObservableCollection<Diagnosis>();
        SelectedDiagnoses = new ObservableCollection<Diagnosis>();
        AvailableServices = new ObservableCollection<Service>();
        SelectedServices = new ObservableCollection<Service>();
        AvailableEquipments = new ObservableCollection<Equipment>();
        SelectedEquipments = new ObservableCollection<Equipment>();

        // Подписки на изменения коллекций
        SelectedServices.CollectionChanged += (_, _) => CanRemoveService = SelectedServices.Any();
        SelectedEquipments.CollectionChanged += (_, _) => CanRemoveEquipment = SelectedEquipments.Any();
        SelectedDiagnoses.CollectionChanged += (_, _) => CanRemoveDiagnosis = SelectedDiagnoses.Any();
    }

    private async Task InitializeForExistingAsync()
    {
        if (_existingAppointment == null) return;
        try
        {
            // Загружаем связанные сущности для _patient и _office
            var loadedPatientTask = _context.Patients.FindAsync(_existingAppointment.Patient);
            var loadedOfficeTask = _context.Offices.FindAsync(_existingAppointment.Office);
            var loadedSpecialistTask = _context.Specialists.FindAsync(_existingAppointment.Specialist);

            await Task.WhenAll(loadedPatientTask.AsTask(), loadedOfficeTask.AsTask(), loadedSpecialistTask.AsTask());

            _patient = await loadedPatientTask;
            _office = await loadedOfficeTask;
            var loadedSpecialist = await loadedSpecialistTask;

            if (_patient == null || _office == null)
            {
                Debug.WriteLine("Ошибка: Не удалось загрузить пациента или кабинет для существующего приёма.");
                return;
            }

            var displaySpecialist = loadedSpecialist ?? _specialist;

            // Общая логика загрузки списков и информации
            await LoadCommonDataAsync(displaySpecialist);

            // Загрузка данных конкретного приёма (услуг, диагнозов и т.д.)
            LoadAppointmentData(); // Этот метод должен быть синхронным или обработан отдельно
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Критическая ошибка при инициализации существующего приёма: {ex}");
        }
    }

    private async Task InitializeForNewAsync()
    {
        if (_patient == null || _office == null)
        {
            Debug.WriteLine("Ошибка: Пациент или Кабинет не инициализирован для нового приёма.");
            // TODO: Обработать ошибку
            return;
        }

        try
        {
            await LoadCommonDataAsync(_specialist);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Критическая ошибка при инициализации нового приёма: {ex}");
        }
    }

    private async Task LoadCommonDataAsync(Specialist specialistToDisplay)
    {
        if (_patient == null || _office == null) return;

        specialistToDisplay.FirstName = Cryptography.Decrypt(_specialist.FirstName);
        specialistToDisplay.MiddleName = Cryptography.Decrypt(_specialist.MiddleName);
        specialistToDisplay.LastName = Cryptography.Decrypt(_specialist.LastName);
        
        OfficeLabel = _office.Number;
        LoadPatientInfo(_patient, specialistToDisplay);

        var officeTask = LoadOfficeDataAsync(_office);
        var serviceTask = LoadServiceDataAsync(specialistToDisplay);
        var diagnosisTask = LoadDiagnosisDataAsync();
        var purposeTask = LoadPurposeDataAsync();

        // Ожидаем завершения всех загрузок
        await Task.WhenAll(officeTask, serviceTask, diagnosisTask, purposeTask);

        SelectedAvailableService = AvailableServices.FirstOrDefault();
        SelectedAvailableEquipment = AvailableEquipments.FirstOrDefault();
        SelectedAvailableDiagnosis = AvailableDiagnoses.FirstOrDefault();
        SelectedPurpose = AvailablePurposes.FirstOrDefault();
    }

    private void LoadAppointmentData()
    {
        if (_existingAppointment == null) return;

        SelectedPurpose = AvailablePurposes.FirstOrDefault(p => p.PurposeId == _existingAppointment.Purpose);
        Commentaries = _existingAppointment.Commentaries ?? string.Empty;
        TotalPrice = _existingAppointment.TotalPrice;

        using var dbContext = new ClinicDbContext();

        var appointmentServices = dbContext.Appointments
            .Where(a => a.AppointmentNumber == _existingAppointment.AppointmentNumber)
            .SelectMany(a => a.Services)
            .ToList();

        foreach (var service in appointmentServices)
        {
            var serviceToAdd = AvailableServices.FirstOrDefault(s => s.Id == service.Id);
            if (serviceToAdd != null && !SelectedServices.Contains(serviceToAdd)) // Добавлена проверка на дубликат
            {
                SelectedServices.Add(serviceToAdd);
                AvailableServices.Remove(serviceToAdd);
            }
        }

        var appointmentEquipments = dbContext.Appointments
            .Where(a => a.AppointmentNumber == _existingAppointment.AppointmentNumber)
            .SelectMany(a => a.Equipment)
            .ToList();

        foreach (var equipment in appointmentEquipments)
        {
            var equipmentToAdd = AvailableEquipments.FirstOrDefault(eq => eq.EquipmentId == equipment.EquipmentId);
            if (equipmentToAdd != null && !SelectedEquipments.Contains(equipmentToAdd))
            {
                SelectedEquipments.Add(equipmentToAdd);
                AvailableEquipments.Remove(equipmentToAdd);
            }
        }

        var appointmentDiagnoses = dbContext.Appointments
            .Where(a => a.AppointmentNumber == _existingAppointment.AppointmentNumber)
            .SelectMany(a => a.Diagnoses)
            .ToList();

        foreach (var diagnosis in appointmentDiagnoses)
        {
            var diagnosisToAdd = AvailableDiagnoses.FirstOrDefault(d => d.DiagnosisId == diagnosis.DiagnosisId);
            if (diagnosisToAdd != null && !SelectedDiagnoses.Contains(diagnosisToAdd))
            {
                SelectedDiagnoses.Add(diagnosisToAdd);
                AvailableDiagnoses.Remove(diagnosisToAdd);
            }
        }
    }

    private async Task LoadOfficeDataAsync(Office office)
    {
        ClinicDbContext asyncContext = new();
        var officeWithEquipment = await asyncContext.Offices
            .Include(o => o.Equipment)
            .FirstOrDefaultAsync(o => o.OfficeId == office.OfficeId);

        if (officeWithEquipment?.Equipment != null)
        {
            var allEquipment = officeWithEquipment.Equipment.ToList();
            AvailableEquipments = new ObservableCollection<Equipment>(allEquipment);
            _equipmentPositions.Clear();
            for (var i = 0; i < allEquipment.Count; i++) _equipmentPositions[allEquipment[i]] = i;
        }
        else
        {
            AvailableEquipments = new ObservableCollection<Equipment>();
        }

        SelectedAvailableEquipment = AvailableEquipments.FirstOrDefault();
    }

    private async Task LoadServiceDataAsync(Specialist specialist)
    {
        ClinicDbContext asyncContext = new();
        var servicesSpecial = await asyncContext.Services.Where(s => s.Specialist == specialist.Id).ToListAsync();
        var servicesOverall = await asyncContext.Services.Where(s => s.Specialist == 0).ToListAsync();
        servicesOverall.AddRange(servicesSpecial);
        var distinctServices = servicesOverall.Distinct().OrderBy(s => s.Name).ToList();
        AvailableServices = new ObservableCollection<Service>(distinctServices);
        _servicePositions.Clear();
        for (var i = 0; i < distinctServices.Count; i++) _servicePositions[distinctServices[i]] = i;

        SelectedAvailableService = AvailableServices.FirstOrDefault();
    }

    private async Task LoadDiagnosisDataAsync()
    {
        ClinicDbContext asyncContext = new();
        var allDiagnoses = await asyncContext.Diagnoses.OrderBy(d => d.DiagnosisName).ToListAsync();
        AvailableDiagnoses = new ObservableCollection<Diagnosis>(allDiagnoses);
        _diagnosisPositions.Clear();
        for (var i = 0; i < allDiagnoses.Count; i++) _diagnosisPositions[allDiagnoses[i]] = i;

        SelectedAvailableDiagnosis = AvailableDiagnoses.FirstOrDefault();
    }

    private async Task LoadPurposeDataAsync()
    {
        ClinicDbContext asyncContext = new();
        var purposes = await asyncContext.Purposes.OrderBy(p => p.PurposeName).ToListAsync();
        AvailablePurposes = new ObservableCollection<Purpose>(purposes);
        SelectedPurpose = AvailablePurposes.FirstOrDefault();
    }


    private void LoadPatientInfo(Patient patient, Specialist specialist)
    {
        var gender = patient.Gender == 'm' ? "Мужской" : "Женский";
        var insurance = patient.InsuranceId > 0 && patient.InsuranceId.ToString().Length >= 8
            ? patient.InsuranceId.ToString()
            : "Страховка отсутствует";

        PatientInfoText = $"Имя: {patient.FirstName}\n" +
                          $"Фамилия: {patient.LastName}\n" +
                          $"Отчество: {patient.MiddleName}\n" +
                          $"Дата рождения: {patient.Birthday.ToShortDateString()}\n" +
                          $"Пол: {gender}\n" +
                          $"Номер страховки: {insurance}\n" +
                          $"Номер телефона: {patient.Phone}\n" +
                          $"Адрес: {patient.Address}\n" +
                          $"\nСпециалист:\n" +
                          $"Фамилия: {specialist.LastName}\n" +
                          $"Имя: {specialist.FirstName}\n" +
                          $"Отчество: {specialist.MiddleName}";
    }

    [RelayCommand]
    private void AddService()
    {
        if (SelectedAvailableService != null)
        {
            SelectedServices.Add(SelectedAvailableService);

            if (!_servicePositions.ContainsKey(SelectedAvailableService))
                _servicePositions[SelectedAvailableService] = AvailableServices.IndexOf(SelectedAvailableService);

            AvailableServices.Remove(SelectedAvailableService);

            if (SelectedServices.Count > 0) CanRemoveService = true;

            if (AvailableServices.Count > 0) SelectedAvailableService = AvailableServices.First();
            RecalculateTotalPrice();
        }
    }

    [RelayCommand]
    private void RemoveService()
    {
        if (SelectedServiceToRemove != null)
        {
            var tempService = SelectedServiceToRemove;
            SelectedServices.Remove(SelectedServiceToRemove);

            if (_servicePositions.TryGetValue(tempService, out var index))
            {
                index = Math.Min(index, AvailableServices.Count);
                AvailableServices.Insert(index, tempService);
            }
            else
            {
                AvailableServices.Add(tempService);
            }

            if (SelectedServices.Count == 0) CanRemoveService = false;

            SelectedAvailableService = AvailableServices.First();
            RecalculateTotalPrice();
        }
    }

    [RelayCommand]
    private void AddEquipment()
    {
        if (SelectedAvailableEquipment != null)
        {
            SelectedEquipments.Add(SelectedAvailableEquipment);

            if (!_equipmentPositions.ContainsKey(SelectedAvailableEquipment))
                _equipmentPositions[SelectedAvailableEquipment] =
                    AvailableEquipments.IndexOf(SelectedAvailableEquipment);

            AvailableEquipments.Remove(SelectedAvailableEquipment);

            if (SelectedEquipments.Count > 0) CanRemoveEquipment = true;

            if (AvailableEquipments.Count > 0) SelectedAvailableEquipment = AvailableEquipments.First();
        }
    }

    [RelayCommand]
    private void RemoveEquipment()
    {
        if (SelectedEquipmentToRemove != null)
        {
            var tempEquipment = SelectedEquipmentToRemove;
            SelectedEquipments.Remove(SelectedEquipmentToRemove);

            if (_equipmentPositions.TryGetValue(tempEquipment, out var index))
            {
                index = Math.Min(index, AvailableEquipments.Count);
                AvailableEquipments.Insert(index, tempEquipment);
            }
            else
            {
                AvailableEquipments.Add(tempEquipment);
            }

            if (SelectedEquipments.Count == 0) CanRemoveEquipment = false;

            SelectedAvailableEquipment = AvailableEquipments.First();
        }
    }

    [RelayCommand]
    private void AddDiagnosis()
    {
        if (SelectedAvailableDiagnosis != null)
        {
            SelectedDiagnoses.Add(SelectedAvailableDiagnosis);

            if (!_diagnosisPositions.ContainsKey(SelectedAvailableDiagnosis))
                _diagnosisPositions[SelectedAvailableDiagnosis] =
                    AvailableDiagnoses.IndexOf(SelectedAvailableDiagnosis);

            AvailableDiagnoses.Remove(SelectedAvailableDiagnosis);

            if (SelectedDiagnoses.Count > 0) CanRemoveDiagnosis = true;

            if (AvailableDiagnoses.Count > 0) SelectedAvailableDiagnosis = AvailableDiagnoses.First();
        }
    }

    [RelayCommand]
    private void RemoveDiagnosis()
    {
        if (SelectedDiagnosisToRemove != null)
        {
            var tempDiagnosis = SelectedDiagnosisToRemove;
            SelectedDiagnoses.Remove(SelectedDiagnosisToRemove);

            if (_diagnosisPositions.TryGetValue(tempDiagnosis, out var index))
            {
                index = Math.Min(index, AvailableDiagnoses.Count);
                AvailableDiagnoses.Insert(index, tempDiagnosis);
            }
            else
            {
                AvailableDiagnoses.Add(tempDiagnosis);
            }

            if (SelectedDiagnoses.Count == 0) CanRemoveDiagnosis = false;

            SelectedAvailableDiagnosis = AvailableDiagnoses.First();
        }
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitAppointmentAsync()
    {
        if (_patient == null)
        {
            Debug.WriteLine("Ошибка: Пациент не инициализирован перед сохранением.");
            return;
        }

        if (!ValidateInputs()) return;

        try
        {
            if (_isEditMode && _existingAppointment != null && CanEdit)
                await UpdateAppointmentAsync();
            else if (!_isEditMode)
                await CreateAppointmentAsync();
            else
                return;

            Debug.WriteLine("Приём успешно сохранён!");
            GoBack();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Произошла ошибка при сохранении приёма: {ex.Message}");
            Debug.WriteLine(ex.ToString());
        }
    }

    private bool CanSubmit()
    {
        return !IsReadOnly;
    }

    [RelayCommand]
    private void GoBack()
    {
        Debug.WriteLine("Команда Назад вызвана");
        MainWindow mainWindow = new(_specialist, _user);
        mainWindow.Show();
        _appointmentWindow.Close();
    }

    private async Task CreateAppointmentAsync()
    {
        if (_patient == null || _office == null)
            throw new InvalidOperationException("Patient or Office is not initialized.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var discount = (short)(_patient.InsuranceId > 0 ? 10 : 0);

            var appointment = new Appointment
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Patient = _patient.PatientId,
                Office = _office.OfficeId,
                Specialist = _specialist.Id,
                Discount = discount,
                Purpose = SelectedPurpose!.PurposeId,
                Commentaries = Cryptography.Encrypt(Commentaries.Trim()),
                IsPlanned = false,
                TotalPrice = TotalPrice
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            await AddAppointmentRelationsAsync(appointment);

            if (_specialist.Id == null)
                throw new InvalidOperationException("Specialist ID cannot be null when creating receipt");

            var receipt = new Receipt
            {
                AppointmentId = appointment.AppointmentNumber,
                SpecialistId = _specialist.Id,
                TotalSummary = TotalPrice,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Receipts.Add(receipt);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Debug.WriteLine($"Ошибка при создании приёма: {ex}");
            throw;
        }
    }

    private async Task UpdateAppointmentAsync()
    {
        if (_existingAppointment == null) return;
        if (_patient == null) throw new InvalidOperationException("Patient is not initialized for update.");


        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var appointmentToUpdate = await _context.Appointments
                .Include(a => a.Services)
                .Include(a => a.Equipment)
                .Include(a => a.Diagnoses)
                .FirstOrDefaultAsync(a => a.AppointmentNumber == _existingAppointment.AppointmentNumber);

            if (appointmentToUpdate == null)
            {
                Debug.WriteLine("Ошибка: Обновляемый приём не найден.");
                await transaction.RollbackAsync();
                return;
            }

            appointmentToUpdate.Purpose = SelectedPurpose!.PurposeId;
            appointmentToUpdate.Commentaries = Commentaries.Trim();
            appointmentToUpdate.TotalPrice = TotalPrice;
            appointmentToUpdate.TotalPrice = CalculateTotalPriceInternal(_patient);

            appointmentToUpdate.Services.Clear();
            appointmentToUpdate.Equipment.Clear();
            appointmentToUpdate.Diagnoses.Clear();

            await AddAppointmentRelationsAsync(appointmentToUpdate);

            var existingReceipt = await _context.Receipts
                .FirstOrDefaultAsync(r => r.AppointmentId == appointmentToUpdate.AppointmentNumber);

            if (appointmentToUpdate.Specialist != null)
                throw new InvalidOperationException(
                    "Specialist ID in appointment cannot be null when updating/creating receipt");

            if (existingReceipt != null)
            {
                existingReceipt.TotalSummary = TotalPrice;
                existingReceipt.Date = DateOnly.FromDateTime(DateTime.Now);
                existingReceipt.SpecialistId = appointmentToUpdate.Specialist;
                _context.Receipts.Update(existingReceipt);
            }
            else
            {
                var newReceipt = new Receipt
                {
                    AppointmentId = appointmentToUpdate.AppointmentNumber,
                    SpecialistId = appointmentToUpdate.Specialist,
                    TotalSummary = TotalPrice,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Receipts.Add(newReceipt);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Debug.WriteLine($"Ошибка при обновлении приёма: {ex}");
            throw;
        }
    }

    private async Task AddAppointmentRelationsAsync(Appointment appointment)
    {
        var servicesToAdd = await _context.Services.Where(s => SelectedServices.Select(ss => ss.Id).Contains(s.Id))
            .ToListAsync();
        appointment.Services.Clear();
        foreach (var service in servicesToAdd) appointment.Services.Add(service);

        var equipmentToAdd = await _context.Equipments
            .Where(e => SelectedEquipments.Select(se => se.EquipmentId).Contains(e.EquipmentId)).ToListAsync();
        appointment.Equipment.Clear();
        foreach (var equipment in equipmentToAdd) appointment.Equipment.Add(equipment);

        var diagnosesToAdd = await _context.Diagnoses
            .Where(d => SelectedDiagnoses.Select(sd => sd.DiagnosisId).Contains(d.DiagnosisId)).ToListAsync();
        appointment.Diagnoses.Clear();
        foreach (var diagnosis in diagnosesToAdd) appointment.Diagnoses.Add(diagnosis);
    }

    private decimal CalculateTotalPriceInternal(Patient? patient)
    {
        if (patient == null || !SelectedServices.Any()) return 0;

        var total = SelectedServices.Sum(service => service.Price);
        if (patient.InsuranceId > 0) total *= 0.9m;

        return Math.Round(total, 2);
    }

    private void RecalculateTotalPrice()
    {
        TotalPrice = CalculateTotalPriceInternal(_patient);
    }


    private bool ValidateInputs()
    {
        if (SelectedPurpose == null)
        {
            Debug.WriteLine("Ошибка валидации: Причина приёма не выбрана.");
            return false;
        }

        if (!SelectedServices.Any())
        {
            Debug.WriteLine("Ошибка валидации: Не выбрано ни одной услуги.");
            return false;
        }

        if (!SelectedDiagnoses.Any())
        {
            Debug.WriteLine("Ошибка валидации: Не выбрано ни одного диагноза.");
            return false;
        }

        return true;
    }

    partial void OnIsReadOnlyChanged(bool value)
    {
        CanEdit = !value;
        SubmitButtonText = _isEditMode ? CanEdit ? "Внести изменения" : "Просмотр" : "Зарегистрировать приём";
        SubmitAppointmentCommand.NotifyCanExecuteChanged();
    }

    partial void OnSelectedServicesChanged(ObservableCollection<Service> value)
    {
        RecalculateTotalPrice();
        CanRemoveService = value.Any();
        SubmitAppointmentCommand.NotifyCanExecuteChanged();
    }

    partial void OnSelectedEquipmentsChanged(ObservableCollection<Equipment> value)
    {
        CanRemoveEquipment = value.Any();
    }

    partial void OnSelectedDiagnosesChanged(ObservableCollection<Diagnosis> value)
    {
        CanRemoveDiagnosis = value.Any();
        SubmitAppointmentCommand.NotifyCanExecuteChanged();
    }

    partial void OnSelectedPurposeChanged(Purpose? value)
    {
        SubmitAppointmentCommand.NotifyCanExecuteChanged();
    }
}