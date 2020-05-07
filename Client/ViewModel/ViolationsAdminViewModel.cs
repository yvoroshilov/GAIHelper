using Client.MainService;
using Client.Model;
using Client.Util;
using Client.View.Admin.EmployeesTabSubWindows;
using Client.View.Admin.ViolationsTabSubWindows;
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
    public class ViolationsAdminViewModel : ViewModel, IDataErrorInfo {

        private const string searchMark = "searchMark";
        private const string addMark = "addMark";
        private AdminServiceClient adminClient;
        private UserServiceClient userClient;
        public ObservableCollection<ViolationDto> Violations { get; }
        public PersonDto profile;
        public ObservableCollection<Object> Payments { get; }
        public ObservableCollection<ViolationType> ViolationTypes { get; }
        public ViolationDto curViolation;

        #region InputFields
        private string protocolIdSearch;
        [InputProperty(Mark = searchMark)]
        public string ProtocolIdSearch {
            get {
                return protocolIdSearch;
            }
            set {
                protocolIdSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findProtocolIdCheckbox;
        [InputProperty(Mark = searchMark)]
        public bool FindProtocolIdCheckbox {
            get {
                return findProtocolIdCheckbox;
            }
            set {
                findProtocolIdCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string carNumberSearch;
        [InputProperty(Mark = searchMark)]
        public string CarNumberSearch {
            get {
                return carNumberSearch;
            }
            set {
                carNumberSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findCarNumberCheckbox;
        [InputProperty(Mark = searchMark)]
        public bool FindCarNumberCheckbox {
            get {
                return findCarNumberCheckbox;
            }
            set {
                findCarNumberCheckbox = value;
                OnPropertyChanged();
            }
        }

        private DateTime violationDateStart;
        public DateTime ViolationDateStart {
            get {
                return violationDateStart == default ? DateTime.Now : violationDateStart;
            }
            set {
                violationDateStart = value;
                OnPropertyChanged();
            }
        }

        private DateTime violationDateEnd;
        public DateTime ViolationDateEnd {
            get {
                return violationDateEnd == default ? DateTime.Now : violationDateEnd;
            }
            set {
                violationDateEnd = value;
                OnPropertyChanged();
            }
        }

        private bool findViolationDateCheckbox;
        [InputProperty(Mark = searchMark)]
        public bool FindViolationDateCheckbox {
            get {
                return findViolationDateCheckbox;
            }
            set {
                findViolationDateCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string addressSearch;
        [InputProperty(Mark = searchMark)]
        public string AddressSearch {
            get {
                return addressSearch;
            }
            set {
                addressSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findAddressSearchCheckbox;
        [InputProperty(Mark = searchMark)]
        public bool FindAddressSearchCheckbox {
            get {
                return findAddressSearchCheckbox;
            }
            set {
                findAddressSearchCheckbox = value;
                OnPropertyChanged();
            }
        }

        private int shiftIdSearch;
        [InputProperty(Mark = searchMark)]
        public int ShiftIdSearch {
            get {
                return shiftIdSearch;
            }
            set {
                shiftIdSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findShiftIdCheckbox;
        [InputProperty(Mark = searchMark)]
        public bool FindShiftIdCheckbox {
            get {
                return findShiftIdCheckbox;
            }
            set {
                findShiftIdCheckbox = value;
                OnPropertyChanged();
            }
        }

        private double penaltyMin;
        [InputProperty(Mark = searchMark)]
        public double PenaltyMin {
            get {
                return penaltyMin;
            }
            set {
                penaltyMin = value;
                OnPropertyChanged();
            }
        }

        private double penaltyMax;
        [InputProperty(Mark = searchMark)]
        public double PenaltyMax {
            get {
                return penaltyMax;
            }
            set {
                penaltyMax = value;
                OnPropertyChanged();
            }
        }

        private bool findPenaltyCheckbox;
        [InputProperty(Mark = searchMark)]
        public bool FindPenaltyCheckbox {
            get {
                return findPenaltyCheckbox;
            }
            set {
                findPenaltyCheckbox = value;
                OnPropertyChanged();
            }
        }

        private string descriptionSearch;
        [InputProperty(Mark = searchMark)]
        public string DescriptionSearch {
            get {
                return descriptionSearch;
            }
            set {
                descriptionSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findDescriptionCheckbox;
        public bool FindDescriptionCheckbox {
            get {
                return findDescriptionCheckbox;
            }
            set {
                findDescriptionCheckbox = value;
                OnPropertyChanged();
            }
        }

        private ViolationType violationTypeSearch;
        public ViolationType ViolationTypeSearch {
            get {
                return violationTypeSearch;
            }
            set {
                violationTypeSearch = value;
                OnPropertyChanged();
            }
        }

        private bool findViolationTypeCheckbox;
        public bool FindViolationTypeCheckbox {
            get {
                return findViolationTypeCheckbox;
            }
            set {
                findViolationTypeCheckbox = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private PersonDto curPerson;
        public PersonDto CurPerson {
            get {
                return curPerson;
            }
            set {
                curPerson = value;
                OnPropertyChanged();
            }
        }

        private DateTime statisticsStartDate;
        [InputProperty]
        public DateTime StatisticsStartDate {
            get {
                return statisticsStartDate == default ? DateTime.Now : statisticsStartDate;
            }
            set {
                statisticsStartDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime statisticsEndDate;
        [InputProperty]
        public DateTime StatisticsEndDate {
            get {
                return statisticsEndDate == default ? DateTime.Now : statisticsEndDate;
            }
            set {
                statisticsEndDate = value;
                OnPropertyChanged();
            }
        }

        #region Command
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
                            userClient.DeleteViolation(item.id);
                            Violations.Remove(item);
                        }
                    }, obj => {
                        return (obj as ICollection).Count != 0;
                    }));
            }
        }

        private RelayCommand seeStatistics;
        public RelayCommand SeeStatistics {
            get {
                return seeStatistics ??
                    (seeStatistics = new RelayCommand(obj => {
                        if (StatisticsStartDate.CompareTo(StatisticsEndDate) == 1) {
                            MessageBox.Show("Начальная дата для просмотра статистики должна быть раньше, чем конечная", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        List<ViolationDto> violations = adminClient.SearchViolationsDateRange(new ViolationDto(), StatisticsStartDate, StatisticsEndDate).ToList();

                        (obj as Window).IsEnabled = false;
                        StatisticsWindow statisticsWindow = new StatisticsWindow(violations, obj as Window);
                        statisticsWindow.Show();
                    }, obj => {
                        return StatisticsStartDate != default && StatisticsEndDate != default;
                    }));
            }
        }

        private RelayCommand seeViolatorProfile;
        public RelayCommand SeeViolatorProfile {
            get {
                return seeViolatorProfile ??
                    (seeViolatorProfile = new RelayCommand(obj => {
                        List<ViolationDto> selectedViolations = new List<ViolationDto>((obj as ICollection).Cast<ViolationDto>());
                        ViolationDto curViolation = selectedViolations.First();
                        PersonDto curPerson = adminClient.GetPerson(curViolation.personId);

                        CurPerson.actualPenalty = curPerson.actualPenalty;
                        CurPerson.birthday = curPerson.birthday;
                        CurPerson.driverLicense = curPerson.driverLicense;
                        CurPerson.id = curPerson.id;
                        CurPerson.name = curPerson.name;
                        CurPerson.paidPenalty = curPerson.paidPenalty;
                        CurPerson.passportId = curPerson.passportId;
                        CurPerson.patronymic = curPerson.patronymic;
                        CurPerson.surname = curPerson.surname;
                    }, obj => {
                        return (obj as ICollection).Count == 1;
                    }));
            }
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand {
            get {
                return editCommand ??
                    (editCommand = new RelayCommand(obj => {
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
                        if (FindViolationDateCheckbox && ViolationDateStart.CompareTo(ViolationDateEnd) == 1) {
                            MessageBox.Show("Начало периода поиска происшествия должно быть раньше, чем его конец", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                            
                        }

                        if (FindPenaltyCheckbox && PenaltyMax < PenaltyMin) {
                            MessageBox.Show("Нижняя граница штрафа должна быть меньше, чем верхняя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        ViolationDto violation = new ViolationDto();
                        if (FindProtocolIdCheckbox) violation.protocolId = ProtocolIdSearch;
                        if (FindCarNumberCheckbox) violation.carNumber = CarNumberSearch;
                        if (FindAddressSearchCheckbox) violation.address = AddressSearch;
                        if (FindShiftIdCheckbox) violation.shiftId = ShiftIdSearch;
                        if (FindDescriptionCheckbox) violation.description = DescriptionSearch;
                        if (FindViolationTypeCheckbox) violation.violationTypeId = ViolationTypeSearch.Id;

                        List<ViolationDto> result = null;

                        Violations.Clear();
                        if (FindViolationDateCheckbox || FindPenaltyCheckbox) {
                            List<ViolationDto> partialResult = null;
                            if (FindViolationDateCheckbox) {
                                partialResult = adminClient.SearchViolationsDateRange(violation, ViolationDateStart, ViolationDateEnd).ToList();

                                if (FindPenaltyCheckbox) partialResult = partialResult.Where(val => val.penalty >= penaltyMin && val.penalty <= penaltyMax)
                                    .ToList();
                                result = partialResult;
                            } else {
                                partialResult = adminClient.SearchViolationsPenaltyRange(violation, PenaltyMin, PenaltyMax).ToList();
                                if (FindViolationDateCheckbox) partialResult = partialResult.Where(val => (val.date.CompareTo(ViolationDateStart) + val.date.CompareTo(ViolationDateEnd)) == 0).ToList();
                                result = partialResult;
                            }
                        } else {
                            result = adminClient.SearchViolations(violation).ToList();
                        }
                        result.ForEach(val => Violations.Add(val));
                    }));
            }
        }

        private RelayCommand resetSearchFieldsCommand;
        public RelayCommand ResetSearchFieldsCommand {
            get {
                return resetSearchFieldsCommand ??
                    (resetSearchFieldsCommand = new RelayCommand(obj => {
                        ResetForm(searchMark);
                    }));
            }
        }
        #endregion

        public ViolationsAdminViewModel() {
            InstanceContext cntxt = new InstanceContext(new DummyCallbackClass());
            adminClient = new AdminServiceClient(cntxt);
            userClient = new UserServiceClient();

            CurPerson = new PersonDto();
            Violations = new ObservableCollection<ViolationDto>();
            Payments = new ObservableCollection<object>();
            ViolationTypes = new ObservableCollection<ViolationType>(userClient.GetAllViolationTypes()
                .Select(val => Mapper.mapper.Map<ViolationType>(val))
                .ToList());
        }

        #region Error handle
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
                    case nameof(AddressSearch):
                        break;
                    case nameof(ShiftIdSearch):
                        break;
                    case nameof(ViolationDateEnd):
                    case nameof(ViolationDateStart):
                        break;
                    case nameof(PenaltyMin):
                    case nameof(PenaltyMax):
                        break;
                    case nameof(ViolationTypeSearch):
                        break;
                    case nameof(ProtocolIdSearch):
                        foreach (var ch in ProtocolIdSearch) {
                            if (!Char.IsLetterOrDigit(ch)) {
                                error = "Номер протокола может содержать только буквы и цифры";
                                break;
                            }
                        }
                        break;
                    case nameof(CarNumberSearch):
                        foreach (char ch in CarNumberSearch) {
                            if (!Char.IsLetterOrDigit(ch)) {
                                error = "Номер автомобиля может содержать только буквы и цифры";
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
                case nameof(protocolIdSearch):
                    return FindProtocolIdCheckbox;
                case nameof(AddressSearch):
                    return FindAddressSearchCheckbox;
                case nameof(ShiftIdSearch):
                    return FindShiftIdCheckbox;
                case nameof(ViolationDateEnd):
                case nameof(ViolationDateStart):
                    return FindViolationDateCheckbox;
                case nameof(PenaltyMin):
                case nameof(PenaltyMax):
                    return FindPenaltyCheckbox;
                case nameof(ViolationTypeSearch):
                    return FindViolationTypeCheckbox;
                case nameof(ProtocolIdSearch):
                    return FindProtocolIdCheckbox;
                case nameof(CarNumberSearch):
                    return FindCarNumberCheckbox;
                default:
                    return true;
            }
        }
        #endregion
        public string Error => throw new NotImplementedException();
    }
}
