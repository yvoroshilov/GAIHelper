using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.Resources.Converter {
    public class DescriptionConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string res = null;
            if (value != null) {
                string str = (string)value;
                int ind = str.IndexOf(Environment.NewLine);
                if (ind >= 0) { 
                    res = str.Insert(ind, "...");
                } else {
                    res = str;
                }
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
