using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Abb.Cz.Apps.WattCountdown.Helpers
{
    [ValueConversion(typeof(double), typeof(string))]
    internal class DoubleToHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => TimeSpan.FromHours((double)value).ToString(@"hh\:mm");

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
