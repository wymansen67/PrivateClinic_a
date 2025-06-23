using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using AvaloniaPrivateClinic.Encryption;
using AvaloniaPrivateClinic.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaPrivateClinic.ViewModels;

public partial class AppointmentViewViewModel : ViewModelBase
{
    private readonly ClinicDbContext _context = new();

    private readonly Appointment? _existingAppointment;

    [ObservableProperty] private string _commentaries = string.Empty;

    [ObservableProperty] private string _dateLabel = DateTime.Now.ToString("d");

    [ObservableProperty] private string? _officeLabel;

    [ObservableProperty] private string? _patientInfoText;

    [ObservableProperty] private ObservableCollection<Diagnosis> _selectedDiagnoses = new();

    [ObservableProperty] private ObservableCollection<Equipment> _selectedEquipments = new();

    [ObservableProperty] private Purpose? _selectedPurpose;

    [ObservableProperty] private ObservableCollection<Purpose> _selectedPurposes = new();

    [ObservableProperty] private ObservableCollection<Service> _selectedServices = new();

    [ObservableProperty] private decimal _totalPrice;

    [ObservableProperty] private string _windowTitle = "Обзор приёма";
    
    public Action SuccessAction { get; set; }

    public AppointmentViewViewModel(int AppointmentId)
    {
        _existingAppointment = _context.Appointments
            .Include(a => a.SpecialistNavigation)
            .Include(a => a.SpecialistNavigation.SpecializationNavigation)
            .Include(a => a.OfficeNavigation)
            .Include(a => a.PatientNavigation)
            .Include(a => a.Diagnoses)
            .Include(a => a.Equipment)
            .Include(a => a.Services)
            .First(a => a.AppointmentNumber == AppointmentId);

        _existingAppointment.PatientNavigation.LastName =
            Cryptography.Decrypt(_existingAppointment.PatientNavigation.LastName);
        _existingAppointment.PatientNavigation.MiddleName =
            Cryptography.Decrypt(_existingAppointment.PatientNavigation.MiddleName);
        _existingAppointment.PatientNavigation.FirstName =
            Cryptography.Decrypt(_existingAppointment.PatientNavigation.FirstName);

        _existingAppointment.SpecialistNavigation.LastName =
            Cryptography.Decrypt(_existingAppointment.SpecialistNavigation.LastName);
        _existingAppointment.SpecialistNavigation.MiddleName =
            Cryptography.Decrypt(_existingAppointment.SpecialistNavigation.MiddleName);
        _existingAppointment.SpecialistNavigation.FirstName =
            Cryptography.Decrypt(_existingAppointment.SpecialistNavigation.FirstName);

        _existingAppointment.Commentaries = Cryptography.Decrypt(_existingAppointment.Commentaries);
        WindowTitle = $"Обзор приёма — {_existingAppointment.Date:d}";
        LoadAppointmentData();
    }

    private void LoadAppointmentData()
    {
        SelectedPurposes.Add(_context.Purposes.First(p => p.PurposeId == _existingAppointment!.Purpose));
        SelectedPurpose = SelectedPurposes[0];
        Commentaries = _existingAppointment!.Commentaries ?? string.Empty;
        TotalPrice = _existingAppointment.TotalPrice;
        DateLabel = _existingAppointment.Date.ToString("d");

        using var dbContext = new ClinicDbContext();

        foreach (var service in _existingAppointment.Services)
        {
            var serviceToAdd = _existingAppointment.Services.FirstOrDefault(s => s.Id == service.Id);
            if (serviceToAdd != null && !SelectedServices.Contains(serviceToAdd)) SelectedServices.Add(serviceToAdd);
        }

        foreach (var equipment in _existingAppointment.Equipment)
        {
            var equipmentToAdd =
                _existingAppointment.Equipment.FirstOrDefault(eq => eq.EquipmentId == equipment.EquipmentId);
            if (equipmentToAdd != null && !SelectedEquipments.Contains(equipmentToAdd))
                SelectedEquipments.Add(equipmentToAdd);
        }

        foreach (var diagnosis in _existingAppointment.Diagnoses)
        {
            var diagnosisToAdd =
                _existingAppointment.Diagnoses.FirstOrDefault(d => d.DiagnosisId == diagnosis.DiagnosisId);
            if (diagnosisToAdd != null && !SelectedDiagnoses.Contains(diagnosisToAdd))
                SelectedDiagnoses.Add(diagnosisToAdd);
        }

        LoadPatientInfo(_existingAppointment.PatientNavigation, _existingAppointment.SpecialistNavigation);
    }

    private void LoadPatientInfo(Patient patient, Specialist specialist)
    {
        var gender = patient.Gender == 'm' ? "Мужской" : "Женский";
        var insurance = patient.InsuranceId > 0 && patient.InsuranceId.ToString(CultureInfo.CurrentCulture).Length >= 8
            ? patient.InsuranceId.ToString(CultureInfo.CurrentCulture)
            : "Страховка отсутствует";

        PatientInfoText = $"Имя: {patient.FirstName}\n" +
                          $"Фамилия: {patient.LastName}\n" +
                          $"Отчество: {patient.MiddleName}\n" +
                          $"Дата рождения: {patient.Birthday.ToShortDateString()}\n" +
                          $"Пол: {gender}\n" +
                          $"Номер страховки: {insurance}\n" +
                          $"Номер телефона: {patient.Phone}\n\n" +
                          $"Специалист:\n" +
                          $"ФИО: {specialist.LastName} {specialist.FirstName.Substring(0, 1)}. {specialist.MiddleName.Substring(0, 1)}.\n{specialist.SpecializationNavigation.Specialization1}";
    }

    [RelayCommand]
    private void GoBack()
    {
        SuccessAction?.Invoke();
    }
}