using Client.MainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Util {
    public static class ClientInstanceProvider {

        public static string AdminServiceEndpointAddress { get; private set; }

        public static string UserServiceEndpointAddress { get; private set; }

        public static readonly string DefaultAdminServiceEndpointAddress = "net.tcp://localhost:9007/AdminService";

        public static readonly string DefaultUserServiceEndpointAddress = "net.tcp://localhost:9007/UserService";

        private static NetTcpBinding binding = new NetTcpBinding() {
            MaxReceivedMessageSize = 20480000,
        };

        static ClientInstanceProvider() {
            SetDefaultAddresses();
        }

        public static AdminServiceClient GetAdminServiceClient(IAdminServiceCallback callback) {
            EndpointAddress endpointAddr = new EndpointAddress(AdminServiceEndpointAddress);
            return new AdminServiceClient(new InstanceContext(callback), binding, endpointAddr);
        }

        public static AdminServiceClient GetAdminServiceClient() {
            return GetAdminServiceClient(new DummyCallbackClass());
        }

        public static UserServiceClient GetUserServiceClient() {
            EndpointAddress endpointAddr = new EndpointAddress(UserServiceEndpointAddress);
            return new UserServiceClient(binding, endpointAddr);
        }

        [CallbackBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, AutomaticSessionShutdown = false)] 
        public class DummyCallbackClass : IAdminServiceCallback {
            public void SendPenaltyExpired(PersonDto[] persons) {
                throw new NotImplementedException();
            }
        }

        public static void UpdateAddressesFromConfiguration() {
            AdminServiceEndpointAddress = Configuration.AdminServiceEndpointAddress;
            UserServiceEndpointAddress = Configuration.UserServiceEndpointAddress;
        }

        public static void SetDefaultAddresses() {
            AdminServiceEndpointAddress = DefaultAdminServiceEndpointAddress;
            UserServiceEndpointAddress = DefaultUserServiceEndpointAddress;
        }
    }
}
