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
    public class PersonsViewModel : ViewModel, IDataErrorInfo {

        private AdminServiceClient client;
        public ObservableCollection<PersonDto> Persons { get; }
        public PersonDto curSelectedPerson;
        public ObservableCollection<PaymentDto> CurrentPersonPayments { get; }

        #region Search fields
        private string passportIdSearch;
        [InputProperty(true)]
        public string PassportIdSearch {
            get {
                return passportIdSearch;
            }
            set {
                passportIdSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findPassportIdCheckbox;
        [InputProperty]
        public bool FindPassportIdCheckbox {
            get {
                return findPassportIdCheckbox;
            }
            set {
                findPassportIdCheckbox = value;
                OnPropertyChanged();
            }
        }
        
        private string driverLicenseSearch;
        [InputProperty(true)]
        public string DriverLicenseSearch {
            get {
                return driverLicenseSearch;
            }
            set {
                driverLicenseSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findDriverLicenseCheckbox;
        [InputProperty]
        public bool FindDriverLicenseCheckbox {
            get {
                return findDriverLicenseCheckbox;
            }
            set {
                findDriverLicenseCheckbox = value;
                OnPropertyChanged();
            }
        }

        private DateTime birthdaySearch;
        [InputProperty(true)]
        public DateTime BirthdaySearch {
            get {
                return birthdaySearch == default ? DateTime.Now : birthdaySearch;
            }
            set {
                birthdaySearch = value;
                OnPropertyChanged();
            }
        }

        private bool findBrithdayCheckbox;
        [InputProperty]
        public bool FindBirthdayCheckbox {
            get {
                return findBrithdayCheckbox;
            }
            set {
                findBrithdayCheckbox = value;
                OnPropertyChanged();
            }
        }

        private double minPaidPenaltySearch;
        [InputProperty(true)]
        public double MinPaidPenaltySearch {
            get {
                return minPaidPenaltySearch;
            }
            set {
                minPaidPenaltySearch = value;
                OnPropertyChanged();
            }
        }

        private double maxPaidPenaltySearch;
        [InputProperty(true)]
        public double MaxPaidPenaltySearch {
            get {
                return maxPaidPenaltySearch;
            }
            set {
                maxPaidPenaltySearch = value;
                OnPropertyChanged();
            }
        }

        private bool findPaidPenaltyCheckbox;
        [InputProperty]
        public bool FindPaidPenaltyCheckbox {
            get {
                return findPaidPenaltyCheckbox;
            }
            set {
                findPaidPenaltyCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string surnameSearch;
        [InputProperty(true)]
        public string SurnameSearch {
            get {
                return surnameSearch;
            }
            set {
                surnameSearch = value;
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

        private string nameSearch;
        [InputProperty(true)]
        public string NameSearch {
            get {
                return nameSearch;
            }
            set {
                nameSearch = value;
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

        private string patronymicSearch;
        [InputProperty(true)]
        public string PatronymicSearch {
            get {
                return patronymicSearch;
            }
            set {
                patronymicSearch = value;
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

        private double minActualPenaltySearch;
        [InputProperty(true)]
        public double MinActualPenaltySearch {
            get {
                return minActualPenaltySearch;
            }
            set {
                minActualPenaltySearch = value;
                OnPropertyChanged();
            }
        }

        private double maxActualPenaltySearch;
        [InputProperty(true)]
        public double MaxActualPenaltySearch {
            get {
                return maxActualPenaltySearch;
            }
            set {
                maxActualPenaltySearch = value;
                OnPropertyChanged();
            }
        }

        private bool findActualPenaltyCheckbox;
        public bool FindActualPenaltyCheckbox {
            get {
                return findActualPenaltyCheckbox;
            }
            set {
                findActualPenaltyCheckbox = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Command
        private RelayCommand deletePersonCommand;
        public RelayCommand DeletePersonCommand {
            get {
                return deletePersonCommand ??
                    (deletePersonCommand = new RelayCommand(obj => {
                        MessageBoxResult confirmRes = MessageBox.Show("Вы действительно хотите удалить эти профили?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                        if (confirmRes == MessageBoxResult.No) {
                            return;
                        }

                        List<PersonDto> selectedPersons = new List<PersonDto>((obj as ICollection).Cast<PersonDto>());
                        foreach (var item in selectedPersons) {
                            client.DeletePerson(item.id);
                            Persons.Remove(item);
                        }
                    }, obj => {
                        return (obj as ICollection).Count != 0;
                    }));
            }
        }

        private RelayCommand editPersonCommand;
        public RelayCommand EditPersonCommand {
            get {
                return editPersonCommand ??
                    (editPersonCommand = new RelayCommand(obj => {
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }

        private RelayCommand seePaymentsCommand;
        public RelayCommand SeePaymentsCommand {
            get {
                return seePaymentsCommand ??
                    (seePaymentsCommand = new RelayCommand(obj => {
                        List<PersonDto> selectedPersons = new List<PersonDto>((obj as ICollection).Cast<PersonDto>());
                        PersonDto selectedPerson = selectedPersons.Single();

                        CurrentPersonPayments.Clear();
                        client.GetPaymentsByPersonId(selectedPerson.id).ToList().ForEach(val => CurrentPersonPayments.Add(val));
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand {
            get {
                return searchCommand ??
                    (searchCommand = new RelayCommand(obj => {
                        PersonDto searchedPerson = new PersonDto();

                        if (FindPassportIdCheckbox) searchedPerson.passportId = PassportIdSearch;
                        if (FindDriverLicenseCheckbox) searchedPerson.driverLicense = DriverLicenseSearch;
                        if (FindBirthdayCheckbox) searchedPerson.birthday = BirthdaySearch;
                        if (FindSurnameCheckbox) searchedPerson.surname = SurnameSearch;
                        if (FindNameCheckbox) searchedPerson.name = NameSearch;
                        if (FindPatronymicCheckbox) searchedPerson.patronymic = PatronymicSearch;

                        List<PersonDto> result = null;

                        Persons.Clear();
                        if (FindPaidPenaltyCheckbox || FindActualPenaltyCheckbox) {
                            List<PersonDto> partialResult = null;
                            if (FindPaidPenaltyCheckbox) {
                                partialResult = client.SearchPersonsByPaidPenalty(searchedPerson, MinPaidPenaltySearch, MaxPaidPenaltySearch).ToList();

                                if (FindActualPenaltyCheckbox) partialResult = partialResult.Where(val => val.actualPenalty >= MinActualPenaltySearch && val.actualPenalty <= MaxActualPenaltySearch).ToList();
                                result = partialResult;
                            } else {
                                partialResult = client.SearchPersonsByActualPenalty(searchedPerson, MinActualPenaltySearch, MaxActualPenaltySearch).ToList();
                                result = partialResult;
                            }
                        } else {
                            result = client.SearchPersons(searchedPerson).ToList();
                        }
                        result.ForEach(val => Persons.Add(val));
                    }, obj => {
                        return IsAllInputPropsValid(this);
                    }));
            }
        }

        private RelayCommand resetSearchFields;
        public RelayCommand ResetSearchFields {
            get {
                return resetSearchFields ??
                    (resetSearchFields = new RelayCommand(obj => {
                        ResetForm();
                    }));
            }
        }
        #endregion

        public PersonsViewModel() {
            InstanceContext cntxt = new InstanceContext(new DummyCallbackClass());
            client = new AdminServiceClient(cntxt);

            Persons = new ObservableCollection<PersonDto>();
            CurrentPersonPayments = new ObservableCollection<PaymentDto>();
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
                    case nameof(PassportIdSearch):
                        foreach (var ch in PassportIdSearch) {
                            if (!char.IsLetterOrDigit(ch)) {
                                error = "Номер паспорта может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case nameof(DriverLicenseSearch):
                        foreach (var ch in DriverLicenseSearch) {
                            if (!char.IsLetterOrDigit(ch)) {
                                error = "Номер ВУ может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case nameof(BirthdaySearch):
                        break;
                    case nameof(MinPaidPenaltySearch):
                        if (MinPaidPenaltySearch < 0) {
                            error = "Штраф не может быть отрицательным";
                            break;
                        }
                        if (MinPaidPenaltySearch > MaxPaidPenaltySearch) {
                            error = "Минимальный выплаченный штраф не может быть больше текущего";
                            break;
                        }
                        break;
                    case nameof(MaxPaidPenaltySearch):
                        if (MaxPaidPenaltySearch < 0) {
                            error = "Штраф не может быть отрицательным";
                            break;
                        }
                        if (MinPaidPenaltySearch > MaxPaidPenaltySearch) {
                            error = "Минимальный выплаченный штраф не может быть больше текущего";
                            break;
                        }
                        break;
                    case nameof(MinActualPenaltySearch):
                        if (MinActualPenaltySearch < 0) {
                            error = "Штраф не может быть отрицательным";
                            break;
                        }
                        if (MinActualPenaltySearch > MaxPaidPenaltySearch) {
                            error = "Минимальный текущий штраф не может быть больше текущего";
                            break;
                        }
                        break;
                    case nameof(MaxActualPenaltySearch):
                        if (MaxActualPenaltySearch < 0) {
                            error = "Штраф не может быть отрицательным";
                            break;
                        }
                        if (MaxActualPenaltySearch > MaxActualPenaltySearch) {
                            error = "Минимальный текущий штраф не может быть больше текущего";
                            break;
                        }
                        break;
                    case nameof(SurnameSearch):
                        foreach (var ch in SurnameSearch) {
                            if (!char.IsLetter(ch)) {
                                error = "Фамилия может содержать только буквы";
                                break;
                            }
                        }
                        break;
                    case nameof(NameSearch):
                        foreach (var ch in NameSearch) {
                            if (!char.IsLetter(ch)) {
                                error = "Имя может содержать только буквы";
                                break;
                            }
                        }
                        break;
                    case nameof(PatronymicSearch):
                        foreach (var ch in PatronymicSearch) {
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

        private bool GetAssociatedCheckBox(string columnName) {
            switch (columnName) {
                case nameof(PassportIdSearch):
                    return FindPassportIdCheckbox;
                case nameof(DriverLicenseSearch):
                    return FindDriverLicenseCheckbox;
                case nameof(BirthdaySearch):
                    return FindBirthdayCheckbox;
                case nameof(MinPaidPenaltySearch):
                case nameof(MaxPaidPenaltySearch):
                    return FindPaidPenaltyCheckbox;
                case nameof(MinActualPenaltySearch):
                case nameof(MaxActualPenaltySearch):
                    return FindActualPenaltyCheckbox;
                case nameof(SurnameSearch):
                    return FindSurnameCheckbox;
                case nameof(NameSearch):
                    return FindNameCheckbox;
                case nameof(PatronymicSearch):
                    return FindPatronymicCheckbox;
                default:
                    return true;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
