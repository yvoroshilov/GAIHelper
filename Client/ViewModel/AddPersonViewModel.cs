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
using System.Net.Mail;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

namespace Client.ViewModel {
    public class AddPersonViewModel : ViewModel, IDataErrorInfo {

        private AdminServiceClient client;
        private ObservableCollection<PersonDto> persons;

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

        private double? paidPenalty;
        [InputProperty(true)]
        public double? PaidPenalty {
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

        private double? actualPenalty;
        [InputProperty(true)]
        public double? ActualPenalty {
            get {
                return actualPenalty;
            }
            set {
                actualPenalty = value;
                OnPropertyChanged();
            }
        }

        private string email;
        [InputProperty]
        public string Email {
            get {
                return email;
            }
            set {
                email = value;
                OnPropertyChanged();
            }
        }

        private byte[] photo;
        public byte[] Photo {
            get {
                return photo;
            }
            set {
                photo = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ??
                    (addCommand = new RelayCommand(obj => {
                        UserServiceClient userClient = new UserServiceClient();
                        if (client.GetPersonByPassportId(PassportId) != null) {
                            MessageBox.Show("Профиль с таким номером паспорта уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (userClient.GetPersonByDriverLicense(DriverLicense) != null) {
                            MessageBox.Show("Профиль с таким номером ВУ уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        PersonDto person = new PersonDto();
                        person.passportId = PassportId;
                        person.driverLicense = DriverLicense;
                        person.birthday = Birthday;
                        person.paidPenalty = PaidPenalty.Value;
                        person.surname = Surname;
                        person.name = Name;
                        person.patronymic = Patronymic;
                        person.actualPenalty = ActualPenalty.Value;
                        person.email = Email;
                        person.photo = Photo;

                        PersonDto added = client.AddPerson(person);
                        persons.Add(added);
                        ResetForm();
                    }, obj => {
                        return IsAllRequiredFieldsFilled() && IsAllInputPropsValid(this);
                    }));
            }
        }


        private RelayCommand choosePhoto;
        public RelayCommand ChoosePhoto {
            get {
                return choosePhoto ??
                    (choosePhoto = new RelayCommand(obj => {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Image Files(*.PNG;*.JPG;*.BMP;*.GIF)|*.PNG;*.JPG;*.BMP;*.GIF";
                        openFileDialog.Multiselect = false;
                        openFileDialog.AddExtension = true;
                        bool? result = openFileDialog.ShowDialog();
                        if (result == true) {
                            if (!File.Exists(openFileDialog.FileName)) {
                                MessageBox.Show("Выбранный файл больше не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                            if (fileInfo.Length / (1024) > 100) {
                                MessageBox.Show("Файл должен иметь размер менее 100 кб", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            Photo = File.ReadAllBytes(openFileDialog.FileName);
                            CurPhoto = Utility.LoadImage(photo);
                        }
                    }, obj => {
                        return true;
                    }));
            }
        }

        private RelayCommand removePhoto;
        public RelayCommand RemovePhoto {
            get {
                return removePhoto ??
                    (removePhoto = new RelayCommand(obj => {
                        CurPhoto = Utility.NoPhotoImg;
                        Photo = null;
                    }, obj => {
                        return CurPhoto != Utility.NoPhotoImg;
                    }));
            }
        }

        public AddPersonViewModel(ObservableCollection<PersonDto> col) {
            InstanceContext cntxt = new InstanceContext(new DummyCallbackClass());
            client = new AdminServiceClient(cntxt);

            persons = col;
            CurPhoto = Utility.NoPhotoImg;
        }

        protected override void ResetForm(string mark = null) {
            base.ResetForm(mark);
            CurPhoto = Utility.NoPhotoImg;
        }

        public string this[string columnName] {
            get {
                string error = "";

                var props = GetProps().ToList();
                var prop = props.Where(val => val.Name == columnName).Single();
                if ((prop.GetValue(this)?.Equals(Utility.GetDefault(prop.PropertyType)) ?? true) &&
                    columnName != nameof(Email)
                    ) {
                    return "Это поле должно быть заполнено";
                }

                switch (columnName) {
                    case nameof(PassportId):
                        if (PassportId.Length != 9) {
                            error = "Номер паспорта должен быть в формате 'AA1234567'";
                            break;
                        }

                        for (int i = 0; i < Math.Min(2, PassportId.Length); i++) {
                            if (PassportId[i] < 'A' || PassportId[i] > 'B') {
                                error = "Номер паспорта должен быть в формате 'AA1234567'";
                                break;
                            }
                        }

                        for (int i = 2; i < PassportId.Length; i++) {
                            if (!Char.IsDigit(PassportId[i])) {
                                error = "Номер паспорта должен быть в формате 'AA1234567'";
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
                    case nameof(Email):
                        if (Email == null) break;
                        try {
                            new MailAddress(Email);
                        } catch {
                            error = "Электронный записан неверно";
                        }
                        break;
                }
                return error;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
