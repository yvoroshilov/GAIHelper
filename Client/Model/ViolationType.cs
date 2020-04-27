using System.Collections.Generic;

namespace Client.Model {

    public class ViolationType : NotifyingModel {
        private string id;
        public string Id {
            get {
                return id;
            }
            set {
                id = value;
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

        private double maxPenalty;
        public double MaxPenalty {
            get {
                return maxPenalty;
            }
            set {
                maxPenalty = value;
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

        public override string ToString() {
            return id + " " + title;
        }
    }
}
