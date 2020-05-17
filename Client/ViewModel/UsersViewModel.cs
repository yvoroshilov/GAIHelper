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
    public class UsersViewModel : ViewModel, IDataErrorInfo {

        private const string addMark = "addMark";
        private const string searchMark = "searchMark";
        public AdminServiceClient client;
        public ObservableCollection<UserDto> Users { get; }
        public ObservableCollection<string> Roles { get; }

        #region Input fields
        private string loginSearch;
        [InputProperty(true, Mark = searchMark)]
        public string LoginSearch {
            get {
                return loginSearch;
            }
            set {
                loginSearch = value;
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

        private string roleSearch;
        [InputProperty(true, Mark = searchMark)]
        public string RoleSearch {
            get {
                return roleSearch;
            }
            set {
                roleSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findRoleCheckbox;
        [InputProperty]
        public bool FindRoleCheckbox {
            get {
                return findRoleCheckbox;
            }
            set {
                findRoleCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string loginAdd;
        [InputProperty(true, Mark = addMark)]
        public string LoginAdd {
            get {
                return loginAdd;
            }
            set {
                loginAdd = value;
                OnPropertyChanged();
            }
        }

        private string passwordAdd;
        [InputProperty(true, Mark = addMark)]
        public string PasswordAdd {
            get {
                return passwordAdd;
            }
            set {
                passwordAdd = value;
                OnPropertyChanged();
            }
        }

        private string roleAdd;
        [InputProperty(true, Mark = addMark)]
        public string RoleAdd {
            get {
                return roleAdd;
            }
            set {
                roleAdd = value;
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
                        if (client.GetUser(LoginAdd) != null) {
                            MessageBox.Show("Такой логин уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        UserDto user = new UserDto();
                        user.login = LoginAdd;
                        user.password = PasswordAdd;
                        user.role = RoleAdd;

                        client.AddUser(user);
                        Users.Add(user);
                        ResetForm(addMark);
                    }, obj => {
                        return IsAllRequiredFieldsFilled(addMark);
                    }));
            }
        }

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand {
            get {
                return searchCommand ??
                    (searchCommand = new RelayCommand(obj => {
                        UserDto searchedUser = new UserDto();
                        searchedUser.login = LoginSearch;
                        searchedUser.role = RoleSearch;
                        List<UserDto> found = client.SearchUsers(searchedUser).ToList();
                        Users.Clear();
                        found.ForEach(val => Users.Add(val));
                        ResetForm("serachMark");
                    }, obj => {
                        return IsAllInputPropsValid(this, searchMark);
                    }));
            }
        }

        private RelayCommand editPasswordCommand;
        public RelayCommand EditPasswordCommand {
            get {
                return editPasswordCommand ??
                    (editPasswordCommand = new RelayCommand(obj => {
                        List<UserDto> selectedUsers = new List<UserDto>((obj as ICollection).Cast<UserDto>());
                        UserDto curUser = selectedUsers.Single();

                        LoginAdd = curUser.login;
                        PasswordAdd = curUser.password;
                        RoleAdd = curUser.role;
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }

        private RelayCommand acceptEditCommand;
        public RelayCommand AcceptEditCommand {
            get {
                return acceptEditCommand ??
                    (acceptEditCommand = new RelayCommand(obj => {
                        UserDto user = new UserDto();
                        user.login = LoginAdd;
                        user.password = PasswordAdd;
                        user.role = RoleAdd;

                        client.EditUser(user);
                        UserDto found = Users.Where(val => user.login == val.login).Single();

                        found.login = LoginAdd;
                        found.password = PasswordAdd;
                        found.role = RoleAdd;
                        
                        (obj as Window).Close();
                        ResetForm(addMark);
                    }, obj => {
                        return IsAllRequiredFieldsFilled(addMark);
                    }));
            }
        }

        private RelayCommand deleteUserCommand;
        public RelayCommand DeleteUserCommand {
            get {
                return deleteUserCommand ??
                    (deleteUserCommand = new RelayCommand(obj => {
                        MessageBoxResult confirmRes = MessageBox.Show("Вы действительно хотите удалить этих пользователей?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                        if (confirmRes == MessageBoxResult.No) {
                            return;
                        }

                        List<UserDto> selectedUsers = new List<UserDto>((obj as ICollection).Cast<UserDto>());
                        foreach (var item in selectedUsers) {
                            Users.Remove(item);
                            client.DeleteUser(item.login);
                        }
                    }, obj => {
                        return (obj as ICollection).Count != 0;
                    }));
            }
        }
        #endregion


        public UsersViewModel() {
            client = ClientInstanceProvider.GetAdminServiceClient();
            Roles = new ObservableCollection<string>(client.GetAllRoles().Select(val => val.role));
            Users = new ObservableCollection<UserDto>();
        }

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
                    case nameof(LoginSearch):
                        break;
                    case nameof(RoleSearch):
                        break;
                    case nameof(RoleAdd):
                        break;
                    case nameof(PasswordAdd):
                        break;
                    case nameof(LoginAdd):
                        break;
                    default:
                        break;
                }
                return error;
            }
        }

        private bool GetAssociatedCheckBox(string columnName) {
            switch (columnName) {
                case nameof(LoginSearch):
                    return FindLoginCheckbox;
                case nameof(RoleSearch):
                    return FindRoleCheckbox;
                default:
                    return true;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
