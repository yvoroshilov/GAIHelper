using GaiWcfService.Callback;
using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GaiWcfService.Util {
    public sealed class ExpiredPenaltiesChecker {
        private static readonly object padlock = new object();

        private static ExpiredPenaltiesChecker instance = null;
        public static ExpiredPenaltiesChecker Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new ExpiredPenaltiesChecker();
                    }
                    return instance;
                }
            }
        }

        private ConnectedClientsSingleton clients = ConnectedClientsSingleton.Instance;
        private IPersonRepository personRepository = new PersonRepository();
        private MyLogger logger = MyLogger.Instance;
        private Timer timer = new Timer();
        private EmailSender emailSender = EmailSender.Instance;
        public int aa = 0;

        private ExpiredPenaltiesChecker() {
            timer.Elapsed += (s, a) => NotifyClient();
            timer.AutoReset = true;
            timer.Interval = 1000 * 60 * 1;
            timer.Start();
            logger.Write("EXPIRED PENALTIES TIMER STARTED");
        }

        private void NotifyClient() {
            List<Person> debts = personRepository.GetExpiredDebtors();
            if (debts.Count > 0) {
                clients.InvokeAdminCallback(debts
                    .Select(val => Mapper.mapper.Map<PersonDto>(val))
                    .ToList());
                foreach (var debtor in debts) {
                    if (debtor.email != null) {
                        StringBuilder stringBuilder = new StringBuilder("Ваша задолженность по нарушениям: \n");
                        foreach (var violation in debtor.Violations) {
                            if (!violation.paid) {
                                stringBuilder
                                    .Append(violation.violation_type_id)
                                    .Append(" -- ")
                                    .Append(violation.ViolationType.title)
                                    .Append(" -- ")
                                    .Append(Math.Round(violation.penalty, 2) + " р.")
                                    .Append(" -- ")
                                    .AppendLine(violation.date.ToShortDateString());
                            }
                        }
                        stringBuilder
                            .Append("В совокупности составляет ")
                            .Append(Math.Round(debtor.actual_penalty - debtor.paid_penalty, 2).ToString())
                            .AppendLine(" р.")
                            .AppendLine("Оплатите как можно скорее.");

                        emailSender.SendMail(debtor.email, "Оповещение о задолженности", stringBuilder.ToString());
                    }
                }
            }
        }
    }
}
