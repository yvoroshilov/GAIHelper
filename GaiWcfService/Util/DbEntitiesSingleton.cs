using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService {
    public class DbEntitiesSingleton {

        private static readonly object padlock = new object();

        private static DbEntitiesSingleton instance = null;
        public static DbEntitiesSingleton Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new DbEntitiesSingleton();
                    }
                    return instance;
                }
            }
        }

        private GAIDBEntities entities;

        private DbEntitiesSingleton() {
            entities = new GAIDBEntities();
            entities.Database.Log = s => MyLogger.Instance.Write(s);
        }

        public GAIDBEntities GetDbEntities() {
            return entities;
        }
    }

}
