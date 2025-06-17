using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaPrivateClinic.Converters;

public class DiscountConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is short discount) return discount == 0 ? "Отсутствует" : $"{discount}%";
        return "Отсутствует";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}