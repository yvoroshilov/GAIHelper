using System;

namespace Client.Model {
    public class Shift : NotifyingModel {
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

        private int responibleId;
        public int ResponsibleId {
            get {
                return responibleId;
            }
            set {
                responibleId = value;
                OnPropertyChanged();
            }
        }

        private System.DateTime start;
        public System.DateTime Start {
            get {
                return start;
            }
            set {
                start = value;
                OnPropertyChanged();
            }
        }

        public DateTime end;
        public DateTime End {
            get {
                return end;
            }
            set {
                end = value;
                OnPropertyChanged();
            }
        }

        private Employee employee;
        public Employee Employee {
            get {
                return employee;
            }
            set {
                employee = value;
                OnPropertyChanged();
            }
        }
    }
}
