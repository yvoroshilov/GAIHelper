﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace WcfServiceHost {
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer {
        public ProjectInstaller() {
           serviceProcessInstaller1 = new ServiceProcessInstaller();
           serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
           serviceInstaller1 = new ServiceInstaller();
           serviceInstaller1.ServiceName = "GAIHelperWcfService";
           serviceInstaller1.DisplayName = "GAIHelperWcfService";
           serviceInstaller1.Description = "WCF GAIHelper Service Hosted by Windows NT Service";
           serviceInstaller1.StartType = ServiceStartMode.Automatic;
           Installers.Add(serviceProcessInstaller1);
           Installers.Add(serviceInstaller1);
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e) {
            using (ServiceController sc = new ServiceController(serviceInstaller1.ServiceName)) {
                sc.Start();
            }
        }
    }
}
