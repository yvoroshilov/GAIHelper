using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel {
    public abstract class ViewModel : NotifyingModel {

        private PropertyInfo[] props;
        private static Type inputPropertyType = typeof(InputProperty);

        protected virtual void InitializeForm() {

        }

        public ViewModel() {
            props = this.GetType().GetProperties();
        }

        protected virtual void ResetForm() {
            foreach (var prop in props) {
                if (Attribute.IsDefined(prop, inputPropertyType)) {
                    prop.SetValue(this, default);
                }
            }
            InitializeForm();
        }

        protected bool IsAllRequiredFieldsFilled() {
            foreach (var prop in props) {
                if (Attribute.IsDefined(prop, inputPropertyType)) {
                    InputProperty attr = (InputProperty)prop.GetCustomAttribute(inputPropertyType);
                    if (attr.isRequred() && prop.GetValue(this) == null) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
