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
    public class AddViolationWindowViewModel : ViewModel, IDataErrorInfo {

        #region Common
        private MainService.UserServiceClient userClient;
        private AdminServiceClient adminClient;
        public ObservableCollection<ViolationDto> Violations { get; }
        public ReadOnlyCollection<ViolationType> ViolationTypes { get; }
        public ShiftDto CurrentShift { get; }
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

        private double? latitude;
        [InputProperty]
        public double? Latitude{
            get {
                return latitude;
            }
            set {
                latitude = value;
                OnPropertyChanged();
            }
        }

        private double? longitude;
        [InputProperty]
        public double? Longitude {
            get {
                return longitude;
            }
            set {
                longitude = value;
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

        private string driverLicense;
        [InputProperty]
        public string DriverLicense {
            get {
                return driverLicense;
            }
            set {
                driverLicense = value;
                OnPropertyChanged();
            }
        }

        private string protocolId;
        [InputProperty(true)]
        public string ProtocolId {
            get {
                return protocolId;
            }
            set {
                protocolId = value;
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

        private int shiftId;
        [InputProperty(true)]
        public int ShiftId {
            get {
                return shiftId;
            }
            set {
                shiftId = value;
                OnPropertyChanged();
            }
        }

        private DateTime violationDate;
        public DateTime ViolationDate {
            get {
                return violationDate == default ? DateTime.Now : violationDate;
            }
            set {
                violationDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Person info

        private const int NO_LIC_PERSON_ID = 1; 

        private const string NO_LIC_DRIVER_LIC = "NO_LIC";

        private PersonDto currentPerson;
        public PersonDto CurrentPerson {
            get {
                return currentPerson;
            }
            set {
                currentPerson = value;
                OnPropertyChanged();
            }
        }

        public List<ViolationDto> CurrentPersonsViolations { get; set; } = new List<ViolationDto>();
        #endregion

        #region Commands
        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ?? 
                    (addCommand = new RelayCommand(obj => {
                        if (adminClient.GetShiftById(ShiftId) == null) {
                            MessageBox.Show("Смены с таким номером не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (!NoLic) {
                            PersonDto pers = Mapper.mapper.Map<PersonDto>(userClient.GetPersonByDriverLicense(DriverLicense));
                            PersonId = pers.id;
                        } else {
                            PersonId = NO_LIC_PERSON_ID;
                        }

                        ViolationDto violation = new ViolationDto();
                        violation.violationTypeId = SelectedViolationType.Id;
                        violation.personId = PersonId;
                        violation.carNumber = CarNumber;
                        violation.penalty = Penalty;
                        violation.latitude = Latitude;
                        violation.longitude = Longitude;
                        violation.address = Address;
                        violation.description = Description;
                        violation.protocolId = ProtocolId;
                        violation.shiftId = ShiftId;
                        violation.date = ViolationDate;

                        ViolationDto added = userClient.AddViolation(violation);
                        Violations.Add(added);
                        ResetPersonProfile();
                        ResetForm();
                    }, (obj) => {
                        return IsAllRequiredFieldsFilled() && IsAllInputPropsValid(this) && (currentPerson.id != 0 || NoLic);
                    }));
            }
        }

        private RelayCommand checkPersonCommand;
        public RelayCommand CheckPersonCommand {
            get {
                return checkPersonCommand ??
                    (checkPersonCommand = new RelayCommand(obj => {
                        MainService.PersonDto personDto = userClient.GetPersonByDriverLicense(DriverLicense);
                        if (personDto == null) {
                            MessageBox.Show($"Человека с водительским удостоверением № {DriverLicense} не существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ResetPersonProfile();
                        } else {
                            CurrentPerson.driverLicense = personDto.driverLicense;
                            CurrentPerson.id = personDto.id;
                            CurrentPerson.birthday = personDto.birthday;
                            CurrentPerson.name = personDto.name;
                            CurrentPerson.surname = personDto.surname;
                            CurrentPerson.patronymic = personDto.patronymic;
                            CurrentPersonsViolations.AddRange(userClient.GetAllViolations(CurrentPerson.id));
                        }
                    }, obj => {
                        return DriverLicense != null;
                    }));
            }
        }

        #endregion

        #region Form management
        public AddViolationWindowViewModel(ObservableCollection<ViolationDto> col) : base() {
            userClient = new MainService.UserServiceClient();
            adminClient = new AdminServiceClient(new InstanceContext(new DummyCallbackClass()));

            Violations = col;
            ViolationTypes = new ReadOnlyCollection<ViolationType>(userClient
                .GetAllViolationTypes()
                .Select(val => Mapper.mapper.Map<ViolationType>(val))
                .ToList());
            currentPerson = new PersonDto();
        }

        public void ResetPersonProfile() {
            CurrentPerson.id = 0;
            CurrentPerson.name = null;
            CurrentPerson.surname = null;
            CurrentPerson.patronymic = null;
            CurrentPerson.birthday = DateTime.MinValue;
            CurrentPersonsViolations.Clear();
        }

        protected override void ResetForm(string mark = null) {
            initValid = 0;
            NoLic = false;
            base.ResetForm();
        }
        #endregion

        #region Util
        private const int MAX_INIT_VALID_FIELDS = 11;
        private int initValid = 0;

        public string this[string columnName] {
            get {
                if (initValid < MAX_INIT_VALID_FIELDS) {
                    initValid++;
                    return "";
                }
                string error = "";
                switch (columnName) {
                    case nameof(Longitude):
                        if ((Latitude == null && Longitude != null) ||
                            (Longitude == null && Latitude != null)) {
                            error = "Координаты должны быть либо указаны оба либо не указаны оба";
                            break;
                        }

                        if (Longitude != null && (Longitude > 180 || Longitude < -180)) {
                            error = "Долгота должна быть в пределах от -180 до 180 градусов";
                            break;
                        }
                        break;
                    case nameof(Latitude):
                        if ((Latitude == null && Longitude != null) ||
                            (Longitude == null && Latitude != null)) {
                            error = "Координаты должны быть либо указаны оба либо не указаны оба";
                            break;
                        }

                        if (Latitude != null && (Latitude > 90 || Longitude < -90)) {
                            error = "Широта должна быть в пределах от -90 до 90 градусов";
                            break;
                        }
                        break;
                    case nameof(SelectedViolationType):
                        if (SelectedViolationType == null) {
                            error = "Тип нарушения обязателен для заполнения";
                        }
                        break;
                    case nameof(Address):
                        if (Address == null) {
                            error = "Адрес обязателен для заполнения";
                        }
                        break;
                    case nameof(CarNumber):
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
                    case nameof(ProtocolId):
                        if (ProtocolId == null) {
                            error = "Номер протокола обязателен для заполнения";
                            break;
                        }

                        foreach (char ch in ProtocolId) {
                            if (!char.IsLetterOrDigit(ch)) {
                                error = "Номер протокола может содержать только буквы и цифры";
                                break;
                            }
                        }

                        break;
                }
                return error;
            }
        }
        public string Error => throw new NotImplementedException();
        #endregion
    }
}
