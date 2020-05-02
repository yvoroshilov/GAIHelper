using Client.MainService;
using Client.View.User;
using Client.ViewModel;
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
            DataContext = new LoginViewModel();
            dataContext = (DataContext as LoginViewModel);
            dataContext.ClosingRequest += (s, a) => this.Close();
            InitializeComponent();
        }

        private readonly LoginViewModel dataContext;

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter:
                    if (dataContext.LoginCommand.CanExecute(PasswordField)) {
                        dataContext.LoginCommand.Execute(PasswordField);
                    }
                    break;
            }
        }
    }
}
