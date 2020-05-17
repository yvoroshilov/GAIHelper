using Client.MainService;
using Client.Model;
using Client.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
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

        protected virtual void ResetForm(string mark = null) {
            foreach (var prop in props) {
                InputProperty attr = (InputProperty)prop.GetCustomAttribute(inputPropertyType);
                if (Attribute.IsDefined(prop, inputPropertyType) &&
                    (mark == null || mark.Equals(attr.Mark))) {
                    prop.SetValue(this, default);
                }
            }
            InitializeForm();
        }

        protected virtual bool IsAllRequiredFieldsFilled(string mark = null) {
            foreach (var prop in props) {
                if (Attribute.IsDefined(prop, inputPropertyType)) {
                    InputProperty attr = (InputProperty)prop.GetCustomAttribute(inputPropertyType);
                    
                    if ((attr.isRequred() && (prop.GetValue(this)?.Equals(Utility.GetDefault(prop.PropertyType)) ?? true)) &&
                     (mark == null || mark.Equals(attr.Mark))) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected virtual bool IsAllInputPropsValid(IDataErrorInfo errorInfo, string mark = null) {
            foreach (var prop in props) {
                if (Attribute.IsDefined(prop, inputPropertyType)) {
                    InputProperty attr = (InputProperty)prop.GetCustomAttribute(inputPropertyType);
                    
                    if ((errorInfo[prop.Name]) != "" &&
                     (mark == null || mark.Equals(attr.Mark))) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected PropertyInfo[] GetProps() {
            return props;
        }
    }
}
