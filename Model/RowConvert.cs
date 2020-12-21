using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GreenWhale.UpdateCore.Model
{
    [ValueConversion(typeof(int), typeof(int))]
    public class RowConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var result = int.Parse(value.ToString());
                if (result < 0)
                {
                    return null;
                }
                else
                {
                    return ++result;
                }
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var result = int.Parse(value.ToString());
                return --result;
            }
            else
            {
                return 0;
            }
        }
    }
}
