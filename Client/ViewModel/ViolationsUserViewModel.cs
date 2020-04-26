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

        private Violation violation;
        public Violation Violation {
            get {
                return violation;
            }
            set {
                violation = value;
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

        public ViolationsUserViewModel() {
            client = new MainService.UserServiceClient();
            List<MainService.ViolationDto> list = client.GetAllViolationsUser().ToList();
            Violations = new ObservableCollection<Violation>(list.Select(val => Mapper.mapper.Map<Violation>(val)));
            Violations[0].ViolationType.ShortTitle = "test";
            Violations.CollectionChanged += CollectionChanged;
        }
        
        public void CollectionChanged(object obj, NotifyCollectionChangedEventArgs args) {
            switch (args.Action) {
                case NotifyCollectionChangedAction.Add:
                    client.AddViolationUser(Mapper.mapper.Map<MainService.ViolationDto>(violation));
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
