using Client.MainService;
using Client.Util;
using Client.View;
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
            InitializeComponent();
            DataContext = new LoginViewModel(SettingsBtn);
            dataContext = (DataContext as LoginViewModel);
            dataContext.ClosingRequest += (s, a) => {
                isClosedApp = false;
                this.Close();
            };
        }

        private readonly LoginViewModel dataContext;
        private bool isClosedApp = true;

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter:
                    if (dataContext.LoginCommand.CanExecute(PasswordField)) {
                        dataContext.LoginCommand.Execute(PasswordField);
                    }
                    break;
            }
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {
            SettingsWindow settingsWindow = new SettingsWindow(Configuration.AdminServiceEndpointAddress, Configuration.UserServiceEndpointAddress, this);    

            this.IsEnabled = false;

            settingsWindow.Show();
        }
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            if (isClosedApp) Application.Current.Shutdown();
        }
    }
}
