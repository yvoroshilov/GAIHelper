using Client.MainService;
using Client.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.View {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {
        public SettingsWindow(string adminEndp, string userEndp, Window parent) {
            InitializeComponent();
            AdminServiceEndpointField.Text = adminEndp;
            UserServiceEndpointField.Text = userEndp;
            this.parent = parent;
        }

        Window parent;

        private void SaveBtn_Click(object sender, RoutedEventArgs e) {
            if (!CheckFields()) return;
            string adminEndp = AdminServiceEndpointField.Text;
            string userEndp = UserServiceEndpointField.Text;
            try {
                Configuration.UpdateValue(nameof(Configuration.AdminServiceEndpointAddress), adminEndp);
                Configuration.UpdateValue(nameof(Configuration.UserServiceEndpointAddress), userEndp);
            } catch {
                MessageBox.Show("Невозможно получить доступ к конфигурационному файлу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Сохранено успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckBtn_Click(object sender, RoutedEventArgs e) {
            if (!CheckFields()) return;
            string adminEndp = AdminServiceEndpointField.Text;
            string userEndp = UserServiceEndpointField.Text;
            bool good = true;
            AdminServiceClient adminClient = new AdminServiceClient(
                new InstanceContext(new ClientInstanceProvider.DummyCallbackClass()),
                new NetTcpBinding(),
                new EndpointAddress(adminEndp));
            try {
                adminClient.TestAdminService();
            } catch {
                MessageBox.Show("AdminServiceEndpoint: сервис недоступен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                good = false;
            }

            UserServiceClient userClient = new UserServiceClient(
                new NetTcpBinding(),
                new EndpointAddress(userEndp));
            try {
                userClient.TestUserService();
            } catch {
                MessageBox.Show("UserServiceEndpoint: сервис недоступен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                good = false;
            }

            if (good) {
                MessageBox.Show("Оба сервиса доступны", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CheckFields() {
            string adminEndp = AdminServiceEndpointField.Text;
            string userEndp = UserServiceEndpointField.Text;
            if (!Regex.IsMatch(adminEndp, @"^(net\.tcp\:\/\/)\S+")) {
                MessageBox.Show("AdminServiceEndpoint должен начинаться с net.tcp://", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(userEndp, @"^(net\.tcp\:\/\/)\S+")) {
                MessageBox.Show("UserServiceEndpoint должен начинаться с net.tcp://", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void DefaultsBtn_Click(object sender, RoutedEventArgs e) {
            AdminServiceEndpointField.Text = ClientInstanceProvider.DefaultAdminServiceEndpointAddress;
            UserServiceEndpointField.Text = ClientInstanceProvider.DefaultUserServiceEndpointAddress;
        }

        protected override void OnClosed(EventArgs e) {
            this.parent.IsEnabled = true;
            base.OnClosed(e);
        }
    }
}
