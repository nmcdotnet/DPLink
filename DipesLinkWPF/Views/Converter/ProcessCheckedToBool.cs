using System.Globalization;
using System.Windows.Data;
using static SharedProgram.DataTypes.CommonDataType;

namespace DipesLink.Views.Converter
{
    public class ProcessCheckedToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (ProcessCheckeType)value;
            var par = int.Parse((string)parameter);

            if (par == 0)
            {
                if (val == ProcessCheckeType.TotalChecked) { return true; }
                else return false;
            }
            else
            {
                if (val == ProcessCheckeType.TotalChecked) { return false; }
                else return true;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            var par = int.Parse((string)parameter);
            if (par == 0 && val) return ProcessCheckeType.TotalChecked;
            else if (par == 1 && val) return ProcessCheckeType.TotalPassed;
            else throw new NotImplementedException();   
        }
    }
}
