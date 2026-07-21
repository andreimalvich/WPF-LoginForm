using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_LoginForm.Converters;

public class BooleanToInverseVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool boolValue = value is bool b && b;
        // Если true -> скрываем (Collapsed), если false -> показываем (Visible)
        return boolValue ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Visibility visibility && visibility == Visibility.Collapsed;
    }
}
