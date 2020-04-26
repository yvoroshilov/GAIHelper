using System;
using System.Collections.Generic;

namespace Client.Model {
    public partial class Employee : NotifyingModel {

        public List<Shift> Shifts { get; }

        private int certificate_id;
        public int Certificate_id {
            get {
                return certificate_id;
            }
            set {
                certificate_id = value;
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
        private System.DateTime hireDate;
        public System.DateTime HireDate {
            get {
                return hireDate;
            }
            set {
                hireDate = value;
                OnPropertyChanged();
            }
        }
        private string rank;
        public string Rank {
            get {
                return rank;
            }
            set {
                rank = value;
                OnPropertyChanged();
            }
        }
        private string username;
        public string Username {
            get {
                return username;
            }
            set {
                username = value;
                OnPropertyChanged();
            }
        }
        private string password;
        public string Password {
            get {
                return password;
            }
            set {
                password = value;
                OnPropertyChanged();
            }
        }
    }
}
