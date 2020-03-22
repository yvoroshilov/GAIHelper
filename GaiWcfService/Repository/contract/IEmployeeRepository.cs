using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    [ServiceContract]
    public interface IEmployeeRepository {
        [OperationContract]
        void AddEmployee(Employee employee);

        [OperationContract]
        void EditEmployee(int id, Employee employee);

        [OperationContract]
        void DeleteEmployee(int id);

        [OperationContract]
        Employee GetEmployee(int id);

        [OperationContract]
        HashSet<Employee> GetAll();
    }
}
