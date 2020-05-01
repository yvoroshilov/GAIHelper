﻿using GaiWcfService.Dto;
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

        #region Admin
        [OperationContract]
        void AddAdimn(AdminDto admin);

        [OperationContract]
        void EditAdmin(int id, AdminDto admin);

        [OperationContract]
        HashSet<AdminDto> getAllAdmins();
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
        void EditShift(int id, ShiftDto shift);
        #endregion

        #region Violation
        [OperationContract]
        List<ViolationDto> GetAllViolations();
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
