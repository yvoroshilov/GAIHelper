using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    public interface IPaymentRepository {
        void AddPayment(Payment payment);

        void EditPayment(int id, Payment payment);

        void DeletePayment(int id);

        Payment GetPayment(int id);

        HashSet<Payment> GetAll();
    }
}
