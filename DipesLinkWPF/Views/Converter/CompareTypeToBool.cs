using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static SharedProgram.DataTypes.CommonDataType;

namespace DipesLink.Views.Converter
{
    public class CompareTypeToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CompareType && parameter is string parameterString)
            {
                CompareType optionValue = (CompareType)value;
                if (Enum.TryParse(parameterString, out CompareType parameterValue))
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
                if (Enum.TryParse(parameterString, true, out CompareType result))
                {
                    return result;
                }
            }
            return Binding.DoNothing;
        }
    }
}
