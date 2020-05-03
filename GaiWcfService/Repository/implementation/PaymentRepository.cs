using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    class PaymentRepository : IPaymentRepository {
        private GAIDBEntities dbEntities = DbEntitiesSingleton.Instance.GetDbEntities();

        public void AddPayment(Payment payment) {
            payment.Person = dbEntities.Persons.Find(payment.person_id);
            dbEntities.Payments.Add(payment);
            dbEntities.SaveChanges();
        }

        public void DeletePayment(int id) {
            Payment payment = dbEntities.Payments.Find(id);
            dbEntities.Payments.Remove(payment);
            dbEntities.SaveChanges();
        }

        public void EditPayment(int id, Payment payment) {
            Payment oldPayment = dbEntities.Payments.Find(id);
            oldPayment.person_id = payment.person_id;
            oldPayment.payday = payment.payday;
            oldPayment.is_paid = payment.is_paid;
            dbEntities.SaveChanges();            
        }

        public HashSet<Payment> GetAll() {
            return dbEntities.Payments.ToHashSet();
        }

        public Payment GetPayment(int id) {
            return dbEntities.Payments.Find(id);
        }
    }
}
