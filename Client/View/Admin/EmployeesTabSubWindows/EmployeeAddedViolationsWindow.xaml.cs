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

namespace Client.View.Admin.EmployeesTabSubWindows {

    public partial class EmployeeAddedViolationsWindow : Window {
        public EmployeeAddedViolationsWindow(ObservableCollection<ViolationDto> col, Window parent) {
            InitializeComponent();
            DataContext = col;
            this.parent = parent;
        }

        private Window parent;

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
