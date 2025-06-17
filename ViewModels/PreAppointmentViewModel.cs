using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AvaloniaPrivateClinic.ViewModels;

public partial class PreAppointmentViewModel : ViewModelBase
{
    private readonly ClinicDbContext _context = new();

    [ObservableProperty] private ObservableCollection<Equipment> _availableEquipments = new();

    [ObservableProperty] private ObservableCollection<Equipment> _claimedEquipments = new();

    internal Dictionary<Equipment, int> _equipmentPositions = new();

    [ObservableProperty] private ObservableCollection<Office> _offices = new();

    [ObservableProperty] private ObservableCollection<Patient> _patients = new();

    [ObservableProperty] private ObservableCollection<Purpose> _purposes = new();

    [ObservableProperty] private Office _selectedOffice = new();

    [ObservableProperty] private Patient _selectedPatient = new();

    [ObservableProperty] private ObservableCollection<Specialist> _specialists = new();

    public PreAppointmentViewModel(Specialist? specialist, User user, PreAppointmentWindow window)
    {
        Specialist = specialist;
        User = user;
        Window = window;

        ClaimedEquipments = new ObservableCollection<Equipment>();

        BackCommand = new RelayCommand(Back);
    }

    private Specialist? Specialist { get; }
    internal User User { get; set; }
    private PreAppointmentWindow Window { get; }

    [RelayCommand]
    public async Task AddPatient()
    {
        var window = new UserControlWindow();
        var result = await window.ShowDialog<bool?>(this.Window); // this — это родительское окно

        if (result == true)
        {
            LoadPatients();
        }

    }
    
    public ICommand BackCommand { get; }

    public void Back()
    {
        OpenPreviousWindow();
    }

    /*public void AddEquipment() => EquipmentInsert();
    public void RemoveEquipment() => EquipmentRemove();*/
    public void Start()
    {
        StartAppointment();
    }

    private void LoadPatients()
    {
        Patients = new ObservableCollection<Patient>
        (
            _context.Patients
            .ToList()
            .Select(p =>
            {
                p.FirstName = Cryptography.Decrypt(p.FirstName)!;
                p.MiddleName = Cryptography.Decrypt(p.MiddleName)!;
                p.LastName = Cryptography.Decrypt(p.LastName)!;
                return p;
            }).ToList()
        );
        /*if (_newPatient != null)
        {
            SelectNewPatient(patients);
        }
        else
        {
            cbPatients.SelectedIndex = 0;
        }*/
    }

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
        await Dispatcher.UIThread.InvokeAsync(() => { Patients = new ObservableCollection<Patient>(patientsList); });
    }

    internal async Task UpdateOfficesAsync()
    {
        var officesList = await new ClinicDbContext().Offices
            .Include(o => o.TypeNavigation)
            .ToListAsync();
        
        await Dispatcher.UIThread.InvokeAsync(() => { Offices = new ObservableCollection<Office>(officesList); });
    }

    /*private void SelectNewPatient(List<Patient> patients)
    {
        var newPatientInList = patients.FirstOrDefault(p =>
            p.FirstName == _newPatient.FirstName &&
            p.LastName == _newPatient.LastName &&
            p.MiddleName == _newPatient.MiddleName &&
            p.Birthday == _newPatient.Birthday &&
            p.Phone == _newPatient.Phone);

        if (newPatientInList != null)
        {
            cbPatients.SelectedItem = newPatientInList;
        }

        _newPatient = null;
    }*/

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

        await Dispatcher.UIThread.InvokeAsync(() => { Purposes = new ObservableCollection<Purpose>(purposesList); });
    }

    internal async Task UpdateEquipmentsAsync()
    {
        var equipmentsList = await new ClinicDbContext().Equipments.ToListAsync();

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            AvailableEquipments = new ObservableCollection<Equipment>(equipmentsList);

            // Обновляем позиции в UI-потоке
            _equipmentPositions.Clear();
            for (var i = 0; i < AvailableEquipments.Count; i++) _equipmentPositions[AvailableEquipments[i]] = i;
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
        var appointmentWindow = new AppointmentWindow(Specialist, SelectedPatient, SelectedOffice, User, null);
        appointmentWindow.Show();
        Window.Close();
    }
}