using Client.Model;
using Client.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using System.Windows.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Collections;
using Client.MainService;
using System.ServiceModel;

namespace Client.ViewModel {
    public class AddPersonViewModel : ViewModel, IDataErrorInfo {

        private AdminServiceClient client;
        private ObservableCollection<PersonDto> persons;

        #region Fields
        private string passportId;
        [InputProperty(true)]
        public string PassportId {
            get {
                return passportId;
            }
            set {
                passportId = value;
                OnPropertyChanged();
            }
        }

        private string driverLicense;
        [InputProperty(true)]
        public string DriverLicense {
            get {
                return driverLicense;
            }
            set {
                driverLicense = value;
                OnPropertyChanged();
            }
        }

        private DateTime birthday;
        [InputProperty(true)]
        public DateTime Birthday {
            get {
                return birthday == default ? DateTime.Now : birthday;
            }
            set {
                birthday = value;
                OnPropertyChanged();
            }
        }

        private double paidPenalty;
        [InputProperty(true)]
        public double PaidPenalty {
            get {
                return paidPenalty;
            }
            set {
                paidPenalty = value;
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

        private double actualPenalty;
        [InputProperty(true)]
        public double ActualPenalty {
            get {
                return actualPenalty;
            }
            set {
                actualPenalty = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ??
                    (addCommand = new RelayCommand(obj => {
                        PersonDto person = new PersonDto();
                        person.passportId = PassportId;
                        person.driverLicense = DriverLicense;
                        person.birthday = Birthday;
                        person.paidPenalty = PaidPenalty;
                        person.surname = Surname;
                        person.name = Name;
                        person.patronymic = Patronymic;
                        person.actualPenalty = ActualPenalty;

                        PersonDto added = client.AddPerson(person);
                        persons.Add(added);
                        ResetForm();
                    }, obj => {
                        return IsAllRequiredFieldsFilled() && IsAllInputPropsValid(this);
                    }));
            }
        }

        public AddPersonViewModel(ObservableCollection<PersonDto> col) {
            InstanceContext cntxt = new InstanceContext(new DummyCallbackClass());
            client = new AdminServiceClient(cntxt);

            persons = col;
        }
        
        public string this[string columnName] {
            get {
                string error = "";

                var props = GetProps().ToList();
                var prop = props.Where(val => val.Name == columnName).Single();
                if (prop.GetValue(this) == Utility.GetDefault(prop.PropertyType)) {
                    return "Это поле должно быть заполнено";
                }

                switch (columnName) {
                    case nameof(PassportId):
                        foreach (var ch in PassportId) {
                            if (!char.IsLetterOrDigit(ch)) {
                                error = "Номер паспорта может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case nameof(DriverLicense):
                        foreach (var ch in DriverLicense) {
                            if (!char.IsLetterOrDigit(ch)) {
                                error = "Номер ВУ может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case nameof(Birthday):
                        break;
                    case nameof(PaidPenalty):
                        if (PaidPenalty < 0) {
                            error = "Штраф не может быть отрицательным";
                            break;
                        }
                        if (PaidPenalty > ActualPenalty) {
                            error = "Выплаченный штраф не может быть больше текущего";
                            break;
                        }
                        break;
                    case nameof(ActualPenalty):
                        if (ActualPenalty < 0) {
                            error = "Штраф не может быть отрицательным";
                            break;
                        }
                        break;
                    case nameof(Surname):
                        foreach (var ch in Surname) {
                            if (!char.IsLetter(ch)) {
                                error = "Фамилия может содержать только буквы";
                                break;
                            }
                        }
                        break;
                    case nameof(Name):
                        foreach (var ch in Name) {
                            if (!char.IsLetter(ch)) {
                                error = "Имя может содержать только буквы";
                                break;
                            }
                        }
                        break;
                    case nameof(Patronymic):
                        foreach (var ch in Patronymic) {
                            if (!char.IsLetter(ch)) {
                                error = "Фамилия может содержать только буквы";
                                break;
                            }
                        }
                        break;
                }
                return error;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
