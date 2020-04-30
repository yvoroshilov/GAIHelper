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

namespace Client.ViewModel {
    public class ViolationsUserViewModel : ViewModel, IDataErrorInfo {

        #region Common
        private MainService.UserServiceClient client;
        public ObservableCollection<Violation> Violations { get; set; }
        public ReadOnlyCollection<ViolationType> ViolationTypes { get; set; }
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

        private Violation selectedViolation;
        public Violation SelectedViolation {
            get {
                return selectedViolation;
            }
            set {
                selectedViolation = value;
                OnPropertyChanged();
            }
        }

        private string violationTypeId;
        public string ViolationTypeId {
            get {
                return violationTypeId;
            }
            set {
                violationTypeId = value;
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
        #endregion

        #region Commands
        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ?? 
                    (addCommand = new RelayCommand(obj => {
                        ViolationTypeId = selectedViolationType.Id;

                        if (!noLic) {
                            Person pers = Mapper.mapper.Map<Person>(client.GetPerson(DriverLicenseOrProtocol));
                            PersonId = pers.Id;
                        } else {
                            Description += " | № Протокола: " + DriverLicenseOrProtocol;
                        }

                        Violation violation = new Violation();
                        violation.ViolationTypeId = ViolationTypeId;
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
                        initValid = 0;
                        ResetForm();
                    }, (obj) => {
                        return IsAllInputFieldsFilled();
                    }));
            }
        }

        private RelayCommand checkPersonCommand;
        public RelayCommand CheckPersonCommand {
            get {
                return checkPersonCommand ??
                    (checkPersonCommand = new RelayCommand(obj => {
                        MainService.PersonDto person = client.GetPerson(DriverLicenseOrProtocol);
                        if (person == null) {
                            
                        }
                        Mapper.mapper.Map(, currentPerson, typeof (MainService.PersonDto), currentPerson.GetType());
                        
                    }, obj => {
                        return DriverLicenseOrProtocol != null;
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
            Violations.CollectionChanged += CollectionChanged;
            InitializeForm();
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

        public void CollectionChanged(object obj, NotifyCollectionChangedEventArgs args) {
            switch (args.Action) {
                case NotifyCollectionChangedAction.Add:
                    MainService.ViolationDto dto = Mapper.mapper.Map<MainService.ViolationDto>(args.NewItems[0]);
                    client.AddViolation(dto);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public string Error => throw new NotImplementedException();
        #endregion
    }
}
