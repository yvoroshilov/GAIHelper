using Client.MainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client {
    class Class1 {
        public static AdminDto[] Test() {
            AdminServiceClient admin = new AdminServiceClient();
            string test = admin.Test();
            AdminDto[] test2 = admin.getAllAdmins();
            return admin.getAllAdmins();
        }
    }
}
