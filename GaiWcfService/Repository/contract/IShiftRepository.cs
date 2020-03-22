using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IShiftRepository {
        [OperationContract]
        void AddShift(Shift shift);

        [OperationContract]
        void EditShift(int id, Shift shift);

        [OperationContract]
        void DeleteShift(int id);
    }
}
