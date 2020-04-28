using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model {
    public abstract class NotifyingModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void setAllPropsToDefault() {
            var props = this.GetType().GetProperties();
            foreach (var prop in props) {
                prop.SetValue(this, default);
            }
        }
    }
}
