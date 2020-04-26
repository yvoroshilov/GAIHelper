using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Service {
    [ServiceContract]
    public interface IUserService {
        [OperationContract (Name = "AddPaymentUser")]
        void AddPayment(PaymentDto payment);

        [OperationContract(Name = "EditPaymentUser")]
        void EditPayment(int id, PaymentDto payment);

        [OperationContract(Name = "DeletePaymentUser")]
        void DeletePayment(int id);

        [OperationContract(Name = "AddShiftUser")]
        void AddShift(ShiftDto shift);

        [OperationContract(Name = "EditShiftUser")]
        void EditShift(int id, ShiftDto shift);

        [OperationContract(Name = "DeleteShiftUser")]
        void DeleteShift(int id);

        [OperationContract(Name = "AddViolationUser")]
        void AddViolation(ViolationDto violation);

        [OperationContract(Name = "EditViolationUser")]
        void EditViolation(int id, ViolationDto violation);

        [OperationContract(Name = "GetAllViolationsUser")]
        List<ViolationDto> GetAllViolations();

        [OperationContract(Name = "DeleteViolationUser")]
        void DeleteViolation(int id);

        [OperationContract(Name = "AddViolatorUser")]
        void AddViolator(ViolatorDto violator);

        [OperationContract(Name = "EditViolatorUser")]
        void EditViolator(int id, ViolatorDto violator);

        [OperationContract(Name = "DeleteViolatorUser")]
        void DeleteViolator(int id);
    }
}
