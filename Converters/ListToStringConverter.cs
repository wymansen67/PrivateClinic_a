using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using AvaloniaPrivateClinic.Models;

namespace AvaloniaPrivateClinic.Converters;

public class ListToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is List<Appointment> appointments && appointments.Any())
            return string.Join("\n──────────\n", appointments.Select(a =>
                $"{a.PurposeNavigation.PurposeName} ({a.Time})"));

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}