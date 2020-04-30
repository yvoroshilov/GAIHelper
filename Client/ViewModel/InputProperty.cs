using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel {
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class InputProperty : Attribute {

        private readonly bool required;

        public InputProperty() {
            required = false;
        }

        public InputProperty(bool required) {
            this.required = required;
        }

        public bool isRequred() {
            return required;
        }
    }
}
