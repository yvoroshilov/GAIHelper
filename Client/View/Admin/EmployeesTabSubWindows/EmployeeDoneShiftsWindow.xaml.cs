using Client.MainService;
using Client.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
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
    /// <summary>
    /// Interaction logic for EmployeeDoneShifts.xaml
    /// </summary>
    public partial class EmployeeDoneShiftsWindow : Window {
        public EmployeeDoneShiftsWindow(ObservableCollection<ShiftDto> col, Window parent) {
            InitializeComponent();
            this.col = col;
            shiftAndCount = new ObservableCollection<Tuple<ShiftDto, int>>();
            col.CollectionChanged += Col_CollectionChanged;
            DataContext = shiftAndCount;
            this.parent = parent;
            adminClient = ClientInstanceProvider.GetAdminServiceClient();
        }

        private void Col_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    shiftAndCount.Add(Tuple.Create(e.NewItems[0] as ShiftDto, GetViolationCount((e.NewItems[0] as ShiftDto).id)));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        private Window parent;
        private AdminServiceClient adminClient;
        private ObservableCollection<ShiftDto> col;
        private ObservableCollection<Tuple<ShiftDto, int>> shiftAndCount;

        protected override void OnClosed(EventArgs e) {
            col.CollectionChanged -= Col_CollectionChanged;
            parent.IsEnabled = true;
            base.OnClosed(e);
        }

        private void StatisticsBtn_Click(object sender, RoutedEventArgs e) {
            int shiftId = (ShiftTable.SelectedItems[0] as Tuple<ShiftDto, int>).Item1.id;
            List<ViolationDto> violations = adminClient
                .GetViolationsByShiftId(shiftId)
                .ToList();
            StatisticsWindow window = new StatisticsWindow(violations, this);
            window.Show();
        }

        private void ShiftTable_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ShiftTable.SelectedItems.Count == 1) {
                StatisticsBtn.IsEnabled = true;
            } else {
                StatisticsBtn.IsEnabled = false;
            }
        }

        private int GetViolationCount(int shiftId) {
            AdminServiceClient adminClient = ClientInstanceProvider.GetAdminServiceClient();
            return adminClient
                .GetViolationsByShiftId(shiftId)
                .Count();
        }


    }
}
