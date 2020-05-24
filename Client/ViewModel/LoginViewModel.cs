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

        public LoginViewModel(Button settingsBtn) {
            try {
                Configuration.LoadConfiguration();
            } catch {
                MessageBox.Show("Невозможно получить доступ к конфигурационному файлу. Будут использованы значения по умолчанию", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                settingsBtn.IsEnabled = false;
            }
        }


        private RelayCommand loginCommand;
        public RelayCommand LoginCommand {
            get {
                return loginCommand ??
                    (loginCommand = new RelayCommand(obj => {
                        ClientInstanceProvider.UpdateAddressesFromConfiguration();
                        client = ClientInstanceProvider.GetAdminServiceClient(new Cllbck());

                        UserDto user;
                        try {
                            user = client.GetUser(Login);
                        } catch {
                            MessageBox.Show("Сервер недопступен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        PasswordBox passwordBox = (PasswordBox)obj;

                        if (user == null || user.password != passwordBox.Password) {
                            MessageBox.Show("Неверно введён логин или пароль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                            passwordBox.Password = "";
                            return;
                        }

                        if (user.role == "ROLE_USER") {
                            try {
                                ClientInstanceProvider.GetUserServiceClient().TestUserService();
                            } catch {
                                MessageBox.Show("Сервер недопступен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                        }


                        MainServiceSubscribeState res = client.Subscribe(user.login);
                        if (user.role == "ROLE_USER") {
                            if (client.GetEmployeeByUserLogin(user.login) == null) {
                                MessageBox.Show("Под этой учётной записью не зарегистрировано ни одного работника", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                                passwordBox.Password = "";
                                return;
                            }

                            if (res == MainServiceSubscribeState.NOT_SUBSCRIBED) {
                                MessageBox.Show("Пользователь уже вошёл в программу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                passwordBox.Password = "";
                                return;
                            }
                        }

                        if (user.role == "ROLE_USER") {
                            EmployeeDto empDto = client.GetEmployeeByUserLogin(user.login);
                            ShiftDto curShift = client.GetCurrentShift(empDto.certificateId);
                            UserDashboard userDashboard;
                            if (res == MainServiceSubscribeState.SUBSCRIBE_UPDATED) {
                                List<ViolationDto> recentViolations = client.GetViolationsByShiftId(curShift.id).ToList();
                                userDashboard = new UserDashboard(curShift, recentViolations);
                            } else {
                                userDashboard = new UserDashboard(curShift);
                            }
                            userDashboard.Show();
                        } else if (user.role == "ROLE_ADMIN") {
                            adminDashboard = new AdminDashboard(user.login);
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
