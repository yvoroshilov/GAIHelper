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
    public class AddEmployeeViewModel : ViewModel, IDataErrorInfo  {

        private static readonly string addMark = "addMark";

        private AdminServiceClient client;
        private ObservableCollection<EmployeeDto> Employees { get; }

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

        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ??
                    (addCommand = new RelayCommand(obj => {
                        if (client.GetEmployeeById(CertificateIdAdd) != null) {
                            MessageBox.Show("Сотрудник с номером удостоверения " + CertificateIdAdd + " уже зарегистрирован ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            CertificateIdAdd = default;
                            return;
                        }

                        if (client.GetUser(LoginAdd) == null) {
                            MessageBox.Show("Аккаунта с логином " + LoginAdd + " не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            LoginAdd = null;
                            return;
                        }

                        if (client.GetEmployeeByUserLogin(LoginAdd) != null) {
                            MessageBox.Show("Под логином " + LoginAdd + " уже зарегистрирован пользователь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            LoginAdd = null;
                            return;
                        }

                        EmployeeDto addedEmpl = new EmployeeDto();
                        addedEmpl.certificateId = CertificateIdAdd;
                        addedEmpl.userLogin = LoginAdd;
                        addedEmpl.hireDate = HireDateAdd;
                        addedEmpl.surname = SurnameAdd;
                        addedEmpl.patronymic = PatronymicAdd;
                        addedEmpl.name = NameAdd;
                        client.AddEmployee(addedEmpl);
                        Employees.Add(addedEmpl);
                        ResetForm(addMark);

                    }, obj => {
                        return IsAllRequiredFieldsFilled("addMark") && IsAllInputPropsValid(this, addMark);
                    }));
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName] {
            get {
                string error = "";

                var props = GetProps().ToList();
                var prop = props.Where(val => val.Name == columnName).Single();
                if (prop.GetValue(this) == null) {
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

        public AddEmployeeViewModel(ObservableCollection<EmployeeDto> col) {
            client = ClientInstanceProvider.GetAdminServiceClient();

            Employees = col;
        }
    }
}
