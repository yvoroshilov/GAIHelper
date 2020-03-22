using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IViolationTypeRepository {
        [OperationContract]
        void AddViolationType(ViolationType violationType);

        [OperationContract]
        void EditViolationType(int id, ViolationType violationType);

        [OperationContract]
        void DeleteViolationType(int id);

        [OperationContract]
        ViolationType GetViolationType(int id);

        [OperationContract]
        HashSet<ViolationType> GetAll();
    }
}
