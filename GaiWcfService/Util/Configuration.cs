using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Util {
    public static class Configuration {

        public static string MailLogin { get; private set; }

        public static string MailPassword { get; private set; }

        public static string DocDir { get; private set; }

        public static string LogFile { get; private set; }

        public static readonly string DEFAULT_PATH = Path.GetPathRoot(Environment.SystemDirectory) + @"GAIHelperConfig\config.properties";

        public static void LoadConfiguration(string path) {
            Dictionary<string, object> dict = parseProperties(path);
            var props = typeof(Configuration).GetProperties();
            foreach (var prop in props) {
                object value;
                dict.TryGetValue(prop.Name, out value);
                prop.SetValue(null, value);
            }
        }

        public static void LoadConfiguration() {
            LoadConfiguration(DEFAULT_PATH);
        }

        private static Dictionary<string, object> parseProperties(string path) {
            string[] lines = File.ReadAllLines(path);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (string line in lines) {
                int eqInd = line.IndexOf('=');
                string key = line.Substring(0, eqInd);
                string val = line.Substring(eqInd + 1);
                dict.Add(key, val);
            }
            return dict;
        }
    }
}
