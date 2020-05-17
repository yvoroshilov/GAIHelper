using GaiWcfService.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceHost {
    public partial class MainServiceHost : ServiceBase {

        private ServiceHost m_svcHost = null ;

        public MainServiceHost() {
            //Configuration configuration = ConfigurationManager.
            //OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            //configuration.AppSettings.Settings["BaseAddr"].Value = "GOVNO";
            //configuration.Save();
            //ConfigurationManager.RefreshSection("appSettings");
            Configuration.LoadConfiguration();
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            if (m_svcHost != null) m_svcHost.Close();

            m_svcHost = new ServiceHost(typeof(MainService), new Uri(Configuration.BaseAddress));
            m_svcHost.Open();
        }

        protected override void OnStop() {
            if (m_svcHost != null)
            {
                m_svcHost.Close();
                m_svcHost = null;
            }
        }
    }
}
