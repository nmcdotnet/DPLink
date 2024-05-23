using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DipesLink.Views.Converter
{
    class EventsLogImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? status = value as string;
            switch (status)
            {
                case "INFO ":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Info_Event.png", UriKind.Absolute));
                case "WARN ":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Warning_Event.png", UriKind.Absolute));
                case "ERROR":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Error_Event.png", UriKind.Absolute));
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
