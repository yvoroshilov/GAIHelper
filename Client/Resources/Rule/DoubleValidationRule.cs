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
            string val = value?.ToString();
            if (val == null || (Regex.IsMatch(val, @"^-?\d+\.?\d*$") && !val.EndsWith("."))) {
                if (val == null || !val.EndsWith("0") || !val.Contains(".")) {
                    return new ValidationResult(true, null);
                } else {
                    return new ValidationResult(false, $"Удалите последний ноль либо введите не ноль");
                }
            } else {
                return new ValidationResult(false, $"Значение '{(string)value}' не подходит для этого поля");
            }
        }
    }
}
