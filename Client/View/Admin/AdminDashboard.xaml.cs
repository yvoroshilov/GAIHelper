using Client.View.Admin.EmployeesTabSubWindows;
using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Client.Util;
using Client.View.Admin.UsersTabSubWindows;
using Client.View.Admin.ViolationsTabSubWindows;
using System.Collections;
using Client.MainService;

namespace Client.View.Admin {
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window {
        public AdminDashboard() {
            InitializeComponent();
        }

        private EmployeesViewModel employeesViewModel = new EmployeesViewModel();
        private UsersViewModel usersViewModel = new UsersViewModel();
        private ViolationsAdminViewModel violationsAdminViewModel = new ViolationsAdminViewModel();

        private void Button_Click(object sender, RoutedEventArgs e) {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!(e.Source is TabControl)) return;
            switch((e.AddedItems[0] as TabItem).Name) {
                case "EmployeesTab":
                    EmployeesTabGrid.DataContext = employeesViewModel;
                    break;
                case "UsersTab":
                    UsersTabGrid.DataContext = usersViewModel;
                    break;
                case "ViolationsTab":
                    ViolationsTabGrid.DataContext = violationsAdminViewModel;
                    break;
            }
        }

        private void SearchThisFieldCheckboxChecked(object sender, RoutedEventArgs e) {
            StackPanel stackPanel = (StackPanel)VisualTreeHelper.GetParent((CheckBox)sender);
            foreach (var item in stackPanel.Children) {
                if (item != sender) {
                    (item as FrameworkElement).IsEnabled = true;
                }
            }
        }

        private void SearchThisFieldCheckboxUnchecked(object sender, RoutedEventArgs e) {
            StackPanel stackPanel = (StackPanel)VisualTreeHelper.GetParent((CheckBox)sender);
            foreach (var item in stackPanel.Children) {
                if (item != sender) {
                    (item as FrameworkElement).IsEnabled = false;
                }
                if (item is TextBox) {
                    TextBox curTextBox = item as TextBox;
                    string propName = curTextBox.GetBindingExpression(TextBox.TextProperty).ResolvedSourcePropertyName;
                    object source = curTextBox.GetBindingExpression(TextBox.TextProperty).DataItem;
                    PropertyInfo propInfo = source.GetType().GetProperty(propName);
                    propInfo.SetValue(source, Utility.GetDefault(propInfo.PropertyType));
                } else if (item is ComboBox) {
                    ComboBox curComboBox = item as ComboBox;
                    string propName = curComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).ResolvedSourcePropertyName;
                    object source = curComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).DataItem;
                    PropertyInfo propInfo = source.GetType().GetProperty(propName);
                    propInfo.SetValue(source, Utility.GetDefault(propInfo.PropertyType));
                }
            }
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        #region Employees tab
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                EmployeeTable.UnselectAll();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Escape:
                    EmployeeTable.UnselectAll();
                    break;
                case Key.Delete:
                    if (employeesViewModel.DeleteCommand.CanExecute(EmployeeTable.SelectedItems)) {
                        employeesViewModel.DeleteCommand.Execute(EmployeeTable.SelectedItems);
                    }
                    break;
                default:
                    break;
            }
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;
            EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(this);

            editEmployeeWindow.Show();

            editEmployeeWindow.DataContext = employeesViewModel;
            editEmployeeWindow.PatronymicField.ClearValue(TextBox.ToolTipProperty);
            editEmployeeWindow.NameField.ClearValue(TextBox.ToolTipProperty);
            editEmployeeWindow.SurnameField.ClearValue(TextBox.ToolTipProperty);
            editEmployeeWindow.HireDateField.ClearValue(TextBox.ToolTipProperty);
            editEmployeeWindow.LoginField.ClearValue(TextBox.ToolTipProperty);
        }

        private void SeeAddedViolationsButton_Click(object sender, RoutedEventArgs e) {
            EmployeeAddedViolationsWindow window = new EmployeeAddedViolationsWindow();
            window.DataContext = employeesViewModel.EmployeeAddedViolations;
            window.Show();
        }

        private void SeeDoneShiftsButton_Click(object sender, RoutedEventArgs e) {
            EmployeeDoneShiftsWindow window = new EmployeeDoneShiftsWindow();
            window.DataContext = employeesViewModel.EmployeeDoneShifts;
            window.Show();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(this);

            addEmployeeWindow.Show();

            addEmployeeWindow.DataContext = employeesViewModel;
        }

        #endregion

        #region Users tab
        private void EditUserPasswordBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(this); 

            changePasswordWindow.Show();

            changePasswordWindow.DataContext = usersViewModel;

        }
        #endregion

        #region Violation
        private void AddViolationBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;
            AddViolationWindow addViolationWindow = new AddViolationWindow(violationsAdminViewModel.Violations, this);
            addViolationWindow.Show();
        }

        private void EditViolationBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;
            EditViolationWindow editViolationWindow = new EditViolationWindow(violationsAdminViewModel.Violations, violationsAdminViewModel.curViolation, this);
            editViolationWindow.Show();
        }

        private void ViolationsTable_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            violationsAdminViewModel.curViolation = e.AddedItems[0] as ViolationDto;
        }
        #endregion
    }
}
