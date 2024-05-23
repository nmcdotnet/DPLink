using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DipesLink.Views.Converter
{
    public class AddOne : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return (int)value + 1;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is int)
            //{
            //    return (int)value - 1;
            //}
            //// Có thể xử lý thêm các kiểu dữ liệu khác nếu cần
            //return value;
            throw new NotImplementedException();
        }
    }
}
