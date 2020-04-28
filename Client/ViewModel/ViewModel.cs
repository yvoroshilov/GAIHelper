using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel {
    public abstract class ViewModel : NotifyingModel {
        public abstract void InitializeForm();

        public abstract void ResetForm();
    }
}
