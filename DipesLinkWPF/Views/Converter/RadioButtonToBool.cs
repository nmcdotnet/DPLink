using System.Globalization;
using System.Windows.Data;

namespace DipesLink.Views.Converter
{
    public class RadioButtonToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string par = (string)parameter;
            bool val = (bool)value;
            if (par != null)
            {
                if (par == "0")
                {
                    if (val)
                    {
                        return true;
                    }
                    else return false;
                }
                else if (par == "1")
                {
                    if (val)
                    {
                        return false;
                    }
                    else return true;
                }
                else return val;
            }
            else { return val; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string par = (string)parameter;
            bool val = (bool)value;
            if (par != null)
            {
                if (par == "0")
                {
                    if (val) { return true; } else { return false; }
                }
                else if (par == "1")
                {
                    if (val) { return false; } else return false;
                }
                else
                {
                    return val;
                }
            }
            else
            {
                return val;
            }
        }
    }
}
