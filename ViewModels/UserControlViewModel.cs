using System;
using System.Xml;
using AvaloniaPrivateClinic.Encryption;
using AvaloniaPrivateClinic.Models;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia.Dto;

namespace AvaloniaPrivateClinic.ViewModels;

public partial class UserControlViewModel : ViewModelBase
{
    private readonly ClinicDbContext _context = new();
    private string _firstName = "";
        private string _middleName = "";
        private string _lastName = "";
        private decimal _insuranceId;
        private DateOnly _birthday = DateOnly.FromDateTime(DateTime.Today);
        private char _gender;
        private decimal _phone;
        private string _address = "";

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
            //var db = new ClinicDbContext();

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

            /*MessageBox.Show($"{newPatient.FirstName}\n" +
                $"{newPatient.MiddleName}\n" +
                $"{newPatient.LastName}\n" +
                $"{newPatient.InsuranceId}\n" +
                $"{newPatient.Birthday}\n" +
                $"{newPatient.Gender}\n" +
                $"{newPatient.Phone}\n" +
                $"{newPatient.Address}");*/

            _context.Patients.Add(newPatient);
            _context.SaveChanges();
            PatientFirstName = string.Empty;
            PatientMiddleName = string.Empty;
            PatientLastName = string.Empty;
            PatientInsuranceId = 0;
            PatientBirthday = DateTime.Today;
            PatientGender = string.Empty;
            PatientPhone = 0;
            PatientAddress = string.Empty;
;        }
}