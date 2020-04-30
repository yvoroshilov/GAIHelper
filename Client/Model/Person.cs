using System;
using System.Collections.Generic;

namespace Client.Model {
    public partial class Person : NotifyingModel {
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

        private string passportId;
        public string PassportId {
            get {
                return passportId;
            }
            set {
                passportId = value;
                OnPropertyChanged();
            }
        }

        private string driverLicense;
        public string DriverLicense {
            get {
                return driverLicense;
            }
            set {
                driverLicense = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                OnPropertyChanged();
            }
        }

        private string surname;
        public string Surname {
            get {
                return surname;
            }
            set {
                surname = value;
                OnPropertyChanged();
            }
        }

        private string patronymic;
        public string Patronymic {
            get {
                return patronymic;
            }
            set {
                patronymic = value; 
                OnPropertyChanged();
            }
        }

        private DateTime birthday;
        public DateTime Birthday {
            get {
                return birthday;
            }
            set {
                birthday = value;
                OnPropertyChanged();
            }
        }

        private float actualPenalty;
        public float ActulaPenalty {
            get {
                return actualPenalty;
            }
            set {
                actualPenalty = value;
                OnPropertyChanged();
            }
        }

        private float paidPenalty;
        public float PaidPenalty {
            get {
                return paidPenalty;
            }
            set {
                paidPenalty = value;
                OnPropertyChanged();
            }
        }
    }
}
