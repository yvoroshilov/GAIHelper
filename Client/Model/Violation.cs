using System;
using System.ComponentModel;

namespace Client.Model {
    
    public class Violation : NotifyingModel  {
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

        private string violationTypeId;
        public string ViolationTypeId {
            get {
                return violationTypeId;
            }
            set {
                violationTypeId = value;
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

        private string protocolId;
        public string ProtocolId {
            get {
                return protocolId;
            }
            set {
                protocolId = value;
                OnPropertyChanged();
            }
        }

        private System.DateTime date;
        public System.DateTime Date {
            get {
                return date;
            }
            set {
                date = value;
                OnPropertyChanged();
            }
        }

        private double penalty;
        public double Penalty {
            get {
                return penalty;
            }
            set {
                penalty = value;
                OnPropertyChanged();
            }
        }

        private double locationN;
        public double LocationN{
            get {
                return locationN;
            }
            set {
                locationN = value;
                OnPropertyChanged();
            }
        }

        private double locationE;
        public double LocationE {
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

        private int shiftId;
        public int ShiftId {
            get {
                return shiftId;
            }
            set {
                shiftId = value;
                OnPropertyChanged();
            }
        }
    }
}
