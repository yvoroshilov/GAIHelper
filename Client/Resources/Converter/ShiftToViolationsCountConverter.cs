using Client.MainService;
using Client.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.Resources.Converter {
    public class ShiftToViolationsCountConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            ShiftDto shift = value as ShiftDto;
            AdminServiceClient adminClient = ClientInstanceProvider.GetAdminServiceClient();
            return adminClient
                .GetViolationsByShiftId(shift.id)
                .Count();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
