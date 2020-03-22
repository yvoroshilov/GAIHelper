using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    class PaymentRepository : IPaymentRepository {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddPayment(Payment payment) {
            dbEntities.instance.Payments.Add(payment);
            dbEntities.instance.SaveChanges();
        }

        public void DeletePayment(int id) {
            Payment payment = dbEntities.instance.Payments.Find(id);
            dbEntities.instance.Payments.Remove(payment);
            dbEntities.instance.SaveChanges();
        }

        public void EditPayment(int id, Payment payment) {
            Payment oldPayment = dbEntities.instance.Payments.Find(id);
            dbEntities.instance.Entry(oldPayment).CurrentValues.SetValues(payment);
            
        }
    }
}
