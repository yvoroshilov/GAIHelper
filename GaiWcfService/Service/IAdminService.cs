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
        void EditUser(UserDto user);

        [OperationContract]
        HashSet<UserDto> getAllUsers();
        #endregion

        #region Employee
        [OperationContract]
        void AddEmployee(EmployeeDto employee);

        [OperationContract]
        EmployeeDto GetEmployeeByUserLogin(string login);
            
        [OperationContract]
        void EditEmployee(EmployeeDto employee);

        [OperationContract]
        List<EmployeeDto> SearchEmployees(EmployeeDto searchedEmployee);

        [OperationContract]
        void DeleteEmployee(int id);
        #endregion

        #region Shift
        [OperationContract]
        int OpenShift(int responsibleId);

        [OperationContract]
        void CloseShift(int responsibleId);

        [OperationContract]
        ShiftDto GetCurrentShift(int responsibleId);

        [OperationContract]
        List<ShiftDto> GetAllShiftsByResponsibleId(int responsibleId);
        #endregion

        #region Violation
        [OperationContract]
        List<ViolationDto> SearchViolations(ViolationDto violation);

        [OperationContract]
        List<ViolationDto> GetAllViolationsByResponsibleId(int responsibleId);
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
        void EditPerson(PersonDto person);

        [OperationContract]
        List<PersonDto> SearchPersons(PersonDto searchedPerson);

        [OperationContract]
        void DeletePerson(int id);
        #endregion
    }
}
