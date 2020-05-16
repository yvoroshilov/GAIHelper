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
    public partial class StatisticsWindow : Window {
        public StatisticsWindow(List<ViolationDto> violations, Window parent) {
            InitializeComponent();
            DataContext = this;
            this.parent = parent;

            List<IGrouping<int, ViolationDto>> violationsGrouped = violations
                .GroupBy(val => (int)Math.Ceiling(val.penalty / 30))
                .ToList();


            var config = Mappers.Xy<IGrouping<int, ViolationDto>>()
                .X(val => val.Key)
                .Y(val => val.Count());

            Series = new SeriesCollection(config) {
                new ColumnSeries {
                    Values = new ChartValues<IGrouping<int, ViolationDto>>(violationsGrouped)
                }
            };

            Formatter = value => {
                int val = (int)value * INTERVAL;
                return $"{val - INTERVAL} - {val} р.";
            };
        }

        private static readonly int INTERVAL = 30;
        private Window parent;
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection Series { get; set; }

        protected override void OnClosed(EventArgs e) {
            parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
