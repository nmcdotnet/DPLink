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
    public class CompleteConditionToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CompleteCondition && parameter is string parameterString)
            {
                CompleteCondition optionValue = (CompleteCondition)value;
                if (Enum.TryParse(parameterString, out CompleteCondition parameterValue))
                {
                    return optionValue == parameterValue;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parameterString = (string)parameter;
            bool boolValue =(bool)value;
           
                if (Enum.TryParse(parameterString, true, out CompleteCondition result))
                {
                    return result;
                }
            
            return Binding.DoNothing;
        }
    }
}
