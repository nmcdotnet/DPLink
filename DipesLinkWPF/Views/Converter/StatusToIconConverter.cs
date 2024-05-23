using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DipesLink.Views.Converter
{
    public class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            switch (status)
            {
                case "Printed":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/printed.png", UriKind.Absolute));
                case "Waiting":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/waiting.png", UriKind.Absolute));
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
