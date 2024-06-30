using System;
using System.Globalization;
using System.Windows.Data;
using static SharedProgram.DataTypes.CommonDataType;

namespace DipesLink.Views.Converter
{
    public class OperationStatusToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Handle Value Type
            switch (value)
            {
                case OperationStatus operationStatus when (operationStatus == OperationStatus.Running || operationStatus == OperationStatus.Processing):
                    return false;

                case OperationStatus operationStatus when operationStatus == OperationStatus.Stopped:
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
