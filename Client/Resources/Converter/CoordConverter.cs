using Client.MainService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.Resources.Converter {
    public class CoordConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            double? latitude = values[0] as double?;
            double? longitude = values[1] as double?;
            if (latitude != null && longitude != null) {
                return Math.Round(latitude.Value, 6) + " " + Math.Round(longitude.Value, 6) + "";
            } else if (latitude == null && longitude == null) {
                return null;
            } else {
                throw new Exception("One of coords is null");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
