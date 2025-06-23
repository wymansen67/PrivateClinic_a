using System;
using AvaloniaPrivateClinic.Encryption;
using AvaloniaPrivateClinic.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaPrivateClinic.ViewModels;

public partial class UserControlViewModel : ViewModelBase
{
    private readonly ClinicDbContext _context = new();
    private string _address = "";
    private DateOnly _birthday = DateOnly.FromDateTime(DateTime.Today);
    private string _firstName = "";
    private char _gender;
    private decimal _insuranceId;
    private string _lastName = "";
    private string _middleName = "";
    private decimal _phone;

    [ObservableProperty] private Patient? _selectedPatient;

    [ObservableProperty] private string _title = string.Empty;

    public UserControlViewModel()
    {
        Title = "Заполнение данных о новом пациенте";
    }

    public UserControlViewModel(Patient patient)
    {
        Title = "Изменение данных пациента";
        SelectedPatient = patient;
        PatientFirstName = patient.FirstName;
        PatientMiddleName = patient.MiddleName;
        PatientLastName = patient.LastName;
        PatientInsuranceId = patient.InsuranceId;
        PatientBirthday = DateTime.Parse(patient.Birthday.ToString());
        PatientGender = patient.Gender.ToString();
        PatientPhone = patient.Phone;
        PatientAddress = patient.Address;
    }

    public string PatientFirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
    }

    public string PatientMiddleName
    {
        get => _middleName;
        set
        {
            _middleName = value;
            OnPropertyChanged();
        }
    }

    public string PatientLastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged();
        }
    }

    public decimal PatientInsuranceId
    {
        get => _insuranceId;
        set
        {
            _insuranceId = value;
            OnPropertyChanged();
        }
    }

    public DateTime PatientBirthday
    {
        get => _birthday.ToDateTime(TimeOnly.MinValue);
        set
        {
            _birthday = DateOnly.FromDateTime(value);
            OnPropertyChanged();
        }
    }

    public string PatientGender
    {
        get => _gender.ToString();
        set
        {
            _gender = string.IsNullOrEmpty(value) ? ' ' : value[0];
            OnPropertyChanged();
        }
    }

    public decimal PatientPhone
    {
        get => _phone;
        set
        {
            _phone = value;
            OnPropertyChanged();
        }
    }

    public string PatientAddress
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    public void SavePatient()
    {
        if (SelectedPatient == null)
        {
            // Новый пациент
            var newPatient = new Patient
            {
                FirstName = Cryptography.Encrypt(PatientFirstName),
                MiddleName = Cryptography.Encrypt(PatientMiddleName),
                LastName = Cryptography.Encrypt(PatientLastName),
                InsuranceId = PatientInsuranceId,
                Birthday = DateOnly.FromDateTime(PatientBirthday),
                Gender = string.IsNullOrEmpty(PatientGender) ? ' ' : PatientGender[0],
                Phone = PatientPhone,
                Address = PatientAddress
            };

            _context.Patients.Add(newPatient);
        }
        else
        {
            // Изменение уже существующего
            SelectedPatient.FirstName = Cryptography.Encrypt(PatientFirstName);
            SelectedPatient.MiddleName = Cryptography.Encrypt(PatientMiddleName);
            SelectedPatient.LastName = Cryptography.Encrypt(PatientLastName);
            SelectedPatient.InsuranceId = PatientInsuranceId;
            SelectedPatient.Birthday = DateOnly.FromDateTime(PatientBirthday);
            SelectedPatient.Gender = string.IsNullOrEmpty(PatientGender) ? ' ' : PatientGender[0];
            SelectedPatient.Phone = PatientPhone;
            SelectedPatient.Address = PatientAddress;

            _context.Patients.Update(SelectedPatient);
        }

        _context.SaveChanges();
        _context.Dispose();
        ClearFields();
    }

    private void ClearFields()
    {
        PatientFirstName = string.Empty;
        PatientMiddleName = string.Empty;
        PatientLastName = string.Empty;
        PatientInsuranceId = 0;
        PatientBirthday = DateTime.Today;
        PatientGender = string.Empty;
        PatientPhone = 0;
        PatientAddress = string.Empty;
        SelectedPatient = null;
    }
}