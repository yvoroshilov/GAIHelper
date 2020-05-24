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
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace Client.ViewModel {
    public class EditViolationWindowViewModel : ViewModel, IDataErrorInfo {

        #region Common
        private MainService.UserServiceClient userClient;
        private AdminServiceClient adminClient;
        public ObservableCollection<ViolationDto> Violations { get; }
        public ReadOnlyCollection<ViolationType> ViolationTypes { get; }
        public ViolationDto curViolation;

        private string currentFilePath;
        public string CurrentFilePath {
            get {
                return currentFilePath;
            }
            set {
                currentFilePath = value;
                OnPropertyChanged();
            }
        }
        
        private byte[] curFile;

        private BitmapImage curPhoto;
        public BitmapImage CurPhoto {
            get {
                return curPhoto;
            }
            set {
                curPhoto = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private int id;
        public int Id {
            get {
                return id;
            }
            set {
                id = value;
                OnPropertyChanged();
            }
        }

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

        private int? shiftId;
        [InputProperty(true)]
        public int? ShiftId {
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

        private string docPath;
        public string DocPath {
            get {
                return docPath;
            }
            set {
                docPath = value;
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
        private RelayCommand acceptEditCommand;
        public RelayCommand AcceptEditCommand {
            get {
                return acceptEditCommand ??
                    (acceptEditCommand = new RelayCommand(obj => {
                        if (adminClient.GetShiftById(ShiftId.Value) == null) {
                            MessageBox.Show("Смены с таким номером не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        curViolation.violationTypeId = SelectedViolationType.Id;
                        curViolation.carNumber = CarNumber;
                        curViolation.penalty = Penalty;
                        curViolation.latitude = Latitude;
                        curViolation.longitude = Longitude;
                        curViolation.address = Address;
                        curViolation.description = Description;
                        curViolation.protocolId = ProtocolId;
                        curViolation.shiftId = ShiftId.Value;
                        curViolation.date = ViolationDate;
                        if (NoLic) {
                            curViolation.personId = NO_LIC_PERSON_ID;
                        } else {
                            curViolation.personId = userClient.GetPersonByDriverLicense(DriverLicense).id;
                        }
                        userClient.EditViolation(curViolation);

                        if (CurrentFilePath == null && curViolation.docPath != null) {
                            userClient.RemoveViolationFile(curViolation.id);
                            curViolation.docPath = null;
                        }
                        if (curFile != null && CurrentFilePath != null) {
                            curViolation.docPath = userClient.AddViolationFile(curViolation.id, curFile, new FileInfo(CurrentFilePath).Name).docPath;
                        }
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
                            curPhoto = Utility.LoadImage(CurrentPerson.photo);
                            CurrentPersonsViolations.AddRange(userClient.GetAllViolations(CurrentPerson.id));
                        }
                    }, obj => {
                        return this[nameof(DriverLicense)].Equals("");
                    }));
            }
        }

        private RelayCommand chooseFile;
        public RelayCommand ChooseFile {
            get {
                return chooseFile ??
                    (chooseFile = new RelayCommand(obj => {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Multiselect = false;
                        openFileDialog.AddExtension = true;
                        bool? result = openFileDialog.ShowDialog();
                        if (result == true) {
                            if (!File.Exists(openFileDialog.FileName)) {
                                MessageBox.Show("Выбранный файл больше не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                            if (fileInfo.Length / (1024 * 1024) > 10) {
                                MessageBox.Show("Файл должен иметь размер менее 10 мб", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            curFile = File.ReadAllBytes(openFileDialog.FileName);
                            CurrentFilePath = openFileDialog.FileName;
                        }
                    }, obj => {
                        return true;
                    }));
            }
        }

        private RelayCommand removeFile;
        public RelayCommand RemoveFile {
            get {
                return removeFile ??
                    (removeFile = new RelayCommand(obj => {
                        CurrentFilePath = null;
                        curFile = null;
                    }, obj => {
                        return CurrentFilePath != null;
                    }));
            }
        }
        #endregion

        #region Form management
        public EditViolationWindowViewModel(ObservableCollection<ViolationDto> col, ViolationDto curViolation) : base() {
            userClient = ClientInstanceProvider.GetUserServiceClient();
            adminClient = ClientInstanceProvider.GetAdminServiceClient();
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
            Latitude = curViolation.latitude;
            Longitude = curViolation.longitude;
            Address = curViolation.address;
            Description = curViolation.description;
            ProtocolId = curViolation.protocolId;
            ShiftId = curViolation.shiftId;
            ViolationDate = curViolation.date;
            CurPhoto = Utility.LoadImage(CurrentPerson.photo);
            CurrentFilePath = curViolation.docPath;
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
            CurPhoto = null;
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

                        if (Latitude != null && (Latitude > 90 || Latitude < -90)) {
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
                    case nameof(DriverLicense):
                        if (NoLic) break;
                        if (DriverLicense == null) {
                            error = "Номер водительского удостоверения обязателен для заполнения";
                            break;
                        }

                        foreach (char ch in DriverLicense) {
                            if (!char.IsLetterOrDigit(ch)) {
                                if (!NoLic) {
                                    error = "Номер ВУ может содержать только буквы и цифры";
                                }
                                break;
                            }
                        }

                        break;
                    case nameof(ShiftId):
                        if (ShiftId == null) {
                            error = "Номер смены обязателен для заполнения";
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
