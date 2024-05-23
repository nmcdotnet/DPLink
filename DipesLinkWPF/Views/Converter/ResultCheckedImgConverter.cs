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
    public class ResultCheckedImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? status = value as string;
            switch (status)
            {
                case "Valid":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Valid_res.png", UriKind.Absolute));
                case "Invalided":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Invalid_res.png", UriKind.Absolute));
                case "Duplicated":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Duplicated_res.png", UriKind.Absolute));
                case "Null":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Null_res.png", UriKind.Absolute));
                case "Missed":
                    return new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/Missed_res.png", UriKind.Absolute));
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
