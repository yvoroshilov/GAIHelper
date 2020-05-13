using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.Resources.Rule {
    public class IntegerValidationRule : ValidationRule {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            string val = (string)value;

            int res;
            if (int.TryParse(val, out res)) {
                return new ValidationResult(true, null);
            } else {
                return new ValidationResult(false, $"Значение '{(string)value}' не подходит для этого поля");
            }
        }
    }
}
