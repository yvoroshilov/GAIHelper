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
            dataContext.PropertyChanged += OnPenaltyChanged;
            this.parent = parent;
            
        }

        private AddPersonViewModel dataContext;

        private void OnPenaltyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(dataContext.PaidPenalty) || e.PropertyName == nameof(dataContext.ActualPenalty)) {
                if (new DoubleValidationRule().Validate(dataContext.PaidPenalty, null).IsValid &&
                    new DoubleValidationRule().Validate(dataContext.ActualPenalty, null).IsValid) {
                    PaidPenaltyField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                    ActualPenaltyField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                }
            }
        }

        private Window parent;

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
