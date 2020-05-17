using Client.MainService;
using Client.Model;
using Client.Util;
using Client.View.Admin.EmployeesTabSubWindows;
using Client.View.Admin.ViolationsTabSubWindows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Client.ViewModel {
    public class MapWindowViewModel : ViewModel {

        public static readonly BitmapImage NOT_FOUND_IMG = new BitmapImage(new Uri("../../../Resources/Img/map_not_found.jpg", UriKind.Relative));
        public static readonly BitmapImage CHOOSE_MAP_IMG = new BitmapImage(new Uri("../../../Resources/Img/choose_map.jpg", UriKind.Relative));

        public ObservableCollection<ViolationDto> Violations { get; }
        private MapImageGrabber mapImageGrabber;

        private BitmapImage src;
        public BitmapImage Src {
            get {
                return src;
            }
            set {
                src = value;
                OnPropertyChanged();
            }
        }


        private int zoom = -1;
        public int Zoom {
            get {
                return zoom;
            }
            set {
                zoom = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand acceptCommand;
        public RelayCommand AcceptCommand {
            get {
                return acceptCommand ??
                    (acceptCommand = new RelayCommand(async obj => {
                        List<ViolationDto> selectedViolations = new List<ViolationDto>((obj as ICollection).Cast<ViolationDto>());
                        if (selectedViolations.Count > 0) {
                            int[] numbers = new int[selectedViolations.Count];
                            for (int i = 0; i < numbers.Length; i++) {
                                numbers[i] = Violations.IndexOf(selectedViolations[i]) + 1;
                            }
                            BitmapImage result;
                            try {
                                result = await mapImageGrabber.GetImage(Zoom, numbers, selectedViolations.ToArray());
                            } catch {
                                MessageBox.Show("Произошла ошибка при подключении к серверу Яндекса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                Src = NOT_FOUND_IMG;
                                return;
                            }
                            
                            if (result != null) {
                                Src = result;
                            } else {
                                Src = NOT_FOUND_IMG;
                            }
                        } else {
                            Src = CHOOSE_MAP_IMG;
                        }
                    }));
            }
        }
            
        public MapWindowViewModel(ICollection<ViolationDto> violations) {
            Violations = new ObservableCollection<ViolationDto>(violations);
            mapImageGrabber = new MapImageGrabber();
        }
    }
}
