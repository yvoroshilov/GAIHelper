using Client.Model;
using Client.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
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


namespace Client {
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window {
        public Dashboard() {
            DataContext = new ViolationsUserViewModel();
            dataContext = (ViolationsUserViewModel)DataContext;
            InitializeComponent();
            dataContext.PropertyChanged += InfoMessageBox;
        }

        private readonly ViolationsUserViewModel dataContext;

        private void NoDriverLicenseCheckBox_Checked(object sender, RoutedEventArgs e) {
            foreach (var item in PersonInfoGrid.Children) {
                if (item is Control) {
                    ((Control)item).IsEnabled = false;
                }
            }
            DriverLicenseLabel.Content = "№ протокола*";
            dataContext.DriverLicenseOrProtocol = null;
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
            DriverLicenseLabel.Content = "№ ВУ*";
        }

        private void ViolationField_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            IEnumerator enumerator = e.AddedItems.GetEnumerator();
            if (enumerator.MoveNext()) {
                ViolationType type = enumerator.Current as ViolationType;
                double min = dataContext.ViolationTypes.Where(val => val.Id == type.Id).FirstOrDefault().MinPenalty;
                dataContext.Penalty = min;
            } else {
                PenaltyField.Text = "";
            }
        }

        private void CheckPersonBtn_Click(object sender, RoutedEventArgs e) {
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (e.WidthChanged && !(e.PreviousSize.Width == 0)) {
                DescriptionColumn.Width = Math.Pow(e.NewSize.Width / e.PreviousSize.Width, 3) * DescriptionColumn.ActualWidth;
            }
        }

        private void AddViolationBtn_Click(object sender, RoutedEventArgs e) {
            ViolationField.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateTarget();
            AddressField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            CoordNField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            CoordEField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            CarNumberField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            DriverLicenseField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            DescriptionField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void InfoMessageBox(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName == "CurrentPerson" && dataContext.CurrentPerson == null) {
                MessageBox.Show($"Человека с таким водительским удостоверением № {dataContext.DriverLicenseOrProtocol} не существует");
            }
        }
    }
}
