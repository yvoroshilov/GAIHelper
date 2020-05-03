using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class EmployeeRepository : IEmployeeRepository {
        private GAIDBEntities dbEntities = DbEntitiesSingleton.Instance.GetDbEntities();

        public void AddEmployee(Employee employee) {
            dbEntities.Employees.Add(employee);
            dbEntities.SaveChanges();
        }

        public void DeleteEmployee(int id) {
            Employee employee = dbEntities.Employees.Find(id);
            dbEntities.Employees.Remove(employee);
            dbEntities.SaveChanges();
        }

        public void EditEmployee(int id, Employee employee) {
            Employee oldEmployee = dbEntities.Employees.Find(id);
            oldEmployee.name = employee.name;
            oldEmployee.surname = employee.surname;
            oldEmployee.patronymic = employee.patronymic;
            oldEmployee.hire_date = employee.hire_date;
            oldEmployee.rank = employee.rank;
            oldEmployee.Shifts = employee.Shifts;
            
            dbEntities.SaveChanges();
        }

        public HashSet<Employee> GetAll() {
            return dbEntities.Employees.ToHashSet();
        }

        public Employee GetEmployee(int id) {
            return dbEntities.Employees.Find(id);
        }
    }
}
