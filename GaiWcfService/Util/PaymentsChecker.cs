using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GaiWcfService.Util {
    public class PaymentsChecker {
        private static readonly object padlock = new object();

        private static PaymentsChecker instance = null;
        public static PaymentsChecker Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new PaymentsChecker();
                    }
                    return instance;
                }
            }
        }

        private IPaymentRepository paymentRepository = new PaymentRepository();
        private IPersonRepository personRepository = new PersonRepository();
        private IViolationRepository violationRepository = new ViolationRepository();
        private MyLogger logger = MyLogger.Instance;

        private PaymentsChecker() {
            Timer checkTimer = new Timer();
            checkTimer.Elapsed += CheckPayments;
            checkTimer.AutoReset = true;
            checkTimer.Interval = 1000 * 60 * 1;
            checkTimer.Start();
            logger.Write("PAYMENTS CHECKER STARTED");
        }

        private void CheckPayments(object sender, ElapsedEventArgs e) {
            List<Payment> newPayments = paymentRepository.GetAll();
            foreach (Payment payment in newPayments) {
                payment.Person.paid_penalty += payment.amount;
                personRepository.EditPerson(payment.Person);
                Utility.CalculateViolationsPaid(payment.person_id);
                logger.Write("PAYMENT " + payment.id + " PROCEED");
            }
            paymentRepository.DeleteAllPayments();
        }
    }
}
