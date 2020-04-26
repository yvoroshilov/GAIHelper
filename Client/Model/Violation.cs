using System;

namespace Client.Model {
    
    public class Violation : NotifyingModel {
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

        private string carNumber;
        public string CarNumber {
            get {
                return carNumber;
            }
            set {
                carNumber = value;
                OnPropertyChanged();
            }
        }

        public System.DateTime date;
        public System.DateTime Date {
            get {
                return date;
            }
            set {
                date = value;
                OnPropertyChanged();
            }
        }

        private float penalty;
        public float Penalty {
            get {
                return penalty;
            }
            set {
                penalty = value;
                OnPropertyChanged();
            }
        }

        private float locationN;
        public float LocationN{
            get {
                return locationN;
            }
            set {
                locationN = value;
                OnPropertyChanged();
            }
        }

        private float locationE;
        public float LocationE {
            get {
                return locationE;
            }
            set {
                locationE = value;
                OnPropertyChanged();
            }
        }

        private string address;
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
        public string Description {
            get {
                return description;
            }
            set {
                description = value;
                OnPropertyChanged();
            }
        }

        private ViolationType violationType;
        public ViolationType ViolationType {
            get {
                return violationType;
            }
            set {
                violationType = value;
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
