using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IAdminRepository {
        [OperationContract]
        void AddAdimn(Admin admin);

        [OperationContract]
        void EditAdmin(int id, Admin admin);

        [OperationContract]
        Admin GetAdmin(int id);

        [OperationContract]
        HashSet<Admin> GetAll();
    }
}
