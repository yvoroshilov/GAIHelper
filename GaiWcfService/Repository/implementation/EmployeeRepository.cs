using GaiWcfService.Repository.contract;
using GaiWcfService.Service;
using GaiWcfService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GaiWcfService.Util.DbEntitiesSingleton;

namespace GaiWcfService.Repository.implementation {
    public class EmployeeRepository : IEmployeeRepository {

        public void AddEmployee(Employee employee) {
            GAIDBEntities entities = dbEntities;
            entities.Employees.Add(employee);
            entities.SaveChanges();
        }

        public Employee GetEmployeeByUserLogin(string login) {
            return dbEntities.Employees
                .Where(val => val.user_login == login)
                .SingleOrDefault();
        }

        public void DeleteEmployee(int id) {
            GAIDBEntities entities = dbEntities;
            Employee employee = entities.Employees.Find(id);
            entities.Employees.Remove(employee);
            entities.SaveChanges();
        }

        public void EditEmployee(Employee employee) {
            GAIDBEntities entities = dbEntities;
            Employee oldEmployee = entities.Employees.Find(employee.certificate_id);
            oldEmployee.user_login = employee.user_login;
            oldEmployee.name = employee.name;
            oldEmployee.surname = employee.surname;
            oldEmployee.patronymic = employee.patronymic;
            oldEmployee.hire_date = employee.hire_date;
            
            entities.SaveChanges();
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
                (searchedEmpl.user_login == default || val.user_login.ToLower().Contains(searchedEmpl.user_login.ToLower())) &&
                (searchedEmpl.name == default || val.name.ToLower().Contains(searchedEmpl.name.ToLower())) && 
                (searchedEmpl.surname == default || val.surname.ToLower().Contains(searchedEmpl.surname.ToLower())) &&
                (searchedEmpl.patronymic == default || val.patronymic.ToLower().Contains(searchedEmpl.patronymic.ToLower())) &&
                (searchedEmpl.hire_date == default || val.hire_date == searchedEmpl.hire_date))
                .OrderBy(val => val.hire_date)
                .ToList();
        }
    }
}
