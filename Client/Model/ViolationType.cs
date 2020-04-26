using System.Collections.Generic;

namespace Client.Model {

    public class ViolationType : NotifyingModel {
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

        private string shortTitle;
        public string ShortTitle {
            get {
                return shortTitle;
            }
            set {
                shortTitle = value;
                OnPropertyChanged();
            }
        }

        private string title;
        public string Title {
            get {
                return title;
            }
            set {
                title = value;
                OnPropertyChanged();
            }
        }

        private double minPenalty;
        public double MinPenalty {
            get {
                return minPenalty;
            }
            set {
                minPenalty = value;
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

        private List<Violation> violations;
        public List<Violation> Violations {
            get {
                return violations;
            }
            set {
                violations = value;
                OnPropertyChanged();
            }
        }
    }
}
