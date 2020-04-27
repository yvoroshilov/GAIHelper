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

        /* --- ADMIN ACTIONS --- */

        [OperationContract]
        void AddAdimn(AdminDto admin);

        [OperationContract]
        void EditAdmin(int id, AdminDto admin);

        [OperationContract]
        HashSet<AdminDto> getAllAdmins();

        /* --------------------- */
        /* --- EMPLOYEE ACTIONS --- */

        [OperationContract]
        void AddEmployee(EmployeeDto employee);

        [OperationContract]
        void EditEmployee(int id, EmployeeDto employee);

        [OperationContract]
        void DeleteEmployee(int id);

        /* ------------------------ */
        /* --- SHIFT ACTIONS --- */

        [OperationContract]
        void EditShift(int id, ShiftDto shift);

        /* --------------------- */
        /* --- VIOLATIONS ACTIONS --- */

        [OperationContract]
        List<ViolationDto> GetAllViolations();

        [OperationContract]
        void DeleteViolation(int id);

        /* -------------------------- */
        /* --- VIOLATION TYPE ACTIONS --- */

        [OperationContract]
        void AddViolationType(ViolationTypeDto violationType);

        [OperationContract]
        void EditViolationType(int id, ViolationTypeDto violationType);

        [OperationContract]
        void DeleteViolationType(int id);

        /* ------------------------------ */
        /* --- PERSON ACTIONS --- */

        [OperationContract]
        void AddPerson(PersonDto person);

        [OperationContract]
        void EditPerson(int id, PersonDto person);

        [OperationContract]
        void DeletePerson(int id);

        /* ----------------------- */
    }
}
