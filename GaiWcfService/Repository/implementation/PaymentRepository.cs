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
            payment.Person = dbEntities.instance.Persons.Find(payment.person_id);
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
            oldPayment.person_id = payment.person_id;
            oldPayment.payday = payment.payday;
            oldPayment.is_paid = payment.is_paid;
            dbEntities.instance.SaveChanges();            
        }

        public HashSet<Payment> GetAll() {
            return dbEntities.instance.Payments.ToHashSet();
        }

        public Payment GetPayment(int id) {
            return dbEntities.instance.Payments.Find(id);
        }
    }
}
