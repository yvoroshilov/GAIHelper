using GaiWcfService.Repository.contract;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaiWcfService.Repository.implementation {
    public class EmployeeRepository : IEmployeeRepository {
        private DbEntitiesSingleton dbEntities = DbEntitiesSingleton.GetDbEntities();

        public void AddEmployee(Employee employee) {
            dbEntities.instance.Employees.Add(employee);
            dbEntities.instance.SaveChanges();
        }

        public void DeleteEmployee(int id) {
            Employee employee = dbEntities.instance.Employees.Find(id);
            dbEntities.instance.Employees.Remove(employee);
            dbEntities.instance.SaveChanges();
        }

        public void EditEmployee(int id, Employee employee) {
            Employee oldEmployee = dbEntities.instance.Employees.Find(id);
            dbEntities.instance.Entry(oldEmployee).CurrentValues.SetValues(employee);
        }
    }
}
