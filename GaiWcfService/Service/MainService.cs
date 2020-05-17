using GaiWcfService.Dto;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GaiWcfService.Service {
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class MainService : IAdminService, IUserService {

        public static void Configure(ServiceConfiguration config) {
            config.LoadFromConfiguration();
            Configuration.LoadConfiguration();
            typeof(ExpiredPenaltiesChecker).GetProperty("Instance").GetValue(ExpiredPenaltiesChecker.Instance);
            typeof(PaymentsChecker).GetProperty("Instance").GetValue(PaymentsChecker.Instance);
        }

        public byte[] GetTest() {
            return new byte[] { 200 };
        }

        public void SetTest(int test) {
            Utility.CalculateViolationsPaid(19);
        }

        public string test() {
            return "TEST";
        }

        public byte[] TestAdminService() {
            return new byte[] { 200 };
        }

        public byte[] TestUserService() {
            return new byte[] { 200 };
        }
    }
}
