using Client.MainService;
using Client.Model;
using Client.View.User;
using Client.ViewModel;
using System;
using System.Collections;
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


namespace Client.View.Admin.ViolationsTabSubWindows {

    public partial class AddViolationWindow : Window {
        public AddViolationWindow(ObservableCollection<ViolationDto> col, Window parent) {
            DataContext = new AddViolationWindowViewModel(col);
            dataContext = (AddViolationWindowViewModel)DataContext;
            dataContext.CurrentPerson.PropertyChanged += OnCurrentPersonIdChanged;
            this.parent = parent;
            InitializeComponent();
        }

        private readonly AddViolationWindowViewModel dataContext;

        private PersonsViolations personsViolationsWindow;

        private Window parent;

        private void NoDriverLicenseCheckBox_Checked(object sender, RoutedEventArgs e) {
            foreach (var item in PersonInfoGrid.Children) {
                if (item is Control) {
                    ((Control)item).IsEnabled = false;
                }
            }
            DriverLicenseField.IsEnabled = false;
            DriverLicenseLabel.IsEnabled = false;
            dataContext.ResetPersonProfile();
            dataContext.DriverLicense = "";
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
            DriverLicenseField.IsEnabled = true;
            DriverLicenseLabel.IsEnabled = true;
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

        private void AddViolationBtn_Click(object sender, RoutedEventArgs e) {
            ViolationField.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateTarget();
            AddressField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            CoordNField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            CoordEField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            CarNumberField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            DriverLicenseField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            DescriptionField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void DriverLicenseField_TextChanged(object sender, TextChangedEventArgs e) {
            dataContext.ResetPersonProfile();
        }

        private void ShowPersonsViolationsBtn_Click(object sender, RoutedEventArgs e) {
            if (personsViolationsWindow == null || !personsViolationsWindow.IsLoaded) {
                personsViolationsWindow = new PersonsViolations();
                personsViolationsWindow.DataContext = dataContext.CurrentPersonsViolations;
                personsViolationsWindow.Show();
            } else {
                personsViolationsWindow.Activate();
            }
        }

        private void OnCurrentPersonIdChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName != nameof(dataContext.CurrentPerson.id)) {
                return;
            }
            if (dataContext.CurrentPerson.id == 0) {
                ShowPersonsViolationsBtn.IsEnabled = false;
            } else {
                ShowPersonsViolationsBtn.IsEnabled = true;
            }
        }

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.Close();
        }
    }
}
