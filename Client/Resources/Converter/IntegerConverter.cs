using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.Resources.Converter {
    class IntegerConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string val = value.ToString();
            if (val == null) {
                return 0;
            }

            int res = 0;
            while (val.Length != 0 && !int.TryParse(val, out res)) {
                val = val.Substring(0, val.Length - 1);
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
