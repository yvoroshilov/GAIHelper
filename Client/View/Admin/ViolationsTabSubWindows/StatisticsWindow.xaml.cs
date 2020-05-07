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

namespace Client.View.Admin.ViolationsTabSubWindows {
    public partial class StatisticsWindow : Window {
        public StatisticsWindow(List<ViolationDto> violations, Window parent) {
            InitializeComponent();
            DataContext = this;
            this.violations = violations;
            this.parent = parent;


            var config = Mappers.Xy<ViolationDto>()
                .X(val => val.date.Date.Ticks / TimeSpan.FromDays(1).Ticks)
                .Y(val => CountViolations(val.date));
            Series = new SeriesCollection(config) {
                new ColumnSeries {
                    Values = new ChartValues<ViolationDto>(violations),
                }
            };
            Formatter = value => new DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("d");
        }

        private Window parent;
        private List<ViolationDto> violations;
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection Series { get; set; }

        private int CountViolations(DateTime date) {
            return violations.Where(val => val.date.Date == date.Date).Count();
        }

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
