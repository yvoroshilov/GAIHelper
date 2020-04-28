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

namespace Client.ViewModel {
    public class ViolationsUserViewModel : ViewModel, IDataErrorInfo {

        private MainService.UserServiceClient client;
        public ObservableCollection<Violation> Violations { get; set; }
        public ReadOnlyCollection<ViolationType> ViolationTypes { get; set; }

        private ViolationType selectedViolationType;
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

        private Violation addedViolation;
        public Violation AddedViolation {
            get {
                return addedViolation;
            }
            set {
                addedViolation = value;
                OnPropertyChanged();
            }
        }

        private bool noLic;
        public bool NoLic {
            get {
                return noLic;
            }
            set {
                noLic = value;
                OnPropertyChanged();
            }
        }

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

        private RelayCommand addCommand;
        public RelayCommand AddCommand {
            get {
                return addCommand ?? 
                    (addCommand = new RelayCommand(obj => {
                        if (selectedViolationType == null) return;
                        addedViolation.ViolationTypeId = selectedViolationType.Id;

                        if (!noLic) {
                            Person pers = Mapper.mapper.Map<Person>(client.GetPerson(AddedViolation.DriverLicenseOrProtocol));
                            addedViolation.PersonId = pers.Id;
                        } else {
                            addedViolation.Description += " | № Протокола: " + AddedViolation.DriverLicenseOrProtocol;
                        }

                        Violations.Add((Violation)AddedViolation.Clone());
                        isSelectedItemNew = true;
                        ResetForm();
                    }));
            }
        }

        private RelayCommand checkPersonCommand;
        public RelayCommand CheckPersonCommand {
            get {
                return checkPersonCommand ??
                    (checkPersonCommand = new RelayCommand(obj => {
                        Mapper.mapper.Map(client.GetPerson(AddedViolation.DriverLicenseOrProtocol), currentPerson, typeof (MainService.PersonDto), currentPerson.GetType());
                    }));
            }
        }

        public ViolationsUserViewModel() {
            client = new MainService.UserServiceClient();

            Violations = new ObservableCollection<Violation>();
            ViolationTypes = new ReadOnlyCollection<ViolationType>(client
                .GetAllViolationTypes()
                .Select(val => Mapper.mapper.Map<ViolationType>(val))
                .ToList());
            addedViolation = new Violation();
            currentPerson = new Person();
            Violations.CollectionChanged += CollectionChanged;
            InitializeForm();
        }

        public override void InitializeForm() {
            addedViolation.Date = DateTime.Now;
        }

        public override void ResetForm() {
            SelectedViolationType = null;
            AddedViolation.setAllPropsToDefault();
            CurrentPerson.setAllPropsToDefault();

            InitializeForm();
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

        private bool isSelectedItemNew = true;

        public string this[string columnName] {
            get {
                string error = "";
                switch (columnName) {
                    case "SelectedViolationType":
                        if (selectedViolationType == null && !isSelectedItemNew) {
                            error = "Тип нарушения обязателен для заполнения";
                        }
                        isSelectedItemNew = false;
                        break;
                    default:
                        break;
                }
                return error;
            }
        }
    }
}
