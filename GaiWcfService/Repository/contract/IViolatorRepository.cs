using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IViolatorRepository {
        [OperationContract]
        void AddViolator(Violator violator);

        [OperationContract]
        void EditViolator(int id, Violator violator);

        [OperationContract]
        void DeleteViolator(int id);

        [OperationContract]
        Violator GetViolator(int id);

        [OperationContract]
        HashSet<Violator> GetAll();
    }
}
