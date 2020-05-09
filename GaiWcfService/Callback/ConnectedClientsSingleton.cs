using GaiWcfService.Repository.contract;
using GaiWcfService.Repository.implementation;
using GaiWcfService.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GaiWcfService.Callback {
    public sealed class ConnectedClientsSingleton {
        
        private static readonly object padlock = new object();

        private static ConnectedClientsSingleton instance = null;
        public static ConnectedClientsSingleton Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new ConnectedClientsSingleton();
                    }
                    return instance;
                }
            }
        }

        private ConcurrentDictionary<string, (ICallbackService callback, bool isCandidateForDeletion)> channels;
        private MyLogger logger = MyLogger.Instance;
        private Timer timer = new Timer();
        private IUserRepository userRepository = new UserRepository();
        private IShiftRepository shiftRepository = new ShiftRepository();
        public object removeMethodLock = new object();

        private ConnectedClientsSingleton() {
            channels = new ConcurrentDictionary<string, (ICallbackService, bool)>();
            timer.Elapsed += (s, a) => RemoveNotOpened();
            timer.AutoReset = true;
            timer.Interval = 1000 * 60 * 0.5;
            timer.Start();
            logger.Write("timer1 started");
        }

        private void RemoveNotOpened() {
            lock (removeMethodLock) {
                logger.Write("count: " + channels.Count);
                foreach (var item in channels) {
                    ICommunicationObject commObj = item.Value.callback as ICommunicationObject;
                    logger.Write(item.Key + " ------------- " + commObj.State);
                    if (commObj.State != CommunicationState.Opened) {
                        (ICallbackService, bool) stub = default;
                        if (item.Value.isCandidateForDeletion) {
                            if (channels.TryRemove(item.Key, out stub)) {
                                logger.Write(item.Key + " ---------- " + "CALLBACK REMOVED");
                            } else {
                                logger.Write(item.Key + " ---------- " + "CALLBACK NOT REMOVED");
                            }

                            int responsibleId = userRepository.GetUser(item.Key)
                                .Employees.First().certificate_id;
                            Shift shift = shiftRepository.GetOpenedShiftByResponsibleId(responsibleId);
                            shift.end = DateTime.Now;
                            shiftRepository.EditShift(shift.id, shift);
                            logger.Write(item.Key + " ---------- " + "SHIFT CLOSED");
                        } else {
                            channels.TryUpdate(item.Key, (item.Value.callback, true), item.Value);
                        }
                    }
                }
            }
        }

        public bool RegisterChannel(string login, ICallbackService channel) {
            return channels.TryAdd(login, (channel, false));
        }

        public void UpdateChannel(string login, ICallbackService channel) {
            (ICallbackService, bool) comp = default;
            channels.TryGetValue(login, out comp);
            channels.TryUpdate(login, (channel, false), comp);
        }

        public bool IsOpened(string login) {
            (ICallbackService, bool) res = default;
            channels.TryGetValue(login, out res);
            return (res.Item1 as ICommunicationObject).State == CommunicationState.Opened;
        }

        public void CloseConnection(string login) {
            (ICallbackService, bool) stub = default;
            channels.TryRemove(login, out stub);
        }
    }
}
