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

namespace Client.ViewModel {
    public class ViolationsUserViewModel : ViewModel, IDataErrorInfo {

        #region Common
        private MainService.UserServiceClient client;
        public ObservableCollection<Violation> Violations { get; }
        public ReadOnlyCollection<ViolationType> ViolationTypes { get; }
        #endregion

        #region Input fields
        private ViolationType selectedViolationType;
        [InputProperty(true)]
        public ViolationType SelectedViolationType {
            get {
                return selectedViolationType;
            }
            set {
                selectedViolationType = value;
                OnPropertyChanged();
            }
        }

        private int personId;
        public int PersonId {
            get {
                return personId;
            }
            set {
                personId = value;
                OnPropertyChanged();
            }
        }

        private string carNumber;
        [InputProperty(true)]
        public string CarNumber {
            get {
                return carNumber;
            }
            set {
                carNumber = value;
                OnPropertyChanged();
            }
        }

        private System.DateTime date;
        public System.DateTime Date {
            get {
                return DateTime.Now;
            }
        }

        private double penalty;
        [InputProperty(true)]
        public double Penalty {
            get {
                return penalty;
            }
            set {
                penalty = value;
                OnPropertyChanged();
            }
        }

        private double locationN;
        [InputProperty]
        public double LocationN{
            get {
                return locationN;
            }
            set {
                locationN = value;
                OnPropertyChanged();
            }
        }

        private double locationE;
        [InputProperty]
        public double LocationE {
            get {
                return locationE;
            }
            set {
                locationE = value;
                OnPropertyChanged();
            }
        }

        private string address;
        [InputProperty(true)]
        public string Address {
            get {
                return address;
            }
            set {
                address = value;
                OnPropertyChanged();
            }
        }

        private string description;
        [InputProperty]
        public string Description {
            get {
                return description;
            }
            set {
                description = value;
                OnPropertyChanged();
            }
        }

        private string driverLicenseOrProtocol;
        [InputProperty(true)]
        public string DriverLicenseOrProtocol {
            get {
                return driverLicenseOrProtocol;
            }
            set {
                driverLicenseOrProtocol = value;
                OnPropertyChanged();
            }
        }

