using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.Resources.Rule {
    public class DoubleValidationRule : ValidationRule {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            string val = (string)value;
                
            if (val == null || (Regex.IsMatch(val, @"^-?\d+\.?\d*$") && !val.EndsWith("."))) {
                return new ValidationResult(true, null);
            } else {
                return new ValidationResult(false, $"Значение '{(string)value}' не подходит для этого поля");
            }
        }
    }
}
