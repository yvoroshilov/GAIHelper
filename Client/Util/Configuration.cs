using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Util {
    public static class Configuration {

        public static string AdminServiceEndpointAddress { get; private set; }

        public static string UserServiceEndpointAddress { get; private set; }

        public static readonly string DEFAULT_PATH = Path.GetPathRoot(Environment.SystemDirectory) + @"GAIHelperConfig\client_config.properties";

        private static string path;

        public static void LoadConfiguration(string path) {
            Dictionary<string, object> dict = ParseProperties(path);
            AssignValues(dict);
            Configuration.path = path;
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

        private static void AssignValues(Dictionary<string, object> dict) {
            var props = typeof(Configuration).GetProperties();
            foreach (var prop in props) {
                object value;
                dict.TryGetValue(prop.Name, out value);
                prop.SetValue(null, value);
            }
        }

        public static void UpdateValue(string key, string value) {
            string actualPath = path ?? DEFAULT_PATH;
            if (!File.Exists(actualPath)) {
                throw new Exception("File cannot be accessed");
            }
            Dictionary<string, object> dict = ParseProperties(actualPath);
            object val;
            dict.TryGetValue(key, out val);

            if (val != null) {
                dict[key] = value;
            } else {
                dict.Add(key, value);
            }
            File.WriteAllLines(actualPath, dict.Select(val => $"{val.Key}={val.Value as string}"));
            AssignValues(dict);
        }

        public static void CreateConfigFile() {
            string dirPath = DEFAULT_PATH.Remove(DEFAULT_PATH.LastIndexOf('\\'));
            Directory.CreateDirectory(dirPath);
            File.Create(DEFAULT_PATH).Close();
        }
    }
}
