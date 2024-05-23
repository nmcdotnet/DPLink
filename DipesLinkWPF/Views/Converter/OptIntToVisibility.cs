using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DipesLink.Views.Converter
{
    public class OptIntToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            int par = int.Parse((string)parameter);
            return val == par ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
