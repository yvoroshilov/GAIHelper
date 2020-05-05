using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Util {
    public static class Utility {
        public static object GetDefault(Type type) {
            if (type.IsValueType) {
                return Activator.CreateInstance(type);
            } else if (type.Equals(typeof(DateTime))) {
                return default(DateTime);
            } else {
                return null;
            }
        }
    }
}