        private bool noLic;
        [InputProperty]
        public bool NoLic {
            get {
                return noLic;
            }
            set {
                noLic = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Person info

        private const int NO_LIC_PERSON_ID = 1; 

        private const string NO_LIC_DRIVER_LIC = "NO_LIC";

        private Person currentPerson;
        public Person CurrentPerson {
            get {
                return currentPerson;
            }
            set {
                currentPerson = value;
                OnPropertyChanged();
            }
        }

        public List<Violation> CurrentPersonsViolations { get; set; } = new List<Violation>();
        #endregion

        #region Commands
        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ?? 
                    (addCommand = new RelayCommand(obj => {
                        if (!noLic) {
                            Person pers = Mapper.mapper.Map<Person>(client.GetPerson(DriverLicenseOrProtocol));
                            PersonId = pers.Id;
                        } else {
                            PersonId = NO_LIC_PERSON_ID;
                            Description += " | № Протокола: " + DriverLicenseOrProtocol;
                        }

                        Violation violation = new Violation();
                        violation.ViolationTypeId = SelectedViolationType.Id;
                        violation.PersonId = PersonId;
                        violation.CarNumber = CarNumber;
                        violation.Date = Date;
                        violation.Penalty = Penalty;
                        violation.LocationN = LocationN;
                        violation.LocationE = LocationE;
                        violation.Address = Address;
                        violation.Description = Description;
                        violation.DriverLicenseOrProtocol = DriverLicenseOrProtocol;

                        Violations.Add(violation);
                        ResetPersonProfile();
                        ResetForm();
                    }, (obj) => {
                        return IsAllRequiredFieldsFilled() && (currentPerson.Id != 0 || NoLic);
                    }));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand {
            get {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand(obj => {
                        MessageBoxResult confirmRes = MessageBox.Show("Вы действительно хотите удалить эти нарушения?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                        if (confirmRes == MessageBoxResult.No) {
                            return;
                        }

                        List<Violation> selectedViolations = new List<Violation>((obj as ICollection).Cast<Violation>());
                        foreach (var item in selectedViolations) {
                            Violations.Remove(item);
                        }
                    }, obj => {
                        return (obj as ICollection).Count != 0;
                    }));
            }
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand {
            get {
                return editCommand ??
                    (editCommand = new RelayCommand(obj => {
                        List<Violation> selectedViolations = new List<Violation>((obj as ICollection).Cast<Violation>());
                        Violation curViolation = selectedViolations.First();
                        SelectedViolationType = ViolationTypes.Where(val => val.Id == curViolation.ViolationTypeId).First();
                        CarNumber = curViolation.CarNumber;
                        Penalty = curViolation.Penalty;
                        LocationN = curViolation.LocationN;
                        LocationE = curViolation.LocationE;
                        Address = curViolation.Address;
                        Description = curViolation.Description;
                        DriverLicenseOrProtocol = curViolation.DriverLicenseOrProtocol;
                        if (curViolation.PersonId != NO_LIC_PERSON_ID) {
                            CheckPersonCommand.Execute(null);
                            NoLic = false;
                        } else {
                            NoLic = true;
                        }
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }

        private RelayCommand checkPersonCommand;
        public RelayCommand CheckPersonCommand {
            get {
                return checkPersonCommand ??
                    (checkPersonCommand = new RelayCommand(obj => {
                        MainService.PersonDto personDto = client.GetPerson(DriverLicenseOrProtocol);
                        if (personDto == null) {
                            MessageBox.Show($"Человека с водительским удостоверением № {DriverLicenseOrProtocol} не существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ResetPersonProfile();
                        } else {
                            Mapper.mapper.Map(
                                personDto, 
                                currentPerson, 
                                typeof (MainService.PersonDto),
                                currentPerson.GetType());
                            CurrentPersonsViolations.AddRange(client.GetAllViolations(CurrentPerson.Id)
                                .Select(val => Mapper.mapper.Map<Violation>(val))
                                .ToList());
                        }
                    }, obj => {
                        return DriverLicenseOrProtocol != null;
                    }));
            }
        }

        private RelayCommand cancelEditCommand;
        public RelayCommand CancelEditCommand {
            get {
                return cancelEditCommand ??
                    (cancelEditCommand = new RelayCommand(obj => {
                        ResetForm();
                        ResetPersonProfile();
                    }));
            }
        }

        private RelayCommand acceptEditCommand;
        public RelayCommand AcceptEditCommand {
            get {
                return acceptEditCommand ??
                    (acceptEditCommand = new RelayCommand(obj => {
                        List<Violation> selectedViolations = new List<Violation>((obj as ICollection).Cast<Violation>());
                        Violation curViolation = selectedViolations.First();
                        curViolation.ViolationTypeId = SelectedViolationType.Id;
                        curViolation.CarNumber = CarNumber;
                        curViolation.Penalty = Penalty;
                        curViolation.LocationN = LocationN;
                        curViolation.LocationE = LocationE;
                        curViolation.Address = Address;
                        curViolation.Description = Description;
                        curViolation.DriverLicenseOrProtocol = DriverLicenseOrProtocol;
                        if (NoLic) {
                            curViolation.PersonId = NO_LIC_PERSON_ID;
                        } else {
                            curViolation.PersonId = client.GetPerson(DriverLicenseOrProtocol).id;
                        }
                        client.EditViolation(Mapper.mapper.Map<MainService.ViolationDto>(curViolation));
                        ResetForm();
                    }, obj => {
                        return IsAllRequiredFieldsFilled() && (currentPerson.Id != 0 || NoLic);
                    }));
            }
        }
        #endregion

        #region Form management
        public ViolationsUserViewModel() : base() {
            client = new MainService.UserServiceClient();

            Violations = new ObservableCollection<Violation>();
            ViolationTypes = new ReadOnlyCollection<ViolationType>(client
                .GetAllViolationTypes()
                .Select(val => Mapper.mapper.Map<ViolationType>(val))
                .ToList());
            currentPerson = new Person();
            Violations.CollectionChanged += ViolationCollectionChanged;
            InitializeForm();
        }

        public void ResetPersonProfile() {
            CurrentPerson.Id = 0;
            CurrentPerson.Name = null;
            CurrentPerson.Surname = null;
            CurrentPerson.Patronymic = null;
            CurrentPerson.Birthday = DateTime.MinValue;
            CurrentPersonsViolations.Clear();
        }

        protected override void ResetForm() {
            initValid = 0;
            NoLic = false;
            base.ResetForm();
        }
        #endregion

        #region Util
        private const int MAX_INIT_VALID_FIELDS = 6;
        private int initValid = 0;

        public string this[string columnName] {
            get {
                if (initValid < MAX_INIT_VALID_FIELDS) {
                    initValid++;
                    return "";
                }
                string error = "";
                switch (columnName) {
                    case "SelectedViolationType":
                        if (SelectedViolationType == null) {
                            error = "Тип нарушения обязателен для заполнения";
                        }
                        break;
                    case "Address":
                        if (Address == null) {
                            error = "Адрес обязателен для заполнения";
                        }
                        break;
                    case "CarNumber":
                        if (CarNumber == null) {
                            error = "Номер автомобиля обязателен для заполнения";
                            break;
                        }

                        foreach (char ch in carNumber) {
                            if (!Char.IsLetterOrDigit(ch)) {
                                error = "Номер автомобиля может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case "DriverLicenseOrProtocol":
                        if (DriverLicenseOrProtocol == null) {
                            if (noLic) {
                                error = "Номер протокола обязателен для заполнения";
                            } else {
                                error = "Номер водительского удостоверения обязателен для заполнения";
                            }
                            break;
                        }

                        foreach (char ch in DriverLicenseOrProtocol) {
                            if (!char.IsLetterOrDigit(ch)) {
                                if (noLic) {
                                    error = "Номер ВУ может содержать только буквы и цифры";
                                } else {
                                    error = "Номер протокола может содержать только буквы и цифры";
                                }
                                break;
                            }
                        }

                        break;
                }
                return error;
            }
        }

        public void ViolationCollectionChanged(object obj, NotifyCollectionChangedEventArgs args) {
            switch (args.Action) {
                case NotifyCollectionChangedAction.Add:
                    Violation newItem = (Violation)args.NewItems[0];
                    MainService.ViolationDto dto = Mapper.mapper.Map<MainService.ViolationDto>(newItem);
                    MainService.ViolationDto saved = client.AddViolation(dto);
                    newItem.Id = saved.id;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Violation item in args.OldItems) {
                        client.DeleteViolation(item.Id);
                    }
                    break;
            }
        }

        public string Error => throw new NotImplementedException();
        #endregion
    }
}
