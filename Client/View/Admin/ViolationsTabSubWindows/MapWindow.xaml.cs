using Client.MainService;
using Client.ViewModel;
using System;
using System.Collections;
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

namespace Client.View.Admin.ViolationsTabSubWindows {
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window {
        public MapWindow(ObservableCollection<ViolationDto> violations, Window parent) {
            InitializeComponent();
            DataContext = new MapWindowViewModel(violations);
            dataContext = DataContext as MapWindowViewModel;
            foreach (var item in dataContext.Violations) {
                ViolationsTable.SelectedItems.Add(item);
            }
            this.parent = parent;
        }

        Window parent;
        MapWindowViewModel dataContext;

        private void ViolationsTable_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            dataContext.AcceptCommand.Execute(ViolationsTable.SelectedItems);
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (ZoomIndicator != null) {
                ZoomIndicator.Content = ZoomSlider.Value != -1 ? e.NewValue.ToString() : "Авто";
            }
            ZoomSlider.SelectionEnd = e.NewValue;
        }

        private void ZoomSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e) {
            if (ZoomSlider.Value != dataContext.Zoom) {
                dataContext.Zoom = (int)ZoomSlider.Value;
                dataContext.AcceptCommand.Execute(ViolationsTable.SelectedItems);
            }
        }

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
