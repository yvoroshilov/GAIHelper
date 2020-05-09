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
    public class EmployeesViewModel : ViewModel, IDataErrorInfo {
        private const string addMark = "addMark";
        private AdminServiceClient client;
        public ObservableCollection<EmployeeDto> Employees { get; }
        public List<ViolationDto> EmployeeAddedViolations { get; }
        public List<ShiftDto> EmployeeDoneShifts { get; }

        #region Search fields
        private int certificateId;
        [InputProperty(true)]
        public int CertificateId {
            get {
                return certificateId;
            }
            set {
                certificateId = value;
                OnPropertyChanged();
            }
        }

        private bool findCertificateIdFieldCheckbox;
        [InputProperty]
        public bool FindCertificateIdFieldCheckbox {
            get {
                return findCertificateIdFieldCheckbox;
            }
            set {
                findCertificateIdFieldCheckbox = value;
                OnPropertyChanged();
            }
        }

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

        private bool findLoginCheckbox;
        [InputProperty]
        public bool FindLoginCheckbox {
            get {
                return findLoginCheckbox;
            }
            set {
                findLoginCheckbox = value;
                OnPropertyChanged();
            }
        }

        private DateTime hireDate;
        [InputProperty(true)]
        public DateTime HireDate {
            get {
                return hireDate == default ? DateTime.Now : hireDate;
            }
            set {
                hireDate = value;   
                OnPropertyChanged();
            }
        }

        private bool findHireDateCheckbox;
        [InputProperty]
        public bool FindHireDateCheckbox {
            get {
                return findHireDateCheckbox;
            }
            set {
                findHireDateCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string name;
        [InputProperty(true)]
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                OnPropertyChanged();
            }
        }

        private bool findNameCheckbox;
        [InputProperty]
        public bool FindNameCheckbox {
            get {
                return findNameCheckbox;
            }
            set {
                findNameCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string surname;
        [InputProperty(true)]
        public string Surname {
            get {
                return surname;
            }
            set {
                surname = value;
                OnPropertyChanged();
            }
        }

        private bool findSurnameCheckbox;
        [InputProperty]
        public bool FindSurnameCheckbox {
            get {
                return findSurnameCheckbox;
            }
            set {
                findSurnameCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string patronymic;
        [InputProperty(true)]
        public string Patronymic {
            get {
                return patronymic;
            }
            set {
                patronymic = value;
                OnPropertyChanged();
            }
        }
        
        private bool findPatronymicCheckbox;
        [InputProperty]
        public bool FindPatronymicCheckbox {
            get {
                return findPatronymicCheckbox;
            }
            set {
                findPatronymicCheckbox = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Add input fields
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

        #region Commands
        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ??
                    (addCommand = new RelayCommand(obj => {
                        if (client.GetUser(LoginAdd) == null) {
                            MessageBox.Show("Аккаунта с логином " + LoginAdd + " не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            LoginAdd = "";
                            return;
                        }

                        if (client.GetEmployeeByUserLogin(LoginAdd) != null) {
                            MessageBox.Show("Под логином " + LoginAdd + " уже зарегистрирован пользователь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            LoginAdd = "";
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

                        (obj as Window).Close();
                    }, obj => {
                        return IsAllRequiredFieldsFilled("addMark") && IsAllInputPropsValid(this, addMark);
                    }));
            }
        }

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand {
            get {
                return searchCommand ??
                    (searchCommand = new RelayCommand(obj => {
                        EmployeeDto searchedEmpl = new EmployeeDto();
                        if (FindCertificateIdFieldCheckbox) searchedEmpl.certificateId = CertificateId;
                        if (FindLoginCheckbox) searchedEmpl.userLogin = Login;
                        if (FindHireDateCheckbox) searchedEmpl.hireDate = HireDate;
                        if (FindNameCheckbox) searchedEmpl.name = Name;
                        if (FindSurnameCheckbox) searchedEmpl.surname = Surname;
                        if (FindPatronymicCheckbox) searchedEmpl.patronymic = Patronymic;
                        
                        List<EmployeeDto> found = client.SearchEmployees(searchedEmpl).ToList();
                        Employees.Clear();
                        found.ForEach(val => Employees.Add(val));
                    }, obj => {
                        return IsAllInputPropsValid(this);
                    }));
            }
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand {
            get {
                return editCommand ??
                    (editCommand = new RelayCommand(obj => {
                        List<EmployeeDto> selectedEmployee = new List<EmployeeDto>((obj as ICollection).Cast<EmployeeDto>());
                        EmployeeDto curEmployee = selectedEmployee.Single();
                        CertificateIdAdd = curEmployee.certificateId;
                        LoginAdd = curEmployee.userLogin;
                        HireDateAdd = curEmployee.hireDate;
                        SurnameAdd = curEmployee.surname;
                        PatronymicAdd = curEmployee.patronymic;
                        NameAdd = curEmployee.name;
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand {
            get {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand(obj => {
                        MessageBoxResult confirmRes = MessageBox.Show("Вы действительно хотите удалить этих сотрудников?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                        if (confirmRes == MessageBoxResult.No) {
                            return;
                        }
                        
                        List<EmployeeDto> selectedEmployees = new List<EmployeeDto>((obj as ICollection).Cast<EmployeeDto>());
                        foreach (var item in selectedEmployees) {
                            Employees.Remove(item);
                            client.DeleteEmployee(item.certificateId);
                        }
                    }, obj => {
                        return (obj as ICollection).Count != 0;
                    }));
            }
        }

        private RelayCommand acceptEditCommand;
        public RelayCommand AcceptEditCommand {
            get {
                return acceptEditCommand ??
                    (acceptEditCommand = new RelayCommand(obj => {
                        EmployeeDto addedEmpl = new EmployeeDto();
                        
                        if (client.GetUser(LoginAdd) == null) {
                            MessageBox.Show("Аккаунта с логином " + LoginAdd + " не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            LoginAdd = "";
                            return;
                        }

                        addedEmpl.certificateId = CertificateIdAdd;
                        addedEmpl.userLogin = LoginAdd;
                        addedEmpl.hireDate = HireDateAdd;
                        addedEmpl.surname = SurnameAdd;
                        addedEmpl.patronymic = PatronymicAdd;
                        addedEmpl.name = NameAdd;

                        client.EditEmployee(addedEmpl);
                        EmployeeDto found = Employees.Where(val => addedEmpl.certificateId == val.certificateId).Single();

                        found.userLogin = LoginAdd;
                        found.hireDate = HireDateAdd;
                        found.surname = SurnameAdd;
                        found.patronymic = PatronymicAdd;
                        found.name = NameAdd;

                        (obj as Window).Close();
                    }, obj => {
                        return IsAllRequiredFieldsFilled("addMark");
                    }));
            }
        }

        private RelayCommand clearFormCommand;
        public RelayCommand ClearFormCommand {
            get {
                return clearFormCommand ??
                    (clearFormCommand = new RelayCommand(obj => {
                        ResetForm();
                    }));
            }
        }

        private RelayCommand seeAddedViolationsCommand;
        public RelayCommand SeeAddedViolationsCommand { get {
                return seeAddedViolationsCommand ?? 
                    (seeAddedViolationsCommand = new RelayCommand(obj => {
                        List<EmployeeDto> selectedEmployee = new List<EmployeeDto>((obj as ICollection).Cast<EmployeeDto>());
                        EmployeeDto curEmployee = selectedEmployee.Single();

                        EmployeeAddedViolations.Clear();
                        EmployeeAddedViolations.AddRange(client.GetAllViolationsByResponsibleId(curEmployee.certificateId));
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }

        private RelayCommand seeEmployeeDoneShiftsCommand;
        public RelayCommand SeeEmployeeDoneShiftsCommand {
            get {
                return seeEmployeeDoneShiftsCommand ??
                    (seeEmployeeDoneShiftsCommand = new RelayCommand(obj => {
                        List<EmployeeDto> selectedEmployee = new List<EmployeeDto>((obj as ICollection).Cast<EmployeeDto>());
                        EmployeeDto curEmployee = selectedEmployee.Single();

                        EmployeeAddedViolations.Clear();
                        EmployeeDoneShifts.AddRange(client.GetAllShiftsByResponsibleId(curEmployee.certificateId));
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }
        #endregion

        public EmployeesViewModel() {
            Employees = new ObservableCollection<EmployeeDto>();
            EmployeeAddedViolations = new List<ViolationDto>();
            EmployeeDoneShifts = new List<ShiftDto>();
            InstanceContext cntxt = new InstanceContext(new DummyCallbackClass());
            client = new AdminServiceClient(cntxt);
        }

        #region Error handle
        public string this[string columnName] {
            get {
                string error = "";

                if (!GetAssociatedCheckBox(columnName)) return "";

                var props = GetProps().ToList();
                var prop = props.Where(val => val.Name == columnName).Single();
                if (prop.GetValue(this)?.Equals(Utility.GetDefault(prop.PropertyType)) ?? true) {
                    return "Это поле должно быть заполнено";
                }

                switch (columnName) {
                    case nameof(CertificateIdAdd):
                        break;
                    case nameof(CertificateId):
                        break;
                    case nameof(LoginAdd):
                        foreach (var chr in LoginAdd) {
                            if (!Char.IsLetterOrDigit(chr)) {
                                error = "Логин может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case nameof(Login):
                        foreach (var chr in Login) {
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
                    case nameof(Surname):
                        foreach (var chr in Surname) {
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
                    case nameof(Name):
                        foreach (var chr in Name) {
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
                    case nameof(Patronymic):
                        foreach (var chr in Patronymic) {
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

        private bool GetAssociatedCheckBox(string columnName) {
            switch (columnName) {
                case nameof(CertificateId):
                    return FindCertificateIdFieldCheckbox;
                case nameof(Login):
                    return FindLoginCheckbox;
                case nameof(HireDate):
                    return FindHireDateCheckbox;
                case nameof(Name):
                    return FindNameCheckbox;
                case nameof(Surname):
                    return FindSurnameCheckbox;
                case nameof(Patronymic):
                    return FindPatronymicCheckbox;
                default:
                    return true;
            }
        }

        public string Error => throw new NotImplementedException();
        #endregion
    }
}
