using Client.Model;
using Client.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
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


namespace Client {
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window {
        public Dashboard() {
            InitializeComponent();
            DataContext = new ViolationsUserViewModel();
            dataContext = (ViolationsUserViewModel)DataContext;
        }

        private readonly ViolationsUserViewModel dataContext;

        private void NoDriverLicenseCheckBox_Checked(object sender, RoutedEventArgs e) {
            foreach (var item in PersonInfoGrid.Children) {
                if (item is Control) {
                    ((Control)item).IsEnabled = false;
                }
            }
            BindingOperations.ClearBinding(DriverLicenseField, TextBox.TextProperty);
            DriverLicenseLabel.Content = "№ протокола*";
            dataContext.CurDriverLicenseOrProtocol = "";
        }   

        private void NoDriverLicenseCheckBox_Unchecked(object sender, RoutedEventArgs e) {
            foreach (var item in PersonInfoGrid.Children) {
                Control control = item as Control;
                if (item is Control) {
                    control.IsEnabled = true;
                    if (control.Name == ShowPersonsViolationsBtn.Name) {
                        control.IsEnabled = false;
                    }
                }
            }
            BindingOperations.SetBinding(DriverLicenseField, TextBox.TextProperty, new Binding("CurDriverLicenseOrProtocol"));
            DriverLicenseLabel.Content = "№ ВУ*";
        }

        private void ViolationField_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            IEnumerator enumerator = e.AddedItems.GetEnumerator();
            if (enumerator.MoveNext()) {
                ViolationType type = enumerator.Current as ViolationType;
                double min = ((ViolationsUserViewModel)DataContext).ViolationTypes.Where(val => val.Id == type.Id).FirstOrDefault().MinPenalty;
                PenaltyField.Text = min.ToString();
            } else {
                PenaltyField.Text = "";
            }
        }
    }
}
