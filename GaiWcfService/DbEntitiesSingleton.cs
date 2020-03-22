using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService {
    public class DbEntitiesSingleton {
        private static DbEntitiesSingleton dbEntities;
        public GAIDBEntities instance { get; set; }

        private DbEntitiesSingleton() {
            instance = new GAIDBEntities();
        }

        public static DbEntitiesSingleton GetDbEntities() {
            if (dbEntities != null) {
                dbEntities = new DbEntitiesSingleton();
            }
            return dbEntities;
        }
    }
}
