using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GaiWcfService.Util.DbEntitiesSingleton;

namespace GaiWcfService.Repository.implementation {
    class PaymentRepository : IPaymentRepository {
        public void AddPayment(Payment payment) {
            GAIDBEntities entities = dbEntities;
            payment.Person = entities.Persons.Find(payment.person_id);
            entities.Payments.Add(payment);
            entities.SaveChanges();
        }

        public void DeletePayment(int id) {
            GAIDBEntities entities = dbEntities;
            Payment payment = entities.Payments.Find(id);
            entities.Payments.Remove(payment);
            entities.SaveChanges();
        }

        public void EditPayment(Payment payment) {
            GAIDBEntities entities = dbEntities;
            Payment oldPayment = entities.Payments.Find(payment.id);
            oldPayment.person_id = payment.person_id;
            oldPayment.amount = payment.amount;
            oldPayment.date = payment.date;
            entities.SaveChanges();            
        }

        public List<Payment> GetAll() {
            return dbEntities.Payments
                .OrderBy(val => val.date)
                .ToList();
        }

        public Payment GetPayment(int id) {
            return dbEntities.Payments.Find(id);
        }

        public List<Payment> GetLastNPayments(int n) {
            GAIDBEntities entities = dbEntities;
            int count = entities.Payments.Count();
            return entities.Payments
                .OrderByDescending(val => val.date)
                .Take(Math.Min(count, n))
                .ToList();
        }

        public List<Payment> GetPaymentsByPersonId(int personId) {
            return dbEntities.Payments
                .Where(val => val.person_id == personId)
                .OrderBy(val => val.date)
                .ToList();
        }

        public List<Payment> GetPaymentsAfterLast(int lastPaymentId) {
            return dbEntities.Payments
                .OrderBy(val => val.id)
                .SkipWhile(val => val.id <= lastPaymentId)
                .OrderBy(val => val.date)
                .ToList();
        }

        public void DeleteAllPayments() {
            GAIDBEntities entities = dbEntities;
            entities.Payments.RemoveRange(entities.Payments);
            entities.SaveChanges();
        }
    }
}
