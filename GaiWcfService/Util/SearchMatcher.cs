using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Util {
    public class SearchMatcher<T> {

        private List<string> excludedProps;
        
        public SearchMatcher(params string[] excludedProps) {
            this.excludedProps = excludedProps.ToList();
        }

        public bool CheckProps(T check, T searched) {
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (var prop in props) {
                if (!prop.GetMethod.IsVirtual && prop.GetValue(searched) != GetDefault(prop.PropertyType)) {
                    if (prop.PropertyType == typeof(string)) {
                        string checkedPropVal = prop.GetValue(check) as string;
                        string searchedPropVal = prop.GetValue(searched) as string;
                        if (!checkedPropVal.Contains(searchedPropVal)) return false;
                    } else if (!excludedProps.Contains(prop.Name)) {
                        if (!prop.GetValue(check).Equals(prop.GetValue(searched))) return false;
                    }
                }
            }
            return true;
        }

        public object GetDefault(Type type) {
            if (type.IsValueType) {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
