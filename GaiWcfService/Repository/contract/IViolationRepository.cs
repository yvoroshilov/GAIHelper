using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IViolationRepository {
        [OperationContract]
        Violation AddViolation(Violation violation);

        [OperationContract]
        void EditViolation(Violation violation);

        [OperationContract]
        void DeleteViolation(int id);

        [OperationContract]
        Violation GetViolation(int id);

        [OperationContract]
        List<Violation> GetAllViolations(int personId);
    }
}
