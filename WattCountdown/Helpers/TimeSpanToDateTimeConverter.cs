using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Abb.Cz.Apps.WattCountdown.Helpers
{
    [ValueConversion(typeof(TimeSpan), typeof(DateTime))]
    class TimeSpanToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => new DateTime(1, 1, 1, 0, 0, 0).Add((TimeSpan)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => ((DateTime)value).TimeOfDay;
    }
}
