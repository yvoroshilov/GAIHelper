using Client.Model;
using Client.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using System.Windows.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Collections;
using Client.MainService;
using System.ServiceModel;

namespace Client.ViewModel {
    public class PaymentsViewModel : ViewModel {

        private static readonly int COUNT = 100;

        private AdminServiceClient client;
        public ObservableCollection<PaymentDto> Payments { get; }

        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand {
            get {
                return refreshCommand ??
                    (refreshCommand = new RelayCommand(obj => {
                        Payments.Clear();
                        client.GetLastNPayments(COUNT)
                            .ToList()
                            .ForEach(val => Payments.Add(val));
                    }));
            }
        }

        public PaymentsViewModel() {
            client = ClientInstanceProvider.GetAdminServiceClient();

            Payments = new ObservableCollection<PaymentDto>(client.GetLastNPayments(COUNT));
        }
    }
}
