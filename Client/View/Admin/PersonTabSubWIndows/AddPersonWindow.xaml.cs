using Client.MainService;
using Client.Resources.Rule;
using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// <summary>
    /// Interaction logic for AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window {
        public AddPersonWindow(ObservableCollection<PersonDto> col, Window parent) {
            InitializeComponent();
            DataContext = new AddPersonViewModel(col);
            dataContext = DataContext as AddPersonViewModel;
            this.parent = parent;
            
        }

        private AddPersonViewModel dataContext;

        private Window parent;

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
