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
using Client.View.Admin.PersonTabSubWIndows;
using System.ComponentModel;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Core;

namespace Client.View.Admin {
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window {
        public AdminDashboard() {
            InitializeComponent();
            notifier = new Notifier(cfg => {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: this,
                    corner: Corner.TopRight,
                    offsetX: 10,  
                    offsetY: 10);

                cfg.DisplayOptions.TopMost = false;

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = this.Dispatcher;
            });
        }

        public Notifier notifier;
        private EmployeesViewModel employeesViewModel;
        private UsersViewModel usersViewModel;
        private ViolationsAdminViewModel violationsAdminViewModel;
        private PersonsViewModel personsViewModel;
        private PaymentsViewModel paymentsViewModel;
        private ViolationTypesViewModel violationTypesViewModel;
        public bool closedByExit = false;

        #region Common
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!(e.Source is TabControl)) return;
            switch((e.AddedItems[0] as TabItem).Name) {
                case "EmployeesTab":
                    if (EmployeesTabGrid.DataContext == null) {
                        employeesViewModel = new EmployeesViewModel();
                        EmployeesTabGrid.DataContext = employeesViewModel;
                    }
                    break;
                case "UsersTab":
                    if (UsersTabGrid.DataContext == null) {
                        usersViewModel = new UsersViewModel();
                        UsersTabGrid.DataContext = usersViewModel;
                    }
                    break;
                case "ViolationsTab":
                    if (ViolationsTabGrid.DataContext == null) {
                        violationsAdminViewModel = new ViolationsAdminViewModel();
                        ViolationsTabGrid.DataContext = violationsAdminViewModel;
                    }
                    break;
                case "PersonsTab":
                    if (PersonsTabGrid.DataContext == null) {
                        personsViewModel = new PersonsViewModel();
                        personsViewModel.PropertyChanged += OnPenaltyChanged;
                        PersonsTabGrid.DataContext = personsViewModel;
                    }
                    break;
                case "PaymentsTab":
                    paymentsViewModel = new PaymentsViewModel();
                    PaymentsTabGrid.DataContext = paymentsViewModel;
                    break;
                case "ViolationTypesTab":
                    if (ViolationTypesTabGrid.DataContext == null) {
                        violationTypesViewModel = new ViolationTypesViewModel();
                        ViolationTypesTabGrid.DataContext = violationTypesViewModel;
                    }
                    notifier.ShowInformation("test");
                    break;
                case "ExitTab":
                    MainWindow main = new MainWindow();
                    main.Show();
                    closedByExit = true;
                    this.Close();
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
                if (item is Grid) {
                    foreach (var item2 in (item as Grid).Children) {
                        ClearStackPanel(item2 as FrameworkElement);
                    }
                } else {
                    ClearStackPanel(item as FrameworkElement);
                }
            }
        }

        private void ClearStackPanel(FrameworkElement item) {
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

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            if (!closedByExit) Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem)) {
                EmployeeTable.UnselectAll();
                UserTable.UnselectAll();
                ViolationsTable.UnselectAll();
                PersonTable.UnselectAll();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Escape:
                    EmployeeTable.UnselectAll();
                    UserTable.UnselectAll();
                    ViolationsTable.UnselectAll();
                    PersonTable.UnselectAll();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Employees tab
        private void EmployeeTable_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (EmployeeTable.SelectedItems.Count == 1) {
                employeesViewModel.CurrentEmployee = EmployeeTable.SelectedItems[0] as EmployeeDto;
            }
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e) {
            EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(employeesViewModel.CurrentEmployee, this);

            this.IsEnabled = false;

            editEmployeeWindow.Show();

        }

        private void SeeAddedViolationsButton_Click(object sender, RoutedEventArgs e) {
            EmployeeAddedViolationsWindow window = new EmployeeAddedViolationsWindow(employeesViewModel.EmployeeAddedViolations, this);

            this.IsEnabled = false;

            window.Show();
        }

        private void SeeDoneShiftsButton_Click(object sender, RoutedEventArgs e) {
            EmployeeDoneShiftsWindow window = new EmployeeDoneShiftsWindow(employeesViewModel.EmployeeDoneShifts, this);

            this.IsEnabled = false;

            window.Show();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e) {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(employeesViewModel.Employees, this);

            this.IsEnabled = false;

            addEmployeeWindow.Show();
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
            if (ViolationsTable.SelectedItems.Count == 1) {
                violationsAdminViewModel.curViolation = ViolationsTable.SelectedItems[0] as ViolationDto;
            }
        }

        private void SeeViolatorProfileBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;
            ViolatorProfileWindow violatorProfileWindow = new ViolatorProfileWindow(this);
            violatorProfileWindow.DataContext = violationsAdminViewModel.CurPerson;
            violatorProfileWindow.Show();
        }
        #endregion

        #region Persons
        private void AddPersonBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;

            AddPersonWindow addPersonWindow = new AddPersonWindow(personsViewModel.Persons, this);

            addPersonWindow.Show();
        }

        private void EditPersonButton_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;

            EditPersonWindow editPersonWindow = new EditPersonWindow(personsViewModel.curSelectedPerson, this);

            editPersonWindow.Show();
        }

        private void PersonTable_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (PersonTable.SelectedItems.Count == 1) {
                personsViewModel.curSelectedPerson = PersonTable.SelectedItems[0] as PersonDto;
            }
        }

        private void SeePaymentsBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;

            PaymentsWindow paymentsWindow = new PaymentsWindow(personsViewModel.CurrentPersonPayments, this);

            paymentsWindow.Show();
        }

        private void SeePersonsViolationsBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;

            PersonsViolations personsViolations = new PersonsViolations(personsViewModel.CurrentPersonViolations, this);

            personsViolations.Show();
        }

        private void OnPenaltyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(personsViewModel.MinPaidPenaltySearch) || 
                e.PropertyName == nameof(personsViewModel.MinActualPenaltySearch) ||
                e.PropertyName == nameof(personsViewModel.MaxPaidPenaltySearch) ||
                e.PropertyName == nameof(personsViewModel.MaxActualPenaltySearch)) {
                MinPaidPenaltyField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                MinActualPenaltyField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                MaxPaidPenaltyField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                MaxActualPenaltyField.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            }
        }
        #endregion

        public void ShowNotification(List<PersonDto> debtors) {
            MessageOptions opts = new MessageOptions() {
                NotificationClickAction = new Action<NotificationBase>((notificationBase) => {
                    MainTabControl.SelectedItem = PersonsTab;

                    personsViewModel.Persons.Clear();
                    debtors.ForEach(val => personsViewModel.Persons.Add(val));

                    notificationBase.Close();
                })
            };
            notifier?.ShowWarning("Просрочены сроки по уплате штрафов. Количество людей: " + debtors.Count, opts);
        }
    }
}
