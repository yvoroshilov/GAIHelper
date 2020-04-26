using System;

namespace Client.Model {
    
    public class Payment : NotifyingModel {

        private long id;
        public long Id {
            get {
                return id;
            }
            set {
                id = value;
                OnPropertyChanged();
            }
        }

        private int violatorId;
        public int ViolatorId {
            get {
                return violatorId;
            }
            set {
                violatorId = value;
                OnPropertyChanged();
            }
        }

        private System.DateTime payday;
        public System.DateTime Payday {
            get {
                return payday;
            }
            set {
                payday = value;
                OnPropertyChanged();
            }
        }

        private bool isPaid;
        public bool IsPaid {
            get {
                return isPaid;
            }
            set {
                isPaid = value;
                OnPropertyChanged();
            }
        }

        private Violator violator;
        public Violator Violator {
            get {
                return violator;
            }
            set {
                violator = value;
                OnPropertyChanged();
            }
        }
    }
}
