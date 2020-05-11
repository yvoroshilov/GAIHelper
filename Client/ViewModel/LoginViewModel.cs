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
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Client.ViewModel {
    public class LoginViewModel : ViewModel {

        private AdminServiceClient client;
        public EventHandler ClosingRequest;
        private static AdminDashboard adminDashboard = null;

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

        [CallbackBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, AutomaticSessionShutdown = false)] 
        private class Cllbck : IAdminServiceCallback {
            public void SendPenaltyExpired(PersonDto[] persons) {
                adminDashboard?.ShowNotification(persons.ToList());
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


                        MainServiceSubscribeState res = client.Subscribe(user.login);
                        if (user.role == "ROLE_USER") {
                            if (client.GetEmployeeByUserLogin(user.login) == null) {
                                MessageBox.Show("Под этой учётной записью не зарегистрировано ни одного работника", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                                passwordBox.Password = "";
                                return;
                            }
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

                        if (user.role == "ROLE_USER") {
                            EmployeeDto empDto = client.GetEmployeeByUserLogin(user.login);
                            ShiftDto curShift = client.GetCurrentShift(empDto.certificateId);
                            new UserDashboard(curShift).Show();
                        } else if (user.role == "ROLE_ADMIN") {
                            adminDashboard = new AdminDashboard();
                            adminDashboard.Show();
                        }
                        ClosingRequest(null, null);
                    }, obj => {
                        return IsAllRequiredFieldsFilled() && (obj as PasswordBox).Password != "";
                    }));
            }
        }
    }
}
