using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.contract {
    public interface IEmployeeRepository {
        void AddEmployee(Employee employee);

        void EditEmployee(int id, Employee employee);

        void DeleteEmployee(int id);

        Employee GetEmployee(int id);

        Employee GetEmployeeByUserLogin(string login);

        HashSet<Employee> GetAll();
    }
}
