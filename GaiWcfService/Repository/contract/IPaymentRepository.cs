using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IPaymentRepository {
        [OperationContract]
        void AddPayment(Payment payment);

        [OperationContract]
        void EditPayment(int id, Payment payment);

        [OperationContract]
        void DeletePayment(int id);

        [OperationContract]
        Payment GetPayment(int id);

        [OperationContract]
        HashSet<Payment> GetAll();
    }
}
