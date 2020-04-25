using GaiWcfService.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceHost {
    public partial class MainServiceHost : ServiceBase {

        private ServiceHost m_svcHost = null ;

        public MainServiceHost() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            if (m_svcHost != null) m_svcHost.Close();
            m_svcHost = new ServiceHost(typeof(MainService));
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
