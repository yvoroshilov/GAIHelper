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

        /s private violationId


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

        public ViolationsUserViewModel() {
            client = new MainService.UserServiceClient();
            Violations = new ObservableCollection<Violation>();
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
