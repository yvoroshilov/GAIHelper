using GaiWcfService.Callback;
using GaiWcfService.Dto;
using GaiWcfService.Repository.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace GaiWcfService.Service {
    [ServiceContract(CallbackContract = typeof(ICallbackService))]
    public interface IAdminService {

        [OperationContract]
        MainService.SubscribeState Subscribe(string login);

        [OperationContract]
        void SetTest(int lel);

        [OperationContract]
        Message GetTest();

        #region User
        [OperationContract]
        void AddUser(UserDto User);

        [OperationContract]
        UserDto GetUser(string login);

        [OperationContract]
        void EditUser(string login, UserDto user);

        [OperationContract]
        HashSet<UserDto> getAllUsers();
        #endregion

        #region Employee
        [OperationContract]
        void AddEmployee(EmployeeDto employee);

        [OperationContract]
        void EditEmployee(int id, EmployeeDto employee);

        [OperationContract]
        void DeleteEmployee(int id);
        #endregion

        #region Shift
        [OperationContract]
        int OpenShift(int responsibleId);

        [OperationContract]
        void CloseShift(int responsibleId);
        #endregion

        #region ViolationType
        [OperationContract]
        void AddViolationType(ViolationTypeDto violationType);

        [OperationContract]
        void EditViolationType(int id, ViolationTypeDto violationType);

        [OperationContract]
        void DeleteViolationType(int id);
        #endregion

        #region Person
        [OperationContract]
        void AddPerson(PersonDto person);

        [OperationContract]
        void EditPerson(int id, PersonDto person);

        [OperationContract]
        void DeletePerson(int id);
        #endregion
    }
}
