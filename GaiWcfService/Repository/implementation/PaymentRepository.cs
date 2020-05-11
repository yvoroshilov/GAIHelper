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
            oldPayment.amount = payment.amount;
            oldPayment.date = payment.date;
            dbEntities.SaveChanges();            
        }

        public List<Payment> GetAll() {
            return dbEntities.Payments.ToList();
        }

        public Payment GetPayment(int id) {
            return dbEntities.Payments.Find(id);
        }

        public List<Payment> GetLastNPayments(int n) {
            int count = dbEntities.Payments.Count();
            return dbEntities.Payments
                .OrderByDescending(val => val.date)
                .Take(Math.Min(count, n))
                .ToList();
        }

        public List<Payment> GetPaymentsByPersonId(int personId) {
            return dbEntities.Payments
                .Where(val => val.person_id == personId)
                .ToList();
        }

        public List<Payment> GetPaymentsAfterLast(int lastPaymentId) {
            return dbEntities.Payments
                .OrderBy(val => val.id)
                .SkipWhile(val => val.id <= lastPaymentId)
                .ToList();
        }

        public void DeleteAllPayments() {
            dbEntities.Payments.RemoveRange(dbEntities.Payments);
            dbEntities.SaveChanges();
        }
    }
}
