using Client.MainService;
using Client.Model;
using Client.Util;
using Client.View.Admin;
using Client.View.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.ViewModel {
    public class LoginViewModel : ViewModel {

        private AdminServiceClient client;
        public EventHandler ClosingRequest;

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

        private  class Cllbck : IAdminServiceCallback {
            public string Test(string str) {
                return "zdarova";
            }
        }

        public LoginViewModel() {
            InstanceContext cntx = new InstanceContext(new Cllbck());
            client = new AdminServiceClient(cntx);
        }


        private RelayCommand loginCommand;
        public RelayCommand LoginCommand {
            get {
                return loginCommand ??
                    (loginCommand = new RelayCommand(obj => {
                        UserDto user = client.GetUser(Login);
                        PasswordBox passwordBox = (PasswordBox)obj;

                        if (user == null || user.password != passwordBox.Password) {
                            MessageBox.Show("Неверно введён логин или пароль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                            passwordBox.Password = "";
                            return;
                        }


                        if (user.role == "ROLE_USER") {
                            if (client.GetEmployeeByUserLogin(user.login) == null) {
                                MessageBox.Show("Под этой учётной записью не зарегистрировано ни одного работника", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                                passwordBox.Password = "";
                                return;
                            }
                            MainServiceSubscribeState res = client.Subscribe(user.login);
                            switch (res) {
                                case MainServiceSubscribeState.SUBSCRIBED:
                                    break;
                                case MainServiceSubscribeState.SUBSCRIBE_UPDATED:
                                    break;
                                case MainServiceSubscribeState.NOT_SUBSCRIBED:
                                    MessageBox.Show("Пользователь уже вошёл в программу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                    passwordBox.Password = "";
                                    return;
                                default:
                                    break;
                            }
                        }

                        Window dashboard = null;
                        if (user.role == "ROLE_USER") {
                            EmployeeDto empDto = client.GetEmployeeByUserLogin(user.login);
                            ShiftDto curShift = client.GetCurrentShift(empDto.certificateId);
                            dashboard = new UserDashboard(curShift);
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
