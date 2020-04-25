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
            AdminDto[] test = null;
            try {
                test = Class1.Test();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            /*
            AdminServiceClient client = new AdminServiceClient();
            AdminDto[] admins2 = null;
            try { 
            admins2 = (await client.getAllAdminsAsync());
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show(admins2[2].username);
            */
            
        }
    }
}
