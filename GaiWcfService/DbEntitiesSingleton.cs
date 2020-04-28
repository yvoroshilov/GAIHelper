using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService {
    public class DbEntitiesSingleton {

        private static DbEntitiesSingleton dbEntities;

        private static MyLogger logger;

        public GAIDBEntities instance;

        private DbEntitiesSingleton() {
            instance = new GAIDBEntities();
            logger = new MyLogger();
            instance.Database.Log = s => logger.Write(s);
        }

        public static DbEntitiesSingleton GetDbEntities() {
            if (dbEntities == null) {
                dbEntities = new DbEntitiesSingleton();
            }
            return dbEntities;
        }

        public static MyLogger GetLogger() {
            return logger;
        }
    }

    public class MyLogger : IDisposable {

        private StreamWriter sw = new StreamWriter(new FileStream(@"D:\labs\kursach\GAIHelper\bat\test.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite));

        internal MyLogger() {
            sw.AutoFlush = true;
        }

        public void Write(string s) {
            sw.Write(DateTime.Now + " | " + s + '\n');
        }

        public void Dispose() {
            sw.Close();
        }
    }
}
