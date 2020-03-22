using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GaiWcfService.Service {
    [ServiceContract]
    public interface IAdminService {
        [OperationContract]
        string Test();

        [OperationContract]
        void AddAdimn(AdminDto admin);

        [OperationContract]
        void EditAdmin(int id, AdminDto admin);

        [OperationContract]
        void AddEmployee(EmployeeDto employee);

        [OperationContract]
        void EditEmployee(int id, EmployeeDto employee);

        [OperationContract]
        void DeleteEmployee(int id);

        [OperationContract]
        void AddPayment(PaymentDto payment);

        [OperationContract]
        void EditPayment(int id, PaymentDto payment);

        [OperationContract]
        void DeletePayment(int id);

        [OperationContract]
        void AddShift(ShiftDto shift);

        [OperationContract]
        void EditShift(int id, ShiftDto shift);

        [OperationContract]
        void DeleteShift(int id);

        [OperationContract]
        void AddViolation(ViolationDto violation);

        [OperationContract]
        void EditViolation(int id, ViolationDto violation);

        [OperationContract]
        void DeleteViolation(int id);

        [OperationContract]
        void AddViolationType(ViolationTypeDto violationType);

        [OperationContract]
        void EditViolationType(int id, ViolationTypeDto violationType);

        [OperationContract]
        void DeleteViolationType(int id);

        [OperationContract]
        void AddViolator(ViolatorDto violator);

        [OperationContract]
        void EditViolator(int id, ViolatorDto violator);

        [OperationContract]
        void DeleteViolator(int id);
    }
}
