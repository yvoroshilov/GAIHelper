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

        public Employee GetEmployeeByUserLogin(string login) {
            return dbEntities.Employees.Where(val => val.user_login == login).SingleOrDefault();
        }

        public void DeleteEmployee(int id) {
            Employee employee = dbEntities.Employees.Find(id);
            dbEntities.Employees.Remove(employee);
            dbEntities.SaveChanges();
        }

        public void EditEmployee(Employee employee) {
            Employee oldEmployee = dbEntities.Employees.Find(employee.certificate_id);
            oldEmployee.user_login = employee.user_login;
            oldEmployee.name = employee.name;
            oldEmployee.surname = employee.surname;
            oldEmployee.patronymic = employee.patronymic;
            oldEmployee.hire_date = employee.hire_date;
            
            dbEntities.SaveChanges();
        }

        public HashSet<Employee> GetAll() {
            return dbEntities.Employees.ToHashSet();
        }

        public Employee GetEmployee(int id) {
            return dbEntities.Employees.Find(id);
        }

        public List<Employee> SearchEmployees(Employee searchedEmpl) {
            return dbEntities.Employees.Where(val => 
                (searchedEmpl.certificate_id == default || val.certificate_id == searchedEmpl.certificate_id) &&
                (searchedEmpl.user_login == default || val.user_login.Contains(searchedEmpl.user_login)) &&
                (searchedEmpl.name == default || val.name.Contains(searchedEmpl.name)) && 
                (searchedEmpl.surname == default || val.surname.Contains(searchedEmpl.surname)) &&
                (searchedEmpl.patronymic == default || val.patronymic.Contains(searchedEmpl.patronymic)) &&
                (searchedEmpl.hire_date == default || val.hire_date == searchedEmpl.hire_date))
                .ToList();
        }
    }
}
