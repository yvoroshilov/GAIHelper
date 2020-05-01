using Client.MainService;
using Client.View.User;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            AdminServiceClient admin = new AdminServiceClient();
            AdminDto[] test2 = admin.getAllAdmins();
            MessageBox.Show(test2[0].username + ":" + test2[0].password);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }
    }
}
