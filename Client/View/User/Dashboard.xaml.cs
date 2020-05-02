﻿using Client.Model;
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


namespace Client.View.User {

    public partial class Dashboard : Window {
        public Dashboard() {
            DataContext = new ViolationsUserViewModel();
            dataContext = (ViolationsUserViewModel)DataContext;
            dataContext.CurrentPerson.PropertyChanged += OnCurrentPersonIdChanged;
            InitializeComponent();
        }

        private readonly ViolationsUserViewModel dataContext;

        private PersonsViolations personsViolationsWindow;

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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                ViolationTable.UnselectAll();
            }

        private void DriverLicenseField_TextChanged(object sender, TextChangedEventArgs e) {
            dataContext.ResetPersonProfile();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Escape:
                    ViolationTable.UnselectAll();
                    break;
                case Key.Delete:
                    if (dataContext.DeleteCommand.CanExecute(ViolationTable.SelectedItems)) {
                        dataContext.DeleteCommand.Execute(ViolationTable.SelectedItems);
                    }
                    break;
                default:
                    break;
            }
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
            if (args.PropertyName != "Id") {
                return;
            }
            if (dataContext.CurrentPerson.Id == 0) {
                ShowPersonsViolationsBtn.IsEnabled = false;
            } else {
                ShowPersonsViolationsBtn.IsEnabled = true;
            }
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e) {
            ExitEditMode();
        }

        private void AcceptEdit_Click(object sender, RoutedEventArgs e) {
            ExitEditMode();
        }

        private void EditViolationBtn_Click(object sender, RoutedEventArgs e) {
            EnterEditMode();
        }

        private void EnterEditMode() {
            ViolationTable.IsEnabled = false;
            this.KeyDown -= Window_KeyDown;
            this.MouseDown -= Window_MouseDown;
            BindingOperations.ClearBinding(AddViolationBtn, Button.CommandProperty);
            AddViolationBtn.IsEnabled = false;
            CancelEditBtn.Visibility = Visibility.Visible;
            AcceptEditBtn.Visibility = Visibility.Visible;
            DeleteViolationBtn.Visibility = Visibility.Hidden;
            EditViolationBtn.Visibility = Visibility.Hidden;
        }

        private void ExitEditMode() {
            ViolationTable.IsEnabled = true;
            this.KeyDown += Window_KeyDown;
            this.MouseDown += Window_MouseDown;
            BindingOperations.SetBinding(AddViolationBtn, Button.CommandProperty, new Binding("AddCommand"));
            AddViolationBtn.IsEnabled = true;;
            CancelEditBtn.Visibility = Visibility.Hidden;
            AcceptEditBtn.Visibility = Visibility.Hidden;
            DeleteViolationBtn.Visibility = Visibility.Visible;
            EditViolationBtn.Visibility = Visibility.Visible;
        }
    }
}
