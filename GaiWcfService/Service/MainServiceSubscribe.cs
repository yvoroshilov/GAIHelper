using GaiWcfService.Callback;
using GaiWcfService.Dto;
using GaiWcfService.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GaiWcfService.Service {
    public partial class MainService {

        private ConnectedClientsSingleton clients = ConnectedClientsSingleton.Instance;

        public enum SubscribeState {
            SUBSCRIBED,
            SUBSCRIBE_UPDATED,
            NOT_SUBSCRIBED
        }

        public SubscribeState Subscribe(string login) {
            lock (clients.removeMethodLock) {
                User user = userRepository.GetUser(login);

                ICallbackService callback = OperationContext.Current.GetCallbackChannel<ICallbackService>();

                bool addRes = clients.RegisterChannel(login, callback);

                if (user.role != "ROLE_USER") {
                    ExpiredPenaltiesChecker.Instance.aa = 3;
                    return SubscribeState.NOT_SUBSCRIBED;
                }
                
                if (addRes) {
                    MyLogger.Instance.Write(user.Employees.Count.ToString());
                    OpenShift(user.Employees.First().certificate_id);
                    MyLogger.Instance.Write(user.Employees.First().certificate_id.ToString());
                    return SubscribeState.SUBSCRIBED;
                } else {
                    if (clients.IsOpened(login)) {
                        return SubscribeState.NOT_SUBSCRIBED;
                    }
                    clients.UpdateChannel(login, callback);
                    return SubscribeState.SUBSCRIBE_UPDATED;
                }
            }
        }
    }
}
