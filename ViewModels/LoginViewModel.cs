using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using AvaloniaPrivateClinic.Encryption;
using AvaloniaPrivateClinic.Models;
using AvaloniaPrivateClinic.Utility;
using AvaloniaPrivateClinic.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace AvaloniaPrivateClinic.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly ClinicDbContext _context = new();

    [ObservableProperty] private string _password;

    [ObservableProperty] private string _username;

    public LoginViewModel()
    {
        Username = "wyman"; Password = "728281938293DA";
        //Username = "admin"; Password = "BfE44*AGv*796!";
    }

    [RelayCommand]
    private async Task Login(Window window)
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Имя пользователя не может быть пустым");

            await box.ShowAsync();
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Пароль не может быть пустым");

            await box.ShowAsync();
            return;
        }

        try
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == Username);

            if (user == null)
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Пользователь не существует", ButtonEnum.Ok, Icon.Error);

                await box.ShowAsync();
                return;
            }

            if (!user.IsActive)
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Ошибка",
                        "Пользователь не активен. Уточните информацию у своего руководства", ButtonEnum.Ok, Icon.Error);

                await box.ShowAsync();
                return;
            }

            var isPasswordValid = PasswordEncryption.VerifyPassword(
                Password,
                user.PasswordHash,
                user.Salt
            );

            if (isPasswordValid)
            {
                /*var manager = new WindowNotificationManager(window)
                {
                    Position = NotificationPosition.TopCenter,
                    MaxItems = 3
                };
                manager.Show(new Notification("Info", "Подождите немного. Происходит синхронизация данных", NotificationType.Information, TimeSpan.FromSeconds(10)));*/
                var specialist = _context.Specialists.Include(s => s.SpecializationNavigation)
                    .FirstOrDefault(u => u.Id == user.UserId);
                if (specialist != null)
                {
                    specialist.FirstName = Cryptography.Decrypt(specialist.FirstName);
                    specialist.LastName = Cryptography.Decrypt(specialist.LastName);
                    specialist.MiddleName = Cryptography.Decrypt(specialist.MiddleName);
                }

                MainWindow mainWindow = new(specialist, user);
                mainWindow.Show();
                window.Close();
            }
            else
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Ошибка",
                        "Неверные данные. Проверьте корректность данных и попробуйте ещё раз.", ButtonEnum.Ok,
                        Icon.Error);

                await box.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Ошибка", ex.Message + "\n" + ex, ButtonEnum.Ok, Icon.Error);

            await box.ShowAsync();
        }
    }
}