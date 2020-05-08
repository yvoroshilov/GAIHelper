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
            double? coordN = values[0] as double?;
            double? coordE = values[1] as double?;
            if (coordN != null && coordE != null) {
                return Math.Round(coordN.Value, 6) + "N " + Math.Round(coordE.Value, 6) + "E";
            } else if (coordN == null && coordE == null) {
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
