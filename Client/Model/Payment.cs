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

        private Person person;
        public Person Person {
            get {
                return person;
            }
            set {
                person = value;
                OnPropertyChanged();
            }
        }
    }
}
