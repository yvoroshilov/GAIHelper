using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel {
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class InputProperty : Attribute {

        private bool Required { get; }

        public string Mark { get; set; }

        public InputProperty() {
            Required = false;
        }

        public InputProperty(bool required) {
            this.Required = required;
        }

        public bool isRequred() {
            return Required;
        }
    }
}
