using Client.Model;
using Client.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Client.ViewModel {
    public class ViolationsUserViewModel : NotifyingModel {

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

        private string curDriverLicenseOrProtocol;
        public string CurDriverLicenseOrProtocol {
            get {
                return curDriverLicenseOrProtocol;
            }
            set {
                curDriverLicenseOrProtocol = value;
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
                        Violation violation = new Violation();
                        Violations.Add(violation);
                    }));
            }
        }

        private RelayCommand checkPersonCommand;
        public RelayCommand CheckPersonCommand {
            get {
                return checkPersonCommand ??
                    (checkPersonCommand = new RelayCommand(obj => {
                        Mapper.mapper.Map(client.GetPerson(curDriverLicenseOrProtocol), currentPerson, typeof (MainService.PersonDto), currentPerson.GetType());
                    }));
            }
        }

        public ViolationsUserViewModel() {
            client = new MainService.UserServiceClient();

            Violations = new ObservableCollection<Violation>();
            ViolationTypes = new ReadOnlyCollection<ViolationType>(client.GetAllViolationTypes().Select(val => Mapper.mapper.Map<ViolationType>(val)).ToList());
            addedViolation = new Violation();
            currentPerson = new Person();
            Violations.CollectionChanged += CollectionChanged;
        }
        
        public void CollectionChanged(object obj, NotifyCollectionChangedEventArgs args) {
            switch (args.Action) {
                case NotifyCollectionChangedAction.Add:
                    //client.AddViolation(Mapper.mapper.Map<MainService.ViolationDto>(violation));
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
    }
}
