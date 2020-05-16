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
        public ObservableCollection<ViolationDto> EmployeeAddedViolations { get; }
        public ObservableCollection<ShiftDto> EmployeeDoneShifts { get; }
        public EmployeeDto CurrentEmployee;

        #region Search fields
        private int? certificateId;
        [InputProperty(true)]
        public int? CertificateId {
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
        #endregion

        #region Commands
        private RelayCommand searchCommand;
        public RelayCommand SearchCommand {
            get {
                return searchCommand ??
                    (searchCommand = new RelayCommand(obj => {
                        EmployeeDto searchedEmpl = new EmployeeDto();
                        if (FindCertificateIdFieldCheckbox) searchedEmpl.certificateId = CertificateId.Value;
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
                        client.GetAllViolationsByResponsibleId(curEmployee.certificateId).ToList().ForEach(val => EmployeeAddedViolations.Add(val));
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

                        EmployeeDoneShifts.Clear();
                        client.GetAllShiftsByResponsibleId(curEmployee.certificateId).ToList().ForEach(val => EmployeeDoneShifts.Add(val));
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }
        #endregion

        public EmployeesViewModel() {
            Employees = new ObservableCollection<EmployeeDto>();
            EmployeeAddedViolations = new ObservableCollection<ViolationDto>();
            EmployeeDoneShifts = new ObservableCollection<ShiftDto>();
            InstanceContext cntxt = new InstanceContext(new DummyCallbackClass());
            client = new AdminServiceClient(cntxt);
            CurrentEmployee = new EmployeeDto();
        }

        #region Error handle
        public string this[string columnName] {
            get {
                string error = "";

                if (!GetAssociatedCheckBox(columnName)) return "";

                var props = GetProps().ToList();
                var prop = props.Where(val => val.Name == columnName).Single();
                if (prop.GetValue(this) == Utility.GetDefault(prop.PropertyType)) {
                    return "Это поле должно быть заполнено";
                }

                switch (columnName) {
                    case nameof(CertificateId):
                        break;
                    case nameof(Login):
                        foreach (var chr in Login) {
                            if (!Char.IsLetterOrDigit(chr)) {
                                error = "Логин может содержать только буквы и цифры";
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
                    case nameof(Name):
                        foreach (var chr in Name) {
                            if (!Char.IsLetter(chr)) {
                                error = "Имя может содержать только буквы";
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
