using GaiWcfService.Callback;
using GaiWcfService.Dto;
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
                ICallbackService callback = OperationContext.Current.GetCallbackChannel<ICallbackService>();
                User user = userRepository.GetUser(login);
                if (user.role != "ROLE_USER") {
                    return SubscribeState.NOT_SUBSCRIBED;
                }

                bool addRes = clients.RegisterChannel(login, callback);
                if (addRes) {
                    OpenShift(user.Employees.First().certificate_id);
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
