using Client.MainService;
using System;
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
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;

namespace Client.View.Admin.EmployeesTabSubWindows {
    public partial class AllTimeStatisticsWindow : Window {
        public AllTimeStatisticsWindow(List<ViolationDto> violations, Window parent) {
            InitializeComponent();
            DataContext = this;
            this.parent = parent;

            List<IGrouping<DateTime, ViolationDto>> violationsGrouped = violations
                .GroupBy(val => val.date.Date)
                .OrderBy(val => val.Key)
                .ToList();

            var config = Mappers.Xy<IGrouping<DateTime, ViolationDto>>()
                .X(val => (int)(val.Key.Date.Ticks / TimeSpan.FromDays(1).Ticks))
                .Y(val => val.Count());
            Series = new SeriesCollection(config) {
                new LineSeries {
                    Values = new ChartValues<IGrouping<DateTime, ViolationDto>>(violationsGrouped),
                    LineSmoothness = 0
                }
            };
            Formatter = value => new DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("d");
        }

        private Window parent;
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection Series { get; set; }

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
