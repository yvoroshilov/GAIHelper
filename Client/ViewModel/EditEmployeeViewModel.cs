using Client.MainService;
using Client.Util;
using Client.View.Admin.EmployeesTabSubWindows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModel {
    public class EditEmployeeViewModel : ViewModel, IDataErrorInfo {

        private static readonly string addMark = "addMark";

        private AdminServiceClient client;
        private EmployeeDto employee;

        #region Input fields
        private int certificateIdAdd;
        [InputProperty(true, Mark = "addMark")]
        public int CertificateIdAdd {
            get {
                return certificateIdAdd;
            }
            set {
                certificateIdAdd = value;
                OnPropertyChanged();
            }
        }

        private string loginAdd;
        [InputProperty(true, Mark = "addMark")]
        public string LoginAdd {
            get {
                return loginAdd;
            }
            set {
                loginAdd = value;
                OnPropertyChanged();
            }
        }

        private DateTime hireDateAdd;
        [InputProperty(true, Mark = "addMark")]
        public DateTime HireDateAdd {
            get {
                return hireDateAdd == default ? DateTime.Now : hireDateAdd;
            }
            set {
                hireDateAdd = value;
                OnPropertyChanged();
            }
        }

        private string surnameAdd;
        [InputProperty(true, Mark = "addMark")]
        public string SurnameAdd {
            get {
                return surnameAdd;
            }
            set {
                surnameAdd = value;
                OnPropertyChanged();
            }
        }

        private string nameAdd;
        [InputProperty(true, Mark = "addMark")]
        public string NameAdd {
            get {
                return nameAdd;
            }
            set {
                nameAdd = value;
                OnPropertyChanged();
            }
        }

        private string patronymicAdd;
        [InputProperty(true, Mark = "addMark")]
        public string PatronymicAdd {
            get {
                return patronymicAdd;
            }
            set {
                patronymicAdd = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private RelayCommand acceptEditCommand;
        public RelayCommand AcceptEditCommand {
            get {
                return acceptEditCommand ??
                    (acceptEditCommand = new RelayCommand(obj => {
                        if (client.GetUser(LoginAdd) == null) {
                            MessageBox.Show("Аккаунта с логином " + LoginAdd + " не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            LoginAdd = "";
                            return;
                        }

                        employee.certificateId = CertificateIdAdd;
                        employee.userLogin = LoginAdd;
                        employee.hireDate = HireDateAdd;
                        employee.surname = SurnameAdd;
                        employee.patronymic = PatronymicAdd;
                        employee.name = NameAdd;

                        client.EditEmployee(employee);

                        (obj as Window).Close();
                    }, obj => {
                        return IsAllRequiredFieldsFilled(addMark);
                    }));
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName] {
            get {
                string error = "";

                if (!IsAllRequiredFieldsFilled()) {
                    return "Это поле должно быть заполнено";
                }

                switch (columnName) {
                    case nameof(CertificateIdAdd):
                        break;
                    case nameof(LoginAdd):
                        foreach (var chr in LoginAdd) {
                            if (!Char.IsLetterOrDigit(chr)) {
                                error = "Логин может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case nameof(SurnameAdd):
                        foreach (var chr in SurnameAdd) {
                            if (!Char.IsLetter(chr)) {
                                error = "Фамилия может содержать только буквы";
                                break;
                            }
                        }
                        break;
                    case nameof(NameAdd):
                        foreach (var chr in NameAdd) {
                            if (!Char.IsLetter(chr)) {
                                error = "Имя может содержать только буквы";
                                break;
                            }
                        }
                        break;
                    case nameof(PatronymicAdd):
                        foreach (var chr in PatronymicAdd) {
                            if (!Char.IsLetter(chr)) {
                                error = "Фамилия может содержать только буквы";
                                break;
                            }
                        }
                        break;
                }
                return error;
            }
        }

        public EditEmployeeViewModel(EmployeeDto employee) {
            InstanceContext cntxt = new InstanceContext(new DummyCallbackClass());
            client = new AdminServiceClient(cntxt);

            CertificateIdAdd = employee.certificateId;
            LoginAdd = employee.userLogin;
            HireDateAdd = employee.hireDate;
            SurnameAdd = employee.surname;
            PatronymicAdd = employee.patronymic;
            NameAdd = employee.name;

            this.employee = employee;
        }
    }
}
