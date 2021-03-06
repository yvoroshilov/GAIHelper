﻿using Client.Model;
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
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using Client.Resources.Converter;

namespace Client.ViewModel {
    public class ViolationsUserViewModel : ViewModel, IDataErrorInfo {

        #region Common
        private MainService.UserServiceClient client;
        public ObservableCollection<ViolationDto> Violations { get; }
        public ReadOnlyCollection<ViolationType> ViolationTypes { get; }
        public ShiftDto CurrentShift { get; }

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
                penalty = (double)new MoneyConverter().Convert(value, null, null, null);
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
                        if (!NoLic) {
                            PersonDto pers = Mapper.mapper.Map<PersonDto>(client.GetPersonByDriverLicense(DriverLicense));
                            PersonId = pers.id;
                        } else {
                            PersonId = NO_LIC_PERSON_ID;
                        }

                        ViolationDto violation = new ViolationDto();
                        violation.violationTypeId = SelectedViolationType.Id;
                        violation.personId = PersonId;
                        violation.carNumber = CarNumber;
                        violation.date = Date;
                        violation.penalty = Penalty;
                        violation.latitude = Latitude;
                        violation.longitude = Longitude;
                        violation.address = Address;
                        violation.description = Description;
                        violation.protocolId = ProtocolId;
                        violation.shiftId = CurrentShift.id;

                        Violations.Add(violation);
                        if (curFile != null) {
                            ViolationDto withFile = client.AddViolationFile(violation.id, curFile, new FileInfo(CurrentFilePath).Name);
                            violation.docPath = withFile.docPath;
                        }

                        ResetPersonProfile();
                        ResetForm();
                    }, (obj) => {
                        return IsAllRequiredFieldsFilled() && IsAllInputPropsValid(this) && (currentPerson.id != 0 || NoLic);
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

                        List<ViolationDto> selectedViolations = new List<ViolationDto>((obj as ICollection).Cast<ViolationDto>());
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
                        List<ViolationDto> selectedViolations = new List<ViolationDto>((obj as ICollection).Cast<ViolationDto>());
                        ViolationDto curViolation = selectedViolations.SingleOrDefault();
                        SelectedViolationType = ViolationTypes.Where(val => val.Id == curViolation.violationTypeId).First();
                        CarNumber = curViolation.carNumber;
                        Penalty = curViolation.penalty;
                        Latitude = curViolation.latitude;
                        Longitude = curViolation.longitude;
                        Address = curViolation.address;
                        Description = curViolation.description;
                        ProtocolId = curViolation.protocolId;
                        CurrentFilePath = curViolation.docPath;
                        if (curViolation.personId != NO_LIC_PERSON_ID) {
                            DriverLicense = CurrentPerson.driverLicense;
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
                        MainService.PersonDto personDto = client.GetPersonByDriverLicense(DriverLicense);
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
                            if (personDto.photo == null) {
                                CurPhoto = Utility.NoPhotoImg;
                            } else {
                                CurPhoto = Utility.LoadImage(personDto.photo);
                            }
                            CurrentPersonsViolations.AddRange(client.GetAllViolations(CurrentPerson.id));
                        }
                    }, obj => {
                        return this[nameof(DriverLicense)].Equals("");
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
                        List<ViolationDto> selectedViolations = new List<ViolationDto>((obj as ICollection).Cast<ViolationDto>());
                        ViolationDto curViolation = selectedViolations.First();
                        curViolation.violationTypeId = SelectedViolationType.Id;
                        curViolation.carNumber = CarNumber;
                        curViolation.penalty = Penalty;
                        curViolation.latitude = Latitude;
                        curViolation.longitude = Longitude;
                        curViolation.address = Address;
                        curViolation.description = Description;
                        curViolation.protocolId = ProtocolId;
                        if (NoLic) {
                            curViolation.personId = NO_LIC_PERSON_ID;
                        } else {
                            curViolation.personId = client.GetPersonByDriverLicense(DriverLicense).id;
                        }
                        client.EditViolation(curViolation);

                        if (CurrentFilePath == null && curViolation.docPath != null) {
                            client.RemoveViolationFile(curViolation.id);
                            curViolation.docPath = null;
                        }
                        if (curFile != null && CurrentFilePath != null) {
                            curViolation.docPath = client.AddViolationFile(curViolation.id, curFile, new FileInfo(CurrentFilePath).Name).docPath;
                        }
                        ResetForm();

                    }, obj => {
                        return IsAllRequiredFieldsFilled() && IsAllInputPropsValid(this) && (currentPerson.id != 0 || NoLic);
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

        private RelayCommand downloadFile;
        public RelayCommand DownloadFile {
            get {
                return downloadFile ??
                    (downloadFile = new RelayCommand(obj => {
                        ViolationDto violation = (obj as ICollection).Cast<ViolationDto>().Single();
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.AddExtension = true;
                        saveFileDialog.FileName = new FileInfo(violation.docPath).Name;
                        bool? result = saveFileDialog.ShowDialog();
                        if (result == true) {
                            byte[] file = client.GetViolationFile(violation.id);
                            File.WriteAllBytes(saveFileDialog.FileName, file);
                        }
                    }, obj => {
                        return (obj as ICollection).Count == 1 && (obj as ICollection).Cast<ViolationDto>().SingleOrDefault()?.docPath != null;
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
        public ViolationsUserViewModel(ShiftDto shift, List<ViolationDto> recentViolations = null) : base() {
            client = ClientInstanceProvider.GetUserServiceClient();

            Violations = new ObservableCollection<ViolationDto>(recentViolations ?? new List<ViolationDto>());
            ViolationTypes = new ReadOnlyCollection<ViolationType>(client
                .GetAllViolationTypes()
                .Select(val => Mapper.mapper.Map<ViolationType>(val))
                .ToList());
            currentPerson = new PersonDto();
            Violations.CollectionChanged += ViolationCollectionChanged;
            CurrentShift = shift;
            CurPhoto = Utility.NoPhotoImg;
        }

        public void ResetPersonProfile() {
            CurrentPerson.id = 0;
            CurrentPerson.name = null;
            CurrentPerson.surname = null;
            CurrentPerson.patronymic = null;
            CurrentPerson.birthday = DateTime.MinValue;
            CurPhoto = Utility.NoPhotoImg;
            CurrentPersonsViolations.Clear();
        }

        protected override void ResetForm(string mark = null) {
            NoLic = false;
            CurrentFilePath = null;
            curFile = null;
            base.ResetForm();
        }
        #endregion

        #region Util
        public string this[string columnName] {
            get {
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
                            if (!Char.IsDigit(ch) && !(ch >= 'A' && ch <= 'Z')) {
                                error = "Номер автомобиля может содержать только латинские буквы верхнего регистра и цифры";
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
                }
                return error;
            }
        }

        public void ViolationCollectionChanged(object obj, NotifyCollectionChangedEventArgs args) {
            switch (args.Action) {
                case NotifyCollectionChangedAction.Add:
                    ViolationDto newItem = (ViolationDto)args.NewItems[0];
                    MainService.ViolationDto dto = Mapper.mapper.Map<MainService.ViolationDto>(newItem);
                    MainService.ViolationDto saved = client.AddViolation(dto);
                    newItem.id = saved.id;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ViolationDto item in args.OldItems) {
                        client.DeleteViolation(item.id);
                    }
                    break;
            }
        }

        public string Error => throw new NotImplementedException();
        #endregion
    }
}
