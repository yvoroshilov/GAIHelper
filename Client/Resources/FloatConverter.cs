using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.Resources {
    [ValueConversion(typeof(double), typeof(string))]
    public class FloatConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            string val = (string)value;
            if (val == "") {
                return 0;
            } else {
                return value;
            }
        }
    }
}
