using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaPrivateClinic.Converters;

public class BoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool v) return v ? "Да" : "Нет";
        return "Нет";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() == "Да";
    }
}