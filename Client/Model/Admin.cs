namespace Client.Model {
    public partial class Admin : NotifyingModel {
        private int id;
        public int Id { get; }

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
