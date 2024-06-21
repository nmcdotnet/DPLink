using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static SharedProgram.DataTypes.CommonDataType;
using static DipesLink.Views.Enums.ViewEnums;

namespace DipesLink.Views.Converter
{
    public class PrinterSeriesToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (PrinterSeries)value;
            var par = int.Parse((string)parameter);
            if (par == 0)
            {
                // || val == PrinterSeries.None
                if (val == PrinterSeries.RynanSeries) { return true; }
                else return false;
            }
            else if (par == 1)
            {
                // || val == PrinterSeries.None
                if (val == PrinterSeries.RynanSeries) { return false; }
                else return true;
            }
            else { return false; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            var par = int.Parse((string)parameter);
            if (par == 0)
            {
                return val == true ? PrinterSeries.RynanSeries : PrinterSeries.Standalone;
            }
            else
            {
                return val == true ? PrinterSeries.Standalone : PrinterSeries.RynanSeries;
            }
           
        }
    }
}
