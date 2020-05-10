using Client.MainService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.View.Admin.PersonTabSubWIndows {

    public partial class PersonsViolations : Window {
        public PersonsViolations(ObservableCollection<ViolationDto> violations, Window parent) {
            InitializeComponent();
            this.parent = parent;
            DataContext = violations;
        }

        Window parent;

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
