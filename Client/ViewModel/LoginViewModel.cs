using Client.MainService;
using Client.Model;
using Client.Util;
using Client.View.Admin;
using Client.View.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.ViewModel {
    public class LoginViewModel : ViewModel {

        private AdminServiceClient client = new AdminServiceClient();
        public event EventHandler ClosingRequest;

        private string login;
        [InputProperty(true)]
        public string Login {
            get {
                return login;
            }
            set {
                login = value;
                OnPropertyChanged();
            }
        }


        private RelayCommand loginCommand;
        public RelayCommand LoginCommand {
            get {
                return loginCommand ??
                    (loginCommand = new RelayCommand(obj => {
                        UserDto user = client.GetUserByLogin(Login);
                        PasswordBox passwordBox = (PasswordBox)obj;

                        if (user == null || user.password != passwordBox.Password) {
                            MessageBox.Show("Неверно введён логин или пароль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                            passwordBox.Password = "";
                            return;
                        }

                        Window dashboard = null;
                        if (user.role == "ROLE_USER") {
                            dashboard = new UserDashboard();
                        } else if (user.role == "ROLE_ADMIN") {
                            dashboard = new AdminDashboard();
                        }
                        dashboard.Show();
                        ClosingRequest(null, null);
                        
                    }, obj => {
                        return IsAllRequiredFieldsFilled() && (obj as PasswordBox).Password != "";
                    }));
            }
        }
    }
}
