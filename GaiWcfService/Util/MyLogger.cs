using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Util {
    public class MyLogger : IDisposable {

        private static readonly object padlock = new object();

        private static MyLogger instance = null;
        public static MyLogger Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new MyLogger();
                    }
                    return instance;
                }
            }
        }

        private StreamWriter sw;

        private MyLogger() {
            if (Configuration.LogFile != null) {
                sw = new StreamWriter(Configuration.LogFile, true);
            } else {
                sw = StreamWriter.Null;
            }
            sw.AutoFlush = true;
        }

        public void Write(string s) {
            sw.Write(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " | " + s + '\n');
        }

        public void Dispose() {
            sw.Close();
        }
    }
}
