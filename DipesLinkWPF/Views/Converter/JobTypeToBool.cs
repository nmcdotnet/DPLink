using System.Globalization;
using System.Windows.Data;
using static SharedProgram.DataTypes.CommonDataType;

namespace DipesLink.Views.Converter
{
    public class JobTypeToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JobType && parameter is string parameterString)
            {
                JobType optionValue = (JobType)value;
                if (Enum.TryParse(parameterString, out JobType parameterValue))
                {
                    return optionValue == parameterValue;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue && parameter is string parameterString)
            {
                if (Enum.TryParse(parameterString, true, out JobType result))
                {
                    return result;
                }
            }
            return Binding.DoNothing;
        }
    }
}
