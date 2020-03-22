using GaiWcfService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GaiWcfService.Service {
    [ServiceContract]
    public interface IAdminService : IUserService {
        void AddEmployee(EmployeeDto employee);

        void EditEmployee(int id, Employee employee);

        void DeleteEmployee(int id);

        void AddShift(ShiftDto shift);

        void EditShift(int id, ShiftDto shift);

        void DeleteShift(int id);

        void AddViolationType(ViolationTypeDto violationType);

        void EditViolationType(int id, ViolationTypeDto violationType);

        void DeleteViolationType(int id);

        [OperationContract]
        string Test();
    }
}
