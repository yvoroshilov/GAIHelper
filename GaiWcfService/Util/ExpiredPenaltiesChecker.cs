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
        public int aa = 0;

        private ExpiredPenaltiesChecker() {
            timer.Elapsed += (s, a) => NotifyClient();
            timer.AutoReset = true;
            timer.Interval = 1000 * 60 * 1;
            timer.Start();
            logger.Write("EXPIRED PENALTIES TIMER STARTED");
        }

        private void NotifyClient() {
            List<PersonDto> debts = personRepository.GetExpiredDebtors()
                .Select(val => Mapper.mapper.Map<PersonDto>(val))
                .ToList();
            if (debts.Count > 0) clients.InvokeAdminCallback(debts);
        }
    }
}
