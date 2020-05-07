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
    public class EditViolationWindowViewModel : ViewModel, IDataErrorInfo {

        #region Common
        private MainService.UserServiceClient userClient;
        private AdminServiceClient adminClient;
        public ObservableCollection<ViolationDto> Violations { get; }
        public ReadOnlyCollection<ViolationType> ViolationTypes { get; }
        public ViolationDto curViolation;
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

        private double? locationN;
        [InputProperty]
        public double? LocationN{
            get {
                return locationN;
            }
            set {
                locationN = value;
                OnPropertyChanged();
            }
        }

        private double? locationE;
        [InputProperty]
        public double? LocationE {
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
                            PersonDto pers = userClient.GetPersonByDriverLicense(DriverLicense);
                            PersonId = pers.id;
                        } else {
                            PersonId = NO_LIC_PERSON_ID;
                        }

                        ViolationDto violation = new ViolationDto();
                        violation.violationTypeId = SelectedViolationType.Id;
                        violation.personId = PersonId;
                        violation.carNumber = CarNumber;
                        violation.penalty = Penalty;
                        violation.locationN = LocationN;
                        violation.locationE = LocationE;
                        violation.address = Address;
                        violation.description = Description;
                        violation.protocolId = ProtocolId;
                        violation.shiftId = ShiftId;
                        violation.date = ViolationDate;

                        userClient.AddViolation(violation);
                        Violations.Add(violation);
                        ResetPersonProfile();
                        ResetForm();
                    }, (obj) => {
                        return IsAllRequiredFieldsFilled() && IsAllInputPropsValid(this) && (currentPerson.id != 0 || NoLic);
                    }));
            }
        }

        private RelayCommand acceptEditCommand;
        public RelayCommand AcceptEditCommand {
            get {
                return acceptEditCommand ??
                    (acceptEditCommand = new RelayCommand(obj => {
                        if (adminClient.GetShiftById(ShiftId) == null) {
                            MessageBox.Show("Смены с таким номером не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        curViolation.violationTypeId = SelectedViolationType.Id;
                        curViolation.carNumber = CarNumber;
                        curViolation.penalty = Penalty;
                        curViolation.locationN = LocationN;
                        curViolation.locationE = LocationE;
                        curViolation.address = Address;
                        curViolation.description = Description;
                        curViolation.protocolId = ProtocolId;
                        curViolation.shiftId = ShiftId;
                        curViolation.date = ViolationDate;
                        if (NoLic) {
                            curViolation.personId = NO_LIC_PERSON_ID;
                        } else {
                            curViolation.personId = userClient.GetPersonByDriverLicense(DriverLicense).id;
                        }

                        userClient.EditViolation(curViolation);
                        ResetForm();

                        (obj as Window).Close();
                    }, obj => {
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
        public EditViolationWindowViewModel(ObservableCollection<ViolationDto> col, ViolationDto curViolation) : base() {
            userClient = new MainService.UserServiceClient();
            adminClient = new AdminServiceClient(new InstanceContext(new DummyCallbackClass()));
            this.curViolation = curViolation;

            Violations = col;
            ViolationTypes = new ReadOnlyCollection<ViolationType>(userClient
                .GetAllViolationTypes()
                .Select(val => Mapper.mapper.Map<ViolationType>(val))
                .ToList());
            currentPerson = new PersonDto();

            SelectedViolationType = ViolationTypes.Where(val => val.Id == curViolation.violationTypeId).SingleOrDefault();
            CarNumber = curViolation.carNumber;
            Penalty = curViolation.penalty;
            LocationN = curViolation.locationN;
            LocationE = curViolation.locationE;
            Address = curViolation.address;
            Description = curViolation.description;
            ProtocolId = curViolation.protocolId;
            ShiftId = curViolation.shiftId;
            ViolationDate = curViolation.date;
            if (curViolation.personId != NO_LIC_PERSON_ID) {
                DriverLicense = adminClient.GetPerson(curViolation.personId).driverLicense;
                CheckPersonCommand.Execute(null);
                NoLic = false;
            } else {
                NoLic = true;
            }
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
        private const int MAX_INIT_VALID_FIELDS = 7;
        private int initValid = 0;

        public string this[string columnName] {
            get {
                if (initValid < MAX_INIT_VALID_FIELDS) {
                    initValid++;
                    return "";
                }
                string error = "";
                switch (columnName) {
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
