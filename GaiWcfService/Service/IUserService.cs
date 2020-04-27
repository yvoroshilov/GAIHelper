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
        [OperationContract]
        void AddPayment(PaymentDto payment);

        [OperationContract]
        void EditPayment(int id, PaymentDto payment);

        [OperationContract]
        void DeletePayment(int id);

        [OperationContract]
        void AddShift(ShiftDto shift);

        [OperationContract]
        void AddViolation(ViolationDto violation);

        [OperationContract]
        void EditViolation(int id, ViolationDto violation);

        [OperationContract]
        List<ViolationTypeDto> GetAllViolationTypes();
    }
}
