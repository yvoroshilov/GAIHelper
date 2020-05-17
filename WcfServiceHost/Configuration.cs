using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceHost {
    public static class Configuration {

        public static string BaseAddress { get; private set; }

        public static readonly string DEFAULT_PATH = Path.GetPathRoot(Environment.SystemDirectory) + @"GAIHelperConfig\host_config.properties";

        public static void LoadConfiguration(string path) {
            Dictionary<string, object> dict = ParseProperties(path);
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

        private static Dictionary<string, object> ParseProperties(string path) {
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
